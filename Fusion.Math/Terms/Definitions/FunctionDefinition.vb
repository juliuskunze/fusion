Public Class FunctionDefinition
    Inherits Definition

    Public Sub New(definition As String, userContext As TermContext)
        MyBase.New(definition, userContext)
    End Sub

    Public Function GetNamedFunctionExpression() As NamedFunctionExpression
        Dim typeName = CompilerTools.GetStartingValidVariableName(_Left.TrimStart)
        Dim type = _UserContext.Types.Where(Function(t) t.Name = typeName).Single

        Dim rest = _Left.Substring(startIndex:=typeName.Length).Trim
        Dim functionName = CompilerTools.GetStartingValidVariableName(rest)
        Dim rest2 = rest.Substring(startIndex:=functionName.Length)
        Dim parameters = CompilerTools.GetArguments(argumentsInBrackets:=rest2.TrimStart).Select(Function(name) Expression.Parameter(type:=GetType(Double), name:=name)).ToArray



        Dim term = New Term(term:=_Term, userContext:=_UserContext.Merge(New TermContext(constants:={}, parameters:=parameters, Functions:={}, types:={})))
        Dim lambdaExpression = Expression.Lambda(body:=term.GetExpression, parameters:=parameters)

        Return New NamedFunctionExpression(name:=functionName, type:=type, lambdaExpression:=lambdaExpression)
    End Function

End Class

