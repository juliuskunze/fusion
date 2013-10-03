Public Class SquareMatrixTests
    <Test()> <Ignore()>
    Public Shared Sub NewByOrder()

        Dim sm As New SquareMatrix(2)

        Assert.True(sm.Height = 2 And sm.Width = 2)

        Assert.True(sm * sm.Inverse = SquareMatrix.Identity(sm.Order))
    End Sub
End Class