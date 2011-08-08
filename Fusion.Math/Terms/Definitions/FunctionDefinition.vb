Public Class FunctionDefinition
    Inherits Definition

    Public Sub New(definition As String, userContext As TermContext)
        MyBase.New(definition, userContext)
    End Sub

    Public Function GetNamedFunctionExpression() As NamedFunctionExpression
        Dim rest As String = Nothing
        Dim namedFunction = CompilerTools.GetStartingTypedAndNamedVariable(definition:=_Left, types:=_UserContext.Types, out_rest:=rest)
        Dim parameters = CompilerTools.GetArguments(argumentsInBrackets:=rest.TrimEnd).Select(Function(parameterText)
                                                                                                  Dim parameterRest As String = Nothing
                                                                                                  Dim parameter = CompilerTools.GetStartingTypedAndNamedVariable(definition:=parameterText, types:=_UserContext.Types, out_rest:=parameterRest)
                                                                                                  If parameterRest <> "" Then Throw New ArgumentException("End of statement expected.")
                                                                                                  Return New NamedParameter(parameter.Name, parameter.Type)
                                                                                              End Function).ToArray



        Dim term = New Term(term:=_Term, userContext:=_UserContext.Merge(New TermContext(constants:={}, parameters:=parameters, Functions:={}, types:={})), type:=namedFunction.Type)
        Dim lambdaExpression = Expression.Lambda(body:=term.GetExpression, parameters:=parameters.Select(Function(p) p.ParameterExpression))

        Return New NamedFunctionExpression(name:=namedFunction.Name, Type:=namedFunction.Type, parameters:=parameters, lambdaExpression:=lambdaExpression)
    End Function

End Class

