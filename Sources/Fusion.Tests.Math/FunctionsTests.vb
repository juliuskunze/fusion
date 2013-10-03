Imports Fusion.Math.Functions

Public Class FunctionsTests
    <Test()> Public Sub TestGcd()
        Assert.True(Gcd(23, 45) = 1)
        Assert.True(Gcd(12, 24) = 12)
        Assert.True(Gcd(24, 36) = 12)
        Assert.True(Gcd(0, 10) = 10)

        Assert.True(Gcd(5, -10) = 5)
        Assert.True(Gcd(-12, -18) = 6)

        Try
            Gcd(0, 0)
            Assert.Fail()
        Catch ex As ArgumentException
        End Try
    End Sub

    <Test()> Public Sub TestLcm()
        Assert.True(Lcm(5, 7) = 35)
        Assert.True(Lcm(12, 24) = 24)
        Assert.True(Lcm(4, 10) = 20)
        Assert.True(Lcm(0, 10) = 0)

        Assert.True(Lcm(5, -10) = 10)
        Assert.True(Lcm(-4, -10) = 20)
        Assert.True(Lcm(0, 0) = 0)
    End Sub

    <Test()>
    Public Sub TestChoosingPossibilityCount()
        Assert.AreEqual(0, ChoosingPossibilityCount(total:=3, chosen:=5, respectOrder:=False, respectDuplication:=False))
        Assert.AreEqual(12, ChoosingPossibilityCount(total:=12, chosen:=1, respectOrder:=False, respectDuplication:=False))
    End Sub

    <Test>
    Public Sub Test_Primes()
        Primes(upperBound:=2).Single.Should.Be.EqualTo(2)
    End Sub

    <Test>
    Public Sub Test_Primes_Sum()
        Primes(upperBound:=5000000).Select(Function(x) CLng(x)).Sum.Should.Be.EqualTo(838596693108)
    End Sub

    <Test>
    Public Sub Test_BigInteger_Power_Digit_Sum()
        BigInteger.Pow(New BigInteger(2), 3000).ToString.Select(Function(x) CInt(x.ToString)).Sum().Should.Be.EqualTo(3871)
    End Sub 
End Class
