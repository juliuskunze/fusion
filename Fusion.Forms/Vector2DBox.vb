Public Class Vector2DBox
    <System.ComponentModel.Browsable(False)> Public Property Vector As Vector2D
        Get
            Return New Vector2D(Me.Txt.Text)
        End Get
        Set(ByVal value As Vector2D)
            Me.Txt.Text = value.ToString
        End Set
    End Property
End Class
