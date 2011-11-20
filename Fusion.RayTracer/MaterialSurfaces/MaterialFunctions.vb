Public Class MaterialFunctions(Of TMaterial)

    Public Shared Function Checkerboard(xVector As Vector3D, yVector As Vector3D, material1 As TMaterial, material2 As TMaterial) As Func(Of Vector3D, TMaterial)
        Return Function(location)
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

End Class
