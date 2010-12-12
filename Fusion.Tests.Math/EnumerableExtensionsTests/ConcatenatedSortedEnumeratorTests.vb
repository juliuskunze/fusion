Public Class ConcatenatedSortedEnumeratorTests

    <Test()>
    Public Sub Test()
        Dim sortedEnumerator1 = (New List(Of Double) From {1, 2, 4}).GetEnumerator
        Dim sortedEnumerator2 = (New List(Of Double) From {3, 7.5, 17}).GetEnumerator
        Dim sortedEnumerator3 = (New List(Of Double) From {5, 6, 32}).GetEnumerator

        Dim enumeratorEnumerable = New List(Of IEnumerator(Of Double)) From {sortedEnumerator1, sortedEnumerator2, sortedEnumerator3}

        Dim concatenatedEnumerable = New ConcatenatedSortedEnumerator(Of Double)(enumeratorEnumerable, Function(a) a)

        concatenatedEnumerable.Reset()
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(1, concatenatedEnumerable.Current)
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(2, concatenatedEnumerable.Current)
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(3, concatenatedEnumerable.Current)
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(4, concatenatedEnumerable.Current)
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(5, concatenatedEnumerable.Current)
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(6, concatenatedEnumerable.Current)
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(7.5, concatenatedEnumerable.Current)
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(17, concatenatedEnumerable.Current)
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(32, concatenatedEnumerable.Current)
        Assert.AreEqual(False, concatenatedEnumerable.MoveNext)
    End Sub

    <Test()>
    Public Sub TestEmpty()
        Dim emptyEnumerator1 = (New List(Of Double)).GetEnumerator
        Dim emptyEnumerator2 = (New List(Of Double)).GetEnumerator

        Dim enumeratorEnumerable = New List(Of IEnumerator(Of Double)) From {emptyEnumerator1, emptyEnumerator2}

        Dim concatenatedEnumerable = New ConcatenatedSortedEnumerator(Of Double)(enumeratorEnumerable, Function(a) a)

        concatenatedEnumerable.Reset()
        Assert.AreEqual(concatenatedEnumerable.MoveNext, False)
    End Sub

    <Test()>
    Public Sub TestCombination()
        Dim emptyEnumerator = (New List(Of Double)).GetEnumerator
        Dim sortedEnumerator = (New List(Of Double) From {3, 1, 2}).GetEnumerator

        Dim enumeratorEnumerable = New List(Of IEnumerator(Of Double)) From {emptyEnumerator, sortedEnumerator}

        Dim concatenatedEnumerable = New ConcatenatedSortedEnumerator(Of Double)(enumeratorEnumerable, Function(a) a)

        concatenatedEnumerable.Reset()
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(concatenatedEnumerable.Current, 1)
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(concatenatedEnumerable.Current, 2)
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(concatenatedEnumerable.Current, 3)
        Assert.AreEqual(concatenatedEnumerable.MoveNext, False)
    End Sub
    
End Class