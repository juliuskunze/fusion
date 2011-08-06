Public Interface IRemission(Of TLight)

    Function GetRemission(light As TLight) As TLight

    ReadOnly Property IsBlack() As Boolean

End Interface