Public Class LorentzTransformationAtSightRayTests

    Private Const c = Fusion.Physics.Constants.SpeedOfLight

    <Test()>
    Public Sub Inverse()
        Const randomWavelength = 700 * 10 ^ -9

        Dim t = New LorentzTransformationAtSightRay(relativeVelocity:=New Vector3D(c / 2, 0, 0), SightRay:=New Ray(New Vector3D, New Vector3D(0, 1, 0)))

        Dim inverse = t.InverseAtSightRay

        Assert.That(Abs(randomWavelength - inverse.TransformWavelength(t.TransformWavelength(randomWavelength))) < 10 ^ -10)
    End Sub

End Class
