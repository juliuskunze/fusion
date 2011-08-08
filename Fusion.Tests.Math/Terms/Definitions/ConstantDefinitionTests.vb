Public Class ConstantDefinitionTests

    <Test()>
    Public Sub Test()
        Dim d = New ConstantDefinition(definition:="Real a = 4", userContext:=TermContext.Empty)
        Dim e = d.GetNamedConstantExpression
        Assert.AreEqual(e.Name, "a")
        Assert.AreEqual(CDbl(e.ConstantExpression.Value), 4)
    End Sub


    <Test()>
    Public Sub Test2()
        Dim d = New ConstantDefinition(definition:="Real b12 = (2+2)/4", userContext:=TermContext.Empty)
        Dim e = d.GetNamedConstantExpression
        Assert.That(e.Name = "b12")
        Assert.That(CDbl(e.ConstantExpression.Value) = 1)
    End Sub

    <Test()>
    Public Sub Test3()
        Dim d = New ConstantDefinition(Definition:="Real b = (a+2)/4", userContext:=New TermContext(constants:={New ConstantDefinition(Definition:="Real a = 2", userContext:=TermContext.Empty).GetNamedConstantExpression}, parameters:={}, Functions:={}, types:=NamedTypes.DefaultTypes))

        Dim e = d.GetNamedConstantExpression
        Assert.That(e.Name = "b")
        Assert.That(CDbl(e.ConstantExpression.Value) = 1)
    End Sub

End Class
