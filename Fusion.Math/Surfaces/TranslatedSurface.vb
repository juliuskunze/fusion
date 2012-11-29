Public Class TranslatedSurface
    Implements ISurface

    Private ReadOnly _Original As ISurface
    Private ReadOnly _Translation As Vector3D

    Public Sub New(original As ISurface, translation As Vector3D)
        _Original = original
        _Translation = translation
    End Sub

    Public Function FirstIntersection(ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Dim intersection = _Original.FirstIntersection(TranslatedRay(ray, -_Translation))

        Return TranslatedSurfacePoint(intersection, _Translation)
    End Function

    Public Function Intersections(ray As Ray) As System.Collections.Generic.IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Dim rayIntersections = _Original.Intersections(TranslatedRay(ray, -_Translation))

        Return rayIntersections.Select(Function(intersection) TranslatedSurfacePoint(intersection, _Translation))
    End Function

    Public Shared Function TranslatedRay(original As Ray, translation As Vector3D) As Ray
        Return New Ray(Origin:=original.Origin + translation, direction:=original.NormalizedDirection)
    End Function

    Public Shared Function TranslatedSurfacePoint(original As SurfacePoint, translation As Vector3D) As SurfacePoint
        Return New SurfacePoint(location:=original.Location + translation, normal:=original.NormalizedNormal)
    End Function
End Class
