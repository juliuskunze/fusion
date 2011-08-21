Public Class Compiler(Of TResult)

    Private ReadOnly _Text As String

    Private ReadOnly _BaseContext As TermContext
    Public ReadOnly Property BaseContext As TermContext
        Get
            Return _BaseContext
        End Get
    End Property

    Private ReadOnly _ResultType As NamedType

    Private ReadOnly _Definitions As IEnumerable(Of String)

    Public Sub New(text As String, baseContext As TermContext, typeNamedTypeDictionary As TypeNamedTypeDictionary)
        _Text = text
        _BaseContext = baseContext
        _ResultType = typeNamedTypeDictionary.GetNamedType(GetType(TResult))
        _Definitions = _Text.Split(";"c)
    End Sub

    Public Function GetTermContext() As TermContext
        Dim context = _BaseContext

        For definitionIndex = 0 To _Definitions.Count - 3
            Dim definitionString = _Definitions(definitionIndex)

            Dim definition = New Assignment(definition:=definitionString, context:=context)
            If definition.IsFunctionAssignment Then
                context = context.Merge(New TermContext(Functions:={New FunctionAssignment(definition:=definitionString, context:=context).GetFunctionInstance}))
            Else
                context = context.Merge(New TermContext(constants:={New ConstantAssignment(definition:=definitionString, context:=context).GetNamedConstantExpression}))
            End If
        Next

        Return context
    End Function

    Public Function GetResult() As TResult
        Dim context = Me.GetTermContext

        Dim returnStatement = _Definitions(_Definitions.Count - 2)
        If _Definitions.Last <> "" Then Throw New ArgumentException("Missing ';' after return statement.")

        Dim returnTerm = New Term(Term:=GetReturnTerm(returnStatement), Type:=_ResultType, context:=context)

        Return returnTerm.GetDelegate(Of Func(Of TResult)).Invoke
    End Function

    Private Function GetReturnTerm(returnStatement As String) As String
        Const returnKeyword = "return"

        Dim trim = returnStatement.Trim
        If Not CompilerTools.IdentifierEquals(trim.GetStartingIdentifier, returnKeyword) Then Throw New ArgumentException("Missing return statement.")

        Return trim.Substring(startIndex:=returnKeyword.Count)
    End Function

End Class
