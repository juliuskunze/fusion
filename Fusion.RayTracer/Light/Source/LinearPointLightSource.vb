Public Class LinearPointLightSource(Of TLight As {ILight(Of TLight), New})
    Inherits PointLightSource(Of TLight)

    Public Sub New(location As Vector3D, baseLightByTime As Func(Of Double, TLight))
        MyBase.New(location, baseLightByTime, brightnessFactorByDistance:=Function(distance) 1 / distance)
    End Sub
End Class