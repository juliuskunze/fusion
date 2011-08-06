Public Class TruncatedSurface
    Implements ISurface
    
    Public Sub New(baseSurface As ISurface, truncatingPointSet As IPointSet3D)
        Me.BaseSurface = baseSurface
        Me.TruncatingPointSet = truncatingPointSet
    End Sub

    Public Property BaseSurface As ISurface
    Public Property TruncatingPointSet As IPointSet3D

    Public Function Intersections(ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return From intersection In Me.BaseSurface.Intersections(ray)
               Where Not Me.TruncatingPointSet.Contains(intersection.Location)
    End Function

    Public Function FirstIntersection(ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Return Me.Intersections(ray).FirstOrDefault
    End Function
End Class
