Public Module ColorConvertion

    Public Function ColorFromHsv(ByVal hue As Double, ByVal saturation As Double, ByVal value As Double) As Color
        If 0 <= saturation AndAlso saturation <= 1 AndAlso
           0 <= value AndAlso value <= 1 Then

            hue *= 6 / (2 * PI)

            hue = hue Mod 6
            If hue < 0 Then
                hue += 6
            End If

            value *= 255

            Dim partOfCircle0To5 = Floor(hue)
            Dim locationInPart As Double = (hue - partOfCircle0To5)
            Dim increasing = CByte(value * (1 - (1 - locationInPart) * saturation))
            Dim max = CByte(value)
            Dim decreasing = CByte(value * (1 - locationInPart * saturation))
            Dim min = CByte(value * (1 - saturation))

            Dim red, green, blue As Byte

            Select Case partOfCircle0To5
                Case 0
                    red = max
                    green = increasing
                    blue = min
                Case 1
                    red = decreasing
                    green = max
                    blue = min
                Case 2
                    red = min
                    green = max
                    blue = increasing
                Case 3
                    red = min
                    green = decreasing
                    blue = max
                Case 4
                    red = increasing
                    green = min
                    blue = max
                Case 5
                    red = max
                    green = min
                    blue = decreasing
            End Select

            Return Color.FromArgb(red, green, blue)
        Else
            Throw New ArgumentOutOfRangeException("The arguments saturation and value have to be in [0; 1].")
        End If
    End Function

End Module
