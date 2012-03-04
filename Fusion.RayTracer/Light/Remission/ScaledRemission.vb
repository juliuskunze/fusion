Public Class ScaledRemission(Of TLight As {ILight(Of TLight), New})
    Implements IRemission(Of TLight)

    Public Property Albedo As Double

    Public Sub New(albedo As Double)
        Me.Albedo = albedo
    End Sub

    Public Function GetRemission(light As TLight) As TLight Implements IRemission(Of TLight).GetRemission
        Return light.MultiplyBrightness(Albedo)
    End Function

    Public ReadOnly Property IsBlack As Boolean Implements IRemission(Of TLight).IsBlack
        Get
            Return Albedo = 0
        End Get
    End Property
End Class
