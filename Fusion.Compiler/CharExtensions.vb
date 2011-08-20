Imports System.Runtime.CompilerServices

Public Module CharExtensions

    <Extension()>
    Public Function IsValidVariableStartChar(c As Char) As Boolean
        Return Char.IsLetter(c)
    End Function

    <Extension()>
    Public Function IsValidVariableChar(c As Char) As Boolean
        Return Char.IsLetterOrDigit(c)
    End Function

End Module
