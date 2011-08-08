Public Class DefinitionsTests

    <Test()>
    Public Sub TestConstant()
        Dim definitions = New Definitions("Real a = 4" & Microsoft.VisualBasic.ControlChars.Cr & "Real b = a/2")
        Assert.AreEqual(2, definitions.GetTermContext.Constants.Count)
        Dim a = definitions.GetTermContext.Constants.First
        Dim b = definitions.GetTermContext.Constants.Last

        Assert.AreEqual("a", a.Instance.Name)
        Assert.AreEqual(4, CDbl(a.Expression.Value))

        Assert.AreEqual("b", b.Instance.Name)
        Assert.AreEqual(2, CDbl(b.Expression.Value))
    End Sub

    <Test()>
    Public Sub TestFunction()
        Dim definitions = New Definitions("Real square(Real x) = x^2" & Microsoft.VisualBasic.ControlChars.Cr & "Real c = square{4}")

        Dim square = definitions.GetTermContext.Functions.Single
        Dim c = definitions.GetTermContext.Constants.Single

        Assert.AreEqual("c", c.Instance.Name)
        Assert.AreEqual(16, CDbl(c.Expression.Value))

        Assert.AreEqual("square", square.Type.Name)
    End Sub

    <Test()>
    Public Sub TestConstantNotDefined()
        Dim definitions = New Definitions("Real a = b" & Microsoft.VisualBasic.ControlChars.Cr & "Real b = a")
        Try
            definitions.GetTermContext()
            Assert.Fail()
        Catch ex As ArgumentException
            Assert.That(ex.Message.Contains("'b' is not defined in this context"))
        End Try
    End Sub

End Class
