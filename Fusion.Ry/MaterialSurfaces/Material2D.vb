Public Class Material2D

    Public Sub New(ByVal lightSourceColor As Color,
                   ByVal scatteringRemission As ILightRemission(Of ExactColor),
                   ByVal reflectionRemission As ILightRemission(Of ExactColor),
                   ByVal transparencyRemission As ILightRemission(Of ExactColor),
                   Optional ByVal refractionIndexQuotient As Double = 1)
        Me.New(New ExactColor(lightSourceColor), scatteringRemission, reflectionRemission, transparencyRemission, refractionIndexQuotient)
    End Sub

    Public Sub New(ByVal sourceLight As ExactColor,
                   ByVal scatteringRemission As ILightRemission(Of ExactColor),
                   ByVal reflectionRemission As ILightRemission(Of ExactColor),
                   ByVal transparencyRemission As ILightRemission(Of ExactColor),
                   Optional ByVal refractionIndexQuotient As Double = 1)
        Me.SourceLight =  SourceLight
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

    Public Property SourceLight As ExactColor

    Public Property ReflectionRemission As ILightRemission(Of ExactColor)
    Public ReadOnly Property Reflects As Boolean
        Get
            Return Not Me.ReflectionRemission.NoRemission
        End Get
    End Property

    Public Property TransparencyRemission As ILightRemission(Of ExactColor)
    Public ReadOnly Property IsTranslucent As Boolean
        Get
            Return Not Me.TransparencyRemission.NoRemission
        End Get
    End Property

    Public Property ScatteringRemission As ILightRemission(Of ExactColor)
    Public ReadOnly Property Scatters As Boolean
        Get
            Return Not Me.ScatteringRemission.NoRemission
        End Get
    End Property

    Public Function Clone() As Material2D
        Return New Material2D(Me.SourceLight,
                              Me.ScatteringRemission,
                              Me.ReflectionRemission,
                              Me.TransparencyRemission,
                              Me.RefractionIndexRatio)
    End Function

End Class
