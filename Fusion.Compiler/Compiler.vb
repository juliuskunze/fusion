Public Class Compiler(Of TResult)

    Protected _LocatedString As LocatedString

    Private ReadOnly _BaseContext As TermContext
    Public ReadOnly Property BaseContext As TermContext
        Get
            Return _BaseContext
        End Get
    End Property

    Private ReadOnly _CursorPosition As Integer

    Private ReadOnly _ResultType As NamedType

    Private _Instructions As IEnumerable(Of LocatedString)

    Public Sub New(text As String, baseContext As TermContext, typeNamedTypeDictionary As TypeNamedTypeDictionary, Optional cursorPosition As Integer = 0)
        _LocatedString = text.ToAnalized.ToLocated
        _BaseContext = baseContext
        _CursorPosition = cursorPosition
        _ResultType = typeNamedTypeDictionary.GetNamedType(GetType(TResult))
        _Instructions = _LocatedString.Split({";"c})
    End Sub

    Public Function GetResult() As TResult
        Dim context = _BaseContext

        For Each instruction In _Instructions
            If Not instruction.Trim.ToString.Any Then Continue For

            If IsReturnTerm(instruction) Then
                Return New Term(Term:=GetReturnTerm(instruction), TypeInformation:=New TypeInformation(_ResultType), context:=context).GetDelegate(Of Func(Of TResult)).Invoke
            End If

            Dim definition = New Assignment(definition:=instruction, context:=context)
            If definition.IsFunctionAssignment Then
                context = context.Merge(New TermContext(Functions:={New FunctionAssignment(definition:=instruction, context:=context).GetFunctionInstance}))
            Else
                context = context.Merge(New TermContext(constants:={New ConstantAssignment(definition:=instruction, context:=context).GetNamedConstantExpression}))
            End If
        Next

        Throw New CompilerException("Missing return statement.")
    End Function

    Const _ReturnKeyword = "return"

    Private Shared Function IsReturnTerm(returnStatement As LocatedString) As Boolean
        Dim trim = returnStatement.Trim

        If Not trim.ToString.Any Then Return False
        Return CompilerTools.IdentifierEquals(trim.GetStartingIdentifier.ToString, _ReturnKeyword)
    End Function

    Private Shared Function GetReturnTerm(returnStatement As LocatedString) As LocatedString
        Dim trim = returnStatement.Trim
        If Not CompilerTools.IdentifierEquals(trim.GetStartingIdentifier.ToString, _ReturnKeyword) Then Throw New ArgumentException("Return statement expected.")

        Return trim.Substring(startIndex:=_ReturnKeyword.Count)
    End Function

End Class
