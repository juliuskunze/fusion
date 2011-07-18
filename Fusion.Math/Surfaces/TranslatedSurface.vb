Public Class TranslatedSurface
    Implements ISurface

    Private ReadOnly _Original As ISurface
    Private ReadOnly _Translation As Vector3D

    Public Sub New(ByVal original As ISurface, ByVal translation As Vector3D)
        _Original = original
        _Translation = translation
    End Sub

    Public Function FirstIntersection(ByVal ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Dim intersection = _Original.FirstIntersection(TranslatedRay(ray, -_Translation))

        Return TranslatedSurfacePoint(intersection, _Translation)
    End Function

    Public Function Intersections(ByVal ray As Ray) As System.Collections.Generic.IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Dim rayIntersections = _Original.Intersections(TranslatedRay(ray, -_Translation))

        Return rayIntersections.Select(Function(intersection) TranslatedSurfacePoint(intersection, _Translation))
    End Function

    Public Shared Function TranslatedRay(ByVal original As Ray, ByVal translation As Vector3D) As Ray
        Return New Ray(Origin:=original.Origin + translation, direction:=original.NormalizedDirection)
    End Function

    Public Shared Function TranslatedSurfacePoint(ByVal original As SurfacePoint, ByVal translation As Vector3D) As SurfacePoint
        Return New SurfacePoint(location:=original.Location + translation, normal:=original.NormalizedNormal)
    End Function

End Class
