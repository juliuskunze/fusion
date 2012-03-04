Public Class AffineMap2DTests

    <Test()>
    Public Sub TestScalingBefore()
        Dim a = New Vector2D(2, 1)

        Dim testVector = New Vector2D(1, 1)
        Dim map = AffineMap2D.Translation(a).Before(AffineMap2D.Scaling(2)).Before(AffineMap2D.Translation(-a))

        Assert.AreEqual(map.Apply(testVector), New Vector2D(4, 3))
    End Sub

    <Test()>
    Public Sub TestScalingAfter()
        Dim a = New Vector2D(2, 1)

        Dim testVector = New Vector2D(1, 1)
        Dim map = AffineMap2D.Translation(-a).After(AffineMap2D.Scaling(2)).After(AffineMap2D.Translation(a))

        Assert.AreEqual(map.Apply(testVector), New Vector2D(4, 3))
    End Sub

End Class