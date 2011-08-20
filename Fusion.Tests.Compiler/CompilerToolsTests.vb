Public Class CompilerToolsTests

    <Test()>
    Public Sub TestGetArguments()
        Assert.AreEqual({"3", " f(3, 4)", " <3,4, 5>   "}.ToList, CompilerTools.GetArguments("{3, f(3, 4), <3,4, 5>   }").ToList)
    End Sub

    <Test()>
    Public Sub TestSplitIfSeparatorIsNotInBrackets()
        Assert.AreEqual({"3", " f(3, 4)", " <3,4, 5>   "}.ToList, CompilerTools.SplitIfSeparatorIsNotInBrackets("3, f(3, 4), <3,4, 5>   ", ","c, bracketTypes:=CompilerTools.AllowedBracketTypes).ToList)
    End Sub

    <Test()>
    Public Sub TestGetStartingValidVariableName()
        Assert.AreEqual("a", CompilerTools.GetStartingValidVariableName("a"))
        Assert.Throws(Of ArgumentException)(Sub() CompilerTools.GetStartingValidVariableName("'a"))
    End Sub

    <Test()>
    Public Sub TestGetStartingType()
        Dim rest As String = Nothing
        Dim type = CompilerTools.GetStartingType("Collection<Real>", types:=NamedTypes.Default, out_rest:=rest)
        Dim argument = type.TypeArguments.Single

        Assert.AreEqual(rest, "")

        Assert.AreEqual(type.SystemType, GetType(IEnumerable(Of Double)))
        Assert.AreEqual(argument.SystemType, GetType(Double))
        Assert.AreEqual(type.Name, "Collection")
        Assert.AreEqual(argument.Name, "Real")
    End Sub

    <Test()>
    Public Sub TestGetStartingTypedAndNamedVariable()
        Dim t = CompilerTools.GetStartingTypedAndNamedVariable("Real a", types:=New NamedTypes({NamedType.Real}))
        Assert.AreEqual("a", t.Name)
        Assert.AreEqual(NamedType.Real, t.Type)

        Dim rest = ""
        Dim t2 = CompilerTools.GetStartingTypedAndNamedVariable("Vector b (3,4)", types:=New NamedTypes({NamedType.Vector3D}), out_rest:=rest)
        Assert.AreEqual("b", t2.Name)
        Assert.AreEqual(NamedType.Vector3D, t2.Type)
        Assert.AreEqual(" (3,4)", rest)
    End Sub

End Class
