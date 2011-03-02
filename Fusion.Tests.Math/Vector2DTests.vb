Public Class Vector2DTests

    <Test()> Public Shared Sub NewFromXY()
        Dim v As New Vector2D(1.5, -2)
        Assert.True(v.X = 1.5 AndAlso v.Y = -2)
    End Sub

    <Test()> Public Shared Sub NewFromNothing()
        Assert.True(New Vector2D = New Vector2D(0, 0))
    End Sub

    <Test()> Public Shared Sub NewFromPointF()
        Dim v As New Vector2D(New PointF(1.5, -2))
        Assert.True(v.X = 1.5 AndAlso v.Y = -2)
    End Sub

    <Test()> Public Shared Sub NewFromSizeF()
        Dim v As New Vector2D(New SizeF(1.5, -2))
        Assert.True(v.X = 1.5 AndAlso v.Y = -2)
    End Sub

    <Test()> Public Shared Sub OperatorEqual()
        Assert.True((New Vector2D(1, 2) = New Vector2D(1, 2)))
    End Sub

    <Test()> Public Shared Sub OperatorUnequal()
        Assert.True((New Vector2D(1, 2) <> New Vector2D(1, 3)))
    End Sub

    <Test()> Public Shared Sub OperatorPlus()
        Assert.True((New Vector2D(1, 1) + New Vector2D(1, -2) = New Vector2D(2, -1)))
    End Sub

    <Test()> Public Shared Sub OperatorMinus()
        Assert.True((New Vector2D(1, 1) - New Vector2D(1, -2) = New Vector2D(0, 3)))
        Assert.True((-New Vector2D(1, -2) = New Vector2D(-1, 2)))
    End Sub

    <Test()> Public Shared Sub OperatorMultiply()
        Assert.True((2 * New Vector2D(-1, 2) = New Vector2D(-2, 4)))
        Assert.True((New Vector2D(-1, 2) * 2 = New Vector2D(-2, 4)))
    End Sub

    <Test()> Public Shared Sub OperatorDivide()
        Assert.True((New Vector2D(-1, 2) / 2 = New Vector2D(-0.5, 1)))
    End Sub

    <Test()> Public Shared Sub FromLengthAndArgument()
        Dim v As Vector2D = Vector2D.FromLengthAndArgument(2, 0)
        Assert.True(v.Length = 2 AndAlso v.LengthSquared = 4 AndAlso v.Argument = 0)
        Assert.True(v = New Vector2D(2, 0))
    End Sub

    <Test()> Public Shared Sub ScaleToLength()
        Dim scaled = New Vector2D(0, 2).ScaledToLength(newLength:=3)

        Assert.That(scaled = New Vector2D(0, 3))
    End Sub

    <Test()> Public Shared Sub Fit()
        Assert.True(Vector2D.Fit(New Vector2D(1, 10 ^ -6), New Vector2D(1, 0), maxRelativeError:=10 ^ -5))
        Assert.True(Not Vector2D.Fit(New Vector2D(1, 10 ^ -6), New Vector2D(1, 0), maxRelativeError:=10 ^ -7))
    End Sub

    <Test()> Public Shared Sub SetArgument()
        Dim v = New Vector2D(0, 2).RotateToArgument(newArgument:=PI)
        Assert.True(Vector2D.Fit(v, New Vector2D(-2, 0)))
    End Sub

    <Test()> Public Shared Sub ZeroVector()
        Assert.True(Vector2D.Zero = New Vector2D(0, 0))
    End Sub

    <Test()> Public Shared Sub ToRowMatrix()
        Assert.True(New Vector2D(1, 2).ToRowMatrix = New Matrix(New Double(,) {{1, 2}}))
    End Sub

    <Test()> Public Shared Sub ToColumnMatrix()
        Dim v = New Vector2D(1, 2)
        Assert.True(New Vector2D(1, 2).ToColumnMatrix = New Matrix(New Double(,) {{1}, {2}}))
    End Sub

    <Test()> Public Shared Sub NewFromMatrix()
        Dim v As New Vector2D(1.5, -2)
        Dim v2 = New Vector2D(v.ToColumnMatrix)
        Dim v3 = New Vector2D(v.ToRowMatrix)
        Assert.True(v = v2)
        Assert.True(v = v3)
    End Sub

    <Test()> Public Shared Sub DotProduct()
        Assert.True(Vector2D.DotProduct(New Vector2D(1, 2), New Vector2D(1, 2)) = 5)
    End Sub

    <Test()> Public Shared Sub CrossProduct()
        Assert.True(Vector2D.CrossProduct(New Vector2D(1, 0), New Vector2D(0, 2)) = 2)
    End Sub

    <Test()> Public Shared Sub TestToString()
        Assert.True((New Vector2D(1, 2)).ToString() = "(1|2)")
    End Sub
End Class
