Public Class CompilerExceptionWithHelp
    Inherits Exception

    Private ReadOnly _CompileHelp As CompileHelp
    Public ReadOnly Property CompileHelp As CompileHelp
        Get
            Return _CompileHelp
        End Get
    End Property

    Public ReadOnly Property InnerCompilerException As CompilerException
        Get
            Return CType(MyBase.InnerException, CompilerException)
        End Get
    End Property

    Public Sub New(compilerExcpetion As CompilerException, compileHelp As CompileHelp)
        MyBase.New(Message:="An error occured during compilation.", InnerException:=compilerExcpetion)
        _CompileHelp = compileHelp
    End Sub

End Class
