Imports System.ComponentModel

Public MustInherit Class Renderer(Of TResult)

    Private ReadOnly _Stopwatch As Stopwatch = Stopwatch.StartNew
    Protected WithEvents _BackgroundWorker As New ComponentModel.BackgroundWorker With {.WorkerReportsProgress = True, .WorkerSupportsCancellation = True}

    Public ReadOnly Property ElapsedTime As TimeSpan
        Get
            Return _Stopwatch.Elapsed
        End Get
    End Property

    Public Sub CancelAsync()
        _BackgroundWorker.CancelAsync()
    End Sub

    Public Event ProgressChanged(e As ComponentModel.ProgressChangedEventArgs)

    Private Sub BackgroundWorker_ProgressChanged(sender As Object, e As ComponentModel.ProgressChangedEventArgs) Handles _BackgroundWorker.ProgressChanged
        RaiseEvent ProgressChanged(e)
    End Sub

    Public Event Completed(e As RenderResultEventArgs(Of TResult))

    Private Sub BackgroundWorker_Completed(sender As Object, e As ComponentModel.RunWorkerCompletedEventArgs) Handles _BackgroundWorker.RunWorkerCompleted
        OnCompleted(e)
        RaiseEvent Completed(New RenderResultEventArgs(Of TResult)(CType(CType(e.Result, Object), TResult), cancelled:=e.Cancelled, ElapsedTime:=Me.ElapsedTime))
    End Sub

    Protected Overridable Sub OnCompleted(ByVal e As RunWorkerCompletedEventArgs)
        If e.Error IsNot Nothing Then Throw e.Error

        _Stopwatch.Stop()
    End Sub

    Protected Sub ReportProgress(relativeProgress As Double)
        _BackgroundWorker.ReportProgress(CInt(relativeProgress * 100))
    End Sub

    Public Sub RunAsync()
        _BackgroundWorker.RunWorkerAsync()
    End Sub

End Class
