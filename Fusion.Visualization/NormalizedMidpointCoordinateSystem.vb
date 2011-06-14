Public Class NormalizedMidpointCoordinateSystem

    Public Sub New(ByVal pictureSize As Vector2D)
        Me.PictureSize = pictureSize
    End Sub

    Private _Origin As Vector2D

    Private _PictureSize As Vector2D
    Public Property PictureSize As Vector2D
        Get
            Return _PictureSize
        End Get
        Set(ByVal value As Vector2D)
            _PictureSize = value
            _Origin = _PictureSize / 2
        End Set
    End Property

    ''' <summary>
    ''' Projects a pixel location on a picture to a vector, which origin is the midpoint of the picture
    ''' and where the right picture bound has an x-coordinate 1; the y-coordinate has the same scale and is up-orientated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Function VirtualLocation(ByVal pixelLocation As Vector2D) As Vector2D
        Return (New Vector2D(pixelLocation.X, Me.PictureSize.Y - pixelLocation.Y) - _Origin) / (Me.PictureSize.X / 2)
    End Function

End Class
