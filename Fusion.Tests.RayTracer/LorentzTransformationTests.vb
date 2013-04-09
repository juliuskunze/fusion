Public Class LorentzTransformationTests
    Private Const c = Constants.SpeedOfLight

    Private ReadOnly _NullTransformation As New LorentzTransformation(relativeVelocity:=New Vector3D)

    <Test()>
    Public Sub Velocity0()

        Dim sightRayInT = New SightRay(originEvent:=New SpaceTimeEvent, direction:=New Vector3D(0, 1, 0))

        Dim sightRayInS = _NullTransformation.Inverse.AtSightRay(sightRay:=sightRayInT).SemiTransformSightRay

        Assert.AreEqual(sightRayInT.OriginEvent, sightRayInS.OriginEvent)
        Assert.AreEqual(sightRayInT.Ray.NormalizedDirection, sightRayInS.Ray.NormalizedDirection)
        Assert.AreEqual(_NullTransformation.Inverse.AtSightRay(sightRayInT).TransformWavelength(wavelength:=17), 17)
        Assert.AreEqual(_NullTransformation.AtSightRay(sightRayInT).TransformSpectralRadiance(spectralRadiance:=17), 17)

        Dim randomRayInS = New SightRay(New SpaceTimeEvent, New Vector3D(43, -12, 4))

        Assert.AreEqual(_NullTransformation.Inverse.AtSightRay(randomRayInS).TransformWavelength(wavelength:=17), 17)
        Assert.AreEqual(_NullTransformation.AtSightRay(randomRayInS).TransformSpectralRadiance(spectralRadiance:=17), 17)

        Dim randomEventInS = New SpaceTimeEvent(New Vector3D(1, 2, 3), 5)

        Assert.AreEqual(_NullTransformation.TransformEvent(randomEventInS), randomEventInS)

        Dim randomVelocityInS = New Vector3D(1, 2, 3)

        Assert.AreEqual(_NullTransformation.TransformVelocity(randomVelocityInS), randomVelocityInS)
    End Sub

    Private Const beta = 0.5
    Private ReadOnly gamma As Double = 1 / Sqrt(1 - beta ^ 2)
    Private Const v = beta * c
    Private _RelativeVelocity As New Vector3D(v, 0, 0)
    Private ReadOnly _Transformation As New LorentzTransformation(relativeVelocity:=_RelativeVelocity)
    Private ReadOnly _VectorComparer As New Vector3DRoughComparer(10 ^ -15)

    <Test()>
    Public Sub PositiveVelocity()
        Assert.AreEqual(_Transformation.RelativeVelocity, _RelativeVelocity)
        Assert.AreEqual(_Transformation.NormalizedRelativeVelocity, _RelativeVelocity.Normalized)
        Assert.AreEqual(_Transformation.Beta, beta)
        Assert.AreEqual(_Transformation.Gamma, gamma)

        Dim parallelRay = New SightRay(New SpaceTimeEvent, direction:=New Vector3D(1, 0, 0))
        Dim transformedParallelRay = _Transformation.AtSightRay(parallelRay).SemiTransformSightRay
        Assert.AreEqual(parallelRay.OriginLocation, transformedParallelRay.OriginLocation)
        Assert.That(_VectorComparer.Equals(parallelRay.Ray.NormalizedDirection, transformedParallelRay.Ray.NormalizedDirection))

        Dim antiParallelRay = New SightRay(New SpaceTimeEvent, direction:=New Vector3D(1, 0, 0))
        Dim transformedAntiparallelRay = _Transformation.AtSightRay(antiParallelRay).SemiTransformSightRay
        Assert.AreEqual(antiParallelRay.OriginLocation, transformedAntiparallelRay.OriginLocation)
        Assert.That(_VectorComparer.Equals(antiParallelRay.Ray.NormalizedDirection, transformedAntiparallelRay.Ray.NormalizedDirection))

        Const randomWavelength = 0.0000005
        Dim ray = New SightRay(New SpaceTimeEvent, New Vector3D(1, 1, 1))
        Assert.Greater(randomWavelength, _Transformation.AtSightRay(parallelRay).TransformWavelength(randomWavelength))
        Assert.Greater(ray.Ray.NormalizedDirection.X, _Transformation.Inverse.AtSightRay(ray).SemiTransformSightRay.Ray.NormalizedDirection.X)

        Const randomSpectralRadiance = 17
        Assert.Greater(_Transformation.AtSightRay(parallelRay).TransformSpectralRadiance(randomSpectralRadiance), randomSpectralRadiance)

        Dim originEvent = New SpaceTimeEvent(New Vector3D, 0)

        Assert.AreEqual(_Transformation.TransformEvent(originEvent), originEvent)

        Const t = 10
        Dim spaceOriginEvent = New SpaceTimeEvent(New Vector3D, t)

        Assert.AreEqual(_Transformation.TransformEvent(spaceOriginEvent), New SpaceTimeEvent(New Vector3D(-gamma * t * v, 0, 0), gamma * t))

        Const x = 10
        Dim timeOriginEvent = New SpaceTimeEvent(New Vector3D(x, 0, 0), 0)

        Assert.AreEqual(_Transformation.TransformEvent(timeOriginEvent), New SpaceTimeEvent(New Vector3D(gamma * x, 0, 0), -gamma * v * x / c ^ 2))
    End Sub

    <Test()>
    Public Sub TestInverse()
        Dim softVectorComparer As New Vector3DRoughComparer(10 ^ -8)

        Dim randomVelocityInS = New Vector3D(0, -3, 0)

        Assert.That(softVectorComparer.Equals(_Transformation.Inverse.TransformVelocity(_Transformation.TransformVelocity(randomVelocityInS)), randomVelocityInS))
        Assert.AreEqual(_Transformation.Inverse.Before(_Transformation).TransformVelocity(randomVelocityInS), randomVelocityInS)
        Assert.AreEqual(_Transformation.Before(_Transformation.Inverse).TransformVelocity(randomVelocityInS), randomVelocityInS)
    End Sub

    <Test()>
    Public Sub Test_inverse_translated()
        Dim startEvent = New SpaceTimeEvent(New Vector3D(2, 3, 4), 10)
        Dim t = New LorentzTransformation(New Vector3D(1, 0, 0), startEvent:=startEvent)

        Dim inverse = t.Inverse
        
        Dim inverseStartEvent = inverse.StartEvent
        t.Inverse.Inverse.StartEvent.Should.Be.EqualTo(t.StartEvent)
        inverse.TransformEvent(inverseStartEvent).Should.Be.EqualTo(New SpaceTimeEvent)
        
        inverseStartEvent.Time.Should.Be.EqualTo(-10)
        inverseStartEvent.Location.Should.Be.EqualTo(New Vector3D(-2, -3, -4) + New Vector3D(10, 0, 0))
    End Sub

    <Test()>
    Public Sub TransformedVelocity_And_Before()
        Const ux = 10000
        Dim parallelVelocity = New Vector3D(ux, 0, 0)

        Dim transformedVelocity = _Transformation.TransformVelocity(parallelVelocity)

        Assert.AreEqual(transformedVelocity, New Vector3D((ux - v) / (1 - v * ux / c ^ 2), 0, 0))
        Assert.AreEqual(transformedVelocity, _Transformation.Before(_NullTransformation).TransformVelocity(parallelVelocity))
        Assert.AreEqual(transformedVelocity, _NullTransformation.Before(_Transformation).TransformVelocity(parallelVelocity))

        Const uy = 10
        Dim orthogonalVelocity = New Vector3D(0, uy, 0)

        Assert.AreEqual(_Transformation.TransformVelocity(orthogonalVelocity), New Vector3D(-v, uy / gamma, 0))

        Dim parallelLightVelocity = New Vector3D(c, 0, 0)

        Assert.AreEqual(_Transformation.TransformVelocity(parallelLightVelocity), parallelLightVelocity)

        Dim antiParallelLightVelocity = New Vector3D(-c, 0, 0)

        Assert.AreEqual(_Transformation.TransformVelocity(antiParallelLightVelocity), antiParallelLightVelocity)

        Dim orthogonalLightVelocity = New Vector3D(0, c, 0)

        Assert.AreEqual(_Transformation.TransformVelocity(orthogonalLightVelocity).Length, c)
    End Sub

    <Test>
    Public Sub Translation()
        Dim t = New LorentzTransformation(relativeVelocity:=New Vector3D, startEvent:=New SpaceTimeEvent(New Vector3D(4, 5, 6), 7))

        Dim transformed = t.TransformEvent(New SpaceTimeEvent(New Vector3D(1, 1, 1), 1))

        Assert.AreEqual(1 - 7, transformed.Time)
        Assert.AreEqual(New Vector3D(1, 1, 1) - New Vector3D(4, 5, 6), transformed.Location)
    End Sub

    <Test>
    Public Sub Before_with_translation()
        Dim t1 = New LorentzTransformation(relativeVelocity:=New Vector3D, startEvent:=New SpaceTimeEvent(New Vector3D, 7))
        Dim t2 = New LorentzTransformation(relativeVelocity:=New Vector3D(1, 0, 0), startEvent:=New SpaceTimeEvent)
        Dim t = t1.Before(t2)

        Dim someEvent = New SpaceTimeEvent(New Vector3D(1, 1, 1), 1)

        Dim transformed = t.TransformEvent(someEvent)
        transformed.Time.Should.Be.EqualTo(-6)
        transformed.Location.Should.Be.EqualTo(New Vector3D(7, 1, 1))
    End Sub

    <Test>
    Public Sub Before_with_translation_complicated()
        Dim t1 = New LorentzTransformation(relativeVelocity:=New Vector3D, startEvent:=New SpaceTimeEvent(New Vector3D, 7))
        Dim t2 = New LorentzTransformation(relativeVelocity:=New Vector3D(1, 0, 0), startEvent:=New SpaceTimeEvent(New Vector3D(4, 0, 0), 7))
        Dim t = t1.Before(t2)

        Dim someEvent = New SpaceTimeEvent(New Vector3D(23, 64, 73), -4)

        Assert.AreEqual(t.TransformEvent(someEvent), t2.TransformEvent(t1.TransformEvent(someEvent)))
    End Sub
End Class
