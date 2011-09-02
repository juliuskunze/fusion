Public Class InvalidTermException
    Inherits LocatedCompilerException

    Public Sub New(term As LocatedString, message As String)
        MyBase.New(locatedString:=term, message:=message)
    End Sub

    Public Sub New(term As LocatedString)
        MyBase.New(LocatedString:=term, Message:=String.Format("The term is invalid: '{0}'", term.ToString))
    End Sub

End Class
