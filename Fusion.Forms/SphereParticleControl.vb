Public Class SphereParticleControl
    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal particle As SphereParticle2D)
        InitializeComponent()

        Me.Particle = particle
    End Sub

    Private Sub btnChangeColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeColor.Click
        If cldColor.ShowDialog = DialogResult.OK Then
            pbxColor.BackColor = cldColor.Color
        End If
    End Sub

    <System.ComponentModel.Browsable(False)> Public Property Particle As SphereParticle2D
        Get
            Return New SphereParticle2D(mass:=vbxMass.Value, charge:=vbxCharge.Value, Location:=v2bLocation.Vector, velocity:=v2bVelocity.Vector, radius:=vbxRadius.Value, Color:=pbxColor.BackColor)
        End Get
        Set(ByVal value As SphereParticle2D)
            vbxMass.Value = value.Mass
            vbxCharge.Value = value.Charge
            v2bLocation.Vector = value.Location
            v2bVelocity.Vector = value.Velocity
            vbxRadius.Value = value.Radius
            pbxColor.BackColor = value.Color
        End Set
    End Property

End Class
