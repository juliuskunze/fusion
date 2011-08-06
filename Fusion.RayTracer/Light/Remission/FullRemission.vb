Public Class FullRemission(Of TLight As {ILight(Of TLight), New})
    Implements IRemission(Of TLight)

    Public ReadOnly Property IsBlack As Boolean Implements IRemission(Of TLight).IsBlack
        Get
            Return False
        End Get
    End Property

    Public Function GetRemission(light As TLight) As TLight Implements IRemission(Of TLight).GetRemission
        Return light
    End Function

End Class
