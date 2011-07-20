Public Class RgbLightToColorConverter
    Implements ILightToColorConverter(Of RgbLight)

    ''' <summary>
    ''' Wandelt das RgbLight in eine darstellbare System.Drawing.Color um. 
    ''' Ist eine der Komponenten (R, G, oder B) größer als 1, so wird die Helligkeit der Farbe so skaliert, dass die größte Komponente 1 wird.
    ''' </summary>
    Public Function Convert(ByVal light As RgbLight) As System.Drawing.Color Implements ILightToColorConverter(Of RgbLight).Convert
        If Max(light.Red, Max(light.Green, light.Blue)) > 1 Then
            Return GetColor(displayableRgbLight:=light / Max(light.Red, Max(light.Green, light.Blue)))
        End If

        Return GetColor(displayableRgbLight:=light)
    End Function

    Private Shared Function GetColor(ByVal displayableRgbLight As RgbLight) As Color
        Return Color.FromArgb(red:=GetByteComponent(displayableRgbLight.Red),
                              green:=GetByteComponent(displayableRgbLight.Green),
                              blue:=GetByteComponent(displayableRgbLight.Blue))
    End Function

    Private Shared Function GetByteComponent(ByVal colorComponent As Double) As Byte
        Return CByte(colorComponent * Byte.MaxValue)
    End Function

    Friend Shared Function GetComponent(ByVal byteColorComponent As Byte) As Double
        Return byteColorComponent / Byte.MaxValue
    End Function

End Class
