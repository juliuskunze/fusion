Public Class MultiMaterialBox(Of TMaterial)
    Implements ISurface(Of TMaterial)

    Private ReadOnly _Box As Box

    Private ReadOnly _LowerXMaterial As TMaterial
    Private ReadOnly _LowerYMaterial As TMaterial
    Private ReadOnly _LowerZMaterial As TMaterial

    Private ReadOnly _UpperXMaterial As TMaterial
    Private ReadOnly _UpperYMaterial As TMaterial
    Private ReadOnly _UpperZMaterial As TMaterial

    Private ReadOnly _LowerXNormal As New Vector3D(-1, 0, 0)
    Private ReadOnly _LowerYNormal As New Vector3D(0, -1, 0)
    Private ReadOnly _LowerZNormal As New Vector3D(0, 0, -1)

    Private ReadOnly _UpperXNormal As New Vector3D(1, 0, 0)
    Private ReadOnly _UpperYNormal As New Vector3D(0, 1, 0)
    Private ReadOnly _UpperZNormal As New Vector3D(0, 0, 1)

    Public Sub New(ByVal box As Box,
                   ByVal lowerXMaterial As TMaterial,
                   ByVal upperXMaterial As TMaterial,
                   ByVal lowerYMaterial As TMaterial,
                   ByVal upperYMaterial As TMaterial,
                   ByVal lowerZMaterial As TMaterial,
                   ByVal upperZMaterial As TMaterial)
        _Box = box

        _LowerXMaterial = lowerXMaterial
        _UpperXMaterial = upperXMaterial
        _LowerYMaterial = lowerYMaterial
        _UpperYMaterial = upperYMaterial
        _LowerZMaterial = lowerZMaterial
        _UpperZMaterial = upperZMaterial
    End Sub

    Public Function FirstIntersection(ByVal ray As Math.Ray) As Math.SurfacePoint Implements Math.ISurface.FirstIntersection
        Return _Box.FirstIntersection(ray)
    End Function

    Public Function Intersections(ByVal ray As Math.Ray) As System.Collections.Generic.IEnumerable(Of Math.SurfacePoint) Implements Math.ISurface.Intersections
        Return _Box.Intersections(ray)
    End Function

    Public Function FirstMaterialIntersection(ByVal ray As Math.Ray) As SurfacePoint(Of TMaterial) Implements ISurface(Of TMaterial).FirstMaterialIntersection
        Return Me.GetMaterialSurfacePoint(Me.FirstIntersection(ray))
    End Function

    Public Function MaterialIntersections(ByVal ray As Math.Ray) As System.Collections.Generic.IEnumerable(Of SurfacePoint(Of TMaterial)) Implements ISurface(Of TMaterial).MaterialIntersections
        Return Me.Intersections(ray).Select(Function(surfacePoint) Me.GetMaterialSurfacePoint(surfacePoint))
    End Function

    Private Function GetMaterialSurfacePoint(ByVal surfacePoint As SurfacePoint) As SurfacePoint(Of TMaterial)
        If surfacePoint Is Nothing Then Return Nothing

        Select Case surfacePoint.NormalizedNormal
            Case _LowerXNormal
                Return New SurfacePoint(Of TMaterial)(surfacePoint, _LowerXMaterial)
            Case _UpperXNormal
                Return New SurfacePoint(Of TMaterial)(surfacePoint, _UpperXMaterial)

            Case _LowerYNormal
                Return New SurfacePoint(Of TMaterial)(surfacePoint, _LowerYMaterial)
            Case _UpperYNormal
                Return New SurfacePoint(Of TMaterial)(surfacePoint, _UpperYMaterial)

            Case _LowerZNormal
                Return New SurfacePoint(Of TMaterial)(surfacePoint, _LowerZMaterial)
            Case _UpperZNormal
                Return New SurfacePoint(Of TMaterial)(surfacePoint, _UpperZMaterial)

            Case Else
                Throw New ArgumentOutOfRangeException("surfacePoint.NormalizedNormal")
        End Select
    End Function

End Class
