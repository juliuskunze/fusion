Public Class AffineMap2D
    Implements IMap2D

    Public Sub New(ByVal linearMap As LinearMap2D)
        Me.New(linearMap:=linearMap, transformationVector:=Vector2D.Zero)
    End Sub

    Public Sub New(ByVal linearMap As LinearMap2D, ByVal transformationVector As Vector2D)
        _linearMap = linearMap
        _translationVector = transformationVector
    End Sub

    Private _linearMap As LinearMap2D
    Public ReadOnly Property LinearMap() As LinearMap2D
        Get
            Return _linearMap
        End Get
    End Property

    Private _translationVector As Vector2D
    Public Property TranslationVector() As Vector2D
        Get
            Return _translationVector
        End Get
        Set(ByVal value As Vector2D)
            _translationVector = value
        End Set
    End Property

    Public Shared ReadOnly Property Identity() As AffineMap2D
        Get
            Return New AffineMap2D
        End Get
    End Property

    Public Function Apply(ByVal v As Vector2D) As Vector2D Implements IMap2D.Apply
        Return Me.LinearMap.Apply(v) + Me.TranslationVector
    End Function

    Public Function After(ByVal map As AffineMap2D) As AffineMap2D
        Return New AffineMap2D(Me.LinearMap.After(map.LinearMap), Me.Apply(map.TranslationVector))
    End Function

    Public Function Before(ByVal map As AffineMap2D) As AffineMap2D
        Return map.After(Me)
    End Function

    Public Function Before(ByVal map As LinearMap2D) As AffineMap2D
        Return New AffineMap2D(map).After(Me)
    End Function

    Public Function At(ByVal location As Vector2D) As AffineMap2D
        Return AffineMap2D.Translation(location).After(Me).After(AffineMap2D.Translation(-location))
    End Function

    Public Sub New()
        Me.New(LinearMap2D.Identity, Vector2D.Zero)
    End Sub

    Public Shared Function Translation(ByVal translationVector As Vector2D) As AffineMap2D
        Return New AffineMap2D(LinearMap2D.Identity, translationVector)
    End Function

    Public Shared Function Rotation(ByVal rotationAngle As Double) As AffineMap2D
        Return New AffineMap2D(LinearMap2D.Rotation(rotationAngle), Vector2D.Zero)
    End Function

    Public Shared Function Reflection(ByVal axisAngle As Double) As AffineMap2D
        Return New AffineMap2D(LinearMap2D.Reflection(axisAngle), Vector2D.Zero)
    End Function

    Public Shared Function Scaling(ByVal factor As Double) As AffineMap2D
        Return New AffineMap2D(LinearMap2D.Scaling(factor), Vector2D.Zero)
    End Function

    Public Shared Function HorizontalShearing(ByVal shearingFactor As Double) As AffineMap2D
        Return New AffineMap2D(LinearMap2D.HorizontalShearing(shearingFactor), Vector2D.Zero)
    End Function

    Public Shared Function HorizontalScaling(ByVal scalingFactor As Double) As AffineMap2D
        Return New AffineMap2D(LinearMap2D.HorizontalScaling(scalingFactor), Vector2D.Zero)
    End Function

    Public Function Inverse() As AffineMap2D
        Dim inverseLinearMap = Me.LinearMap.Inverse
        Return New AffineMap2D(inverseLinearMap, -inverseLinearMap.Apply(Me.TranslationVector))
    End Function

End Class
