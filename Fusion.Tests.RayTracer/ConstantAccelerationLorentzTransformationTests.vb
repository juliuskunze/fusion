Public Class ConstantAccelerationLorentzTransformationTests
    <Test>
    Public Sub Test_Rocket()
        Dim acceleration = New Vector3D(9.81, 0, 0)
        Const rocketTime = 8 * Year
        Const earthTime = 1840 * Year
        Const distance = 1839 * LightYear
        Const gamma = 1895

        Dim accelerationTransformation = New ConstantAccelerationLorentzTransformation(acceleration:=acceleration)

        Dim transformation = accelerationTransformation.GetConstantVelocityTransformationAtTime(acceleratedFrameTime:=rocketTime)

        Dim rocketEvent = transformation.Inverse.TransformEvent(New SpaceTimeEvent(time:=rocketTime))
        Assert.AreEqual(rocketEvent.Time / Year, earthTime / Year, delta:=50)

        rocketEvent.Location.Y.Should.Be.EqualTo(0)
        rocketEvent.Location.Z.Should.Be.EqualTo(0)
        Assert.AreEqual(rocketEvent.Location.X / LightYear, distance / LightYear, 50)

        Assert.AreEqual(transformation.Gamma, gamma, 50)
    End Sub
End Class