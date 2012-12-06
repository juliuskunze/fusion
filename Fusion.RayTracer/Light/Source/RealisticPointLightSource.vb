Public Class RealisticPointLightSource(Of TLight As {ILight(Of TLight), New})
    Inherits PointLightSource(Of TLight)

    Public Sub New(location As Vector3D, baseLightByTime As Func(Of Double, TLight))
        MyBase.New(location, baseLightByTime, brightnessFactorByDistance:=Function(distance) 1 / distance ^ 2)
    End Sub
End Class