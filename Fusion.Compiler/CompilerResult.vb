Public Class CompilerResult(Of TResult)

    Private ReadOnly _Result As TResult
    Public ReadOnly Property Result As TResult
        Get
            Return _Result
        End Get
    End Property

    Private ReadOnly _CompileHelp As CompileHelp
    Public ReadOnly Property CompileHelp As CompileHelp
        Get
            Return _CompileHelp
        End Get
    End Property

    Public Sub New(result As TResult, compileHelp As CompileHelp)
        _Result = result
        _CompileHelp = compileHelp
    End Sub

End Class
