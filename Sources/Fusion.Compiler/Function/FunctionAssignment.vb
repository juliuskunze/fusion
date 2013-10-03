Public Class FunctionAssignment
    Inherits Assignment

    Public Sub New(definition As LocatedString, context As TermContext)
        MyBase.New(definition, context)
    End Sub

    Public Function GetFunctionInstance() As FunctionInstance
        Dim signature = FunctionSignature.FromString(s:=_SignatureString, typeContext:=_Context.Types)

        Dim parameterExpressions = From parameter In signature.FunctionType.Parameters Select parameter.Expression

        Dim term = New Term(term:=_TermString,
                            context:=_Context.Merge(New TermContext(parameters:=signature.FunctionType.Parameters)),
                            Type:=signature.FunctionType.ResultType)
        
        Dim lambdaExpression = Expression.Lambda(body:=term.GetExpression, parameters:=parameterExpressions)

        Return New FunctionInstance(signature:=signature, invokableExpression:=lambdaExpression)
    End Function
End Class

