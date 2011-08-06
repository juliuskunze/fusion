Public Class DefinitionsTests

    <Test()>
    Public Sub TestConstant()
        Dim definitions = New Definitions("a = 4" & Microsoft.VisualBasic.ControlChars.Cr & "b = a/2")
        Assert.AreEqual(2, definitions.GetTermContext.Constants.Count)
        Dim a = definitions.GetTermContext.Constants.First
        Dim b = definitions.GetTermContext.Constants.Last

        Assert.AreEqual("a", a.Name)
        Assert.AreEqual(4, CDbl(a.ConstantExpression.Value))

        Assert.AreEqual("b", b.Name)
        Assert.AreEqual(2, CDbl(b.ConstantExpression.Value))
    End Sub

    <Test()>
    Public Sub TestFunction()
        Dim definitions = New Definitions("square(x) = x^2" & Microsoft.VisualBasic.ControlChars.Cr & "c = square(4)")

        Dim square = definitions.GetTermContext.Functions.Single
        Dim c = definitions.GetTermContext.Constants.Single

        Assert.AreEqual("c", c.Name)
        Assert.AreEqual(16, CDbl(c.ConstantExpression.Value))

        Assert.AreEqual("square", square.Name)
    End Sub

    <Test()>
    Public Sub TestConstantNotDefined()
        Dim definitions = New Definitions("a = b" & Microsoft.VisualBasic.ControlChars.Cr & "b = a")
        Try
            definitions.GetTermContext()
            Assert.Fail()
        Catch ex As ArgumentException
            Assert.That(ex.Message.Contains("Constant 'b' is not defined in this context"))
        End Try
    End Sub

End Class
