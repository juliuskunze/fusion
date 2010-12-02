Public Class LingExtensionsTests

    Private Structure TestStructure
        Public Name As String

        Public Function NameLength() As Double
            Return Name.Length
        End Function
    End Structure

    Private _list As New List(Of TestStructure) From {New TestStructure With {.Name = "Short"},
                                            New TestStructure With {.Name = "Very very long"},
                                            New TestStructure With {.Name = "Very long"}}

    <Test()>
    Public Sub MinElement()
        Dim elementWithMinLength = _list.MinItem(Function(testObject) testObject.NameLength)

        Assert.AreEqual("Short", elementWithMinLength.Name)
    End Sub

    <Test()>
    Public Sub MaxElement()
        Dim elementWithMaxLength = _list.MaxItem(Function(testObject) testObject.NameLength)

        Assert.AreEqual("Very very long", elementWithMaxLength.Name)
    End Sub

    <Test()>
    Public Sub OrderByAndFirstVsLinearFirstItem()
        Dim list = New List(Of Double)

        Dim random = New Random
        For i = 1 To 100
            list.Add(random.NextDouble())
        Next

        Dim stopWatch = New Stopwatch
        stopWatch.Start()

        Dim elementLinq = list.OrderBy(Function(number) number).First

        stopWatch.Stop()

        Dim linqTime = stopWatch.Elapsed

        stopWatch.Reset()
        stopWatch.Start()

        Dim elementLinear = list.MinItem(Function(number) number)

        stopWatch.Stop()

        Dim linearTime = stopWatch.Elapsed

        Assert.AreEqual(elementLinq, elementLinear)
        Assert.Greater(linqTime, linearTime)
    End Sub



End Class
