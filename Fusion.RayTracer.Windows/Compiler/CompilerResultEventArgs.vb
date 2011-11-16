Public Class CompilerResultEventArgs(Of TResult)
    Inherits EventArgs

    Private ReadOnly _CompilerResult As RichCompilerResult(Of TResult)
    Public ReadOnly Property CompilerResult As RichCompilerResult(Of TResult)
        Get
            Return _CompilerResult
        End Get
    End Property

    Public Sub New(compilerResult As RichCompilerResult(Of TResult))
        _CompilerResult = compilerResult
    End Sub

End Class
