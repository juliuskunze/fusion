Public Structure HsvColor

    Public Sub New(ByVal hue As Double, ByVal saturation As Double, ByVal value As Double)
        _hue = hue
        _saturation = saturation
        _value = value
    End Sub

    Private _hue As Double
    Public ReadOnly Property Hue As Double
        Get
            Return _hue
        End Get
    End Property

    Private _saturation As Double
    Public ReadOnly Property Saturation As Double
        Get
            Return _saturation
        End Get
    End Property

    Private _value As Double
    Public ReadOnly Property Value As Double
        Get
            Return _value
        End Get
    End Property

    Public Function ToRgbColor() As Color
        If Not (0 <= Me.Saturation AndAlso Me.Saturation <= 1 AndAlso
           0 <= Me.Value AndAlso Me.Value <= 1) Then Throw New ArgumentOutOfRangeException("The arguments saturation and value have to be in [0; 1].")

        Dim h = Me.Hue * 6 / (2 * PI)

        h = h Mod 6
        If h < 0 Then
            h += 6
        End If

        Dim v = Me.Value * 255

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

    Public Shared Function FromRgbColor(ByVal rgbColor As Color) As HsvColor
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

        Dim saturation As Double
        If maximum = 0 Then
            saturation = 0
        Else
            saturation = difference / maximum
        End If

        Dim value = maximum

        Return New HsvColor(hue:=hue, saturation:=saturation, value:=value)
    End Function

End Structure