Public Class CompilerResult(Of TResult)

    Private ReadOnly _Result As TResult
    Public ReadOnly Property Result As TResult
        Get
            Return _Result
        End Get
    End Property

    Private ReadOnly _CompilerHelp As CompilerHelp
    Public ReadOnly Property CompilerHelp As CompilerHelp
        Get
            Return _CompilerHelp
        End Get
    End Property

    Public Sub New(result As TResult, compilerHelp As CompilerHelp)
        _Result = result
        _CompilerHelp = compilerHelp
    End Sub

End Class
