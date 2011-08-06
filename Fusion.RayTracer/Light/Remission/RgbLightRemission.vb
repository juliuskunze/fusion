Public Class RgbLightRemission
    Implements IRemission(Of RgbLight)

    Public Property RedAlbedo As Double
    Public Property GreenAlbedo As Double
    Public Property BlueAlbedo As Double

    Public Sub New(redAlbedo As Double, greenAlbedo As Double, blueAlbedo As Double)
        Me.RedAlbedo = redAlbedo
        Me.GreenAlbedo = greenAlbedo
        Me.BlueAlbedo = blueAlbedo
    End Sub

    Public Sub New(color As RgbLight)
        Me.New(color.Red, color.Green, color.Blue)
    End Sub

    Public Sub New(color As Color)
        Me.New(New RgbLight(color))
    End Sub

    Public Function GetRemission(light As RgbLight) As RgbLight Implements IRemission(Of RgbLight).GetRemission
        Return New RgbLight(red:=Me.RedAlbedo * light.Red,
                              green:=Me.GreenAlbedo * light.Green,
                              blue:=Me.BlueAlbedo * light.Blue)
    End Function

    Public ReadOnly Property IsBlack As Boolean Implements IRemission(Of RgbLight).IsBlack
        Get
            Return Me.RedAlbedo = 0 AndAlso
                   Me.GreenAlbedo = 0 AndAlso
                   Me.BlueAlbedo = 0
        End Get
    End Property
End Class
