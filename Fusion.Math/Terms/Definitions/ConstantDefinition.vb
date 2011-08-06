Public Class ConstantDefinition
    Inherits DefinitionBase

    Public Sub New(definition As String, userContext As TermContext)
        MyBase.New(definition:=definition, userContext:=userContext)
    End Sub

    Public Function GetNamedConstantExpression() As NamedConstantExpression
        Dim term = New Term(term:=_Term, userContext:=_UserContext)

        If Not _NamePart.IsValidVariableName Then Throw New ArgumentException("""" & _NamePart & """ is not a valid constant name.")

        Return New NamedConstantExpression(name:=_NamePart, value:=term.GetDelegate.DynamicInvoke({}))
    End Function

End Class
