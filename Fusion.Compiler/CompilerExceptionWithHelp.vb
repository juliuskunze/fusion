Public Class CompilerExceptionWithHelp
    Inherits Exception

    Private ReadOnly _CompilerHelp As CompilerHelp
    Public ReadOnly Property CompilerHelp As CompilerHelp
        Get
            Return _CompilerHelp
        End Get
    End Property

    Public ReadOnly Property InnerCompilerException As CompilerException
        Get
            Return CType(MyBase.InnerException, CompilerException)
        End Get
    End Property

    Public Sub New(compilerExcpetion As CompilerException, compilerHelp As CompilerHelp)
        MyBase.New(Message:="An error occured during compilation.", InnerException:=compilerExcpetion)
        _CompilerHelp = compilerHelp
    End Sub
End Class
