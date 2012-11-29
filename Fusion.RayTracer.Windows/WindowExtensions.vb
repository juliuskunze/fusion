Imports System.Runtime.CompilerServices

Public Module WindowExtensions

    ''' <summary>
    ''' Erfüllt die Gleichung pixels = pixelFactor * visual.Width.
    ''' </summary>
    <Extension()>
    Public Sub GetPixelFactor(visual As Visual, out_dpiX As Double, out_dpiY As Double)
        Dim m = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice
        out_dpiX = m.M11 * 96
        out_dpiY = m.M22 * 96
    End Sub
End Module
