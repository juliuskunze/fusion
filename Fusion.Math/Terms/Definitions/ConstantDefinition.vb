Public Class ConstantDefinition
    Inherits Definition

    Public Sub New(definition As String, userContext As TermContext)
        MyBase.New(definition:=definition, userContext:=userContext)
    End Sub

    Public Function GetNamedConstantExpression() As NamedConstant
        Dim signatureDefinition = ConstantSignatureDefinition.FromText(definition:=_Left, types:=_UserContext.Types)

        Dim term = New Term(term:=_Term, Type:=signatureDefinition.Type, userContext:=_UserContext)

        Return New NamedConstant(name:=signatureDefinition.Name, Type:=signatureDefinition.Type, value:=term.GetDelegate.DynamicInvoke({}))
    End Function

End Class
