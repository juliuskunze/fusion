Public Interface IRemission(Of TLight)

    Function GetRemission(ByVal light As TLight) As TLight

    ReadOnly Property IsBlack() As Boolean

End Interface