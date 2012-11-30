Public Class MaterialBox(Of TLight)
    Inherits MaterialSurface(Of TLight)

    Private Shared ReadOnly _LowerXNormal As New Vector3D(-1, 0, 0)
    Private Shared ReadOnly _LowerYNormal As New Vector3D(0, -1, 0)
    Private Shared ReadOnly _LowerZNormal As New Vector3D(0, 0, -1)
    Private Shared ReadOnly _UpperXNormal As New Vector3D(1, 0, 0)
    Private Shared ReadOnly _UpperYNormal As New Vector3D(0, 1, 0)
    Private Shared ReadOnly _UpperZNormal As New Vector3D(0, 0, 1)

    Public Sub New(box As Box,
                   lowerXMaterial As Material2D(Of TLight),
                   upperXMaterial As Material2D(Of TLight),
                   lowerYMaterial As Material2D(Of TLight),
                   upperYMaterial As Material2D(Of TLight),
                   lowerZMaterial As Material2D(Of TLight),
                   upperZMaterial As Material2D(Of TLight))
        MyBase.New(surface:=box, materialFunction:=
                   Function(spaceTimeEvent, surfacePoint)
                       If surfacePoint Is Nothing Then Return Nothing

                       Select Case surfacePoint.NormalizedNormal
                           Case _LowerXNormal : Return lowerXMaterial
                           Case _UpperXNormal : Return upperXMaterial
                           Case _LowerYNormal : Return lowerYMaterial
                           Case _UpperYNormal : Return upperYMaterial
                           Case _LowerZNormal : Return lowerZMaterial
                           Case _UpperZNormal : Return upperZMaterial
                           Case Else : Throw New ArgumentOutOfRangeException("surfacePoint")
                       End Select
                   End Function)
    End Sub
End Class
