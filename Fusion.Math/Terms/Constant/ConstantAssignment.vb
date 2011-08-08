Public Class ConstantAssignment
    Inherits Assignment

    Public Sub New(definition As String, userContext As TermContext)
        MyBase.New(definition:=definition, userContext:=userContext)
    End Sub

    Public Function GetNamedConstantExpression() As ConstExpression
        Dim instance = ConstantDeclaration.FromText(definition:=_Left, types:=_UserContext.Types)

        Dim term = New Term(term:=_Term, Type:=instance.Type, userContext:=_UserContext)

        Return New ConstExpression(instance:=instance, value:=term.GetDelegate.DynamicInvoke({}))
    End Function

End Class
