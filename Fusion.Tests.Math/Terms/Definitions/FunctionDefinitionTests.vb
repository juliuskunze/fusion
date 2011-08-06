Public Class FunctionDefinitionTests

    <Test()>
    Public Sub Test()
        Dim e = New FunctionDefinition("f(x) = x", userContext:=TermContext.Empty).GetNamedFunctionExpression

        Assert.That(e.Name = "f")

        Dim t = New Term("f(3)", userContext:=New TermContext(constants:={}, parameters:={}, Functions:={e}))
        Assert.That(t.GetDelegate(Of Func(Of Double)).Invoke = 3)
    End Sub

    <Test()>
    Public Sub TestMultiParameters()
        Dim definition = New FunctionDefinition("product(x,y) = x*y", userContext:=TermContext.Empty).GetNamedFunctionExpression

        Dim t = New Term("Product (4 , 2)", userContext:=New TermContext(constants:={}, parameters:={}, Functions:={definition}))

        Assert.AreEqual("product", definition.Name)
        Assert.AreEqual(8, t.GetDelegate(Of Func(Of Double)).Invoke)
    End Sub

    <Test()>
    Public Sub TestWrongParameterCount()
        Dim definition = New FunctionDefinition("product(x,y) = x*y", userContext:=TermContext.Empty).GetNamedFunctionExpression

        Try
            Dim t = New Term("product (4)", userContext:=New TermContext(constants:={}, parameters:={}, Functions:={definition})).GetDelegate
            Assert.Fail()
        Catch ex As Exception
            Assert.That(ex.Message.Contains("Wrong argument count"))
        End Try
    End Sub

End Class
