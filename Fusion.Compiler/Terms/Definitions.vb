Public Class Definitions

    Private ReadOnly _Definitions As String

    Private ReadOnly _BaseContext As TermContext
    Public ReadOnly Property BaseContext As TermContext
        Get
            Return _BaseContext
        End Get
    End Property

    Public Sub New(definitions As String, baseContext As TermContext)
        _Definitions = definitions
        _BaseContext = baseContext
    End Sub

    Public Function GetTermContext() As TermContext
        Dim definitions = _Definitions.Split(Microsoft.VisualBasic.ControlChars.Cr)

        Dim context = _BaseContext

        For Each definitionString In definitions
            Dim definition = New Assignment(definition:=definitionString, context:=context)
            If definition.IsFunctionAssignment Then
                context = context.Merge(New TermContext(Functions:={New FunctionAssignment(definition:=definitionString, context:=context).GetFunctionInstance}))
            Else
                context = context.Merge(New TermContext(constants:={New ConstantAssignment(definition:=definitionString, context:=context).GetNamedConstantExpression}))
            End If
        Next

        Return context
    End Function

End Class
