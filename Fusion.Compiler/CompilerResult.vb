Public Class CompilerResult(Of TResult)

    Private ReadOnly _Result As TResult
    Public ReadOnly Property Result As TResult
        Get
            Return _Result
        End Get
    End Property

    Private ReadOnly _IntelliSense As IntelliSense
    Public ReadOnly Property IntelliSense As IntelliSense
        Get
            Return _IntelliSense
        End Get
    End Property

    Public Sub New(result As TResult, intelliSense As IntelliSense)
        _Result = result
        _IntelliSense = intelliSense
    End Sub

End Class
