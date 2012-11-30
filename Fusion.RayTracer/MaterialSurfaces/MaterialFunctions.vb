﻿Public Class MaterialFunctions(Of TLight)

    Public Shared Function Checkerboard(xVector As Vector3D, yVector As Vector3D, material1 As Material2D(Of TLight), material2 As Material2D(Of TLight)) As Func(Of SpaceTimeEvent, Material2D(Of TLight))
        Return Function(spaceTimeEvent)
                   Dim location = spaceTimeEvent.Location

                   Dim xLocation = xVector.Normalized * location
                   Dim yLocation = yVector.Normalized * location

                   Dim useMaterial1 =
                           IsInEvenRow(xLocation, rowWidth:=xVector.Length) Xor
                           IsInEvenRow(yLocation, rowWidth:=yVector.Length)

                   If useMaterial1 Then
                       Return material1
                   Else
                       Return material2
                   End If
               End Function
    End Function

    Private Shared Function IsInEvenRow(value As Double, rowWidth As Double) As Boolean
        Dim rest = value Mod (2 * rowWidth)
        If rest < 0 Then
            rest += 2
        End If

        Return rest < 1
    End Function

    Public Shared Function Grid2D(xVector As Vector3D, yVector As Vector3D, backgroundMaterial As Material2D(Of TLight), gridMaterial As Material2D(Of TLight), gridLineWidth As Double) As Func(Of SpaceTimeEvent, Material2D(Of TLight))
        Return Function(spaceTimeEvent)
                   Dim location = spaceTimeEvent.Location
                   Dim xLocation = xVector.Normalized * location
                   Dim yLocation = yVector.Normalized * location

                   Dim useGridMaterial =
                           IsInGrid(xLocation, rowWidth:=xVector.Length, gridLineWidth:=gridLineWidth) OrElse
                           IsInGrid(yLocation, rowWidth:=yVector.Length, gridLineWidth:=gridLineWidth)

                   If useGridMaterial Then
                       Return gridMaterial
                   Else
                       Return backgroundMaterial
                   End If
               End Function
    End Function

    Public Shared Function Grid3D(xVector As Vector3D, yVector As Vector3D, zVector As Vector3D, backgroundMaterial As Material2D(Of TLight), gridMaterial As Material2D(Of TLight), gridLineWidth As Double) As Func(Of SpaceTimeEvent, Material2D(Of TLight))
        Return Function(spaceTimeEvent)
                   Dim location = spaceTimeEvent.Location
                   Dim xLocation = xVector.Normalized * location
                   Dim yLocation = yVector.Normalized * location
                   Dim zLocation = zVector.Normalized * location

                   Dim useGridMaterial =
                           IsInGrid(xLocation, rowWidth:=xVector.Length, gridLineWidth:=gridLineWidth) OrElse
                           IsInGrid(yLocation, rowWidth:=yVector.Length, gridLineWidth:=gridLineWidth) OrElse
                           IsInGrid(zLocation, rowWidth:=zVector.Length, gridLineWidth:=gridLineWidth)

                   If useGridMaterial Then
                       Return gridMaterial
                   Else
                       Return backgroundMaterial
                   End If
               End Function
    End Function

    Private Shared Function IsInGrid(value As Double, rowWidth As Double, gridLineWidth As Double) As Boolean
        Dim m = PositiveMod(value, rowWidth)
        Dim halfGridLineWidth = gridLineWidth / 2

        Return m < halfGridLineWidth OrElse m > rowWidth - halfGridLineWidth
    End Function
End Class
