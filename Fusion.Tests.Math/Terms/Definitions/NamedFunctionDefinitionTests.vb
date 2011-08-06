Public Class NamedFunctionDefinitionTests

    <Test()>
    Public Sub Test()
        Dim e = New NamedFunctionDefinition("f(x) = x", userContext:=TermContext.Empty).GetNamedFunctionExpression

        Assert.That(e.Name = "f")

        Dim T = New Term("f(3)", userContext:=New TermContext(constants:={}, parameters:={}, Functions:={e}))
        Assert.That(T.GetDelegate(Of Func(Of Double)).Invoke = 3)

    End Sub


End Class
