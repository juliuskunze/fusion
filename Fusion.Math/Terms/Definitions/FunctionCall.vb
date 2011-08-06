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
        For functionNameLength = functionCallText.Length To 1 Step -1
            _FunctionName = functionCallText.Substring(0, functionNameLength)

            If _FunctionName.IsValidVariableName Then
                Dim argumentString = functionCallText.Substring(startIndex:=functionNameLength, length:=functionCallText.Length - functionNameLength)
                If Not argumentString.IsInBrackets Then Throw New ArgumentException("Invalid function call.")

                Dim parameterStringWithoutBracktes = argumentString.Substring(1, argumentString.Length - 2)
                _Arguments = parameterStringWithoutBracktes.Split(","c)

                Return
            End If
        Next

        Throw New ArgumentException("Invalid function call.")
    End Sub

End Class
