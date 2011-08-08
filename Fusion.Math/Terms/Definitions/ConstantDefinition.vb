Public Class ConstantDefinition
    Inherits Definition

    Public Sub New(definition As String, userContext As TermContext)
        MyBase.New(definition:=definition, userContext:=userContext)
    End Sub

    Public Function GetNamedConstantExpression() As NamedConstantExpression
        Dim typeName = CompilerTools.GetStartingValidVariableName(_Left.TrimStart)
        Dim type = _UserContext.Types.Parse(typeName)

        Dim rest = _Left.Substring(startIndex:=typeName.Length)
        Dim constantName = CompilerTools.GetStartingValidVariableName(rest.TrimStart)
        'If Not constantName.IsValidVariableName Then Throw New ArgumentException("""" & _Left & """ is not a valid constant name.")
        
        Dim term = New Term(term:=_Term, type:=type, userContext:=_UserContext)

        Return New NamedConstantExpression(name:=constantName, type:=type, value:=term.GetDelegate.DynamicInvoke({}))
    End Function

End Class
