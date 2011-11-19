Public Class CompilerException
    Inherits Exception

    Public Sub New(message As String)
        MyBase.New(message:=message)
    End Sub

    Public Function Locate(locatedString As LocatedString) As LocatedCompilerException
        Return New LocatedCompilerException(locatedString, Me.Message)
    End Function

    Public Function WithHelp(compileHelp As CompileHelp) As CompilerExceptionWithHelp
        Return New CompilerExceptionWithHelp(Me, compileHelp)
    End Function

End Class
