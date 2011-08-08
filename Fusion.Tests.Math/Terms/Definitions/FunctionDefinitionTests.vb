Public Class FunctionDefinitionTests

    <Test()>
    Public Sub TestFunction()
        Dim e = New FunctionDefinition("Real f(Real x) = x", userContext:=TermContext.Empty).GetNamedFunctionExpression

        Assert.That(e.DelegateType.Name = "f")

        Dim t = New Term("f{3}", Type:=NamedType.Real, userContext:=New TermContext(constants:={}, parameters:={}, Functions:={e}))
        Assert.That(t.GetDelegate(Of Func(Of Double)).Invoke = 3)
    End Sub

    <Test()>
    Public Sub TestMultiParameters()
        Dim definition = New FunctionDefinition("  Real product(    Real x, Real y) = x*y", userContext:=TermContext.Empty).GetNamedFunctionExpression

        Dim t = New Term("Product {4 , 2}", Type:=NamedType.Real, userContext:=New TermContext(constants:={}, parameters:={}, Functions:={definition}))

        Assert.AreEqual("product", definition.DelegateType.Name)
        Assert.AreEqual(8, t.GetDelegate(Of Func(Of Double)).Invoke)
    End Sub

    <Test()>
    Public Sub TestWrongArgumentCount()
        Dim definition = New FunctionDefinition("Real product(Real x,Real y) = x*y", userContext:=TermContext.Empty).GetNamedFunctionExpression

        Try
            Dim t = New Term("product {4}", Type:=NamedType.Real, userContext:=New TermContext(constants:={}, parameters:={}, Functions:={definition})).GetDelegate
            Assert.Fail()
        Catch ex As ArgumentException
            Assert.That(ex.Message.Contains("Wrong argument count"))
        End Try
    End Sub

    <Test()>
    Public Sub TestWrongArgumentType()
        Dim definition = New FunctionDefinition("Real product(Real x,Real y) = x*y", userContext:=TermContext.Empty).GetNamedFunctionExpression

        Try
            Dim t = New Term("product {4, <4,3,3>}", Type:=NamedType.Real, userContext:=New TermContext(constants:={}, parameters:={}, Functions:={definition})).GetDelegate
            Assert.Fail()
        Catch ex As ArgumentException
            Assert.AreEqual(ex.Message, "Type 'Vector' is not compatible to type 'Real'.")
        End Try
    End Sub

    <Test()>
    Public Sub TestNestedFunctions()
        Dim product = New FunctionDefinition("Real product(Real x,Real y) = x*y", userContext:=TermContext.Empty).GetNamedFunctionExpression
        Dim quotient = New FunctionDefinition("Real quotient(Real x,Real y) = x/y", userContext:=TermContext.Empty).GetNamedFunctionExpression

        Assert.AreEqual(New Term("product{4, quotient{1, 4}}", Type:=NamedType.Real, userContext:=New TermContext(constants:={}, parameters:={}, Functions:={product, quotient})).GetDelegate(Of Func(Of Double)).Invoke, 1)
    End Sub

    <Test()>
    Public Sub TestDelegateAsParameter()
        Dim exampleWavelengthFunction = New FunctionDefinition("Real i(Real wavelength) = 2*i)", userContext:=TermContext.Empty).GetNamedFunctionExpression
        Dim calculate = New FunctionDefinition("Real intensityAt1(Real intensityFunction(Real wavelength)) = intensityFunction{1}", userContext:=New TermContext(constants:={}, parameters:={}, Functions:={exampleWavelengthFunction}, types:=NamedTypes.Empty)).GetNamedFunctionExpression

        Assert.AreEqual(New Term("intensityAt1{i}", Type:=NamedType.Real, userContext:=New TermContext(constants:={}, parameters:={}, Functions:={exampleWavelengthFunction, calculate})).GetDelegate(Of Func(Of Double)).Invoke, 2)
    End Sub

End Class
