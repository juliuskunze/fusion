Public Class ComponentScaledColorRemission
    Implements IRemission(Of ExactColor)

    Public Property RedAlbedo As Double
    Public Property GreenAlbedo As Double
    Public Property BlueAlbedo As Double

    Public Sub New(ByVal redAlbedo As Double, ByVal greenAlbedo As Double, ByVal blueAlbedo As Double)
        Me.RedAlbedo = redAlbedo
        Me.GreenAlbedo = greenAlbedo
        Me.BlueAlbedo = blueAlbedo
    End Sub

    Public Sub New(ByVal color As ExactColor)
        Me.New(color.Red, color.Green, color.Blue)
    End Sub

    Public Sub New(ByVal color As Color)
        Me.New(New ExactColor(color))
    End Sub

    Public Function GetRemission(ByVal light As ExactColor) As ExactColor Implements IRemission(Of ExactColor).GetRemission
        Return New ExactColor(red:=Me.RedAlbedo * light.Red,
                              green:=Me.GreenAlbedo * light.Green,
                              blue:=Me.BlueAlbedo * light.Blue)
    End Function

    Public ReadOnly Property NoRemission As Boolean Implements IRemission(Of ExactColor).NoRemission
        Get
            Return Me.RedAlbedo = 0 AndAlso
                Me.GreenAlbedo = 0 AndAlso
                Me.BlueAlbedo = 0
        End Get
    End Property
End Class
