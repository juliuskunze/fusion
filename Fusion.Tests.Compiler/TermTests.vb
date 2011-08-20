Public Class TermTests

    <Test()>
    Public Sub TestIndependentTerm1()
        Assert.True(New ConstantTerm(Of Double)("5").GetResult = 5)
        Assert.True(New ConstantTerm(Of Double)("(5)").GetResult = 5)
        Assert.True(New ConstantTerm(Of Double)("1+2").GetResult = 3)
        Assert.True(New ConstantTerm(Of Double)("(1+2)").GetResult = 3)
    End Sub

    <Test()>
    Public Sub TestIndependentTerm2()
        Assert.True(New ConstantTerm(Of Double)("2 *3+ 5 ").GetResult = 11)
        Assert.True(New ConstantTerm(Of Double)("5+3*4").GetResult = 17)
        Assert.True(New ConstantTerm(Of Double)("3*(2 +3)").GetResult = 15)
        Assert.True(New ConstantTerm(Of Double)("3^ 2").GetResult = 9)
    End Sub

    <Test()>
    Public Sub TestIndependentTerm3()
        Assert.True(New ConstantTerm(Of Double)("((3*(4+3/(2+1*1*1+1-1^(1+0)))*2/2))").GetResult = 15)
        Assert.True(New ConstantTerm(Of Double)("2^3^2").GetResult = New ConstantTerm(Of Double)("2^(3^2)").GetResult)
    End Sub

    <Test()>
    Public Sub TestIndependentTerm4()
        Assert.True(New ConstantTerm(Of Double)("-5").GetResult = -5)
        Assert.True(New ConstantTerm(Of Double)("--++-5").GetResult = -5)
    End Sub

    <Test()>
    Public Sub TestIndependentTerm5()
        Assert.True(New ConstantTerm(Of Double)("-3+3").GetResult = 0)
        Assert.True(New ConstantTerm(Of Double)("-3+3-3+3").GetResult = 0)
    End Sub

    <Test()>
    Public Sub TestIndependentTerm6()
        Try
            Dim value = New ConstantTerm(Of Double)("").GetResult
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            Dim value = New ConstantTerm(Of Double)("4)").GetResult
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            Dim value = New ConstantTerm(Of Double)("23+(3+4))").GetResult
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            Dim value = New ConstantTerm(Of Double)("(((").GetResult
            Assert.Fail()
        Catch ex As ArgumentException
        End Try

        Try
            Dim value = New ConstantTerm(Of Double)("2a").GetResult
            Assert.Fail()
        Catch ex As ArgumentException
        End Try
    End Sub

    <Test()>
    Public Sub TestPiAndE()
        Assert.That(New ConstantTerm(Of Double)("Pi").GetResult = System.Math.PI)
        Assert.That(New ConstantTerm(Of Double)("E").GetResult = System.Math.E)
    End Sub

    <Test()>
    Public Sub TestCos()
        Assert.That(New ConstantTerm(Of Double)("Cos{0}").GetResult = 1)
        Assert.That(New ConstantTerm(Of Double)("Cos{pi}").GetResult = -1)
        Assert.That(New ConstantTerm(Of Double)("Cos{pi/2}").GetResult < 10 ^ -15)
    End Sub

    <Test()>
    Public Sub TestExp()
        Assert.That(New ConstantTerm(Of Double)("exp{1}").GetResult = System.Math.E)
    End Sub

    <Test()>
    Public Sub TestMax()
        Assert.That(New ConstantTerm(Of Double)("Max{1, 3}").GetResult = 3)
    End Sub

    <Test()>
    Public Sub TestMin()
        Assert.That(New ConstantTerm(Of Double)("Min{1, 3}").GetResult = 1)
    End Sub

    <Test()>
    Public Sub TestVector3D()
        Assert.That(New ConstantTerm(Of Vector3D)("<3,4,4>").GetResult = New Vector3D(3, 4, 4))
    End Sub

    <Test()>
    Public Sub TestParameter()
        Assert.That(New Term("a+4", Type:=NamedType.Real, context:=TermContext.Default.Merge(New TermContext(parameters:={New NamedParameter(name:="a", Type:=NamedType.Real)}))).GetDelegate(Of Func(Of Double, Double)).Invoke(5) = 9)
        Assert.That(New Term("x^2 + x", Type:=NamedType.Real, context:=TermContext.Default.Merge(New TermContext(parameters:={New NamedParameter(name:="x", Type:=NamedType.Real)}))).GetDelegate(Of Func(Of Double, Double)).Invoke(5) = 30)
        Assert.That(New Term(" a1 ^2 + a1 ", Type:=NamedType.Real, context:=TermContext.Default.Merge(New TermContext(parameters:={New NamedParameter(name:="a1", Type:=NamedType.Real)}))).GetDelegate(Of Func(Of Double, Double)).Invoke(5) = 30)
        Assert.That(New Term("a1^2 + a2", Type:=NamedType.Real, context:=TermContext.Default.Merge(New TermContext(parameters:={New NamedParameter(name:="a1", Type:=NamedType.Real), New NamedParameter(name:="a2", Type:=NamedType.Real)}))).GetDelegate(Of Func(Of Double, Double, Double)).Invoke(5, 3) = 28)
    End Sub

    <Test()>
    Public Sub TestFunction()
        Dim namedMethodExpression = New FunctionInstance(Of Func(Of Double, Double))("square", Function(x As Double) x ^ 2, TypeNamedTypeDictionary:=New TypeNamedTypeDictionary(NamedTypes.Default))
        Dim term = New Term("square{2*x}", Type:=NamedType.Real, context:=TermContext.Default.Merge(New TermContext(parameters:={New NamedParameter(name:="x", Type:=NamedType.Real)}, Functions:={namedMethodExpression})))
        Dim d = term.GetDelegate(Of Func(Of Double, Double))()
        Assert.That(d(5) = 100)
    End Sub

    <Test()>
    Public Sub TestCore()
        Dim f As Expressions.Expression(Of Func(Of Double, Double)) = Function(a As Double) a + 4
        Assert.That(f.Compile()(5) = 9)

        Dim parameter = Expression.Parameter(GetType(Double), "a")
        Dim add = Expression.AddChecked(parameter, Expression.Constant(4.0))
        Dim lambda = Expression.Lambda(Of Func(Of Double, Double))(body:=add, parameters:={parameter})

        Assert.That(lambda.Compile()(5) = 9)
    End Sub

    <Test()>
    Public Sub TestFunctionNotDefined()
        Dim term = New Term("square{1}", Type:=NamedType.Real, context:=TermContext.Default)
        Try
            term.GetDelegate()
            Assert.Fail()
        Catch ex As ArgumentException
            Assert.That(ex.Message = "Function 'square' is not defined in this context.")
        End Try
    End Sub

    <Test()>
    Public Sub TestVector3DWithConstant()
        Assert.That(New Term(Term:="<1,2,a>", Type:=NamedType.Vector3D, context:=TermContext.Default.Merge(New TermContext(parameters:={New NamedParameter(name:="a", Type:=NamedType.Real)}))).GetDelegate(Of Func(Of Double, Vector3D)).Invoke(3.0) = New Vector3D(1, 2, 3))
    End Sub

    <Test()>
    Public Sub TestCollection()
        Dim sequence = New Term(Term:="{3, 4}", Type:=New NamedType(name:="Collection<Real>", systemType:=GetType(IEnumerable(Of Double))), context:=TermContext.Default).GetDelegate(Of Func(Of IEnumerable(Of Double))).Invoke()

        Assert.That(sequence.Count = 2)
        Assert.AreEqual(sequence.First, 3)
        Assert.AreEqual(sequence.Last, 4)
    End Sub

End Class
