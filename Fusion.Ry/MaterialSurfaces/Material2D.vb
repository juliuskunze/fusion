Public Class Material2D

    Public Sub New(Optional ByVal refractionIndexQuotient As Double = 1)
        Me.New(Color.Black, New BlackColorRemission, New BlackColorRemission, New BlackColorRemission, refractionIndexQuotient)
    End Sub

    Public Sub New(ByVal lightSourceColor As Color, Optional ByVal refractionIndexQuotient As Double = 1)
        Me.New(lightSourceColor, New BlackColorRemission, New BlackColorRemission, New BlackColorRemission, refractionIndexQuotient)
    End Sub

    Public Sub New(ByVal lightSourceColor As ExactColor, Optional ByVal refractionIndexQuotient As Double = 1)
        Me.New(lightSourceColor, New BlackColorRemission, New BlackColorRemission, New BlackColorRemission, refractionIndexQuotient)
    End Sub

    Public Sub New(ByVal scatteringRemission As IColorRemission,
                   ByVal reflectionRemission As IColorRemission,
                   ByVal transparencyRemission As IColorRemission,
                   Optional ByVal refractionIndexQuotient As Double = 1)
        Me.New(ExactColor.Black, scatteringRemission, reflectionRemission, transparencyRemission, refractionIndexQuotient)
    End Sub

    Public Sub New(ByVal lightSourceColor As Color,
                   ByVal scatteringRemission As IColorRemission,
                   ByVal reflectionRemission As IColorRemission,
                   ByVal transparencyRemission As IColorRemission,
               Optional ByVal refractionIndexQuotient As Double = 1)
        Me.New(New ExactColor(lightSourceColor), scatteringRemission, reflectionRemission, transparencyRemission, refractionIndexQuotient)
    End Sub

    Public Sub New(ByVal lightSourceColor As ExactColor,
                   ByVal scatteringRemission As IColorRemission,
                   ByVal reflectionRemission As IColorRemission,
                   ByVal transparencyRemission As IColorRemission,
                   Optional ByVal refractionIndexQuotient As Double = 1)
        Me.LightSourceColor = lightSourceColor
        Me.ScatteringRemission = scatteringRemission
        Me.ReflectionRemission = reflectionRemission
        Me.TransparencyRemission = transparencyRemission
        Me.RefractionIndexRatio = refractionIndexQuotient
    End Sub

    Public Property RefractionIndexRatio As Double
    Public ReadOnly Property Refracts As Boolean
        Get
            Return Me.IsTranslucent AndAlso Me.RefractionIndexRatio <> 1
        End Get
    End Property

    Public Property LightSourceColor As ExactColor
    Public ReadOnly Property IsLightSource As Boolean
        Get
            Return Me.LightSourceColor <> ExactColor.Black
        End Get
    End Property

    Public Property ReflectionRemission As IColorRemission
    Public ReadOnly Property Reflects As Boolean
        Get
            Return Not Me.ReflectionRemission.NoRemission
        End Get
    End Property

    Public Property TransparencyRemission As IColorRemission
    Public ReadOnly Property IsTranslucent As Boolean
        Get
            Return Not Me.TransparencyRemission.NoRemission
        End Get
    End Property

    Public Property ScatteringRemission As IColorRemission
    Public ReadOnly Property Scatters As Boolean
        Get
            Return Not Me.ScatteringRemission.NoRemission
        End Get
    End Property

    Public Function Clone() As Material2D
        Return New Material2D(Me.LightSourceColor,
                              Me.ScatteringRemission,
                              Me.ReflectionRemission,
                              Me.TransparencyRemission,
                              Me.RefractionIndexRatio)
    End Function

End Class
