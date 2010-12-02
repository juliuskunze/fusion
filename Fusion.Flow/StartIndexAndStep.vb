Public Class StartIndexAndStep
    Public Property StartIndex As Integer
    Public Property [Step] As Integer

    Public Sub New(ByVal startIndex As Integer, ByVal [step] As Integer)
        Me.StartIndex = startIndex
        Me.Step = [step]
    End Sub
End Class
