Public Class CompilerExceptionWithIntelliSense
    Inherits Exception

    Private ReadOnly _IntelliSense As IntelliSense
    Public ReadOnly Property IntelliSense As IntelliSense
        Get
            Return _IntelliSense
        End Get
    End Property

    Public ReadOnly Property InnerCompilerExcpetion As CompilerException
        Get
            Return CType(MyBase.InnerException, CompilerException)
        End Get
    End Property

    Public Sub New(compilerExcpetion As CompilerException, intelliSense As IntelliSense)
        MyBase.New(Message:="An error occured during compilation.", InnerException:=compilerExcpetion)
        _IntelliSense = intelliSense
    End Sub

End Class
