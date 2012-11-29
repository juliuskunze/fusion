Public Class RectangleTests
    <Test()>
    Public Sub FirstIntersection()
        Dim ray = New Ray(origin:=New Vector3D(2.5, 2.5, 0), direction:=New Vector3D(0, 0, 1))
        Dim ray2 = New Ray(origin:=New Vector3D(1.2, 1.2, 0), direction:=New Vector3D(0, 0, 1))

        Dim rectangle = New Fusion.Math.Rectangle(vertex1:=New Vector3D(3, 1, 1), vertex2:=New Vector3D(1, 1, 1), vertex3:=New Vector3D(1, 3, 1))

        Dim firstIntersecton = rectangle.FirstIntersection(ray)
        Dim firstIntersecton2 = rectangle.FirstIntersection(ray2)

        Assert.AreEqual(New Vector3D(2.5, 2.5, 1), firstIntersecton.Location)
        Assert.AreEqual(New Vector3D(1.2, 1.2, 1), firstIntersecton2.Location)
        Assert.AreEqual(New Vector3D(0, 0, -1), firstIntersecton.NormalizedNormal)
    End Sub

    <Explicit()> <Test()>
    Public Sub FirstIntersectionMidPoint()
        Dim ray = New Ray(origin:=New Vector3D(2, 2, 0), direction:=New Vector3D(0, 0, 1))

        Dim rectangle = New Fusion.Math.Rectangle(vertex2:=New Vector3D(1, 1, 1), vertex3:=New Vector3D(1, 3, 1), vertex1:=New Vector3D(3, 1, 1))

        Dim firstIntersecton = rectangle.FirstIntersection(ray)

        Assert.AreEqual(New Vector3D(2, 2, 1), firstIntersecton.Location)
        Assert.AreEqual(New Vector3D(0, 0, -1), firstIntersecton.NormalizedNormal)
    End Sub

End Class
