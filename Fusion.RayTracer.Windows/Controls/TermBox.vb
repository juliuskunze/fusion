Public Class TermBox
    Inherits ValidatingTextBoxBase(Of Double?)
    
    Protected Overrides Function Convert(ByVal text As String) As Double?
        Try
            Return New IndependentTerm(text).GetResult
        Catch ex As InvalidTermException
            Return Nothing
        End Try
    End Function

End Class
