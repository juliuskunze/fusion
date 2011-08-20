Public Class FunctionAssignment
    Inherits Assignment

    Public Sub New(definition As String, context As TermContext)
        MyBase.New(definition, context)
    End Sub

    Public Function GetFunctionInstance() As FunctionInstance
        Dim signature = FunctionSignature.FromText(text:=_Declaration, typeContext:=_Context.Types)

        Dim parameterExpressions = From parameter In signature.DelegateType.Parameters Select parameter.Expression

        Dim term = New Term(term:=_Term,
                            context:=_Context.Merge(New TermContext(parameters:=signature.DelegateType.Parameters)),
                            Type:=signature.DelegateType.ResultType)
        
        Dim lambdaExpression = Expression.Lambda(body:=term.GetExpression, parameters:=parameterExpressions)

        Return New FunctionInstance(name:=signature.Name, delegateType:=signature.DelegateType, invokableExpression:=lambdaExpression)
    End Function

End Class

