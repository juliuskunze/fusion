Public Class ConcatenadedSortedEnumerable(Of T)
    Implements IEnumerable(Of T)

    Private _sourceEnumerables As IEnumerable(Of IEnumerable(Of T))

    Private _compareValueFunction As Func(Of T, Double)

    Public Sub New(ByVal sourceEnumerables As IEnumerable(Of IEnumerable(Of T)), ByVal compareValueFunction As Func(Of T, Double))
        _sourceEnumerables = sourceEnumerables
        _compareValueFunction = compareValueFunction
    End Sub

    Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of T) Implements System.Collections.Generic.IEnumerable(Of T).GetEnumerator
        Return New ConcatenatedSortedEnumerator(Of T)(_sourceEnumerables.Select(Function(sourceEnumerator) sourceEnumerator.GetEnumerator),
                                                      compareValueFunction:=_compareValueFunction)
    End Function

    Public Function GetEnumeratorObj() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return Me.GetEnumerator
    End Function
End Class