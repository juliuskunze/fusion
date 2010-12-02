Public Class PlaneTests

    Private ReadOnly _yzHalfSpace As Plane

    Public Sub New()
        _yzHalfSpace = New Plane(location:=Vector3D.Zero, normal:=New Vector3D(1, 0, 0))
    End Sub

    <Test()>
    Public Sub Constructor_3Points()
        Dim plane = New Plane(Vector3D.Zero,
                              New Vector3D(1, 0, 0),
                              New Vector3D(0, 1, 0))
        Assert.AreEqual(New Vector3D(0, 0, 1), plane.NormalizedNormal)
    End Sub

    <Test()>
    Public Sub FirstIntersection_Vertical()
        Dim verticalRay = New Ray(origin:=New Vector3D(1, 4, 5), direction:=New Vector3D(-1, 0, 0))

        Dim intersection = _yzHalfSpace.Intersection(verticalRay)
        Assert.AreEqual(New Vector3D(0, 4, 5), intersection.Location)
        Assert.AreEqual(New Vector3D(1, 0, 0), intersection.NormalizedNormal)
        SurfaceTests.SurfaceRayIntersection(_yzHalfSpace, verticalRay)
    End Sub

    <Test()>
    Public Sub FirstIntersection_Parallel()
        Dim parallelRay = New Ray(origin:=New Vector3D(1, 0, 0), direction:=New Vector3D(0, 1, 0))

        Dim intersection = _yzHalfSpace.Intersection(parallelRay)
        Assert.AreEqual(intersection, Nothing)
        SurfaceTests.SurfaceRayIntersection(_yzHalfSpace, parallelRay)
    End Sub

    <Test()>
    Public Sub FirstIntersection_WrongDirection()
        Dim wrongDirectionRay = New Ray(origin:=New Vector3D(1, 0, 0), direction:=New Vector3D(1, 0, 0))

        Dim intersection = _yzHalfSpace.Intersection(wrongDirectionRay)
        Assert.AreEqual(intersection, Nothing)
        SurfaceTests.SurfaceRayIntersection(_yzHalfSpace, wrongDirectionRay)
    End Sub

End Class
