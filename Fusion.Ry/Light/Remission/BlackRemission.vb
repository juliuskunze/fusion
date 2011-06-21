Public Class BlackRemission(Of TLight As {ILight(Of TLight), New})
    Implements IRemission(Of TLight)

    Public ReadOnly Property NoRemission As Boolean Implements IRemission(Of TLight).NoRemission
        Get
            Return True
        End Get
    End Property

    Public Function GetRemission(ByVal light As TLight) As TLight Implements IRemission(Of TLight).GetRemission
        Return New TLight
    End Function
End Class
