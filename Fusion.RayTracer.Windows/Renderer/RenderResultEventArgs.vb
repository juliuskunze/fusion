Public Class RenderResultEventArgs(Of TResult)

    Private ReadOnly _Result As TResult
    Private ReadOnly _Cancelled As Boolean
    Private ReadOnly _ElapsedTime As TimeSpan

    Public Sub New(result As TResult, cancelled As Boolean, elapsedTime As TimeSpan)
        _Result = result
        _Cancelled = cancelled
        _ElapsedTime = elapsedTime
    End Sub

    Public ReadOnly Property Result As TResult
        Get
            If _Cancelled Then Throw New InvalidOperationException("Rendering has been cancelled.")

            Return _Result
        End Get
    End Property

    Public ReadOnly Property Cancelled As Boolean
        Get
            Return _Cancelled
        End Get
    End Property

    Public ReadOnly Property ElapsedTime As TimeSpan
        Get
            Return _ElapsedTime
        End Get
    End Property

End Class
