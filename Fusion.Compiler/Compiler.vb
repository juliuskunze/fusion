Public Class Compiler(Of TResult)

    Protected _LocatedString As LocatedString

    Private ReadOnly _BaseContext As TermContext
    Public ReadOnly Property BaseContext As TermContext
        Get
            Return _BaseContext
        End Get
    End Property

    Private ReadOnly _ResultType As NamedType

    Private _Instructions As IEnumerable(Of LocatedString)

    Public Sub New(text As String, baseContext As TermContext, typeNamedTypeDictionary As TypeNamedTypeDictionary)
        _LocatedString = text.ToAnalized.ToLocated
        _BaseContext = baseContext
        _ResultType = typeNamedTypeDictionary.GetNamedType(GetType(TResult))
        _Instructions = _LocatedString.Split({";"c})
    End Sub

    Public Function GetTermContext(definitionStrings As IEnumerable(Of LocatedString)) As TermContext
        Dim context = _BaseContext

        For Each definitionString In definitionStrings
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
        Dim lastNotNullInstruction = _LocatedString
        Dim lastNotNullInstructionIndex = 0

        For instructionIndex = _Instructions.Count - 1 To 0 Step -1
            Dim instruction = _Instructions(instructionIndex)

            If Not String.IsNullOrWhiteSpace(instruction.ToString) Then
                lastNotNullInstruction = instruction
                lastNotNullInstructionIndex = instructionIndex
                Exit For
            End If
        Next

        Dim returnTermString = GetReturnTerm(lastNotNullInstruction)

        Dim definitionStrings = New List(Of LocatedString)
        For index = 0 To lastNotNullInstructionIndex - 1
            definitionStrings.Add(_Instructions(index))
        Next

        Dim context = Me.GetTermContext(definitionStrings:=definitionStrings)

        Dim returnTerm = New Term(Term:=returnTermString, TypeInformation:=New TypeInformation(_ResultType), context:=context)

        Return returnTerm.GetDelegate(Of Func(Of TResult)).Invoke
    End Function

    Private Const _MissingReturnStatementExceptionMessage = "Missing return statement."

    Private Shared Function GetReturnTerm(returnStatement As LocatedString) As LocatedString
        Const returnKeyword = "return"

        If String.IsNullOrWhiteSpace(returnStatement.ToString) Then ThrowMissingReturnStatementException()

        Dim trim = returnStatement.Trim
        If Not CompilerTools.IdentifierEquals(trim.GetStartingIdentifier.ToString, returnKeyword) Then Throw New LocatedCompilerException(trim, "Return statement expected.")

        Return trim.Substring(startIndex:=returnKeyword.Count)
    End Function

    Private Shared Sub ThrowMissingReturnStatementException()

    End Sub

End Class
