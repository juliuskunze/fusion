<Serializable()>
Public Class SphereParticle3D
    Inherits Particle3D

    Public Sub New(ByVal mass As Double, ByVal location As Vector3D, ByVal radius As Double)
        Me.New(mass:=mass, Charge:=0, location:=location, Velocity:=Vector3D.Zero, Color:=Drawing.Color.Black, radius:=radius)
    End Sub

    Public Sub New(ByVal mass As Double, ByVal location As Vector3D, ByVal velocity As Vector3D, ByVal radius As Double)
        Me.New(mass:=mass, Charge:=0, location:=location, velocity:=velocity, Color:=Drawing.Color.Black, radius:=radius)
    End Sub

    Public Sub New(ByVal mass As Double, ByVal location As Vector3D, ByVal velocity As Vector3D, ByVal color As Color, ByVal radius As Double)
        Me.New(mass:=mass, Charge:=0, location:=location, velocity:=velocity, color:=color, radius:=radius)
    End Sub

    Public Sub New(ByVal mass As Double, ByVal charge As Double, ByVal location As Vector3D, ByVal radius As Double)
        Me.New(mass:=mass, charge:=charge, location:=location, Velocity:=Vector3D.Zero, Color:=Drawing.Color.Black, radius:=radius)
    End Sub

    Public Sub New(ByVal mass As Double, ByVal charge As Double, ByVal location As Vector3D, ByVal velocity As Vector3D, ByVal radius As Double)
        Me.New(mass:=mass, charge:=charge, location:=location, velocity:=velocity, Color:=Drawing.Color.Black, radius:=radius)
    End Sub

    Public Sub New(ByVal mass As Double, ByVal charge As Double, ByVal location As Vector3D, ByVal velocity As Vector3D, ByVal color As Color, ByVal radius As Double)
        MyBase.New(mass:=mass, charge:=charge, location:=location, velocity:=velocity, color:=color)
        Me.Radius = radius
    End Sub

    Public Sub New(ByVal particle As Particle3D, ByVal radius As Double)
        MyBase.New(Mass:=particle.Mass, Charge:=particle.Charge, Location:=particle.Location, Velocity:=particle.Velocity, Color:=particle.Color)
        Me.Radius = radius
    End Sub

    Private _radius As Double
    Public Property Radius() As Double
        Get
            Return _radius
        End Get
        Set(ByVal value As Double)
            _radius = value
        End Set
    End Property

    Public Property Volume() As Double
        Get
            Return Geometry.SphereVolume(Me.Radius)
        End Get
        Set(ByVal value As Double)
            Me.Radius = Geometry.SphereRadiusFromVolume(value)
        End Set
    End Property

    Public Property Density() As Double
        Get
            Return Me.Mass / Me.Volume
        End Get
        Set(ByVal value As Double)
            Me.Volume = Me.Mass / value
        End Set
    End Property

End Class

