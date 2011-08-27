﻿Public Class FunctionAssignmentTests

    <Test()>
    Public Sub TestFunction()
        Dim e = New FunctionAssignment("Real f(Real x) = x", context:=TermContext.Default).GetFunctionInstance

        Assert.That(e.Name = "f")

        Dim t = New Term("f{3}", Type:=NamedType.Real, context:=TermContext.Default.Merge(New TermContext(Functions:={e})))
        Assert.That(t.GetDelegate(Of Func(Of Double)).Invoke = 3)
    End Sub

    <Test()>
    Public Sub TestNamedParameter()
        Dim e = New FunctionAssignment("Real f(Real x) = x", context:=TermContext.Default).GetFunctionInstance

        Assert.That(e.Name = "f")

        Dim context = TermContext.Default.Merge(New TermContext(Functions:={e}))

        Dim t = New Term("f{x : 3}", Type:=NamedType.Real, context:=context)
        Assert.That(t.GetDelegate(Of Func(Of Double)).Invoke = 3)

        Try
            Dim t2 = New Term("f{y : 3}", Type:=NamedType.Real, context:=context).GetDelegate(Of Func(Of Double))()
            Assert.Fail()
        Catch ex As InvalidTermException
            Assert.AreEqual(ex.Message, "Wrong parameter name: 'y'; 'x' expected.")
        End Try
    End Sub

    <Test()>
    Public Sub TestMultiParameters()
        Dim definition = New FunctionAssignment("  Real product(    Real x, Real y) = x*y", context:=TermContext.Default).GetFunctionInstance

        Dim t = New Term("Product {4 , 2}", Type:=NamedType.Real, context:=TermContext.Default.Merge(New TermContext(Functions:={definition})))

        Assert.AreEqual("product", definition.Name)
        Assert.AreEqual(8, t.GetDelegate(Of Func(Of Double)).Invoke)
    End Sub

    <Test()>
    Public Sub TestWrongArgumentCount()
        Dim definition = New FunctionAssignment("Real product(Real x,Real y) = x*y", context:=TermContext.Default).GetFunctionInstance

        Try
            Dim t = New Term("product {4}", Type:=NamedType.Real, context:=TermContext.Default.Merge(New TermContext(Functions:={definition}))).GetDelegate
            Assert.Fail()
        Catch ex As ArgumentException
            Assert.That(ex.Message.Contains("Wrong argument count"))
        End Try
    End Sub

    <Test()>
    Public Sub TestWrongArgumentType()
        Dim definition = New FunctionAssignment("Real product(Real x,Real y) = x*y", context:=TermContext.Default).GetFunctionInstance

        Try
            Dim t = New Term("product {4, [4,3,3]}", Type:=NamedType.Real, context:=TermContext.Default.Merge(New TermContext(Functions:={definition}))).GetDelegate
            Assert.Fail()
        Catch ex As ArgumentException
            Assert.AreEqual(ex.Message, "Type 'Vector' is not compatible to type 'Real'.")
        End Try
    End Sub

    <Test()>
    Public Sub TestNestedFunctions()
        Dim product = New FunctionAssignment("Real product(Real x,Real y) = x*y", context:=TermContext.Default).GetFunctionInstance
        Dim quotient = New FunctionAssignment("Real quotient(Real x,Real y) = x/y", context:=TermContext.Default).GetFunctionInstance

        Assert.AreEqual(New Term("product{4, quotient{1, 4}}", Type:=NamedType.Real, context:=TermContext.Default.Merge(New TermContext(Functions:={product, quotient}))).GetDelegate(Of Func(Of Double)).Invoke, 1)
    End Sub

    <Test()>
    Public Sub TestDelegateAsParameter()
        Dim intensityDelegate = NamedType.NamedDelegateTypeFromText("delegate Real IntensityDelegate(Real wavelength)", typeContext:=NamedTypes.Default)
        Dim context = TermContext.Default.Merge(New TermContext(types:=New NamedTypes({intensityDelegate})))

        Dim intensity = New FunctionAssignment("Real Intensity(Real wavelength) = 2*wavelength", context:=context).GetFunctionInstance
        Dim context2 = context.Merge(New TermContext(Functions:={intensity}))

        Dim calculate = New FunctionAssignment("Real IntensityAt1(IntensityDelegate intensityDelegateInstance) = intensityDelegateInstance{1}", context:=context2).GetFunctionInstance

        Assert.AreEqual(New Term("intensityAt1{Intensity}", Type:=NamedType.Real, context:=context2.Merge(New TermContext(Functions:={calculate}))).GetDelegate(Of Func(Of Double)).Invoke, 2)
    End Sub

End Class