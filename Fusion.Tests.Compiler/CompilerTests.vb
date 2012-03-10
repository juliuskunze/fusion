Public Class CompilerTests

    <Test()>
    Public Sub TestConstant()
        Dim compiler = New Compiler(Of Double)("Real a = 4; Real b = a/2; return b;".ToLocated, baseContext:=TermContext.Default, typeDictionary:=TypeDictionary.Default)
        Assert.AreEqual(2, compiler.Compile.Result)
    End Sub

    <Test()>
    Public Sub TestFunction()
        Dim compiler = New Compiler(Of Double)("Real square(Real x) = x^2; Real c = square(4); return c;".ToLocated, baseContext:=TermContext.Default, typeDictionary:=TypeDictionary.Default)

        Assert.AreEqual(compiler.Compile.Result, 16)
    End Sub

    <Test()>
    Public Sub TestConstantNotDefined()
        Dim compiler = New Compiler(Of Double)("Real a = b; Real b = a; return b".ToLocated, baseContext:=TermContext.Default, typeDictionary:=TypeDictionary.Default)
        Try
            compiler.Compile()
            Assert.Fail()
        Catch ex As CompilerExceptionWithHelp
            Assert.AreEqual("'b' is not defined in this context.", DirectCast(ex.InnerCompilerException, LocatedCompilerException).Message)
        End Try
    End Sub

    <Test()>
    Public Sub TestCompileNothing()
        Dim compiler = New Compiler(Of Double)("".ToLocated, baseContext:=TermContext.Default, typeDictionary:=TypeDictionary.Default)
        Try
            compiler.Compile()
            Assert.Fail()
        Catch ex As CompilerExceptionWithHelp
            Assert.AreEqual("Missing return statement.", ex.InnerCompilerException.Message)
        End Try
    End Sub

    Private Shared Sub TestCollectionTypeMismatch(s As String)
        Dim compiler = New Compiler(Of Double)(s.ToLocated, baseContext:=TermContext.Default, TypeDictionary:=TypeDictionary.Default)
        Try
            compiler.Compile()
            Assert.Fail()
        Catch ex As CompilerExceptionWithHelp
            Assert.AreEqual("Type 'Real' expected.", DirectCast(ex.InnerCompilerException, LocatedCompilerException).Message)
        End Try
    End Sub

    <Test()>
    Public Sub TestCollectionTypeMismatch()
        TestCollectionTypeMismatch("Real a = {}")
        TestCollectionTypeMismatch("Real a = {1}")
        TestCollectionTypeMismatch("Real a = {[1,0,0]}")
    End Sub

    <Test()>
    Public Sub TestEmptyCollection()
        Dim compiler = New Compiler(Of IEnumerable(Of Double))("return {}".ToLocated, baseContext:=TermContext.Default, typeDictionary:=TypeDictionary.Default)

        Assert.AreEqual(compiler.Compile.Result, New Double() {})
    End Sub

    <Test()>
    Public Sub Test_MatchingFunctionGroup()
        Dim sinusFunction2 = FunctionInstance.FromLambdaExpression("Sin", Function(a As Double, b As Double) a + b, TypeDictionary.Default)
        Dim compiler = New Compiler(Of Double)(baseContext:=TermContext.Default.Merge(New TermContext(Functions:={sinusFunction2})), typeDictionary:=TypeDictionary.Default)

        compiler.Update("return Sin(1,2)".ToLocated)
        Dim result = compiler.Compile.Result

        Assert.AreEqual(3, result)
    End Sub

    <Test()>
    Public Sub Test_Function()
        Const term = "Real flair(Real wavelength) =  1; Real flair2(Real wavelength) = flair(wavelength) * 100; return flair2(0)"

        Dim compiler = New Compiler(Of Double)(LocatedString:=term.ToLocated,
                                                                baseContext:=TermContext.Default.Merge(New TermContext(types:=New NamedTypes({New NamedType("RadianceSpectrum", New FunctionType(NamedType.Real, Parameters:={New NamedParameter("wavelength", NamedType.Real)}))}))),
                                                                TypeDictionary:=TypeDictionary.Default)

        compiler.Compile()
    End Sub

End Class
