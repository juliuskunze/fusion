Public Class CompilerResult(Of TResult)

    Private ReadOnly _Result As TResult
    Public ReadOnly Property Result As TResult
        Get
            Return _Result
        End Get
    End Property

    Private ReadOnly _CorrectedText As String
    Public ReadOnly Property CorrectedText As String
        Get
            Return _CorrectedText
        End Get
    End Property

    Public Sub New(result As TResult, correctedText As String)
        _Result = result
        _CorrectedText = correctedText
    End Sub

End Class
