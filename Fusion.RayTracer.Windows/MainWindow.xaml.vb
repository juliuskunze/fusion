Class MainWindow
    Inherits Fluent.RibbonWindow

    Public Sub New()
        Me.InitializeComponent()
    End Sub

    Private Sub RenderButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles _RenderButton.Click
        Me.Render()
    End Sub

    Private Sub Render()
        Try
            Dim bitmapSource = New Bitmap(100, 100)
            Dim simpleBitmap = New SimpleBitmap(200, 200)

            For i = 0 To 100
                simpleBitmap.SetPixel(i, i, color:=System.Drawing.Color.White)
            Next

            '_Image.Source = simpleBitmap.ToBitmapSource
        Catch
        End Try
    End Sub

    Private Class SimpleBitmap

        Private ReadOnly _Width As Integer
        Private ReadOnly _Height As Integer
        Private ReadOnly _Bytes As Byte()

        Private Shared ReadOnly _PixelFormat As PixelFormat = PixelFormats.Rgb24
        Private ReadOnly _Stride As Integer

        Public Sub New(ByVal width As Integer, ByVal height As Integer)
            _Width = width
            _Height = height

            _Stride = 3 * width + (width * 3) Mod 4
            ReDim _Bytes(_Stride * _Height)
        End Sub

        Public Function ToBitmapSource() As BitmapSource
            Dim m As Matrix = PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformToDevice
            Dim dpiX = m.M11 * 96
            Dim dpiY = m.M22 * 96

            Return BitmapSource.Create(pixelWidth:=_Width,
                                       pixelHeight:=_Height,
                                       dpiX:=dpiX,
                                       dpiY:=dpiY,
                                       pixelFormat:=_PixelFormat,
                                       palette:=Nothing,
                                       pixels:=_Bytes,
                                       stride:=_Stride)
        End Function

        Public Sub SetPixel(ByVal x As Integer, ByVal y As Integer, ByVal color As System.Drawing.Color)
            _Bytes(y * _Stride + 3 * x) = color.R
            _Bytes(y * _Stride + 3 * x + 1) = color.G
            _Bytes(y * _Stride + 3 * x + 2) = color.B
        End Sub

    End Class

End Class
