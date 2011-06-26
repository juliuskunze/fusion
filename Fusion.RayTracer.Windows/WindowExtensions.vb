Imports System.Runtime.CompilerServices

Public Module WindowExtensions

    ''' <summary>
    ''' Erfüllt die Gleichung pixels = pixelFactor * visual.Width.
    ''' </summary>
    <Extension()>
    Public Sub GetPixelFactor(ByVal visual As Visual, ByVal out_dpiX As Double, ByVal out_dpiY As Double)
        Dim m = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice
        out_dpiX = m.M11 * 96
        out_dpiY = m.M22 * 96
    End Sub

    <Extension()>
    Public Function SizeInPixels(ByVal panel As Panel) As System.Drawing.Size
        Dim dpiX, dpiY As Double
        panel.GetPixelFactor(out_dpiX:=dpiX, out_dpiY:=dpiY)
        Return New System.Drawing.Size(width:=CInt(dpiX * panel.Width), height:=CInt(dpiY * panel.Height))
    End Function

    Public Function ScreenSize() As System.Drawing.Size
        Return New System.Drawing.Size
    End Function

End Module
