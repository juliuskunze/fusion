﻿Public Class RgbLightToRgbColorConverter
    Implements ILightToRgbColorConverter(Of RgbLight)

    ''' <summary>
    ''' Converts an RgbLight into a displayable System.Drawing.Color. 
    ''' If one of the comonents R, G or B is greater that 1, the brightness of the light is scaled so that the greatest component becomes 1.
    ''' </summary>
    Public Function Run(light As RgbLight) As Color Implements ILightToRgbColorConverter(Of RgbLight).Run
        If Max(light.Red, Max(light.Green, light.Blue)) > 1 Then
            Return GetColor(displayableRgbLight:=light / Max(light.Red, Max(light.Green, light.Blue)))
        End If

        Return GetColor(displayableRgbLight:=light)
    End Function

    Private Shared Function GetColor(displayableRgbLight As RgbLight) As Color
        Return Color.FromArgb(red:=GetByteComponent(displayableRgbLight.Red),
                              green:=GetByteComponent(displayableRgbLight.Green),
                              blue:=GetByteComponent(displayableRgbLight.Blue))
    End Function

    Private Shared Function GetByteComponent(colorComponent As Double) As Byte
        Return CByte(colorComponent * Byte.MaxValue)
    End Function

    Friend Shared Function GetComponent(byteColorComponent As Byte) As Double
        Return byteColorComponent / Byte.MaxValue
    End Function
End Class
