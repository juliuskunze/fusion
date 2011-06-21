''' <summary>
''' A representation of light (i. e. color, wavelength-spectrum, ...).
''' </summary>
Public Interface ILight(Of TLight As {New})

    Function Add(ByVal other As TLight) As TLight

    Function MultiplyBrighness(ByVal factor As Double) As TLight

End Interface
