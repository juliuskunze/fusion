Public Class Definitions

    Private ReadOnly _Definitions As String

    Public Sub New(definitions As String)
        _Definitions = definitions
    End Sub

    Public Function GetTermContext() As TermContext
        Dim definitions = _Definitions.Split(Microsoft.VisualBasic.ControlChars.Cr)
        Dim constants = New List(Of NamedConstantExpression)
        Dim functions = New List(Of NamedFunctionExpression)

        For Each definitionString In definitions
            Dim userContext = New TermContext(constants:=constants, parameters:={}, functions:=functions, types:=NamedType.DefaultTypes)

            Dim definition = New Definition(definition:=definitionString, userContext:=userContext)
            If definition.IsFunctionDefinition Then
                functions.Add(New FunctionDefinition(definition:=definitionString, userContext:=userContext).GetNamedFunctionExpression)
            Else
                constants.Add(New ConstantDefinition(definition:=definitionString, userContext:=userContext).GetNamedConstantExpression)
            End If
        Next

        Return New TermContext(constants:=constants, parameters:={}, functions:=functions, types:=NamedType.DefaultTypes)
    End Function

End Class
