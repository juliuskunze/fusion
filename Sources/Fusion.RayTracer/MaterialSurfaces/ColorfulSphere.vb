Public Class ColorfulSphere
    Inherits MaterialSurface(Of RgbLight)

    Public Sub New(sphere As Sphere, material As Material2D(Of RgbLight))
        Me.New(Center:=sphere.Center, Radius:=sphere.Radius, material:=material)
    End Sub

    Public Sub New(center As Vector3D, radius As Double, material As Material2D(Of RgbLight))
        MyBase.New(New Sphere(center, radius),
                   materialFunction:=Function(spaceTimeEvent As SpaceTimeEvent, surfacePoint As SurfacePoint)
                                         Dim newMaterial = material.Clone()

                                         newMaterial.SourceLight = GetColorFromDirection(surfacePoint.NormalizedNormal)

                                         Return newMaterial
                                     End Function)
    End Sub

    Private Shared Function GetColorFromDirection(normalizedNormal As Vector3D) As RgbLight
        Return New RgbLight(GetColorComponent(normalizedNormal.X),
                              GetColorComponent(normalizedNormal.Y),
                              GetColorComponent(normalizedNormal.Z))
    End Function

    Private Shared Function GetColorComponent(value As Double) As Double
        Return (value + 1) / 2
    End Function
End Class
