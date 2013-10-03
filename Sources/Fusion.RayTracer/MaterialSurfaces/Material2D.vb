Public Class Material2D(Of TLight)

    Public Sub New(sourceLight As TLight,
                   scatteringRemission As IRemission(Of TLight),
                   reflectionRemission As IRemission(Of TLight),
                   transparencyRemission As IRemission(Of TLight),
                   Optional refractionIndexQuotient As Double = 1)
        Me.SourceLight = sourceLight
        Me.ScatteringRemission = scatteringRemission
        Me.ReflectionRemission = reflectionRemission
        Me.TransparencyRemission = transparencyRemission
        Me.RefractionIndexQuotient = refractionIndexQuotient
    End Sub

    Public Property RefractionIndexQuotient As Double
    Public ReadOnly Property Refracts As Boolean
        Get
            Return IsTranslucent AndAlso RefractionIndexQuotient <> 1
        End Get
    End Property

    Public Property SourceLight As TLight

    Public Property ReflectionRemission As IRemission(Of TLight)
    Public ReadOnly Property Reflects As Boolean
        Get
            Return Not ReflectionRemission.IsBlack
        End Get
    End Property

    Public Property TransparencyRemission As IRemission(Of TLight)
    Public ReadOnly Property IsTranslucent As Boolean
        Get
            Return Not TransparencyRemission.IsBlack
        End Get
    End Property

    Public Property ScatteringRemission As IRemission(Of TLight)
    Public ReadOnly Property Scatters As Boolean
        Get
            Return Not ScatteringRemission.IsBlack
        End Get
    End Property

    Public Function Clone() As Material2D(Of TLight)
        Return New Material2D(Of TLight)(SourceLight:=SourceLight,
                                         ScatteringRemission:=ScatteringRemission,
                                         ReflectionRemission:=ReflectionRemission,
                                         TransparencyRemission:=TransparencyRemission,
                                         refractionIndexQuotient:=RefractionIndexQuotient)
    End Function
End Class
