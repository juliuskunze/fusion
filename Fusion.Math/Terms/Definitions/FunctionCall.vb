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

    Public Sub New(functionCallText As String)
        Dim trimmedFunctionCallText = functionCallText.Trim

        _FunctionName = trimmedFunctionCallText.GetStartingValidVariableName
        _Arguments = CompilerTools.GetArguments(argumentsInBrackets:=trimmedFunctionCallText.Substring(startIndex:=_FunctionName.Length, length:=functionCallText.Length - _FunctionName.Length).Trim)
    End Sub

End Class
