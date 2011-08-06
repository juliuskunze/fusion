Public Class AffineMap2D
    Implements IMap2D

    Public Sub New(linearMap As LinearMap2D)
        Me.New(linearMap:=linearMap, transformationVector:=Vector2D.Zero)
    End Sub

    Public Sub New(linearMap As LinearMap2D, transformationVector As Vector2D)
        _LinearMap = linearMap
        _TranslationVector = transformationVector
    End Sub

    Private _LinearMap As LinearMap2D
    Public ReadOnly Property LinearMap() As LinearMap2D
        Get
            Return _LinearMap
        End Get
    End Property

    Private _TranslationVector As Vector2D
    Public Property TranslationVector() As Vector2D
        Get
            Return _TranslationVector
        End Get
        Set(value As Vector2D)
            _TranslationVector = value
        End Set
    End Property

    Public Shared ReadOnly Property Identity() As AffineMap2D
        Get
            Return New AffineMap2D
        End Get
    End Property

    Public Function Apply(v As Vector2D) As Vector2D Implements IMap2D.Apply
        Return Me.LinearMap.Apply(v) + Me.TranslationVector
    End Function

    Public Function After(map As AffineMap2D) As AffineMap2D
        Return New AffineMap2D(Me.LinearMap.After(map.LinearMap), Me.Apply(map.TranslationVector))
    End Function

    Public Function Before(map As AffineMap2D) As AffineMap2D
        Return map.After(Me)
    End Function

    Public Function Before(map As LinearMap2D) As AffineMap2D
        Return New AffineMap2D(map).After(Me)
    End Function

    Public Function At(location As Vector2D) As AffineMap2D
        Return AffineMap2D.Translation(location).After(Me).After(AffineMap2D.Translation(-location))
    End Function

    Public Sub New()
        Me.New(LinearMap2D.Identity, Vector2D.Zero)
    End Sub

    Public Shared Function Translation(translationVector As Vector2D) As AffineMap2D
        Return New AffineMap2D(LinearMap2D.Identity, translationVector)
    End Function

    Public Shared Function Rotation(rotationAngle As Double) As AffineMap2D
        Return New AffineMap2D(LinearMap2D.Rotation(rotationAngle), Vector2D.Zero)
    End Function

    Public Shared Function Reflection(axisAngle As Double) As AffineMap2D
        Return New AffineMap2D(LinearMap2D.Reflection(axisAngle), Vector2D.Zero)
    End Function

    Public Shared Function Scaling(factor As Double) As AffineMap2D
        Return New AffineMap2D(LinearMap2D.Scaling(factor), Vector2D.Zero)
    End Function

    Public Shared Function HorizontalShearing(shearingFactor As Double) As AffineMap2D
        Return New AffineMap2D(LinearMap2D.HorizontalShearing(shearingFactor), Vector2D.Zero)
    End Function

    Public Shared Function HorizontalScaling(scalingFactor As Double) As AffineMap2D
        Return New AffineMap2D(LinearMap2D.HorizontalScaling(scalingFactor), Vector2D.Zero)
    End Function

    Public Function Inverse() As AffineMap2D
        Dim inverseLinearMap = Me.LinearMap.Inverse
        Return New AffineMap2D(inverseLinearMap, -inverseLinearMap.Apply(Me.TranslationVector))
    End Function

End Class
