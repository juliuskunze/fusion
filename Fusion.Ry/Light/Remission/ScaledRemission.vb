Public Class ScaledRemission(Of TLight As {ILight(Of TLight), New})
    Implements IRemission(Of TLight)

    Public Property Albedo As Double

    Public Sub New(ByVal albedo As Double)
        Me.Albedo = albedo
    End Sub

    Public Function GetRemission(ByVal light As TLight) As TLight Implements IRemission(Of TLight).GetRemission
        Return light.MultiplyBrighness(Me.Albedo)
    End Function

    Public ReadOnly Property NoRemission As Boolean Implements IRemission(Of TLight).NoRemission
        Get
            Return Me.Albedo = 0
        End Get
    End Property
End Class
