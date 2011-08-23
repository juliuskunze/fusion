Imports System.Runtime.CompilerServices

Public Module CharExtensions

    <Extension()>
    Public Function IsIdentifierStartChar(c As Char) As Boolean
        Return Char.IsLetter(c)
    End Function

    <Extension()>
    Public Function IsIdentifierChar(c As Char) As Boolean
        Return Char.IsLetterOrDigit(c)
    End Function

End Module
