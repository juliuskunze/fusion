Public Class IntegerTermBox
    Inherits ValidatingTextBoxBase(Of Integer?)
    
    Protected Overrides Function Convert(text As String) As Integer?
        Try
            Return CInt(New IndependentTerm(Of Double)(Me.Text).GetResult)
        Catch ex As InvalidTermException
            Return Nothing
        End Try
    End Function

End Class
