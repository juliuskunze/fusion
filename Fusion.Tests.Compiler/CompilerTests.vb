Public Class CompilerTests

    <Test()>
    Public Sub TestConstant()
        Dim definition = New Compiler(Of Double)("Real a = 4; Real b = a/2; return b;", baseContext:=TermContext.Default, TypeNamedTypeDictionary:=TypeNamedTypeDictionary.Default)
        Assert.AreEqual(2, definition.GetResult.Result)
    End Sub

    <Test()>
    Public Sub TestFunction()
        Dim definition = New Compiler(Of Double)("Real square(Real x) = x^2; Real c = square{4}; return c;", baseContext:=TermContext.Default, TypeNamedTypeDictionary:=TypeNamedTypeDictionary.Default)

        Assert.AreEqual(definition.GetResult.Result, 16)
    End Sub

    <Test()>
    Public Sub TestConstantNotDefined()
        Dim definitions = New Compiler(Of Double)("Real a = b; Real b = a; return b", baseContext:=TermContext.Default, TypeNamedTypeDictionary:=TypeNamedTypeDictionary.Default)
        Try
            definitions.GetResult()
            Assert.Fail()
        Catch ex As CompilerException
            Assert.AreEqual("'b' is not defined in this context.", ex.Message)
        End Try
    End Sub

    <Test()>
    Public Sub TestCompileNothing()
        Dim definitions = New Compiler(Of Double)("", baseContext:=TermContext.Default, TypeNamedTypeDictionary:=TypeNamedTypeDictionary.Default)
        Try
            definitions.GetResult()
            Assert.Fail()
        Catch ex As CompilerException
            Assert.AreEqual("Missing return statement.", ex.Message)
        End Try
    End Sub

End Class
