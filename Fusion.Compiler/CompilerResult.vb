Public Class CompilerResult(Of TResult)

    Private ReadOnly _Result As TResult
    Public ReadOnly Property Result As TResult
        Get
            Return _Result
        End Get
    End Property

    Private ReadOnly _CursorTermContext As TermContext
    Public ReadOnly Property CursorTermContext As TermContext
        Get
            Return _CursorTermContext
        End Get
    End Property

    Public Sub New(result As TResult, cursorTermContext As TermContext)
        _Result = result
        _CursorTermContext = cursorTermContext
    End Sub

End Class
