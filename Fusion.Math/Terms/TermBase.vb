Public MustInherit Class TermBase

    Protected ReadOnly _Term As String
    Protected ReadOnly _Context As TermContext

    Public Sub New(ByVal term As String, ByVal context As TermContext)
        _Term = term
        _Context = context
    End Sub

    Public MustOverride Function GetExpression() As Expression

    Public Function GetDelegate() As System.Delegate
        Return Expression.Lambda(body:=Me.GetExpression, parameters:=_Context.Parameters).Compile
    End Function

    Public Function GetDelegate(Of TDelegate)() As TDelegate
        Dim lambda = Expression.Lambda(Of TDelegate)(body:=Me.GetExpression, parameters:=_Context.Parameters)

        Return lambda.Compile
    End Function

    Public Function TryGetDelegate() As System.Delegate
        Try
            Return Me.GetDelegate
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function TryGetDelegate(Of TDelegate)() As TDelegate
        Try
            Return Me.GetDelegate(Of TDelegate)()
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class
