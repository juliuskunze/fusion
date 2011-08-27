Public Class Compiler(Of TResult)

    Private _Text As String

    Private ReadOnly _BaseContext As TermContext
    Public ReadOnly Property BaseContext As TermContext
        Get
            Return _BaseContext
        End Get
    End Property

    Private ReadOnly _ResultType As NamedType

    Private _Instructions As String()

    Public Sub New(text As String, baseContext As TermContext, typeNamedTypeDictionary As TypeNamedTypeDictionary)
        _Text = text
        _BaseContext = baseContext
        _ResultType = typeNamedTypeDictionary.GetNamedType(GetType(TResult))
        _Instructions = _Text.Split(";"c)
    End Sub

    Public Function GetCorrectedText() As String
        Return _Text.TrimEnd({" "c, Microsoft.VisualBasic.ControlChars.Tab, ";"c})
    End Function

    Public Function GetTermContext(definitionStrings As IEnumerable(Of String)) As TermContext
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

    Public Function GetResult() As CompilerResult(Of TResult)
        Dim lastNotNullInstruction = ""
        Dim lastNotNullInstructionIndex = 0

        For instructionIndex = _Instructions.Count - 1 To 0 Step -1
            Dim instruction = _Instructions(instructionIndex)

            If Not String.IsNullOrWhiteSpace(instruction) Then
                lastNotNullInstruction = instruction
                lastNotNullInstructionIndex = instructionIndex
                Exit For
            End If
        Next

        Dim returnTermString = GetReturnTerm(lastNotNullInstruction)

        Dim definitionStrings = New List(Of String)
        For index = 0 To lastNotNullInstructionIndex - 1
            definitionStrings.Add(_Instructions(index))
        Next

        Dim context = Me.GetTermContext(definitionStrings:=definitionStrings)

        Dim returnTerm = New Term(Term:=returnTermString, Type:=_ResultType, context:=context)
        
        Return New CompilerResult(Of TResult)(result:=returnTerm.GetDelegate(Of Func(Of TResult)).Invoke, correctedText:=Me.GetCorrectedText)
    End Function

    Private Shared Function GetReturnTerm(returnStatement As String) As String
        Const returnKeyword = "return"

        If String.IsNullOrWhiteSpace(returnStatement) Then ThrowMissingReturnStatementException()

        Dim trim = returnStatement.Trim
        If Not CompilerTools.IdentifierEquals(trim.GetStartingIdentifier, returnKeyword) Then ThrowMissingReturnStatementException()

        Return trim.Substring(startIndex:=returnKeyword.Count)
    End Function

    Private Shared Sub ThrowMissingReturnStatementException()
        Throw New CompilerException("Missing return statement.")
    End Sub

End Class
