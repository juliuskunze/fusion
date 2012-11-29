<Serializable()>
Public Class Particle2D
    Public Sub New(mass As Double, location As Vector2D)
        Me.New(mass:=mass, Charge:=0, location:=location, Velocity:=Vector2D.Zero, Color:=Drawing.Color.Black)
    End Sub

    Public Sub New(mass As Double, location As Vector2D, velocity As Vector2D)
        Me.New(mass:=mass, Charge:=0, location:=location, velocity:=velocity, Color:=Drawing.Color.Black)
    End Sub

    Public Sub New(mass As Double, location As Vector2D, velocity As Vector2D, color As Color)
        Me.New(mass:=mass, Charge:=0, location:=location, velocity:=velocity, color:=color)
    End Sub

    Public Sub New(mass As Double, charge As Double, location As Vector2D)
        Me.New(mass:=mass, charge:=charge, location:=location, Velocity:=Vector2D.Zero, Color:=Drawing.Color.Black)
    End Sub

    Public Sub New(mass As Double, charge As Double, location As Vector2D, velocity As Vector2D)
        Me.New(mass:=mass, charge:=charge, location:=location, velocity:=velocity, Color:=Drawing.Color.Black)
    End Sub

    Public Sub New(mass As Double, charge As Double, location As Vector2D, velocity As Vector2D, color As Color)
        Me.Mass = mass
        Me.Charge = charge
        Me.Location = location
        Me.Velocity = velocity
        Me.Color = color
    End Sub

    Public Property Mass() As Double
    Public Property Charge() As Double
    Public Property Location() As Vector2D
    Public Property Velocity() As Vector2D

    Public ReadOnly Property Momentum() As Vector2D
        Get
            Return Mass * Velocity
        End Get
    End Property

    Public ReadOnly Property KineticEngergy() As Double
        Get
            Return 0.5 * Mass * Velocity.LengthSquared
        End Get
    End Property

    Public Sub Move(timeSpan As Double)
        Me.Location += timeSpan * Me.Velocity
    End Sub

    Public Sub Accelerate(timeSpan As Double, force As Vector2D)
        Dim acceleration = force / Me.Mass

        Me.Velocity += timeSpan * acceleration
    End Sub

    Public Property Color() As Color

    Public Function Clone() As Particle2D
        Return New Particle2D(Mass:=Me.Mass, Charge:=Me.Charge, Location:=Me.Location, Velocity:=Me.Velocity, Color:=Me.Color)
    End Function
End Class
