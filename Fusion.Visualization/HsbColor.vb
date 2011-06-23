Public Structure HsbColor

    Private ReadOnly _Hue As Double
    Public ReadOnly Property Hue As Double
        Get
            Return _Hue
        End Get
    End Property

    Private ReadOnly _Saturation As Double
    Public ReadOnly Property Saturation As Double
        Get
            Return _Saturation
        End Get
    End Property

    Private ReadOnly _Brightness As Double
    Public ReadOnly Property Brightness As Double
        Get
            Return _Brightness
        End Get
    End Property

    Public Sub New(ByVal hue As Double, ByVal saturation As Double, ByVal brightness As Double)
        If Not (0 <= hue AndAlso hue < 2 * PI) Then Throw New ArgumentException("Hue has to be an angle in [0, 2 * Pi).")
        _Hue = hue

        If Not (0 <= Me.Saturation AndAlso Me.Saturation <= 1) Then Throw New ArgumentOutOfRangeException("Saturation has to be in [0; 1].")
        _Saturation = saturation

        If Not (0 <= Me.Brightness AndAlso Me.Brightness <= 1) Then Throw New ArgumentOutOfRangeException("Value has to be in [0; 1].")
        _Brightness = brightness
    End Sub

    Public Function ToRgbColor() As Color
        Dim h = Me.Hue * 6 / (2 * PI)

        h = h Mod 6
        If h < 0 Then
            h += 6
        End If

        Dim v = Me.Brightness * 255

        Dim partOfCircle0To5 = Floor(h)
        Dim locationInPart As Double = (h - partOfCircle0To5)
        Dim increasing = CByte(v * (1 - (1 - locationInPart) * Saturation))
        Dim max = CByte(v)
        Dim decreasing = CByte(v * (1 - locationInPart * Saturation))
        Dim min = CByte(v * (1 - Saturation))

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
    End Function

    Public Shared Function FromRgbColor(ByVal rgbColor As Color) As HsbColor
        Dim r = rgbColor.R / Byte.MaxValue
        Dim g = rgbColor.G / Byte.MaxValue
        Dim b = rgbColor.B / Byte.MaxValue
        Dim maximum = Max(Max(r, g), b)
        Dim minimum = Min(Min(r, g), b)
        Dim difference = maximum - minimum

        Dim hue As Double
        If difference = 0 Then
            hue = 0
        Else
            Select Case maximum
                Case r
                    hue = PI / 3 * (0 + (g - b) / difference)
                Case g
                    hue = PI / 3 * (2 + (b - r) / difference)
                Case b
                    hue = PI / 3 * (4 + (r - g) / difference)
            End Select
        End If
        If hue < 0 Then hue += 2 * PI

        Dim saturation As Double
        If maximum = 0 Then
            saturation = 0
        Else
            saturation = difference / maximum
        End If

        Return New HsbColor(hue:=hue, saturation:=saturation, Brightness:=maximum)
    End Function

End Structure