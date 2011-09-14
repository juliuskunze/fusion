Public Class Compiler(Of TResult)

    Protected _LocatedString As LocatedString

    Private ReadOnly _BaseContext As TermContext
    Public ReadOnly Property BaseContext As TermContext
        Get
            Return _BaseContext
        End Get
    End Property

    Private _CurrentIdentifierIfDefined As LocatedString
    Public ReadOnly Property CurrentIdentifierIfDefined As LocatedString
        Get
            Return _CurrentIdentifierIfDefined
        End Get
    End Property

    Private ReadOnly _ResultType As NamedType

    Private _Statements As IEnumerable(Of LocatedString)
    Private _Selection As TextLocation

    Private _TermContextAtSelection As TermContext

    Private _TextChanged As Boolean

    Public Sub Update(newLocatedString As LocatedString,
                      Optional newSelection As TextLocation = Nothing,
                      Optional textChanged As Boolean = False)
        _LocatedString = newLocatedString
        _Selection = newSelection
        _Statements = _LocatedString.Split({";"c})
        _CurrentIdentifierIfDefined = Me.TryGetCurrentIdentifier()

        Me.UpdateIntelliSense()
    End Sub

    Private Function TryGetCurrentIdentifier() As LocatedString
        If _LocatedString Is Nothing OrElse _Selection Is Nothing Then Return Nothing

        Return _LocatedString.TryGetSurroundingIdentifier(_Selection)
    End Function

    Private Sub UpdateIntelliSense()
        _IntelliSense = Me.GetIntelliSense
    End Sub

    Private Function GetIntelliSense() As IntelliSense
        If _TermContextAtSelection Is Nothing Then Return Compiler.IntelliSense.Empty

        Return New IntelliSense(TermContext:=_TermContextAtSelection)
    End Function

    Public Function GetIntelliSenseFilterName() As String
        If _CurrentIdentifierIfDefined Is Nothing Then Return ""

        Return _CurrentIdentifierIfDefined.ToString
    End Function

    Private _IntelliSense As IntelliSense
    Public ReadOnly Property IntelliSense(Optional textChanged As Boolean = False) As IntelliSense
        Get
            Return _IntelliSense
        End Get
    End Property

    Public Sub New(locatedString As LocatedString, baseContext As TermContext, typeNamedTypeDictionary As TypeNamedTypeDictionary)
        Me.New(baseContext:=baseContext, typeNamedTypeDictionary:=typeNamedTypeDictionary)
        Me.Update(newLocatedString:=locatedString)
    End Sub

    Public Sub New(baseContext As TermContext, typeNamedTypeDictionary As TypeNamedTypeDictionary)
        _BaseContext = baseContext
        _ResultType = typeNamedTypeDictionary.GetNamedType(GetType(TResult))
    End Sub

    Public Function Compile() As CompilerResult(Of TResult)
        Dim context = _BaseContext

        Try
            For Each statement In _Statements
                If _Selection IsNot Nothing AndAlso statement.Location.ContainsRange(_Selection) Then
                    _TermContextAtSelection = context
                End If

                If Not statement.Trim.ToString.Any Then Continue For

                If IsReturnStatement(statement) Then
                    Return New CompilerResult(Of TResult)(New Term(Term:=GetReturnTerm(statement), TypeInformation:=New TypeInformation(_ResultType), context:=context).GetDelegate(Of Func(Of TResult)).Invoke, IntelliSense:=_IntelliSense)
                End If

                If IsDelegateDefinition(statement) Then
                    context = context.Merge(New TermContext(types:=New NamedTypes({FunctionSignature.FromString(GetDelegateString(statement), context.Types).AsNamedDelegateType})))
                    Continue For
                End If

                Dim definition = New Assignment(definition:=statement, context:=context)
                If definition.IsFunctionAssignment Then
                    context = context.Merge(New TermContext(Functions:={New FunctionAssignment(definition:=statement, context:=context).GetFunctionInstance}))
                Else
                    context = context.Merge(New TermContext(constants:={New ConstantAssignment(definition:=statement, context:=context).GetNamedConstantExpression}))
                End If
            Next
        Catch ex As CompilerException
            Me.UpdateIntelliSense()
            Throw ex.WithIntelliSense(_IntelliSense)
        End Try

        Throw New CompilerException("Missing return statement.").WithIntelliSense(_IntelliSense)
    End Function

    Const _ReturnKeyword = "return"
    Const _DelegateKeyword = "delegate"

    Private Shared Function IsReturnStatement(statement As LocatedString) As Boolean
        Return StartWithIdentifier(statement, _ReturnKeyword)
    End Function

    Private Shared Function IsDelegateDefinition(statement As LocatedString) As Boolean
        Return StartWithIdentifier(statement, _DelegateKeyword)
    End Function

    Private Shared Function StartWithIdentifier(statement As LocatedString, identifier As String) As Boolean
        Dim trim = statement.Trim

        If Not trim.ToString.Any Then Return False
        Return CompilerTools.IdentifierEquals(trim.GetStartingIdentifier.ToString, identifier)
    End Function

    Private Shared Function GetReturnTerm(returnStatement As LocatedString) As LocatedString
        Dim trim = returnStatement.Trim
        If Not CompilerTools.IdentifierEquals(trim.GetStartingIdentifier.ToString, _ReturnKeyword) Then Throw New ArgumentException("Return statement expected.")

        Return trim.Substring(startIndex:=_ReturnKeyword.Count)
    End Function

    Private Shared Function GetDelegateString(delegateStatement As LocatedString) As LocatedString
        Dim trim = delegateStatement.Trim
        If Not CompilerTools.IdentifierEquals(trim.GetStartingIdentifier.ToString, _DelegateKeyword) Then Throw New ArgumentException("Delegate definition expected.")

        Return trim.Substring(startIndex:=_DelegateKeyword.Count)
    End Function

End Class
