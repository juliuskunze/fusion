Public Class ConstantAssignment
    Inherits Assignment

    Public Sub New(definition As LocatedString, context As TermContext)
        MyBase.New(definition:=definition, context:=context)
    End Sub

    Public Function GetNamedConstantExpression() As ConstantInstance
        Dim instance = ConstantSignature.FromText(text:=_Declaration, typeContext:=_Context.Types)

        Dim term = New Term(term:=_Term, Type:=instance.Type, Context:=_Context)

        Return New ConstantInstance(Signature:=instance, value:=term.GetDelegate.DynamicInvoke({}))
    End Function

End Class
