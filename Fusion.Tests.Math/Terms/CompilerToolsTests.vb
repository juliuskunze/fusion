Public Class CompilerToolsTests

    <Test()>
    Public Sub TestGetArguments()
        Assert.AreEqual({"3", " f(3, 4)", " <3,4, 5>   "}.ToList, CompilerTools.GetArguments("(3, f(3, 4), <3,4, 5>   )").ToList)
    End Sub

    <Test()>
    Public Sub TestSplitIfSeparatorIsNotInBrackets()
        Assert.AreEqual({"3", " f(3, 4)", " <3,4, 5>   "}.ToList, CompilerTools.SplitIfSeparatorIsNotInBrackets("3, f(3, 4), <3,4, 5>   ", ","c, bracketTypes:=CompilerTools.AllowedBracketTypes).ToList)
    End Sub

    Public Sub TestGetStartingValidVariableName()
        Assert.AreEqual("a", CompilerTools.GetStartingValidVariableName("a"))
        Assert.AreEqual("a   ", CompilerTools.GetStartingValidVariableName("a"))
        Assert.Throws(Of ArgumentException)(Sub() CompilerTools.GetStartingValidVariableName("a"))
    End Sub

    Public Sub TestGetStartingTypedAndNamedVariable()
        Dim t = CompilerTools.GetStartingTypedAndNamedVariable("Real a", types:=New NamedTypes({NamedType.Real}))
        Assert.AreEqual("a", t.Name)
        Assert.AreEqual(NamedType.Real, t.Type)

        Dim rest = ""
        Dim t2 = CompilerTools.GetStartingTypedAndNamedVariable("Vector b (3,4)", types:=New NamedTypes({NamedType.Vector3D}), out_rest:=rest)
        Assert.AreEqual("b", t2.Name)
        Assert.AreEqual(NamedType.Vector3D, t2.Type)
        Assert.AreEqual(" (3,4)", rest)

        Assert.Throws(Of ArgumentException)(Sub() CompilerTools.GetStartingValidVariableName("a"))
    End Sub

End Class
