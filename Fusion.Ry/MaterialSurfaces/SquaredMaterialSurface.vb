Public Class SquaredMaterialSurface(Of MaterialType)
    Implements ISurface(Of MaterialType)

    Public Property Surface As ISurface
    Public Property Material1 As MaterialType
    Public Property Material2 As MaterialType

    Public Sub New(ByVal surface As ISurface,
                   ByVal material1 As MaterialType, ByVal material2 As MaterialType,
                   ByVal squaresXVector As Vector3D, ByVal squaresYVector As Vector3D,
                   ByVal squareLength As Double)
        Me.Surface = surface
        Me.Material1 = material1
        Me.Material2 = material2
        Me.SquaresXVector = squaresXVector
        Me.SquaresYVector = squaresYVector
        Me.SquareLength = squareLength
    End Sub

    Public Property SquareLength As Double

    Private _NormalizedSquaresXVector As Vector3D
    Public ReadOnly Property NormalizedSquaresXVector As Vector3D
        Get
            Return _NormalizedSquaresXVector
        End Get
    End Property
    Public WriteOnly Property SquaresXVector As Vector3D
        Set(ByVal value As Vector3D)
            _NormalizedSquaresXVector = value.Normalized
        End Set
    End Property

    Private _NormalizedSquaresYVector As Vector3D
    Private ReadOnly Property NormalizedSquaresYVector As Vector3D
        Get
            Return _NormalizedSquaresYVector
        End Get
    End Property
    Public WriteOnly Property SquaresYVector As Vector3D
        Set(ByVal value As Vector3D)
            _NormalizedSquaresYVector = value.Normalized
        End Set
    End Property

    Private Function IsInEvenRow(ByVal value As Double) As Boolean
        Dim rest = value Mod (2 * Me.SquareLength)
        If rest < 0 Then
            rest += 2
        End If

        Return rest < 1
    End Function

    Public Function Intersections(ByVal ray As Math.Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return Me.Surface.Intersections(ray)
    End Function

    Public Function MaterialIntersections(ByVal ray As Math.Ray) As IEnumerable(Of SurfacePoint(Of MaterialType)) Implements ISurface(Of MaterialType).MaterialIntersections
        Return From surfacePoint In Me.Intersections(ray)
               Select MaterialSurfacePointFromSurfacePoint(surfacePoint:=surfacePoint)
    End Function

    Private Function MaterialSurfacePointFromSurfacePoint(ByVal surfacePoint As SurfacePoint) As SurfacePoint(Of MaterialType)
        Return New SurfacePoint(Of MaterialType)(surfacePoint:=surfacePoint, Material:=Me.Material(surfacePoint))
    End Function

    Private Function Material(ByVal surfacePoint As SurfacePoint) As MaterialType
        Dim xLocation = Me.NormalizedSquaresXVector * surfacePoint.Location
        Dim yLocation = Me.NormalizedSquaresYVector * surfacePoint.Location

        Dim useMaterial1 As Boolean = IsInEvenRow(xLocation) Xor IsInEvenRow(yLocation)

        Dim resultMaterial As MaterialType
        If useMaterial1 Then
            resultMaterial = Me.Material1
        Else
            resultMaterial = Me.Material2
        End If

        Return resultMaterial
    End Function

    Public Function FirstIntersection(ByVal ray As Math.Ray) As Math.SurfacePoint Implements Math.ISurface.FirstIntersection
        Return Me.Surface.FirstIntersection(ray)
    End Function

    Public Function FirstMaterialIntersection(ByVal ray As Math.Ray) As SurfacePoint(Of MaterialType) Implements ISurface(Of MaterialType).FirstMaterialIntersection
        Dim intersection = Me.FirstIntersection(ray)

        If intersection Is Nothing Then Return Nothing

        Return MaterialSurfacePointFromSurfacePoint(intersection)
    End Function
End Class
