Public Class ParameterExpressionReplacer
    Inherits ExpressionVisitor
    
    Private ReadOnly _OldParameter As ParameterExpression
    Private ReadOnly _NewParameter As ParameterExpression

    Public Sub New(ByVal oldParameter As ParameterExpression, ByVal newParameter As ParameterExpression)
        _OldParameter = oldParameter
        _NewParameter = newParameter
    End Sub

    Public Overrides Function Visit(ByVal node As System.Linq.Expressions.Expression) As System.Linq.Expressions.Expression
        Dim parameter = TryCast(node, ParameterExpression)
        If parameter IsNot Nothing AndAlso parameter.Name = _OldParameter.Name Then Return _NewParameter

        Return MyBase.Visit(node)
    End Function


End Class
