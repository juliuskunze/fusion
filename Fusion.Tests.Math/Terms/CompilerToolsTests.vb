Public Class CompilerToolsTests

    <Test()>
    Public Sub TestGetArguments()
        Assert.AreEqual({"3", " f(3, 4)", " <3,4, 5>   "}.ToList, CompilerTools.GetArguments("(3, f(3, 4), <3,4, 5>   )").ToList)
    End Sub

    <Test()>
    Public Sub TestSplitIfSeparatorIsNotInBrackets()
        Assert.AreEqual({"3", " f(3, 4)", " <3,4, 5>   "}.ToList, CompilerTools.SplitIfSeparatorIsNotInBrackets("3, f(3, 4), <3,4, 5>   ", ","c, bracketTypes:=CompilerTools.AllowedBracketTypes).ToList)
    End Sub

End Class
