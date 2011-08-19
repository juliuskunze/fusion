Public Class DefinitionsTests

    <Test()>
    Public Sub TestConstant()
        Dim definitions = New Definitions("Real a = 4" & Microsoft.VisualBasic.ControlChars.Cr & "Real b = a/2", baseContext:=TermContext.Default)
        Assert.AreEqual(4, definitions.GetTermContext.Constants.Count)
        definitions.GetTermContext.Constants.Where(Function(i) i.Signature.Name = "a" AndAlso CDbl(i.Expression.Value) = 4).Single()
        definitions.GetTermContext.Constants.Where(Function(i) i.Signature.Name = "b" AndAlso CDbl(i.Expression.Value) = 2).Single()
    End Sub

    <Test()>
    Public Sub TestFunction()
        Dim definitions = New Definitions("Real square(Real x) = x^2" & Microsoft.VisualBasic.ControlChars.Cr & "Real c = square{4}", baseContext:=TermContext.Default)

        definitions.GetTermContext.Functions.Where(Function(i) i.Name = "square").Single()
        definitions.GetTermContext.Constants.Where(Function(i) i.Signature.Name = "c" AndAlso CDbl(i.Expression.Value) = 16).Single()
    End Sub

    <Test()>
    Public Sub TestConstantNotDefined()
        Dim definitions = New Definitions("Real a = b" & Microsoft.VisualBasic.ControlChars.Cr & "Real b = a", baseContext:=TermContext.Default)
        Try
            definitions.GetTermContext()
            Assert.Fail()
        Catch ex As ArgumentException
            Assert.That(ex.Message.Contains("'b' is not defined in this context"))
        End Try
    End Sub

End Class
