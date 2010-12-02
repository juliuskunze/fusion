Public Class ValueBox
    <System.ComponentModel.Browsable(True)> Public Property Value As Double
        Get
            Return CDbl(Me.Txt.Text)
        End Get
        Set(ByVal value As Double)
            Me.Txt.Text = value.ToString
        End Set
    End Property
End Class
