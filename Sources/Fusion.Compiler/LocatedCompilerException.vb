Public Class LocatedCompilerException
    Inherits CompilerException

    Private ReadOnly _LocatedString As LocatedString
    Public ReadOnly Property LocatedString As LocatedString
        Get
            Return _LocatedString
        End Get
    End Property

    Public Sub New(locatedString As LocatedString, message As String)
        MyBase.New(message:=message)
        _LocatedString = LocatedString
    End Sub
End Class
