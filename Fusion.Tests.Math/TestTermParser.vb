Public Class TestTermParser

    <Test()> Public Sub TestTermToValue1()
        Assert.True(TermParser.Parse("5") = 5)
        Assert.True(TermParser.Parse("(5)") = 5)
        Assert.True(TermParser.Parse("1+2") = 3)
        Assert.True(TermParser.Parse("(1+2)") = 3)
    End Sub

    <Test()> Public Sub TestTermToValue2()
        Assert.True(TermParser.Parse("2*3+5") = 11)
        Assert.True(TermParser.Parse("5+3*4") = 17)
        Assert.True(TermParser.Parse("3*(2+3)") = 15)
        Assert.True(TermParser.Parse("3^2") = 9)
    End Sub

    <Test()> Public Sub TestTermToValue3()
        Assert.True(TermParser.Parse("((3*(4+3/(2+1*1*1+1-1^(1+0)))*2/2))") = 15)
        Assert.True(TermParser.Parse("2^3^2") = TermParser.Parse("2^(3^2)"))
    End Sub

    <Test()> Public Sub TestTermToValue4()
        Assert.True(TermParser.Parse("-5") = -5)
        Assert.True(TermParser.Parse("--++-5") = -5)
    End Sub

    <Test()> Public Sub TestTermToValue5()
        Assert.True(TermParser.Parse("-3+3") = 0)
        Assert.True(TermParser.Parse("-3+3-3+3") = 0)
    End Sub

    <Test()> Public Sub TestTermToValue6()
        Try
            TermParser.Parse("")
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            TermParser.Parse("23+(3+4))")
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            TermParser.Parse("(((")
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            TermParser.Parse("2a")
            Assert.Fail()
        Catch ex As ArgumentException
        End Try
    End Sub

    <Test()>
    Public Sub TestPiAndE()
        Assert.That(TermParser.Parse("Pi") = System.Math.PI)
        Assert.That(TermParser.Parse("E") = System.Math.E)
    End Sub

    <Test()>
    Public Sub TestCos()
        Assert.That(TermParser.Parse("Cos(0)") = 1)
        Assert.That(TermParser.Parse("Cos(pi)") = -1)
        Assert.That(Abs(TermParser.Parse("Cos(pi/2)")) < 10 ^ -15)
    End Sub

    <Test()>
    Public Sub TestExp()
        Assert.That(TermParser.Parse("exp(1)") = System.Math.E)
    End Sub

End Class
