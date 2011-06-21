﻿''' <summary>
''' A representation of light (i. e. color, wavelength-spectrum, ...).
''' </summary>
Public Interface ILight(Of TLight As {ILight(Of TLight), New})

    Function Add(ByVal other As TLight) As TLight

    Function MultiplyBrighness(ByVal factor As Double) As TLight
    Function DivideBrightness(ByVal divisor As Double) As TLight

    Function ToColor() As Color

End Interface