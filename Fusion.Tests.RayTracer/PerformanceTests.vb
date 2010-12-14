Public Class PerformanceTests

    <Test()>
    Public Sub ExamplePicture()
        Dim startTime = Now

        Dim picture = New Ry.RayTracingExamples(pictureSize:=New Size(100, 100)).IluminationRoom.Picture

        Dim neededTime = Now - startTime

        Assert.Less(neededTime, TimeSpan.FromSeconds(0.8D), message:=neededTime.ToString)
    End Sub

End Class
