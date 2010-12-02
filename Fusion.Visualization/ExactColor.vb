''' <summary>
''' A float number rgb-color which is displayable when the red, green and blue color component is in [0; 1].
''' </summary>
''' <remarks></remarks>
Public Structure ExactColor

    Public Red As Double
    Public Green As Double
    Public Blue As Double

    Public Sub New(ByVal color As Color)
        Me.New(Red:=GetExactComponent(color.R),
               Green:=GetExactComponent(color.G),
               Blue:=GetExactComponent(color.B))
    End Sub

    Public Sub New(ByVal red As Double, ByVal green As Double, ByVal blue As Double)
        Me.Red = red
        Me.Green = green
        Me.Blue = blue
    End Sub

    Public Function ToColor() As Color
        Return Color.FromArgb(red:=GetByteComponent(Me.Red),
                              green:=GetByteComponent(Me.Green),
                              blue:=GetByteComponent(Me.Blue))
    End Function

    Public Function ToColorByTruncate() As Color
        Return Color.FromArgb(red:=GetByteComponentByTruncate(Me.Red),
                              green:=GetByteComponentByTruncate(Me.Green),
                              blue:=GetByteComponentByTruncate(Me.Blue))
    End Function

    Private Shared Function GetByteComponent(ByVal exactColorValue As Double) As Byte
        If 0 > exactColorValue OrElse exactColorValue > 1 Then
            Throw New ArgumentException("Color values can only be between 0 and 1 when it is converted to System.Color.")
        End If

        Return CByte(exactColorValue * Byte.MaxValue)
    End Function

    Private Shared Function GetByteComponentByTruncate(ByVal exactColorComponent As Double) As Byte
        Return CByte(Min(Byte.MaxValue, Max(Byte.MinValue, exactColorComponent * Byte.MaxValue)))
    End Function

    Private Shared Function GetExactComponent(ByVal byteColorComponent As Byte) As Double
        Return byteColorComponent / Byte.MaxValue
    End Function

    Public Shared Operator +(ByVal color1 As ExactColor, ByVal color2 As ExactColor) As ExactColor
        Return New ExactColor(Red:=color1.Red + color2.Red,
                              Green:=color1.Green + color2.Green,
                              Blue:=color1.Blue + color2.Blue)
    End Operator

    Public Shared Operator -(ByVal color1 As ExactColor, ByVal color2 As ExactColor) As ExactColor
        Return New ExactColor(Red:=color1.Red - color2.Red,
                              Green:=color1.Green - color2.Green,
                              Blue:=color1.Blue - color2.Blue)
    End Operator

    Public Shared Operator -(ByVal color As ExactColor) As ExactColor
        Return New ExactColor(Red:=-color.Red, Green:=-color.Green, Blue:=-color.Blue)
    End Operator

    Public Shared Operator *(ByVal color As ExactColor, ByVal factor As Double) As ExactColor
        Return New ExactColor(Red:=color.Red * factor,
                              Green:=color.Green * factor,
                              Blue:=color.Blue * factor)
    End Operator

    Public Shared Operator *(ByVal factor As Double, ByVal color As ExactColor) As ExactColor
        Return color * factor
    End Operator

    Public Shared Operator /(ByVal color As ExactColor, ByVal factor As Double) As ExactColor
        Return New ExactColor(Red:=color.Red / factor,
                              Green:=color.Green / factor,
                              Blue:=color.Blue / factor)
    End Operator

    Public Shared Operator =(ByVal color1 As ExactColor, ByVal color2 As ExactColor) As Boolean
        Return color1.Red = color2.Red AndAlso color1.Green = color2.Green AndAlso color1.Blue = color2.Blue
    End Operator

    Public Shared Operator <>(ByVal color1 As ExactColor, ByVal color2 As ExactColor) As Boolean
        Return Not color1 = color2
    End Operator

    Public Shared Function Black() As ExactColor
        Return New ExactColor
    End Function

    Public Shared Function White() As ExactColor
        Return New ExactColor(1, 1, 1)
    End Function

    Public Overrides Function ToString() As String
        Return "Color(" & Me.Red.ToString & ", " & Me.Green.ToString & ", " & Me.Blue.ToString & ")"
    End Function
End Structure
