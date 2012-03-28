Imports System.ComponentModel

Public MustInherit Class Renderer(Of TResult)

    Private ReadOnly _Stopwatch As Stopwatch = Stopwatch.StartNew
    Protected WithEvents _BackgroundWorker As New BackgroundWorker With {.WorkerReportsProgress = True, .WorkerSupportsCancellation = True}

    Public ReadOnly Property ElapsedTime As TimeSpan
        Get
            Return _Stopwatch.Elapsed
        End Get
    End Property

    Public Sub CancelAsync()
        _BackgroundWorker.CancelAsync()
    End Sub

    Public Event ProgressChanged(e As ProgressChangedEventArgs)

    Private Sub BackgroundWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles _BackgroundWorker.ProgressChanged
        RaiseEvent ProgressChanged(e)
    End Sub

    Public Event Completed(e As RenderResultEventArgs(Of TResult))

    Private Sub BackgroundWorker_Completed(sender As Object, e As RunWorkerCompletedEventArgs) Handles _BackgroundWorker.RunWorkerCompleted
        _Stopwatch.Stop()

        RaiseEvent Completed(GetRenderResult(e))
    End Sub

    Private Function GetRenderResult(e As RunWorkerCompletedEventArgs) As RenderResultEventArgs(Of TResult)
        If e.Cancelled Then
            Return RenderResultEventArgs(Of TResult).CancelledResult
        End If

        If e.Error IsNot Nothing Then
            Return New RenderResultEventArgs(Of TResult)([error]:=e.Error)
        End If

        Return New RenderResultEventArgs(Of TResult)(CType(e.Result, TResult), ElapsedTime:=ElapsedTime)
    End Function

    Protected Sub ReportProgress(relativeProgress As Double)
        _BackgroundWorker.ReportProgress(CInt(relativeProgress * 100))
    End Sub

    Public Sub RunAsync()
        _BackgroundWorker.RunWorkerAsync()
    End Sub

End Class
