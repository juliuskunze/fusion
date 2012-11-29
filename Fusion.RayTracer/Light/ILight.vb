''' <summary>
''' A representation of light (i. e. color, wavelength-spectrum, ...).
''' </summary>
Public Interface ILight(Of TLight As ILight(Of TLight))
    Function Add(other As TLight) As TLight

    Function MultiplyBrightness(factor As Double) As TLight
    Function DivideBrightness(divisor As Double) As TLight
End Interface