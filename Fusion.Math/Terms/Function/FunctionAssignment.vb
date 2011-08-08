Public Class FunctionAssignment
    Inherits Assignment

    Public Sub New(definition As String, userContext As TermContext)
        MyBase.New(definition, userContext)
    End Sub

    Public Function GetNamedFunctionExpression() As FunctionExpression
        Dim signature = FunctionDeclaration.FromText(definition:=_Left, types:=_UserContext.Types)

        Dim term = New Term(term:=_Term, userContext:=_UserContext.Merge(New TermContext(constants:={}, parameters:=signature.FunctionType.Parameters, Functions:={}, types:=NamedTypes.Empty)), Type:=signature.FunctionType)
        Dim constantParameterExpressions = From parameter In signature.FunctionType.Parameters Where parameter.IsConstant Select ParameterExpression = parameter.Expression
        Dim lambdaExpression = Expression.Lambda(body:=term.GetExpression, parameters:=constantParameterExpressions)

        Return New FunctionExpression(name:=signature.Name, Type:=signature.FunctionType, lambdaExpression:=lambdaExpression)
    End Function

    Private _FunctionType As FunctionType
    Public ReadOnly Property GetFunctionType As FunctionType
        Get
            Return _FunctionType
        End Get
    End Property

End Class

