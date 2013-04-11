Public Class ConstantRotationLorentzTransformationTests
    Private ReadOnly _RoughtVelocityComparer As New RoughVector3DComparer(100)

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


        Dim lt = t.GetConstantVelocityTransformationAtTime(restTime:=restTime)

        _RoughtVelocityComparer.Equals(lt.RelativeVelocity, New Vector3D(0, 0, v)).Should.Be.True()
        Dim transformed = lt.TransformEvent(New SpaceTimeEvent(startEvent.Location, restTime))

        Assert.AreEqual(transformed.Time, currentPeriods * LorentzTransformation.GetGamma(velocity:=v), delta:=10 ^ -10)
        _RoughtVelocityComparer.Equals(transformed.Location, New Vector3D)
    End Sub
End Class