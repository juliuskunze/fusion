Public Class ConstantRotationLorentzTransformationTests
    Private ReadOnly _VeryRoughtComparer As New RoughVector3DComparer(100)
    Private ReadOnly _RoughtComparer As New RoughVector3DComparer(10 ^ -6)

    <Test>
    Public Sub Test()
        Const r = 4
        Dim center = New Vector3D(3, 3, 3)
        Const v = SpeedOfLight / 2
        Const period = 2 * PI * r / v

        Const startTime = 17
        Dim startEvent = New SpaceTimeEvent(center + New Vector3D(r, 0, 0), startTime)
        Const currentPeriods = 1000 * period
        Const restTime = startTime + currentPeriods

        Dim t = New ConstantRotationLorentzTransformation(center:=center,
                                                          axisDirection:=New Vector3D(0, -1, 0),
                                                          startEvent:=startEvent,
                                                          velocity:=v)


        Dim gamma = LorentzTransformation.GetGamma(v)

        Dim lt = t.InertialToAcceleratedInertial(acceleratedFrameTime:=restTime * gamma)

        _VeryRoughtComparer.Equals(lt.RelativeVelocity, New Vector3D(0, 0, v)).Should.Be.True()
        Dim transformed = lt.TransformEvent(New SpaceTimeEvent(startEvent.Location, restTime))

        Assert.AreEqual(transformed.Time, currentPeriods * LorentzTransformation.GetGamma(velocity:=v), delta:=10 ^ -10)
        _RoughtComparer.Equals(transformed.Location, New Vector3D)
    End Sub

    <Test>
    Public Sub Test_2()
        Dim v = SpeedOfLight / 2
        Dim gamma = LorentzTransformation.GetGamma(v)

        Dim startLocation = New Vector3D(-6, 1.5, 0)
        Dim t = New ConstantRotationLorentzTransformation(center:=New Vector3D(-3, 1.5, 0),
                                                          axisDirection:=New Vector3D(0, 1, 0),
                                                          startEvent:=New SpaceTimeEvent(startLocation, 0),
                                                          velocity:=v)

        Dim acceleratedFrameTime = 2 * PI * 3 / v * gamma
        Dim lt = t.InertialToAcceleratedInertial(acceleratedFrameTime:=acceleratedFrameTime).Inverse

        _VeryRoughtComparer.Equals(lt.RelativeVelocity, New Vector3D(0, 0, -v)).Should.Be.True()

        Dim startLocationEvent = lt.TransformEvent(New SpaceTimeEvent(New Vector3D, time:=acceleratedFrameTime))
        _RoughtComparer.Equals(startLocationEvent.Location, startLocation).Should.Be.True()
        _RoughtComparer.Equals(startLocationEvent.Location, startLocation).Should.Be.True()
    End Sub
End Class