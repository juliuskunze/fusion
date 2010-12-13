Public Class ConcatenatedSortedEnumerableTests

    <Test()>
    Public Sub Test()
        Dim empty = Enumerable.Empty(Of Integer)()
        Dim filled1 = New List(Of Integer) From {1, 2, 4}
        Dim filled2 = New List(Of Integer) From {3, 4, 7}

        Dim concat = New ConcatenadedSortedEnumerable(Of Integer)(New List(Of IEnumerable(Of Integer)) From {empty, filled1, filled2}, compareValueFunction:=Function(number) number)

        Dim numberString As String = ""
        For Each number In concat
            numberString &= number.ToString
        Next

        Assert.AreEqual(numberString, "123447")

    End Sub

End Class