Public Class CompilerToolsTests

    <Test()>
    Public Sub TestGetArguments()
        Assert.AreEqual({"3", " f(3, 4)", " [3,4, 5]   "}.ToList, CompilerTools.GetArguments("(3, f(3, 4), [3,4, 5]   )".ToLocated).Select(Function(located) located.ToString).ToList)
    End Sub

    <Test()>
    Public Sub TestSplitIfSeparatorIsNotInBrackets()
        Assert.AreEqual({"3", " f(3, 4)", " [3,4, 5]   "}.ToList, CompilerTools.SplitIfSeparatorIsNotInBrackets("3, f(3, 4), [3,4, 5]   ".ToLocated, ","c, bracketTypes:=CompilerTools.AllowedBracketTypes).Select(Function(located) located.ToString).ToList)
    End Sub

    <Test()>
    Public Sub TestGetStartingValidVariableName()
        Assert.AreEqual("a", CompilerTools.GetStartingIdentifier("a".ToLocated).ToString)
        Assert.Throws(Of LocatedCompilerException)(Sub() CompilerTools.GetStartingIdentifier("'a".ToLocated))
    End Sub

    <Test()>
    Public Sub TestGetStartingType()
        Dim rest As LocatedString = Nothing
        Dim type = CompilerTools.GetStartingType("Set[Real]".ToLocated, types:=NamedTypes.Default, out_rest:=rest)
        Dim argument = type.TypeArguments.Single

        Assert.AreEqual(rest.ToString, "")

        Assert.AreEqual(type.SystemType, GetType(IEnumerable(Of Double)))
        Assert.AreEqual(argument.SystemType, GetType(Double))
        Assert.AreEqual(type.Name, "Set")
        Assert.AreEqual(argument.Name, "Real")
    End Sub

    <Test()>
    Public Sub TestGetStartingTypedAndNamedVariable()
        Dim t = CompilerTools.GetStartingTypedAndNamedVariable("Real a".ToLocated, types:=New NamedTypes({NamedType.Real}))
        Assert.AreEqual("a", t.Name)
        Assert.AreEqual(NamedType.Real, t.Type)

        Dim rest As LocatedString = Nothing
        Dim t2 = CompilerTools.GetStartingTypedAndNamedVariable("Vector b (3,4)".ToLocated, types:=New NamedTypes({NamedType.Vector3D}), out_rest:=rest)
        Assert.AreEqual("b", t2.Name)
        Assert.AreEqual(NamedType.Vector3D, t2.Type)
        Assert.AreEqual(" (3,4)", rest.ToString)
    End Sub

End Class
