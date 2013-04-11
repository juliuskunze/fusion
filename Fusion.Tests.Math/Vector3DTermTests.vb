Public Class Vector3DTests
    Private ReadOnly _RoughComparer As New RoughVector3DComparer(10 ^ -10)

    <Test()>
    Public Sub Length()
        Assert.AreEqual(New Vector3D(1, 2, 2).Length, 3)
    End Sub

    <Test()>
    Public Sub CrossProduct()
        Assert.That(New Vector3D(1, 0, 0).CrossProduct(New Vector3D(0, 1, 0)) = New Vector3D(0, 0, 1))
    End Sub

    <Test()>
    Public Sub OrthogonalProjection()
        Assert.That(New Vector3D(-2, -2, 0).OrthogonalProjectionOn(New Vector3D(1, 0, 0)) = New Vector3D(-2, 0, 0))
    End Sub

    <Test()>
    Public Sub OrthogonalProjection_2()
        Assert.That(New Vector3D(-2, -2, 0).OrthogonalProjectionOn(New Vector3D(1, 0, 0)) = New Vector3D(-2, 0, 0))
    End Sub

    <Test()>
    Public Sub NewFromString()
        Threading.Thread.CurrentThread.CurrentCulture = New Globalization.CultureInfo("en-US")

        Assert.AreEqual(New Vector3D(0, 1, 2), New Vector3D("<0;1;2>"))
        Assert.AreEqual(New Vector3D(0, 1, 2), New Vector3D("<0|1|2>"))
        Assert.AreEqual(New Vector3D(0, 1, 2), New Vector3D("(0|1|2)"))
        Assert.AreEqual(New Vector3D(0, 1, 2), New Vector3D("0|1|2"))
        Assert.AreEqual(New Vector3D(0, 1, 2), New Vector3D("0,1,2"))
        Assert.AreEqual(New Vector3D(0.1, 1.0, 2), New Vector3D("0.1,1.0,2"))
        Assert.AreEqual(New Vector3D(0.1, 1.0, 2), New Vector3D("{0.1,1.0,2}"))
    End Sub

    <Test>
    Public Sub RotateAroundAxis()
        Dim v = New Vector3D(-4, 0, 0)
        _RoughComparer.Equals(v.RotateAroundAxis(axisOrigin:=New Vector3D, axisDirection:=New Vector3D(0, 5, 0), angle:=PI / 2), New Vector3D(0, 0, 4))
    End Sub

    <Test>
    Public Sub RotateAroundAxis_2()
        Dim v = New Vector3D(4, 0, 0)
        _RoughComparer.Equals(v.RotateAroundAxis(axisOrigin:=New Vector3D, axisDirection:=New Vector3D(0, 2, 0), angle:=PI), New Vector3D(-4, 0, 0))
    End Sub
End Class
