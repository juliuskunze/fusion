Public Class NamedConstantDefinitionTests

    <Test()>
    Public Sub Test()
        Dim d = New NamedConstantDefinition(definition:="a = 4", userContext:=TermContext.Empty)
        Dim e = d.GetNamedConstantExpression
        Assert.That(e.Name = "a")
        Assert.That(CDbl(e.ConstantExpression.Value) = 4)
    End Sub


    <Test()>
    Public Sub Test2()
        Dim d = New NamedConstantDefinition(definition:="b12 = (2+2)/4", userContext:=TermContext.Empty)
        Dim e = d.GetNamedConstantExpression
        Assert.That(e.Name = "b12")
        Assert.That(CDbl(e.ConstantExpression.Value) = 1)
    End Sub

    <Test()>
    Public Sub Test3()
        Dim d = New NamedConstantDefinition(definition:="b = (a+2)/4", userContext:=New TermContext(constants:={New NamedConstantDefinition(definition:="a = 2", userContext:=TermContext.Empty).GetNamedConstantExpression}, parameters:={}, Functions:={}))

        Dim e = d.GetNamedConstantExpression
        Assert.That(e.Name = "b")
        Assert.That(CDbl(e.ConstantExpression.Value) = 1)
    End Sub

End Class
