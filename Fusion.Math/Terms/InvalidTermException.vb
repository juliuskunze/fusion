Public Class InvalidTermException
    Inherits ArgumentException

    Private ReadOnly _Term As String
    Public ReadOnly Property Term As String
        Get
            Return _Term
        End Get
    End Property

    Public Sub New(ByVal term As String)
        MyBase.New(Message:="The term is invalid: " & term)
        _Term = term
    End Sub

    Public Sub New(ByVal term As String, ByVal message As String)
        MyBase.New(message:=message)
        _Term = term
    End Sub

End Class
