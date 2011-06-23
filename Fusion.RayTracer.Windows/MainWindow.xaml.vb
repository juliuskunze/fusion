Class MainWindow
    
    Public Sub New()
        Me.InitializeComponent()
    End Sub

    Private Sub RenderButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles _RenderButton.Click
        Me.Render()
    End Sub

    Private Sub Render()
        Try
            Dim bitmapSource = New System.Drawing.Bitmap(100, 100)
            Dim simpleBitmap = New SimpleBitmap(200, 200)

            For i = 0 To 100
                simpleBitmap.SetPixel(i, i, color:=System.Drawing.Color.White)
            Next

            _Image.Source = simpleBitmap.ToBitmapSource
        Catch
        End Try
    End Sub

End Class
