Public Class RayTracerPicture(Of TLight As {ILight(Of TLight), New})

    Private ReadOnly _LightToRgbColorConverter As ILightToRgbColorConverter(Of TLight)

    Private _PicturSize As Size
    Public Property PictureSize As Size
        Get
            Return _PicturSize
        End Get
        Set(value As Size)
            _PicturSize = value
            _CoordinateSystem = New NormalizedMidpointCoordinateSystem(PictureSize:=New Vector2D(value))
        End Set
    End Property

    Private _CoordinateSystem As NormalizedMidpointCoordinateSystem

    Public Property View As View3D

    Public Property RayTracer As IRayTracer(Of TLight)

    Public Sub New(rayTracer As IRayTracer(Of TLight),
                   pictureSize As Size,
                   view As View3D,
                   lightToRgbColorConverter As ILightToRgbColorConverter(Of TLight))

        If pictureSize = New Size Then Throw New ArgumentNullException("pictureSize")

        Me.PictureSize = pictureSize
        Me.View = view
        Me.RayTracer = rayTracer
        _LightToRgbColorConverter = lightToRgbColorConverter
    End Sub

    Public Function GetPicture() As Bitmap
        Dim bitmap = New Bitmap(Me.PictureSize.Width, Me.PictureSize.Height)

        For bitmapX = 0 To Me.PictureSize.Width - 1
            For bitmapY = 0 To Me.PictureSize.Height - 1
                Me.SetPixelColor(bitmap, bitmapX, bitmapY)
            Next
        Next

        Return bitmap
    End Function

    Public Sub SetPixelColor(targetBitmap As Bitmap, bitmapX As Integer, bitmapY As Integer)
        targetBitmap.SetPixel(x:=bitmapX, y:=bitmapY, color:=Me.GetPixelColor(bitmapX, bitmapY))
    End Sub

    Public Function GetPixelColor(bitmapX As Integer, bitmapY As Integer) As Color
        Dim projectedLocation = _CoordinateSystem.VirtualLocation(pixelLocation:=New Vector2D(bitmapX, bitmapY))
        Dim sightRay = Me.View.SightRay(viewPlaneLocation:=projectedLocation)

        Return _LightToRgbColorConverter.Run(Me.RayTracer.GetLight(sightRay:=sightRay))
    End Function
End Class