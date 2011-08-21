Public Class FunctionCall

    Private ReadOnly _FunctionName As String
    Public ReadOnly Property FunctionName As String
        Get
            Return _FunctionName
        End Get
    End Property

    Private ReadOnly _Arguments As IEnumerable(Of String)
    Public ReadOnly Property Arguments As IEnumerable(Of String)
        Get
            Return _Arguments
        End Get
    End Property

    Public Sub New(text As String)
        Dim trimmedText = text.Trim

        _FunctionName = trimmedText.GetStartingIdentifier
        _Arguments = CompilerTools.GetArguments(argumentsInBrackets:=trimmedText.Substring(startIndex:=_FunctionName.Length, length:=trimmedText.Length - _FunctionName.Length).Trim)
    End Sub

End Class
