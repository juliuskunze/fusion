Public Class ConcatenatedSortedEnumeratorTests
    <Test()>
    Public Sub TestSingleEmpty()
        Dim emptyEnumerator = (New List(Of Double)).GetEnumerator

        Dim enumeratorEnumerable = New List(Of IEnumerator(Of Double)) From {emptyEnumerator}

        Dim concatenatedEnumerable = New ConcatenatedSortedEnumerator(Of Double)(enumeratorEnumerable, Function(a) a)

        concatenatedEnumerable.Reset()
        Assert.AreEqual(concatenatedEnumerable.MoveNext, False)
    End Sub

    <Test()>
    Public Sub TestSingle()
        Dim sortedEnumerator = (New List(Of Double) From {1, 2, 3}).GetEnumerator

        Dim enumeratorEnumerable = New List(Of IEnumerator(Of Double)) From {sortedEnumerator}

        Dim concatenatedEnumerable = New ConcatenatedSortedEnumerator(Of Double)(enumeratorEnumerable, Function(a) a)

        concatenatedEnumerable.Reset()
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(concatenatedEnumerable.Current, 1)
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(concatenatedEnumerable.Current, 2)
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(concatenatedEnumerable.Current, 3)
        Assert.That(Not concatenatedEnumerable.MoveNext)
    End Sub

    <Test()>
    Public Sub TestMultiEmpty()
        Dim emptyEnumerator1 = (New List(Of Double)).GetEnumerator
        Dim emptyEnumerator2 = (New List(Of Double)).GetEnumerator

        Dim enumeratorEnumerable = New List(Of IEnumerator(Of Double)) From {emptyEnumerator1, emptyEnumerator2}

        Dim concatenatedEnumerable = New ConcatenatedSortedEnumerator(Of Double)(enumeratorEnumerable, Function(a) a)
        concatenatedEnumerable.Reset()

        Assert.AreEqual(concatenatedEnumerable.MoveNext, False)
    End Sub

    <Test()>
    Public Sub TestMulti()
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
    Public Sub TestEmptyNotEmptyCombination()
        Dim emptyEnumerator = (New List(Of Double)).GetEnumerator
        Dim sortedEnumerator = (New List(Of Double) From {1, 2, 3}).GetEnumerator

        Dim enumeratorEnumerable = New List(Of IEnumerator(Of Double)) From {emptyEnumerator, sortedEnumerator}

        Dim concatenatedEnumerable = New ConcatenatedSortedEnumerator(Of Double)(enumeratorEnumerable, Function(a) a)

        concatenatedEnumerable.Reset()
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(concatenatedEnumerable.Current, 1)
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(concatenatedEnumerable.Current, 2)
        concatenatedEnumerable.MoveNext()
        Assert.AreEqual(concatenatedEnumerable.Current, 3)
        Assert.That(Not concatenatedEnumerable.MoveNext)
    End Sub


    <Test()>
    Public Sub ForEachBeginsWithMoveNextAndNotReset()
        Dim enumerable = New TestEnumerable

        For Each number In enumerable
        Next

        Assert.AreEqual(enumerable.Enumerator.MovenextCount, 2)
        Assert.That(Not enumerable.Enumerator.DidReset)
    End Sub

    Private Class TestEnumerator
        Implements IEnumerator

        Private _Object As Double = 0

        Public ReadOnly Property Current As Object Implements System.Collections.IEnumerator.Current
            Get
                Return _Object
            End Get
        End Property

        Private _MoveNextCount As Integer = 0
        Public ReadOnly Property MovenextCount As Integer
            Get
                Return _MoveNextCount
            End Get
        End Property

        Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
            _MoveNextCount += 1

            If _Object = 0 Then
                _Object = 1
                Return True
            Else
                Return False
            End If
        End Function

        Public DidReset As Boolean = False

        Public Sub Reset() Implements System.Collections.IEnumerator.Reset
            _Object = 0
            Me.DidReset = True
        End Sub

    End Class

    Private Class TestEnumerable
        Implements IEnumerable

        Public Property Enumerator As TestEnumerator

        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Me.Enumerator = New ConcatenatedSortedEnumeratorTests.TestEnumerator
            Return Me.Enumerator
        End Function
    End Class
End Class