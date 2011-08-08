Public Class ConstantDefinitionTests

    <Test()>
    Public Sub Test()
        Dim d = New ConstantAssignment(definition:="Real a = 4", userContext:=TermContext.Empty)
        Dim e = d.GetNamedConstantExpression
        Assert.AreEqual(e.Instance.Name, "a")
        Assert.AreEqual(CDbl(e.Expression.Value), 4)
    End Sub


    <Test()>
    Public Sub Test2()
        Dim d = New ConstantAssignment(definition:="Real b12 = (2+2)/4", userContext:=TermContext.Empty)
        Dim e = d.GetNamedConstantExpression
        Assert.That(e.Instance.Name = "b12")
        Assert.That(CDbl(e.Expression.Value) = 1)
    End Sub

    <Test()>
    Public Sub Test3()
        Dim d = New ConstantAssignment(Definition:="Real b = (a+2)/4", userContext:=New TermContext(constants:={New ConstantAssignment(Definition:="Real a = 2", userContext:=TermContext.Empty).GetNamedConstantExpression}, parameters:={}, Functions:={}, types:=NamedTypes.DefaultTypes))

        Dim e = d.GetNamedConstantExpression
        Assert.That(e.Instance.Name = "b")
        Assert.That(CDbl(e.Expression.Value) = 1)
    End Sub

End Class
