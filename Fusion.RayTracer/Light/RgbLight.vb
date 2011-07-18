''' <summary>
''' A float number rgb-color which is displayable when the red, green and blue color component is in [0; 1].
''' </summary>
''' <remarks></remarks>
Public Structure RgbLight
    Implements ILight(Of RgbLight)
    
    Private ReadOnly _Red As Double
    Public ReadOnly Property Red As Double
        Get
            Return _Red
        End Get
    End Property

    Private ReadOnly _Green As Double
    Public ReadOnly Property Green As Double
        Get
            Return _Green
        End Get
    End Property

    Private ReadOnly _Blue As Double
    Public ReadOnly Property Blue As Double
        Get
            Return _Blue
        End Get
    End Property

    Public Sub New(ByVal color As Color)
        Me.New(Red:=GetComponent(color.R),
               Green:=GetComponent(color.G),
               Blue:=GetComponent(color.B))
    End Sub

    Public Sub New(ByVal red As Double, ByVal green As Double, ByVal blue As Double)
        If _Red < 0 OrElse _Green < 0 OrElse _Blue < 0 Then Throw New ArgumentOutOfRangeException("Color components must be > 0.")

        _Red = red
        _Green = green
        _Blue = blue
    End Sub

    Private Shared Function GetByteComponent(ByVal exactColorComponent As Double) As Byte
        Return CByte(exactColorComponent * Byte.MaxValue)
    End Function

    Private Shared Function GetComponent(ByVal byteColorComponent As Byte) As Double
        Return byteColorComponent / Byte.MaxValue
    End Function

    Public Shared Operator +(ByVal color1 As RgbLight, ByVal color2 As RgbLight) As RgbLight
        Return New RgbLight(Red:=color1.Red + color2.Red,
                              Green:=color1.Green + color2.Green,
                              Blue:=color1.Blue + color2.Blue)
    End Operator

    Public Shared Operator -(ByVal color1 As RgbLight, ByVal color2 As RgbLight) As RgbLight
        Return New RgbLight(Red:=color1.Red - color2.Red,
                              Green:=color1.Green - color2.Green,
                              Blue:=color1.Blue - color2.Blue)
    End Operator

    Public Shared Operator -(ByVal color As RgbLight) As RgbLight
        Return New RgbLight(Red:=-color.Red, Green:=-color.Green, Blue:=-color.Blue)
    End Operator

    Public Shared Operator *(ByVal color As RgbLight, ByVal factor As Double) As RgbLight
        Return New RgbLight(Red:=color.Red * factor,
                              Green:=color.Green * factor,
                              Blue:=color.Blue * factor)
    End Operator

    Public Shared Operator *(ByVal factor As Double, ByVal color As RgbLight) As RgbLight
        Return color * factor
    End Operator

    Public Shared Operator /(ByVal color As RgbLight, ByVal factor As Double) As RgbLight
        Return New RgbLight(Red:=color.Red / factor,
                              Green:=color.Green / factor,
                              Blue:=color.Blue / factor)
    End Operator

    Public Shared Operator =(ByVal color1 As RgbLight, ByVal color2 As RgbLight) As Boolean
        Return color1.Red = color2.Red AndAlso color1.Green = color2.Green AndAlso color1.Blue = color2.Blue
    End Operator

    Public Shared Operator <>(ByVal color1 As RgbLight, ByVal color2 As RgbLight) As Boolean
        Return Not color1 = color2
    End Operator

    Public Shared Function Black() As RgbLight
        Return New RgbLight
    End Function

    Public Shared Function White() As RgbLight
        Return New RgbLight(1, 1, 1)
    End Function

    Public Overrides Function ToString() As String
        Return "Color(" & Me.Red.ToString & ", " & Me.Green.ToString & ", " & Me.Blue.ToString & ")"
    End Function

    Public Function Add(ByVal other As RgbLight) As RgbLight Implements ILight(Of RgbLight).Add
        Return Me + other
    End Function

    Public Function MultiplyBrightness(ByVal factor As Double) As RgbLight Implements ILight(Of RgbLight).MultiplyBrightness
        Return Me * factor
    End Function

    Public Function DivideBrightness(ByVal divisor As Double) As RgbLight Implements ILight(Of RgbLight).DivideBrightness
        Return Me / divisor
    End Function

    Public Function ToColor() As Color Implements ILight(Of RgbLight).ToColor
        If Max(_Red, Max(_Green, _Blue)) > 1 Then
            Return GetColor(displayableRgbLight:=Me / Max(_Red, Max(_Green, _Blue)))
        End If

        Return GetColor(displayableRgbLight:=Me)
    End Function

    Private Shared Function GetColor(ByVal displayableRgbLight As RgbLight) As Color
        Return Color.FromArgb(red:=GetByteComponent(displayableRgbLight.Red),
                              green:=GetByteComponent(displayableRgbLight.Green),
                              blue:=GetByteComponent(displayableRgbLight.Blue))
    End Function

End Structure
