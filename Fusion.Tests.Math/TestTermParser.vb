Public Class TestTermParser
    <Test()> Public Shared Sub TestTermToValue1()
        Assert.True(TermToValue("5") = 5)
        Assert.True(TermToValue("(5)") = 5)
        Assert.True(TermToValue("1+2") = 3)
        Assert.True(TermToValue("(1+2)") = 3)
    End Sub

    <Test()> Public Shared Sub TestTermToValue2()
        Assert.True(TermToValue("2*3+5") = 11)
        Assert.True(TermToValue("5+3*4") = 17)
        Assert.True(TermToValue("3*(2+3)") = 15)
        Assert.True(TermToValue("3^2") = 9)
    End Sub

    <Test()> Public Shared Sub TestTermToValue3()
        Assert.True(TermToValue("((3*(4+3/(2+1*1*1+1-1^(1+0)))*2/2))") = 15)
        Assert.True(TermToValue("2^3^2") = TermToValue("2^(3^2)"))
    End Sub

    <Test()> Public Shared Sub TestTermToValue4()
        Assert.True(TermToValue("-5") = -5)
        Assert.True(TermToValue("--++-5") = -5)
    End Sub

    <Test()> Public Shared Sub TestTermToValue5()
        Assert.True(TermToValue("-3+3") = 0)
        Assert.True(TermToValue("-3+3-3+3") = 0)
    End Sub

    <Test()> Public Shared Sub TestTermToValue6()
        Try
            TermToValue("")
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            TermToValue("23+(3+4))")
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            TermToValue("(((")
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            TermToValue("2a")
            Assert.Fail()
        Catch ex As ArgumentException
        End Try
    End Sub
End Class
