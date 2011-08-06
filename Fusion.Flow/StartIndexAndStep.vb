Public Class StartIndexAndStep
    Public Property StartIndex As Integer
    Public Property [Step] As Integer

    Public Sub New(startIndex As Integer, [step] As Integer)
        Me.StartIndex = startIndex
        Me.Step = [step]
    End Sub
End Class
