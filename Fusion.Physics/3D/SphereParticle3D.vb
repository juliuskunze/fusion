<Serializable()>
Public Class SphereParticle3D
    Inherits Particle3D

    Public Sub New(mass As Double, location As Vector3D, radius As Double)
        Me.New(mass:=mass, Charge:=0, location:=location, Velocity:=Vector3D.Zero, Color:=Drawing.Color.Black, radius:=radius)
    End Sub

    Public Sub New(mass As Double, location As Vector3D, velocity As Vector3D, radius As Double)
        Me.New(mass:=mass, Charge:=0, location:=location, velocity:=velocity, Color:=Drawing.Color.Black, radius:=radius)
    End Sub

    Public Sub New(mass As Double, location As Vector3D, velocity As Vector3D, color As Color, radius As Double)
        Me.New(mass:=mass, Charge:=0, location:=location, velocity:=velocity, color:=color, radius:=radius)
    End Sub

    Public Sub New(mass As Double, charge As Double, location As Vector3D, radius As Double)
        Me.New(mass:=mass, charge:=charge, location:=location, Velocity:=Vector3D.Zero, Color:=Drawing.Color.Black, radius:=radius)
    End Sub

    Public Sub New(mass As Double, charge As Double, location As Vector3D, velocity As Vector3D, radius As Double)
        Me.New(mass:=mass, charge:=charge, location:=location, velocity:=velocity, Color:=Drawing.Color.Black, radius:=radius)
    End Sub

    Public Sub New(mass As Double, charge As Double, location As Vector3D, velocity As Vector3D, color As Color, radius As Double)
        MyBase.New(mass:=mass, charge:=charge, location:=location, velocity:=velocity, color:=color)
        Me.Radius = radius
    End Sub

    Public Sub New(particle As Particle3D, radius As Double)
        MyBase.New(Mass:=particle.Mass, Charge:=particle.Charge, Location:=particle.Location, Velocity:=particle.Velocity, Color:=particle.Color)
        Me.Radius = radius
    End Sub

    Private _Radius As Double
    Public Property Radius() As Double
        Get
            Return _Radius
        End Get
        Set(value As Double)
            _Radius = value
        End Set
    End Property

    Public Property Volume() As Double
        Get
            Return Geometry.SphereVolume(Me.Radius)
        End Get
        Set(value As Double)
            Me.Radius = Geometry.SphereRadiusFromVolume(value)
        End Set
    End Property

    Public Property Density() As Double
        Get
            Return Me.Mass / Me.Volume
        End Get
        Set(value As Double)
            Me.Volume = Me.Mass / value
        End Set
    End Property

End Class

