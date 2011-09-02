Public Class CompilerResult(Of TResult)

    Private ReadOnly _ErrorMessage As String
    Public ReadOnly Property ErrorMessage As String
        Get
            If Me.WasCompilationSuccessful Then Throw New InvalidOperationException("There is no error message because compilation was successful.")

            Return _ErrorMessage
        End Get
    End Property

    Private ReadOnly _Result As TResult
    Public ReadOnly Property Result As TResult
        Get
            If Not Me.WasCompilationSuccessful Then Throw New InvalidOperationException("There is no result because compilation was not successful.")

            Return _Result
        End Get
    End Property

    Public ReadOnly Property WasCompilationSuccessful As Boolean
        Get
            Return _Result IsNot Nothing
        End Get
    End Property

    Public Sub New(errorMessage As String)
        _ErrorMessage = errorMessage
    End Sub

    Public Sub New(result As TResult)
        If result Is Nothing Then Throw New ArgumentNullException("result")

        _Result = result
    End Sub

End Class
