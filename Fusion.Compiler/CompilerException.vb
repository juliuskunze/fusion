Public Class CompilerException
    Inherits Exception

    Public Sub New(message As String)
        MyBase.New(message:=message)
    End Sub

    Public Function Locate(locatedString As LocatedString) As LocatedCompilerException
        Return New LocatedCompilerException(locatedString, Me.Message)
    End Function

    Public Function WithIntelliSense(intelliSense As IntelliSense) As CompilerExceptionWithIntelliSense
        Return New CompilerExceptionWithIntelliSense(Me, intelliSense)
    End Function

End Class
