Public Class Compiler(Of TResult)

    Protected _LocatedString As LocatedString

    Private ReadOnly _BaseContext As TermContext
    Public ReadOnly Property BaseContext As TermContext
        Get
            Return _BaseContext
        End Get
    End Property

    Private ReadOnly _Selection As TextLocation

    Private ReadOnly _CurrentIdentifier As LocatedString
    Private ReadOnly Property CurrentIdentifier As LocatedString
        Get
            Return _CurrentIdentifier
        End Get
    End Property

    Private ReadOnly _ResultType As NamedType

    Private _Statements As IEnumerable(Of LocatedString)

    Public Sub New(locatedString As LocatedString, baseContext As TermContext, typeNamedTypeDictionary As TypeNamedTypeDictionary, Optional selection As TextLocation = Nothing)
        _LocatedString = locatedString
        _BaseContext = baseContext
        _Selection = selection
        _ResultType = typeNamedTypeDictionary.GetNamedType(GetType(TResult))
        _Statements = _LocatedString.Split({";"c})
        _CurrentIdentifier = _LocatedString.TryGetSurroundingIdentifier(selection)
    End Sub

    Public Function Compile(Optional textChanged As Boolean = False) As CompilerResult(Of TResult)
        Dim context = _BaseContext
        Dim cursorTermContext = context

        Try
            For Each statement In _Statements
                If statement.Location.ContainsRange(_Selection) Then
                    cursorTermContext = context
                End If

                If Not statement.Trim.ToString.Any Then Continue For

                If IsReturnStatement(statement) Then
                    Return New CompilerResult(Of TResult)(New Term(Term:=GetReturnTerm(statement), TypeInformation:=New TypeInformation(_ResultType), context:=context).GetDelegate(Of Func(Of TResult)).Invoke, IntelliSense:=Me.GetIntelliSense(cursorTermContext, textChanged:=textChanged))
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
            Throw ex.WithIntelliSense(Me.GetIntelliSense(cursorTermContext, textChanged))
        End Try

        Throw New CompilerException("Missing return statement.").WithIntelliSense(Me.GetIntelliSense(cursorTermContext, textChanged))
    End Function

    Private Function GetIntelliSense(cursorTermContext As TermContext, textChanged As Boolean) As IntelliSense
        Return If(textChanged, New IntelliSense(TermContext:=cursorTermContext, filter:=_CurrentIdentifier.ToString), IntelliSense.Empty)
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
