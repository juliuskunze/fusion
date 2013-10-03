Public Class RenderResultEventArgs(Of TResult)

    Private ReadOnly _Result As TResult
    Private ReadOnly _Cancelled As Boolean
    Private ReadOnly _Error As Exception
    Private ReadOnly _ElapsedTime As TimeSpan

    Public Sub New(result As TResult, elapsedTime As TimeSpan)
        _Result = result
        _ElapsedTime = elapsedTime
    End Sub

    Private Sub New(cancelled As Boolean)
        _Cancelled = True
    End Sub

    Private Shared ReadOnly _CancelledResult As RenderResultEventArgs(Of TResult) = New RenderResultEventArgs(Of TResult)(Cancelled:=True)
    Public Shared ReadOnly Property CancelledResult As RenderResultEventArgs(Of TResult)
        Get
            Return _CancelledResult
        End Get
    End Property

    Public Sub New([error] As Exception)
        _Error = [error]
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

    Public ReadOnly Property [Error] As Exception
        Get
            Return _Error
        End Get
    End Property

    Public ReadOnly Property WasSuccessful As Boolean
        Get
            Return Not _Cancelled AndAlso _Error Is Nothing
        End Get
    End Property
End Class
