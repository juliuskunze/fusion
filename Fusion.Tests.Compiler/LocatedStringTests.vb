Public Class LocatedStringTests

    <Test()>
    Public Sub Test()
        Dim container = "0123456".ToLocated
        Assert.AreEqual(container.Location.StartIndex, 0)
        Assert.AreEqual(container.ToString, "0123456")
        Assert.AreEqual(container.Substring(startIndex:=2, length:=3).ToString, "234")
        Assert.AreEqual(container.Substring(startIndex:=2).ToString, "23456")

        Dim trimStart = container.TrimStart(trimChars:={"0"c, "1"c})
        Assert.AreEqual(trimStart.Location.StartIndex, 2)
        Assert.AreEqual(trimStart.Length, 5)
        Assert.AreEqual(trimStart.ToString, "23456")

        Dim trimEnd = container.TrimEnd(trimChars:={"6"c})
        Assert.AreEqual(trimEnd.Location.StartIndex, 0)
        Assert.AreEqual(trimEnd.Length, 6)
        Assert.AreEqual(trimEnd.ToString, "012345")

        Dim trim = container.Trim(trimChars:={"0"c, "6"c})
        Assert.AreEqual(trim.Location.StartIndex, 1)
        Assert.AreEqual(trim.Length, 5)
        Assert.AreEqual(trim.ToString, "12345")
    End Sub

    <Test()>
    Public Sub TestWhiteSpace()
        Dim container = New AnalizedString("  AB ", AllowedBracketTypes:={}).ToLocated
        Assert.AreEqual(container.Location.StartIndex, 0)
        Assert.AreEqual(container.ToString, "  AB ")

        Dim trimStart = container.TrimStart
        Assert.AreEqual(trimStart.Location.StartIndex, 2)
        Assert.AreEqual(trimStart.Length, 3)
        Assert.AreEqual(trimStart.ToString, "AB ")

        Dim trimEnd = container.TrimEnd
        Assert.AreEqual(trimEnd.Location.StartIndex, 0)
        Assert.AreEqual(trimEnd.Length, 4)
        Assert.AreEqual(trimEnd.ToString, "  AB")

        Dim trim = container.Trim
        Assert.AreEqual(trim.Location.StartIndex, 2)
        Assert.AreEqual(trim.Length, 2)
        Assert.AreEqual(trim.ToString, "AB")
    End Sub

    <Test()>
    Public Sub TestSplit()
        Dim s = "A;B;".ToLocated
        Assert.AreEqual({"A", "B", ""}.ToList, s.Split({";"c}).Select(Function(subString) subString.ToString))

    End Sub

End Class
