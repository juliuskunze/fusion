Public Class RelativisticRayTracingComilerTests

    <Test()>
    Public Sub Test()
        Dim text = "sphere { <0,0,0>, 5 pigment { color rgb <1,0,0> }}"

        Dim parameter = Expressions.Expression.Parameter(GetType(Double), "Argu")
        Assert.AreEqual(1, Expressions.Expression.Lambda(Of Func(Of Double, Double))(parameter, parameters:={parameter}).Compile.Invoke(1.0))
    End Sub

End Class
