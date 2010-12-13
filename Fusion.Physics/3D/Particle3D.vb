﻿<Serializable()>
Public Class Particle3D
    Public Sub New(ByVal mass As Double, ByVal location As Vector3D)
        Me.New(mass:=mass, Charge:=0, location:=location, Velocity:=Vector3D.Zero, Color:=Drawing.Color.Black)
    End Sub

    Public Sub New(ByVal mass As Double, ByVal location As Vector3D, ByVal velocity As Vector3D)
        Me.New(mass:=mass, Charge:=0, location:=location, velocity:=velocity, Color:=Drawing.Color.Black)
    End Sub

    Public Sub New(ByVal mass As Double, ByVal location As Vector3D, ByVal velocity As Vector3D, ByVal color As Color)
        Me.New(mass:=mass, Charge:=0, location:=location, velocity:=velocity, color:=color)
    End Sub

    Public Sub New(ByVal mass As Double, ByVal charge As Double, ByVal location As Vector3D)
        Me.New(mass:=mass, charge:=charge, location:=location, Velocity:=Vector3D.Zero, Color:=Drawing.Color.Black)
    End Sub

    Public Sub New(ByVal mass As Double, ByVal charge As Double, ByVal location As Vector3D, ByVal velocity As Vector3D)
        Me.New(mass:=mass, charge:=charge, location:=location, velocity:=velocity, Color:=Drawing.Color.Black)
    End Sub

    Public Sub New(ByVal mass As Double, ByVal charge As Double, ByVal location As Vector3D, ByVal velocity As Vector3D, ByVal color As Color)
        Me.Mass = mass
        Me.Charge = charge
        Me.Location = location
        Me.Velocity = velocity
        Me.Color = color
    End Sub

    Private _mass As Double
    Public Property Mass() As Double
        Get
            Return _mass
        End Get
        Set(ByVal value As Double)
            _mass = value
        End Set
    End Property

    Private _charge As Double
    Public Property Charge() As Double
        Get
            Return _charge
        End Get
        Set(ByVal value As Double)
            _charge = value
        End Set
    End Property

    Private _location As Vector3D
    Public Property Location() As Vector3D
        Get
            Return _location
        End Get
        Set(ByVal value As Vector3D)
            _location = value
        End Set
    End Property

    Private _velocity As Vector3D
    Public Property Velocity() As Vector3D
        Get
            Return _velocity
        End Get
        Set(ByVal value As Vector3D)
            _velocity = value
        End Set
    End Property

    Public Shared Function Distance(ByVal p1 As Particle3D, ByVal p2 As Particle3D) As Double
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

    Public Sub Move(ByVal timeSpan As Double)
        Me.Location += timeSpan * Me.Velocity
    End Sub

    Public Sub Accelerate(ByVal timeSpan As Double, ByVal force As Vector3D)
        Dim acceleration = force / Me.Mass

        Me.Velocity += timeSpan * acceleration
    End Sub

    Private _color As Color
    Public Property Color() As Color
        Get
            Return _color
        End Get
        Set(ByVal value As Color)
            _color = value
        End Set
    End Property
End Class