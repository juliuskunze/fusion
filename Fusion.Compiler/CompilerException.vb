Public Class CompilerException
    Inherits Exception

    Public Sub New(message As String)
        MyBase.New(message:=message)
    End Sub

    Public Function Locate(locatedString As LocatedString) As LocatedCompilerException
        Return New LocatedCompilerException(locatedString, Me.Message)
    End Function

    Public Function WithCursorTermContext(cursorTermContext As TermContext) As CompilerExceptionWithCursorTermContext
        Return New CompilerExceptionWithCursorTermContext(Me, cursorTermContext)
    End Function

End Class
