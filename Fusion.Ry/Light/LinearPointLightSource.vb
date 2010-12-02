Public Class LinearPointLightSource
    Implements IPointLightSource

    Public Property Location As Vector3D Implements IPointLightSource.Location
    Public Property ColorAtDistance1 As ExactColor

    Public Sub New(ByVal location As Vector3D, ByVal colorAtDistance1 As Color)
        Me.New(location, New ExactColor(colorAtDistance1))
    End Sub

    Public Sub New(ByVal location As Vector3D, ByVal colorAtDistance1 As ExactColor)
        Me.Location = location
        Me.ColorAtDistance1 = colorAtDistance1
    End Sub

    Public Function LightColor(ByVal surfacePoint As SurfacePoint) As ExactColor Implements IPointLightSource.LightColor
        Dim relativeVector = surfacePoint.Location - Me.Location
        Dim normalizedRelativeVector = relativeVector.Normalized
        Dim distance = relativeVector.Length
        Dim valueFactorByDistance = 1 / distance
        Dim valueFactorByNormal = -surfacePoint.NormalizedNormal.DotProduct(normalizedRelativeVector)
        If valueFactorByNormal <= 0 Then
            Return ExactColor.Black
        End If

        Dim valueFactor = valueFactorByDistance * valueFactorByNormal
        Return Me.ColorAtDistance1 * valueFactor
    End Function
End Class
