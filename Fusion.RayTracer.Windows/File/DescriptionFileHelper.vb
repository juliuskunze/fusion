Public Class DescriptionFileHelper

    Public Shared Function GetFileFilter(mode As CompileMode) As FileFilter
        Select Case mode
            Case CompileMode.Picture : Return New FileFilter(FileFilter:="*.pic", description:="Picture descriptions (*.pic)")
            Case CompileMode.Video : Return New FileFilter(FileFilter:="*.vid", description:="Video descriptions (*.vid)")
            Case Else : Throw New ArgumentOutOfRangeException
        End Select
    End Function

    Public Shared Function OpenFileFilters() As FileFilters
        Return New FileFilters({New FileFilter(FileFilter:="*.pic; *.vid", description:="Video and picture descriptions (*.pic; *.vid)"),
                                GetFileFilter(CompileMode.Picture),
                                GetFileFilter(CompileMode.Video),
                                AllFilesFilter()})
    End Function

    Private Shared ReadOnly _AllFilesFilter As New FileFilter(FileFilter:="*.*", description:="All Files")
    Public Shared Function AllFilesFilter() As FileFilter
        Return _AllFilesFilter
    End Function

    Public Shared Function SaveVideoFilters() As FileFilters
        Return New FileFilters({New FileFilter(FileFilter:="*.avi", description:="Audio Video Interleave (*.avi)")})
    End Function

End Class
