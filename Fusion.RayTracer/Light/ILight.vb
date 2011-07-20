''' <summary>
''' A representation of light (i. e. color, wavelength-spectrum, ...).
''' </summary>
Public Interface ILight(Of TLight As ILight(Of TLight))

    Function Add(ByVal other As TLight) As TLight

    Function MultiplyBrightness(ByVal factor As Double) As TLight
    Function DivideBrightness(ByVal divisor As Double) As TLight

End Interface