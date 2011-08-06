Public Class Definitions

    Private ReadOnly _Definitions As String

    Public Sub New(definitions As String)
        _Definitions = definitions
    End Sub

    Public Function GetTermContext() As TermContext
        Dim definitions = _Definitions.Split(Microsoft.VisualBasic.ControlChars.Cr)
        Dim constants = New List(Of NamedConstantExpression)
        Dim functions = New List(Of NamedFunctionExpression)

        For Each definition In definitions
            Dim userContext = New TermContext(constants:=constants, parameters:={}, functions:=functions)

            Dim constantDefinition = New ConstantDefinition(definition:=definition, userContext:=userContext)
            Try
                constants.Add(constantDefinition.GetNamedConstantExpression)
            Catch ex As ArgumentException
                If constantDefinition.NamePart.IsValidVariableName Then Throw

                functions.Add(New FunctionDefinition(definition:=definition, userContext:=userContext).GetNamedFunctionExpression)
            End Try
        Next

        Return New TermContext(constants:=constants, parameters:={}, functions:=functions)
    End Function

End Class
