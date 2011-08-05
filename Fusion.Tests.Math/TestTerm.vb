Imports System.Linq.Expressions

Public Class TestTerm

    <Test()> Public Sub TestTermToValue1()
        Assert.True(New IndependentTerm("5").Parse = 5)
        Assert.True(New IndependentTerm("(5)").Parse = 5)
        Assert.True(New IndependentTerm("1+2").Parse = 3)
        Assert.True(New IndependentTerm("(1+2)").Parse = 3)
    End Sub

    <Test()> Public Sub TestTermToValue2()
        Assert.True(New IndependentTerm("2*3+5").Parse = 11)
        Assert.True(New IndependentTerm("5+3*4").Parse = 17)
        Assert.True(New IndependentTerm("3*(2+3)").Parse = 15)
        Assert.True(New IndependentTerm("3^2").Parse = 9)
    End Sub

    <Test()> Public Sub TestTermToValue3()
        Assert.True(New IndependentTerm("((3*(4+3/(2+1*1*1+1-1^(1+0)))*2/2))").Parse = 15)
        Assert.True(New IndependentTerm("2^3^2").Parse = New IndependentTerm("2^(3^2)").Parse)
    End Sub

    <Test()> Public Sub TestTermToValue4()
        Assert.True(New IndependentTerm("-5").Parse = -5)
        Assert.True(New IndependentTerm("--++-5").Parse = -5)
    End Sub

    <Test()> Public Sub TestTermToValue5()
        Assert.True(New IndependentTerm("-3+3").Parse = 0)
        Assert.True(New IndependentTerm("-3+3-3+3").Parse = 0)
    End Sub

    <Test()> Public Sub TestTermToValue6()
        Try
            Dim value = New IndependentTerm("").Parse
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            Dim value = New IndependentTerm("23+(3+4))")
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            Dim value = New IndependentTerm("(((")
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            Dim value = New IndependentTerm("2a")
            Assert.Fail()
        Catch ex As ArgumentException
        End Try
    End Sub

    <Test()>
    Public Sub TestPiAndE()
        Assert.That(New IndependentTerm("Pi").Parse = System.Math.PI)
        Assert.That(New IndependentTerm("E").Parse = System.Math.E)
    End Sub

    <Test()>
    Public Sub TestCos()
        Assert.That(New IndependentTerm("Cos(0)").Parse = 1)
        Assert.That(New IndependentTerm("Cos(pi)").Parse = -1)
        Assert.That(New IndependentTerm("Cos(pi/2)").Parse < 10 ^ -15)
    End Sub

    <Test()>
    Public Sub TestExp()
        Assert.That(New IndependentTerm("exp(1)").Parse = System.Math.E)
    End Sub

    <Test()>
    Public Sub TestParameters()
        Assert.That(New Term("a+4", doubleParameterNames:={"a"}).GetDelegate(Of Func(Of Double, Double)).Invoke(5.0) = 9)
        Assert.That(New Term("x^2 + x", doubleParameterNames:={"x"}).GetDelegate(Of Func(Of Double, Double)).Invoke(5.0) = 30)
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
