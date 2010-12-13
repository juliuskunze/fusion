﻿Public Class PointSet2DTransformer
    Implements IPointSet2D

    Public Property PointSet As IPointSet2D
    Private _map As AffineMap2D
    Public Property Map As AffineMap2D
        Get
            Return _map
        End Get
        Set(ByVal value As AffineMap2D)
            _map = value
            _inverseMap = Me.Map.Inverse
        End Set
    End Property
    Private _inverseMap As AffineMap2D

    Public Sub New(ByVal pointSet As IPointSet2D, ByVal map As AffineMap2D)
        Me.PointSet = pointSet
        Me.Map = map
    End Sub

    Public Function Contains(ByVal point As Fusion.Math.Vector2D) As Boolean Implements IPointSet2D.Contains
        Return Me.PointSet.Contains(_inverseMap.Apply(point))
    End Function
End Class