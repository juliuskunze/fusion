Public Class NamedConstantDefinition
    Inherits DefinitionBase

    Public Sub New(definition As String, userContext As TermContext)
        MyBase.New(definition:=definition, userContext:=userContext)
    End Sub

    Public Function GetNamedConstantExpression() As NamedConstantExpression
        Dim parts = _DefinitionWithoutBlanks.Split("="c)
        If parts.Count <> 2 Then Throw New InvalidTermException("Definition invalid.")

        Dim name = parts.First
        Dim term = New Term(term:=parts.Last, userContext:=_UserContext)

        If Not name.IsValidVariableName Then Throw New ArgumentException("""" & name & """ is not a valid constant name.")

        Return New NamedConstantExpression(name:=name, value:=term.GetDelegate.DynamicInvoke({}))
    End Function

End Class
