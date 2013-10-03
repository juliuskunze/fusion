''' <summary>
''' A PointLightSource where brightness does not depend on the distance. 
''' </summary>
Public Class ConstantPointLightSource(Of TLight As {ILight(Of TLight), New})
    Inherits PointLightSource(Of TLight)

    Public Sub New(location As Vector3D, baseLightByTime As Func(Of Double, TLight))
        MyBase.New(location, baseLightByTime, brightnessFactorByDistance:=Function() 1)
    End Sub
End Class