Public Interface IRemission(Of TLight)

    Function Remission(ByVal light As TLight) As TLight

    ReadOnly Property NoRemission() As Boolean

End Interface