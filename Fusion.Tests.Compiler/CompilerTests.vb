Public Class CompilerTests

    <Test()>
    Public Sub TestConstant()
        Dim compiler = New Compiler(Of Double)("Real a = 4; Real b = a/2; return b;".ToLocated, baseContext:=TermContext.Default, TypeNamedTypeDictionary:=TypeNamedTypeDictionary.Default)
        Assert.AreEqual(2, compiler.Compile.Result)
    End Sub

    <Test()>
    Public Sub TestFunction()
        Dim compiler = New Compiler(Of Double)("Real square(Real x) = x^2; Real c = square{4}; return c;".ToLocated, baseContext:=TermContext.Default, TypeNamedTypeDictionary:=TypeNamedTypeDictionary.Default)

        Assert.AreEqual(compiler.Compile.Result, 16)
    End Sub

    <Test()>
    Public Sub TestConstantNotDefined()
        Dim compiler = New Compiler(Of Double)("Real a = b; Real b = a; return b".ToLocated, baseContext:=TermContext.Default, TypeNamedTypeDictionary:=TypeNamedTypeDictionary.Default)
        Try
            compiler.Compile()
            Assert.Fail()
        Catch ex As CompilerExceptionWithIntelliSense
            Assert.AreEqual("'b' is not defined in this context.", DirectCast(ex.InnerCompilerExcpetion, LocatedCompilerException).Message)
        End Try
    End Sub

    <Test()>
    Public Sub TestCompileNothing()
        Dim compiler = New Compiler(Of Double)("".ToLocated, baseContext:=TermContext.Default, TypeNamedTypeDictionary:=TypeNamedTypeDictionary.Default)
        Try
            compiler.Compile()
            Assert.Fail()
        Catch ex As CompilerExceptionWithIntelliSense
            Assert.AreEqual("Missing return statement.", ex.InnerCompilerExcpetion.Message)
        End Try
    End Sub

    <Test()>
    Public Sub TestCollectionTypeMismatch()
        Dim compiler = New Compiler(Of Double)("Real a = {1}".ToLocated, baseContext:=TermContext.Default, TypeNamedTypeDictionary:=TypeNamedTypeDictionary.Default)
        Try
            compiler.Compile()
            Assert.Fail()
        Catch ex As CompilerExceptionWithIntelliSense
            Assert.AreEqual("Type 'Collection[Real]' is not assignable to type 'Real'.", DirectCast(ex.InnerCompilerExcpetion, LocatedCompilerException).Message)
        End Try
    End Sub

End Class
