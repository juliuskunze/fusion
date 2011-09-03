Public Class CompilerExceptionWithCursorTermContext
    Inherits Exception

    Private ReadOnly _CursorTermContext As TermContext
    Public ReadOnly Property CursorTermContext As TermContext
        Get
            Return _CursorTermContext
        End Get
    End Property

    Public ReadOnly Property InnerCompilerExcpetion As CompilerException
        Get
            Return CType(MyBase.InnerException, CompilerException)
        End Get
    End Property

    Public Sub New(compilerExcpetion As CompilerException, cursorTermContext As TermContext)
        MyBase.New(Message:="An error occured during compilation.", InnerException:=compilerExcpetion)
        _CursorTermContext = cursorTermContext
    End Sub

End Class
