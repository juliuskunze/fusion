Public Class PictureRenderer
    Inherits Renderer(Of System.Drawing.Bitmap)

    Private ReadOnly _Picture As RayTracerPicture(Of RadianceSpectrum)

    Public Sub New(picture As RayTracerPicture(Of RadianceSpectrum))
        _Picture = picture
    End Sub

    Private Sub BackgroundWorker_DoWork(sender As Object, e As ComponentModel.DoWorkEventArgs) Handles _BackgroundWorker.DoWork
        Dim resultBitmap = New System.Drawing.Bitmap(_Picture.PictureSize.Width, _Picture.PictureSize.Height)

        For bitmapX = 0 To _Picture.PictureSize.Width - 1
            If _BackgroundWorker.CancellationPending Then
                e.Cancel = True
                Return
            End If

            For bitmapY = 0 To _Picture.PictureSize.Height - 1
                _Picture.SetPixelColor(resultBitmap, bitmapX, bitmapY)
            Next
            ReportProgress(relativeProgress:=(bitmapX + 1) / _Picture.PictureSize.Width)
        Next

        e.Result = resultBitmap
    End Sub

    Public Function Run() As System.Drawing.Bitmap
        Dim resultBitmap = New System.Drawing.Bitmap(_Picture.PictureSize.Width, _Picture.PictureSize.Height)

        For bitmapX = 0 To _Picture.PictureSize.Width - 1
            For bitmapY = 0 To _Picture.PictureSize.Height - 1
                _Picture.SetPixelColor(resultBitmap, bitmapX, bitmapY)
            Next
        Next

        Return resultBitmap
    End Function
End Class
