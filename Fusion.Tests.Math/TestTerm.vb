Public Class TestTerm

    <Test()> Public Sub TestTermToValue1()
        Assert.True(New IndependentTerm("5").GetResult = 5)
        Assert.True(New IndependentTerm("(5)").GetResult = 5)
        Assert.True(New IndependentTerm("1+2").GetResult = 3)
        Assert.True(New IndependentTerm("(1+2)").GetResult = 3)
    End Sub

    <Test()> Public Sub TestTermToValue2()
        Assert.True(New IndependentTerm("2*3+5").GetResult = 11)
        Assert.True(New IndependentTerm("5+3*4").GetResult = 17)
        Assert.True(New IndependentTerm("3*(2+3)").GetResult = 15)
        Assert.True(New IndependentTerm("3^2").GetResult = 9)
    End Sub

    <Test()> Public Sub TestTermToValue3()
        Assert.True(New IndependentTerm("((3*(4+3/(2+1*1*1+1-1^(1+0)))*2/2))").GetResult = 15)
        Assert.True(New IndependentTerm("2^3^2").GetResult = New IndependentTerm("2^(3^2)").GetResult)
    End Sub

    <Test()> Public Sub TestTermToValue4()
        Assert.True(New IndependentTerm("-5").GetResult = -5)
        Assert.True(New IndependentTerm("--++-5").GetResult = -5)
    End Sub

    <Test()> Public Sub TestTermToValue5()
        Assert.True(New IndependentTerm("-3+3").GetResult = 0)
        Assert.True(New IndependentTerm("-3+3-3+3").GetResult = 0)
    End Sub

    <Test()>
    Public Sub TestTermToValue6()
        Try
            Dim value = New IndependentTerm("").GetResult
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            Dim value = New IndependentTerm("4)").GetResult
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            Dim value = New IndependentTerm("23+(3+4))").GetResult
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            Dim value = New IndependentTerm("(((").GetResult
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            Dim value = New IndependentTerm("2a").GetResult
            Assert.Fail()
        Catch ex As ArgumentException
        End Try
    End Sub

    <Test()>
    Public Sub TestPiAndE()
        Assert.That(New IndependentTerm("Pi").GetResult = System.Math.PI)
        Assert.That(New IndependentTerm("E").GetResult = System.Math.E)
    End Sub

    <Test()>
    Public Sub TestCos()
        Assert.That(New IndependentTerm("Cos(0)").GetResult = 1)
        Assert.That(New IndependentTerm("Cos(pi)").GetResult = -1)
        Assert.That(New IndependentTerm("Cos(pi/2)").GetResult < 10 ^ -15)
    End Sub

    <Test()>
    Public Sub TestExp()
        Assert.That(New IndependentTerm("exp(1)").GetResult = System.Math.E)
    End Sub

    <Test()>
    Public Sub TestParameters()
        Assert.That(New Term("a+4", parameterNames:={"a"}, userFunctions:={}).GetDelegate(Of Func(Of Double, Double)).Invoke(5) = 9)
        Assert.That(New Term("x^2 + x", parameterNames:={"x"}, userFunctions:={}).GetDelegate(Of Func(Of Double, Double)).Invoke(5) = 30)
        Assert.That(New Term("a1^2 + a1", parameterNames:={"a1"}, userFunctions:={}).GetDelegate(Of Func(Of Double, Double)).Invoke(5) = 30)
    End Sub

    <Test()>
    Public Sub TestFunction()
        Dim userFunctionExpression = CType(Function(x As Double) x, Expression(Of Func(Of Double, Double)))
        Dim userFunctionExpressionBuilder = Function(parameters As IEnumerable(Of Expression)) userFunctionExpression
        Dim namedMethodExpression = New NamedExpression(name:="user", Expression:=userFunctionExpressionBuilder)
        Dim term = New Term("user(1)", parameterNames:={}, userFunctions:={namedMethodExpression})
        Dim d = term.GetDelegate(Of Func(Of Double))()

        Assert.That(d() = 5)
    End Sub

    <Test()>
    Public Sub TestLambdaExpression()
        Dim f As Expressions.Expression(Of Func(Of Double, Double)) = Function(a As Double) a + 4
        Assert.That(f.Compile()(5) = 9)

        Dim parameter = Expression.Parameter(GetType(Double), "a")
        Dim add = Expression.AddChecked(parameter, Expression.Constant(4.0))
        Dim lambda = Expression.Lambda(Of Func(Of Double, Double))(body:=add, parameters:={parameter})

        Assert.That(lambda.Compile()(5.0) = 9)
    End Sub

End Class
