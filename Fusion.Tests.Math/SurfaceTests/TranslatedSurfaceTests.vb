Public Class TranslatedSurfaceTests

    <Test()>
    Public Sub Test()
        Dim translatedPlane = New TranslatedSurface(New Plane(New Vector3D, New Vector3D(1, 0, 0)), translation:=New Vector3D(3, 0, 0))
        Dim ray = New Ray(New Vector3D(5, 0, 0), New Vector3D(-1, 0, 0))
        Assert.That(translatedPlane.FirstIntersection(ray).Location, [Is].EqualTo(New Vector3D(3, 0, 0)))
    End Sub

End Class
