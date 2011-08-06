Public Class FunctionDefinition
    Inherits DefinitionBase

    Public Sub New(definition As String, userContext As TermContext)
        MyBase.New(WithoutBlanks(definition), userContext)
    End Sub

    Public Function GetNamedFunctionExpression() As NamedFunctionExpression
        Dim functionNameDefinition = New FunctionCall(functionCallText:=_NamePart)
        Dim parameters = functionNameDefinition.Arguments.Select(Function(name) Expression.Parameter(type:=GetType(Double), name:=name)).ToArray

        Dim term = New Term(term:=_Term, userContext:=_UserContext.Merge(New TermContext(constants:={}, parameters:=parameters, Functions:={})))
        Dim lambdaFunction = Expression.Lambda(body:=term.GetExpression, parameters:=parameters)

        Return New NamedFunctionExpression(name:=functionNameDefinition.FunctionName, expressionBuilder:=Function(arguments) Expression.Invoke(lambdaFunction, arguments:=arguments))
    End Function

End Class

