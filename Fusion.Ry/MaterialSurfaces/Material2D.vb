Public Class Material2D(Of TLight)

    Public Sub New(ByVal sourceLight As TLight,
                   ByVal scatteringRemission As IRemission(Of TLight),
                   ByVal reflectionRemission As IRemission(Of TLight),
                   ByVal transparencyRemission As IRemission(Of TLight),
                   Optional ByVal refractionIndexQuotient As Double = 1)
        Me.SourceLight = sourceLight
        Me.ScatteringRemission = scatteringRemission
        Me.ReflectionRemission = reflectionRemission
        Me.TransparencyRemission = transparencyRemission
        Me.RefractionIndexQuotient = refractionIndexQuotient
    End Sub

    Public Property RefractionIndexQuotient As Double
    Public ReadOnly Property Refracts As Boolean
        Get
            Return Me.IsTranslucent AndAlso Me.RefractionIndexQuotient <> 1
        End Get
    End Property

    Public Property SourceLight As TLight

    Public Property ReflectionRemission As IRemission(Of TLight)
    Public ReadOnly Property Reflects As Boolean
        Get
            Return Not Me.ReflectionRemission.NoRemission
        End Get
    End Property

    Public Property TransparencyRemission As IRemission(Of TLight)
    Public ReadOnly Property IsTranslucent As Boolean
        Get
            Return Not Me.TransparencyRemission.NoRemission
        End Get
    End Property

    Public Property ScatteringRemission As IRemission(Of TLight)
    Public ReadOnly Property Scatters As Boolean
        Get
            Return Not Me.ScatteringRemission.NoRemission
        End Get
    End Property

    Public Function Clone() As Material2D(Of TLight)
        Return New Material2D(Of TLight)(SourceLight:=Me.SourceLight,
                                         ScatteringRemission:=Me.ScatteringRemission,
                                         ReflectionRemission:=Me.ReflectionRemission,
                                         TransparencyRemission:=Me.TransparencyRemission,
                                         refractionIndexQuotient:=Me.RefractionIndexQuotient)
    End Function

End Class
