Public Class SphereParticle2DDialog
    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal particle As SphereParticle2D)
        InitializeComponent()

        sphereParticleControl.Particle = particle
    End Sub

    Public Property Particle As SphereParticle2D
        Get
            Return sphereParticleControl.Particle
        End Get
        Set(ByVal value As SphereParticle2D)
            sphereParticleControl.Particle = Particle
        End Set
    End Property

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
End Class