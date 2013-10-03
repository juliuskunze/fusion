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
        _RoughComparer.Equals(v.RotateAroundAxis(axisOrigin:=New Vector3D, axisDirection:=New Vector3D(0, 5, 0), angle:=PI / 2), New Vector3D(0, 0, 4)).Should.Be.True()
    End Sub

    <Test>
    Public Sub RotateAroundAxis_2()
        Dim v = New Vector3D(4, 0, 0)
        _RoughComparer.Equals(v.RotateAroundAxis(axisOrigin:=New Vector3D, axisDirection:=New Vector3D(0, 2, 0), angle:=PI), New Vector3D(-4, 0, 0)).Should.Be.True()
    End Sub

    <Test>
    Public Sub RotateAroundAxis_3()
        Dim anyVector = New Vector3D(-4, 31, -75)
        _RoughComparer.Equals(anyVector.RotateAroundAxis(axisOrigin:=New Vector3D(500, 1000, -27), axisDirection:=New Vector3D(0, 2, 0), angle:=2 * PI), anyVector).Should.Be.True()
    End Sub

    <Test>
    Public Sub RotateAroundAxis_4()
        _RoughComparer.Equals(New Vector3D(0, 0, 0).RotateAroundAxis(axisOrigin:=New Vector3D(1, 0, 0), axisDirection:=New Vector3D(0, 2, 0), angle:=2 * PI), New Vector3D(0, 0, 0)).Should.Be.True()
    End Sub

    <Test>
    Public Sub RotateAroundAxis_5()
        Dim origin  = New Vector3D(-3, 1.5, 0)
        Dim x = New Vector3D(-6, 1.5, 0)
        Dim rotatedX = x.RotateAroundAxis(axisOrigin:=origin, axisDirection:=New Vector3D(0, 1, 0), angle:=6)

        Assert.AreEqual((rotatedX - origin).Length, (x - origin).Length, delta:=0.000000000001)
    End Sub

    <Test>
    Public Sub RotateAroundAxis_6()
        Dim rotated = New Vector3D(-6, 1.5, 0).RotateAroundAxis(axisOrigin:=New Vector3D(-3, 1.5, 0), axisDirection:=New Vector3D(0, 1, 0), angle:=4.7345)
        rotated.Y.Should.Be.EqualTo(1.5)
    End Sub
End Class
