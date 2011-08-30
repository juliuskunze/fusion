Public Class InvalidTermException
    Inherits LocatedCompilerException

    Public Sub New(term As LocatedString, message As String)
        MyBase.New(locatedString:=term, message:=message)
    End Sub

    Public Sub New(term As LocatedString)
        MyBase.New(locatedString:=term, Message:="The term is invalid: " & term.ToString)
    End Sub

End Class
