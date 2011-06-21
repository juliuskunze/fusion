Public Class ExactColorTests

    <Test()>
    Public Sub Black()
        Dim c = Color.FromArgb(0, 0, 0)
        Dim exact = New ExactColor(c)
        Dim c2 = exact.ToColor

        Assert.AreEqual(c, c2)
    End Sub

    <Test()>
    Public Sub UpperBound()
        Dim c = Color.FromArgb(255, 255, 0)
        Dim exact = New ExactColor(1, 1, 0)
        Dim c2 = exact.ToColor

        Assert.AreEqual(c, c2)
    End Sub

    <Test()>
    Public Sub Truncate()
        Dim c = Color.FromArgb(0, 0, 255)
        Dim exact = New ExactColor(-2, 0, 2)
        Dim c2 = exact.ToColorByTruncate

        Assert.AreEqual(c, c2)
    End Sub

End Class
