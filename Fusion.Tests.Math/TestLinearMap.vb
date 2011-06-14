Public Class TestLinearMap

    <Test()> Shared Sub NewFromSquareMatrix()
        Dim m As New SquareMatrix(New Double(,) {{1, 2}, {3, 4}})
        Assert.True((New LinearMap2D(m).MappingMatrix = m))

        Try
            Dim a As New LinearMap2D(New SquareMatrix(order:=1))
            Assert.Fail("Wrong matrix order")
        Catch ex As ArgumentException
        End Try
    End Sub

    <Test()> Shared Sub NewFromMatrixArray()
        Dim m As Double(,) = New Double(,) {{1, 2}, {3, 4}}
        Assert.True(((New LinearMap2D(m)).MappingMatrix = New Matrix(m)))

        Try
            Dim a As New LinearMap2D(New Double(,) {{1}})
            Assert.Fail()
        Catch ex As ArgumentException
        End Try
    End Sub

    <Test()> Shared Sub NewFromNothing()
        Assert.True(((New LinearMap2D()).MappingMatrix = SquareMatrix.Identity(2)))
    End Sub


    <Test()> Shared Sub OperatorEqual()
        Dim l = New LinearMap2D(New Double(,) {{1, 2}, {3, 4}})
        Assert.True(l = l)
    End Sub

    <Test()> Shared Sub OperatorUnequal()
        Dim l1 = New LinearMap2D(New Double(,) {{1, 2}, {3, 4}})
        Dim l2 = New LinearMap2D(New Double(,) {{1, 1}, {3, 4}})
        Assert.True(l1 <> l2)
    End Sub


    <Test()> Shared Sub Identity()
        Assert.True(((LinearMap2D.Identity.MappingMatrix = SquareMatrix.Identity(2))))
    End Sub


    <Test()> Shared Sub ApplyToVector1()
        Dim v = New Vector2D(1, 2)
        Dim l = New LinearMap2D(New Double(,) {{1, 2}, {3, 0}})
        Assert.True(Vector2D.Fit(l.Apply(v), New Vector2D(1 * 1 + 2 * 2, 3 * 1 + 0 * 2)))
    End Sub

    <Test()> Shared Sub ApplyToVector2()
        Dim l = New LinearMap2D(New Double(,) {{1, 2}, {3, 4}})
        Dim v1 = New Vector2D(34, 28)
        Dim v2 = New Vector2D(-17, 13.5)
        Assert.True(Vector2D.Fit(l.Apply(2 * v1 + v2), 2 * l.Apply(v1) + l.Apply(v2)))
    End Sub

    <Test()> Shared Sub ApplyToMap()
        Dim v = New Vector2D(1, 12)
        Dim l1 = New LinearMap2D(New Double(,) {{-4, 12}, {-6, 21}})
        Dim l2 = New LinearMap2D(New Double(,) {{0, 7}, {1, 12}})

        Assert.True(Vector2D.Fit(l2.After(l1).Apply(v), l2.Apply(l1.Apply(v))))
    End Sub

    <Test()> Shared Sub ChangeByMap()
        Dim v = New Vector2D(1, 12)
        Dim l1 = New LinearMap2D(New Double(,) {{-4, 12}, {-6, 21}})
        Dim l2 = New LinearMap2D(New Double(,) {{0, 7}, {1, 12}})

        Assert.True(Vector2D.Fit(l1.Before(l2).Apply(v), l2.After(l1).Apply(v)))
    End Sub


    <Test()> Shared Sub Rotation()
        Dim v = New Vector2D(2, 1)
        Dim l = LinearMap2D.Rotation(PI / 2)

        Assert.True(Vector2D.Fit(l.Apply(v), New Vector2D(-1, 2)))
    End Sub

    <Test()> Shared Sub Reflection()
        Dim v = New Vector2D(1, 2)
        Dim l = LinearMap2D.Reflection(PI * 3 / 4)

        Assert.True(Vector2D.Fit(l.Apply(v), New Vector2D(-2, -1)))
    End Sub

    <Test()> Shared Sub ScalingFromFactor()
        Dim v = New Vector2D(1, 3)
        Dim l = LinearMap2D.Scaling(-2)

        Assert.True(Vector2D.Fit(l.Apply(v), New Vector2D(-2, -6)))
    End Sub

    <Test()> Shared Sub ScalingFromXFactorYFactor()
        Dim v = New Vector2D(1, 3)
        Dim l = LinearMap2D.Scaling(-2, 3)

        Assert.True(Vector2D.Fit(l.Apply(v), New Vector2D(-2, 9)))
    End Sub

    <Test()> Shared Sub HorizontalScaling()
        Dim v = New Vector2D(1, 3)
        Dim l = LinearMap2D.HorizontalScaling(-2)

        Assert.True(Vector2D.Fit(l.Apply(v), New Vector2D(-2, 3)))
    End Sub

    <Test()> <Ignore()>
    Shared Sub HorizontalShearing()
        Dim v = New Vector2D(0, 1)
        Dim l = LinearMap2D.HorizontalScaling(0)

        Assert.True(Vector2D.Fit(l.Apply(v), New Vector2D(0, 0)))
    End Sub

    <Test()> <Ignore()>
    Shared Sub Inverse()
        Dim l = New LinearMap2D(New Double(,) {{-1, 9}, {5, 7}})
        Assert.True(l.Inverse.After(l) = LinearMap2D.Identity())
    End Sub

End Class
