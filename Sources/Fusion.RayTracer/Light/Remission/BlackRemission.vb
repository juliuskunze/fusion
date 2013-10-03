Public Class BlackRemission(Of TLight As {ILight(Of TLight), New})
    Implements IRemission(Of TLight)

    Public ReadOnly Property IsBlack As Boolean Implements IRemission(Of TLight).IsBlack
        Get
            Return True
        End Get
    End Property

    Public Function GetRemission(light As TLight) As TLight Implements IRemission(Of TLight).GetRemission
        Return New TLight
    End Function
End Class
