<Serializable()>
Public Class Particle3D
    Public Sub New(mass As Double, location As Vector3D)
        Me.New(mass:=mass, Charge:=0, location:=location, Velocity:=Vector3D.Zero, Color:=Drawing.Color.Black)
    End Sub

    Public Sub New(mass As Double, location As Vector3D, velocity As Vector3D)
        Me.New(mass:=mass, Charge:=0, location:=location, velocity:=velocity, Color:=Drawing.Color.Black)
    End Sub

    Public Sub New(mass As Double, location As Vector3D, velocity As Vector3D, color As Color)
        Me.New(mass:=mass, Charge:=0, location:=location, velocity:=velocity, color:=color)
    End Sub

    Public Sub New(mass As Double, charge As Double, location As Vector3D)
        Me.New(mass:=mass, charge:=charge, location:=location, Velocity:=Vector3D.Zero, Color:=Drawing.Color.Black)
    End Sub

    Public Sub New(mass As Double, charge As Double, location As Vector3D, velocity As Vector3D)
        Me.New(mass:=mass, charge:=charge, location:=location, velocity:=velocity, Color:=Drawing.Color.Black)
    End Sub

    Public Sub New(mass As Double, charge As Double, location As Vector3D, velocity As Vector3D, color As Color)
        Me.Mass = mass
        Me.Charge = charge
        Me.Location = location
        Me.Velocity = velocity
        Me.Color = color
    End Sub

    Private _Mass As Double
    Public Property Mass() As Double
        Get
            Return _Mass
        End Get
        Set(value As Double)
            _Mass = value
        End Set
    End Property

    Private _Charge As Double
    Public Property Charge() As Double
        Get
            Return _Charge
        End Get
        Set(value As Double)
            _Charge = value
        End Set
    End Property

    Private _Location As Vector3D
    Public Property Location() As Vector3D
        Get
            Return _Location
        End Get
        Set(value As Vector3D)
            _Location = value
        End Set
    End Property

    Private _Velocity As Vector3D
    Public Property Velocity() As Vector3D
        Get
            Return _Velocity
        End Get
        Set(value As Vector3D)
            _Velocity = value
        End Set
    End Property

    Public Shared Function Distance(p1 As Particle3D, p2 As Particle3D) As Double
        Return (p1.Location - p2.Location).Length
    End Function

    Public ReadOnly Property Momentum() As Vector3D
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

    Public Sub Accelerate(timeSpan As Double, force As Vector3D)
        Dim acceleration = force / Me.Mass

        Me.Velocity += timeSpan * acceleration
    End Sub

    Private _Color As Color
    Public Property Color() As Color
        Get
            Return _Color
        End Get
        Set(value As Color)
            _Color = value
        End Set
    End Property
End Class
