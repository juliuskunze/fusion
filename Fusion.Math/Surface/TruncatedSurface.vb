Public Class TruncatedSurface
    Implements ISurface
    
    Public Sub New(ByVal baseSurface As ISurface, ByVal truncatingPointSet As IPointSet3D)
        Me.BaseSurface = baseSurface
        Me.TruncatingPointSet = truncatingPointSet
    End Sub

    Public Property BaseSurface As ISurface
    Public Property TruncatingPointSet As IPointSet3D

    Public Function Intersections(ByVal ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return From intersection In Me.BaseSurface.Intersections(ray)
               Where Not Me.TruncatingPointSet.Contains(intersection.Location)
    End Function

    Public Function FirstIntersection(ByVal ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Return Me.Intersections(ray).FirstOrDefault
    End Function
End Class
