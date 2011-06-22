Class MainWindow 
    
    Private Sub RenderButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles _RenderButton.Click
        Render()
    End Sub

    Private Sub Render()
        Try
            Dim simpleBitmap = New SimpleBitmap(200, 200, dpi:=CInt(_DpiTextBox.Text))

            For i = 0 To 100
                simpleBitmap.SetPixel(i, i, color:=Colors.White)
            Next

            _Image.Source = simpleBitmap.ToBitmapSource
        Catch
        End Try
    End Sub

    Private Class SimpleBitmap

        Private ReadOnly _Width As Integer
        Private ReadOnly _Height As Integer
        Private ReadOnly _Bytes As Byte()
        
        Private Shared ReadOnly _PixelFormat As PixelFormat = PixelFormats.Rgb24
        Private ReadOnly _Stride As Integer
        Private ReadOnly _Dpi As Integer

        Public Sub New(ByVal width As Integer, ByVal height As Integer, ByVal dpi As Integer)
            _Width = width
            _Height = height
            _Dpi = dpi

            _Stride = 3 * width + (width * 3) Mod 4 '+ ((3 * width) Mod 4) 'CInt((_Width * _PixelFormat.BitsPerPixel + 7) / 8)
            ReDim _Bytes(_Stride * _Height)
        End Sub

        Public Function ToBitmapSource() As BitmapSource
            'Dim value = New Random
            'value.NextBytes(_Bytes)

            Return BitmapSource.Create(pixelWidth:=_Width,
                                       pixelHeight:=_Height,
                                       dpiX:=_Dpi,
                                       dpiY:=_Dpi,
                                       pixelFormat:=_PixelFormat,
                                       palette:=Nothing,
                                       pixels:=_Bytes,
                                       stride:=_Stride)
        End Function

        Public Sub SetPixel(ByVal x As Integer, ByVal y As Integer, ByVal color As Color)
            _Bytes(y * _Stride + 3 * x) = color.R
            _Bytes(y * _Stride + 3 * x + 1) = color.G
            _Bytes(y * _Stride + 3 * x + 2) = color.B
        End Sub

    End Class

    Private Sub DpiTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles _DpiTextBox.TextChanged
        Render()
    End Sub


End Class
