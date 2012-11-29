Public Module LinqExtensions
    <Runtime.CompilerServices.Extension()>
    Public Function MinItem(Of T)(enumerable As IEnumerable(Of T), numberFunction As Func(Of T, Double)) As T
        Dim minValue = Double.PositiveInfinity
        Dim result As T

        For Each element In enumerable
            Dim number = numberFunction.Invoke(element)

            If number < minValue Then
                minValue = number
                result = element
            End If
        Next

        Return result
    End Function

    <Runtime.CompilerServices.Extension()>
    Public Function MaxItem(Of T)(enumerable As IEnumerable(Of T), numberFunction As Func(Of T, Double)) As T
        Return enumerable.MinItem(Function(element) -numberFunction.Invoke(element))
    End Function
End Module