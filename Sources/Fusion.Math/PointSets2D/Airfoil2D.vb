Public Class Airfoil2D
    Implements IPointSet2D

    Public Property ChordLength As Double

    Public Property RelativeThickness As Double
    Public Property MaxCamber As Double

    Public Property RelativeMaxCamberLocation As Double

    Public Sub New(chordLength As Double, relativeThickness As Double, maxCamber As Double, relativeMaxCamberLocation As Double)
        Me.ChordLength = chordLength
        Me.RelativeThickness = relativeThickness
        Me.MaxCamber = maxCamber
        Me.RelativeMaxCamberLocation = relativeMaxCamberLocation
    End Sub

    Public Function Contains(point As Fusion.Math.Vector2D) As Boolean Implements IPointSet2D.Contains
        Return lowerBound(point.X) <= point.Y AndAlso point.Y <= upperBound(point.X)
    End Function

    Private Function upperBound(x As Double) As Double
        Dim relativeX = x / Me.ChordLength
        Return camber(x) + RelativeThickness / 0.2 * ChordLength * (0.2969 * Sqrt(relativeX) - 0.126 * relativeX - 0.3516 * relativeX ^ 2 + 0.2843 * relativeX ^ 3 - 0.1015 * relativeX ^ 4)
    End Function

    Private Function lowerBound(x As Double) As Double
        Dim relativeX = x / Me.ChordLength
        Return camber(x) + (-RelativeThickness / 0.2 * ChordLength * (0.2969 * Sqrt(relativeX) - 0.126 * relativeX - 0.3516 * relativeX ^ 2 + 0.2843 * relativeX ^ 3 - 0.1015 * relativeX ^ 4))
    End Function

    Private Function camber(x As Double) As Double
        Dim relativeX = x / Me.ChordLength
        If 0 <= x AndAlso x <= Me.RelativeMaxCamberLocation * Me.ChordLength Then
            Return Me.MaxCamber * x / Me.RelativeMaxCamberLocation ^ 2 * (2 * Me.RelativeMaxCamberLocation - relativeX)
        ElseIf Me.RelativeMaxCamberLocation * Me.ChordLength <= x AndAlso x <= Me.ChordLength Then
            Return Me.MaxCamber * (Me.ChordLength - x) / (1 - Me.RelativeMaxCamberLocation) ^ 2 * (1 + relativeX - 2 * Me.RelativeMaxCamberLocation)
        Else
            Return 0
        End If
    End Function
End Class
