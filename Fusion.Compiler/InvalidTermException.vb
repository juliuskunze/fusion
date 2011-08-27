Public Class InvalidTermException
    Inherits CompilerException

    Private ReadOnly _Term As String
    Public ReadOnly Property Term As String
        Get
            Return _Term
        End Get
    End Property

    Public Sub New(term As String)
        MyBase.New(Message:="The term is invalid: " & term)
        _Term = term
    End Sub

    Public Sub New(term As String, message As String)
        MyBase.New(message:=message)
        _Term = term
    End Sub

End Class
