Public Class DescriptionFileHelper

    Public Shared Function GetFileFilter(mode As CompileMode) As FileFilter
        Select Case mode
            Case CompileMode.Picture : Return New FileFilter(FileFilter:="*.pic", description:="Ray tracer picture scene description (*.pic)")
            Case CompileMode.Video : Return New FileFilter(FileFilter:="*.vid", description:="Ray tracer video scene description (*.vid)")
            Case Else : Throw New ArgumentOutOfRangeException
        End Select
    End Function

    Public Shared Function OpenFileFilters() As FileFilters
        Return New FileFilters({AllFilesFilter()}.Concat({GetFileFilter(CompileMode.Picture), GetFileFilter(CompileMode.Video)}))
    End Function

    Private Shared ReadOnly _AllFilesFilter As New FileFilter(FileFilter:="*.*", description:="All Files")
    Public Shared Function AllFilesFilter() As FileFilter
        Return _AllFilesFilter
    End Function

    Public Shared Function SaveVideoFilters() As FileFilters
        Return New FileFilters({New FileFilter(FileFilter:="*.avi", description:="Audio Video Interleave (*.avi)")})
    End Function

End Class
