Public Class RayTraceDrawer(Of TLight As {ILight(Of TLight), New})

    Public Sub New(ByVal rayTracer As IRayTracer(Of TLight), ByVal pictureSize As Size, ByVal view As View3D)
        If pictureSize = New Size Then Throw New ArgumentNullException("pictureSize")

        Me.PictureSize = pictureSize
        Me.View = view
        Me.RayTracer = rayTracer
    End Sub

    Private _PicturSize As Size
    Public Property PictureSize As Size
        Get
            Return _PicturSize
        End Get
        Set(ByVal value As Size)
            _PicturSize = value
            _CoordinateSystem = New NormalizedMidpointCoordinateSystem(New Vector2D(value))
        End Set
    End Property

    Private _CoordinateSystem As NormalizedMidpointCoordinateSystem

    Public Property View As View3D

    Public Property RayTracer As IRayTracer(Of TLight)

    Private _Origin As Vector2D

    Public Function GetPicture() As Bitmap
        Dim bitmap = New Bitmap(Me.PictureSize.Width, Me.PictureSize.Height)

        For bitmapX = 0 To Me.PictureSize.Width - 1
            For bitmapY = 0 To Me.PictureSize.Height - 1
                Me.SetPixelColor(bitmap, bitmapX, bitmapY)
            Next
            RaiseEvent ProgressIncreased(Me, New ProgressEventArgs((bitmapX + 1) / Me.PictureSize.Width))
        Next

        Return bitmap
    End Function

    Public Sub SetPixelColor(ByVal bitmap As Bitmap, ByVal bitmapX As Integer, ByVal bitmapY As Integer)
        bitmap.SetPixel(x:=bitmapX, y:=bitmapY, color:=Me.GetPixelColor(bitmapX, bitmapY))
    End Sub

    Public Function GetPixelColor(ByVal bitmapX As Integer, ByVal bitmapY As Integer) As Color
        Dim projectedLocation = _CoordinateSystem.VirtualLocation(pixelLocation:=New Vector2D(bitmapX, bitmapY))
        Dim sightRay = Me.View.SightRay(viewPlaneLocation:=projectedLocation)

        Return Me.RayTracer.GetColor(startRay:=sightRay).ToColor
    End Function

    Public Event ProgressIncreased(ByVal sender As Object, ByVal e As ProgressEventArgs)

End Class

Public Class ProgressEventArgs

    Public Property Progress As Double

    Public Sub New(ByVal progress As Double)
        Me.Progress = progress
    End Sub
    
End Class