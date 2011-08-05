Public Class IntegerTermBox
    Inherits ValidatingTextBoxBase(Of Integer?)
    
    Protected Overrides Function Convert(ByVal text As String) As Integer?
        Try
            Return CInt(New IndependentTerm(Me.Text).GetResult)
        Catch ex As InvalidTermException
            Return Nothing
        End Try
    End Function
End Class
