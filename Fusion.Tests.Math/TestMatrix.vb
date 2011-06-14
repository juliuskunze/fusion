Public Class TestMatrix

    <Test()> Public Shared Sub NewFromWidthAndHeight()
        Dim m = New Matrix(width:=1, height:=2)
        Assert.True(m(0, 0) = 0 AndAlso m(1, 0) = 0)
        Assert.True(m.Width = 1 AndAlso m.Height = 2)
    End Sub

    <Test()> Public Shared Sub NewFromArray()
        Dim m = New Matrix(New Double(,) { _
        {1, 2}, _
        {3, 4}, _
        {5, 6}})

        Assert.True(m(2, 1) = 6)
        Assert.True(m.Width = 2 AndAlso m.Height = 3)
    End Sub


    <Test()> Public Shared Sub OperatorEqual()
        Dim a, b As New Matrix(New Double(,) {{1, 2}})
        Assert.True(a = b)
    End Sub

    <Test()> Public Shared Sub OperatorUnequal()
        Dim a = New Matrix(New Double(,) {{1, 2}})
        Dim b = New Matrix(New Double(,) {{2, 2}})
        Assert.True(a <> b)
    End Sub

    <Test()> <Ignore()>
    Public Shared Sub ToArray()
        Dim array = New Double(,) { _
        {1, 2}, _
        {3, 4}, _
        {5, 6}}

        Dim matrix = New Matrix(array)
        Dim arrayFromMatrix = matrix.ToArray

        Assert.True(array.Equals(arrayFromMatrix))

        'For iRow = 0 To matrix.Height - 1
        '    For iCol = 0 To matrix.Width - 1
        '        Assert.True(array(iRow, iCol) = arrayFromMatrix(iRow, iCol))
        '    Next
        'Next

    End Sub

    <Test()> <Ignore()>
    Public Shared Sub SwapRows()
        Dim m = New Matrix(New Double(,) { _
        {1, 2}, _
        {3, 4}, _
        {5, 6}})

        m = m.SwapRows(0, 1)
        Assert.True(m(0, 0) = 3)
        Assert.True(m(1, 1) = 2)
    End Sub

    <Test()> <Ignore()>
    Public Shared Sub MultiplyRow()
        Dim m = New Matrix(New Double(,) { _
        {1, 2}, _
        {3, 4}, _
        {5, 6}})

        m = m.MultiplyRow(0, 5.5)
        Assert.True(m(0, 0) = 5.5)
        Assert.True(m(0, 1) = 11)
    End Sub

    <Test()> <Ignore()>
    Public Shared Sub MultiplyAddRow()
        Dim m = New Matrix(New Double(,) { _
        {1, 2}, _
        {3, 4}, _
        {5, 6}})

        m = m.MultiplyAddRow(0, 1, 2)
        Assert.True(m(0, 0) = 5)
        Assert.True(m(0, 1) = 8)
    End Sub

     <Test()> <Ignore()>
    Public Shared Sub OperatorMultiply()
        Dim v = New Vector2D(1, 2).ToColumnMatrix
        Dim m = New Matrix(New Double(,) {{1, 2}, {3, 0}})
        'Assert.True(Vector2D.Fit(m * v, New Vector2D(1 * 1 + 2 * 2, 3 * 1 + 0 * 2)).ToColumnMatrix)
        Assert.Fail()
    End Sub


End Class
