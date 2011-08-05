Public Class TermTests

    <Test()>
    Public Sub TestIndependentTerm1()
        Assert.True(New IndependentTerm(Of Double)("5").GetResult = 5)
        Assert.True(New IndependentTerm(Of Double)("(5)").GetResult = 5)
        Assert.True(New IndependentTerm(Of Double)("1+2").GetResult = 3)
        Assert.True(New IndependentTerm(Of Double)("(1+2)").GetResult = 3)
    End Sub

    <Test()>
    Public Sub TestIndependentTerm2()
        Assert.True(New IndependentTerm(Of Double)("2*3+5").GetResult = 11)
        Assert.True(New IndependentTerm(Of Double)("5+3*4").GetResult = 17)
        Assert.True(New IndependentTerm(Of Double)("3*(2+3)").GetResult = 15)
        Assert.True(New IndependentTerm(Of Double)("3^2").GetResult = 9)
    End Sub

    <Test()>
    Public Sub TestIndependentTerm3()
        Assert.True(New IndependentTerm(Of Double)("((3*(4+3/(2+1*1*1+1-1^(1+0)))*2/2))").GetResult = 15)
        Assert.True(New IndependentTerm(Of Double)("2^3^2").GetResult = New IndependentTerm(Of Double)("2^(3^2)").GetResult)
    End Sub

    <Test()>
    Public Sub TestIndependentTerm4()
        Assert.True(New IndependentTerm(Of Double)("-5").GetResult = -5)
        Assert.True(New IndependentTerm(Of Double)("--++-5").GetResult = -5)
    End Sub

    <Test()>
    Public Sub TestIndependentTerm5()
        Assert.True(New IndependentTerm(Of Double)("-3+3").GetResult = 0)
        Assert.True(New IndependentTerm(Of Double)("-3+3-3+3").GetResult = 0)
    End Sub

    <Test()>
    Public Sub TestIndependentTerm6()
        Try
            Dim value = New IndependentTerm(Of Double)("").GetResult
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            Dim value = New IndependentTerm(Of Double)("4)").GetResult
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            Dim value = New IndependentTerm(Of Double)("23+(3+4))").GetResult
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            Dim value = New IndependentTerm(Of Double)("(((").GetResult
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            Dim value = New IndependentTerm(Of Double)("2a").GetResult
            Assert.Fail()
        Catch ex As ArgumentException
        End Try
    End Sub

    <Test()>
    Public Sub TestPiAndE()
        Assert.That(New IndependentTerm(Of Double)("Pi").GetResult = System.Math.PI)
        Assert.That(New IndependentTerm(Of Double)("E").GetResult = System.Math.E)
    End Sub

    <Test()>
    Public Sub TestCos()
        Assert.That(New IndependentTerm(Of Double)("Cos(0)").GetResult = 1)
        Assert.That(New IndependentTerm(Of Double)("Cos(pi)").GetResult = -1)
        Assert.That(New IndependentTerm(Of Double)("Cos(pi/2)").GetResult < 10 ^ -15)
    End Sub

    <Test()>
    Public Sub TestExp()
        Assert.That(New IndependentTerm(Of Double)("exp(1)").GetResult = System.Math.E)
    End Sub

    <Test()>
    Public Sub TestParameter()
        Assert.That(New Term("a+4", userContext:=New TermContext(constants:={}, parameters:={Expression.Parameter(GetType(Double), "a")}, Functions:={})).GetDelegate(Of Func(Of Double, Double)).Invoke(5) = 9)
        Assert.That(New Term("x^2 + x", userContext:=New TermContext(constants:={}, parameters:={Expression.Parameter(GetType(Double), "x")}, Functions:={})).GetDelegate(Of Func(Of Double, Double)).Invoke(5) = 30)
        Assert.That(New Term("a1^2 + a1", userContext:=New TermContext(constants:={}, parameters:={Expression.Parameter(GetType(Double), "a1")}, Functions:={})).GetDelegate(Of Func(Of Double, Double)).Invoke(5) = 30)
        Assert.That(New Term("a1^2 + a2", userContext:=New TermContext(constants:={}, parameters:={Expression.Parameter(GetType(Double), "a1"), Expression.Parameter(GetType(Double), "a2")}, Functions:={})).GetDelegate(Of Func(Of Double, Double, Double)).Invoke(5, 3) = 28)
    End Sub

    <Test()>
    Public Sub TestFunction()
        Dim namedMethodExpression = New NamedExpression(name:="square", ExpressionBuilder:=NamedExpression.GetFunctionExpressionBuilder(Of Func(Of Double, Double))(userFunction:=Function(x As Double) x ^ 2))
        Dim term = New Term("square(2*x)", userContext:=New TermContext(constants:={}, parameters:={Expression.Parameter(GetType(Double), "x")}, Functions:={namedMethodExpression}))
        Dim d = term.GetDelegate(Of Func(Of Double, Double))()

        Assert.That(d(5) = 100)
    End Sub

    <Test()>
    Public Sub TestCore()
        Dim f As Expressions.Expression(Of Func(Of Double, Double)) = Function(a As Double) a + 4
        Assert.That(f.Compile()(5) = 9)

        Dim parameter = Expression.Parameter(GetType(Double), "a")
        Dim add = Expression.AddChecked(parameter, Expression.Constant(4.0))
        Dim lambda = Expression.Lambda(Of Func(Of Double, Double))(body:=add, parameters:={parameter})

        Assert.That(lambda.Compile()(5) = 9)
    End Sub

End Class
