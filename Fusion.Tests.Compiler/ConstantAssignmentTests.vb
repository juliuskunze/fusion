Public Class ConstantAssignmentTests

    <Test()>
    Public Sub Test()
        Dim d = New ConstantAssignment(definition:="Real a = 4".ToLocated, context:=TermContext.Default)
        Dim e = d.GetNamedConstantExpression
        Assert.AreEqual(e.Signature.Name, "a")
        Assert.AreEqual(CDbl(e.Expression.Value), 4)
    End Sub


    <Test()>
    Public Sub Test2()
        Dim d = New ConstantAssignment(definition:="Real b12 = (2+2)/4".ToLocated, context:=TermContext.Default)
        Dim e = d.GetNamedConstantExpression
        Assert.That(e.Signature.Name = "b12")
        Assert.That(CDbl(e.Expression.Value) = 1)
    End Sub

    <Test()>
    Public Sub Test3()
        Dim d = New ConstantAssignment(Definition:="Real b = (a+2)/4".ToLocated, context:=New TermContext(constants:={New ConstantAssignment(Definition:="Real a = 2".ToLocated, context:=TermContext.Default).GetNamedConstantExpression}, types:=NamedTypes.Default))

        Dim e = d.GetNamedConstantExpression
        Assert.That(e.Signature.Name = "b")
        Assert.That(CDbl(e.Expression.Value) = 1)
    End Sub

    <Test()>
    Public Sub TestCollection()
        Dim d = New ConstantAssignment(Definition:="Collection[Real] c = {1,2}".ToLocated, context:=TermContext.Default)

        Dim e = d.GetNamedConstantExpression
        Assert.AreEqual(e.Signature.Name, "c")
        Assert.AreEqual(CType(e.Expression.Value, IEnumerable(Of Double)).ToArray, {1, 2})
    End Sub

End Class
