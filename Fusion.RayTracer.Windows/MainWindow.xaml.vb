Imports System.Windows.Controls.Primitives

Public Class MainWindow

    Private WithEvents _RayTracerPicture As RayTracerPicture(Of RadianceSpectrum)
    Private _ResultBitmap As System.Drawing.Bitmap
    Private _RenderStopwatch As Stopwatch

    Private WithEvents _RenderBackgroundWorker As ComponentModel.BackgroundWorker

    Private ReadOnly _RelativisticRayTracerTermContextBuilder As New RelativisticRayTracerTermContextBuilder
    Private ReadOnly _BaseContext As TermContext = _RelativisticRayTracerTermContextBuilder.TermContext
    Private _Compiler As RelativisticRayTracerPictureCompiler


    Private Event SceneChanged()

    Private ReadOnly _SavePictureDialog As New SaveFileDialog

    Public Sub New()
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

        Me.InitializeComponent()

        _RenderBackgroundWorker = New ComponentModel.BackgroundWorker
        _RenderBackgroundWorker.WorkerReportsProgress = True
        _RenderBackgroundWorker.WorkerSupportsCancellation = True

        _SavePictureDialog.DefaultExt = ".png"
        _SavePictureDialog.Filter = "Portable Network Graphics|*.png|Bitmap|*.bmp"
        _SavePictureDialog.FileName = "ray tracing picture"
        _SavePictureDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.Desktop
    End Sub

    Private Sub RenderButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _RenderButton.Click
        If Not Me.TryCompileRayTracerDrawerAndShowErrors Then Return

        _RenderButton.Visibility = Visibility.Collapsed
        _RenderCancelButton.Visibility = Visibility.Visible
        _RenderProgressBar.Visibility = Visibility.Visible
        Me.TaskbarItemInfo.ProgressState = Shell.TaskbarItemProgressState.Normal

        _RenderStopwatch = Stopwatch.StartNew

        _RenderBackgroundWorker.RunWorkerAsync(_RayTracerPicture)
    End Sub

    Private Function TryCompileRayTracerDrawerAndShowErrors() As Boolean
        _Compiler = New RelativisticRayTracerPictureCompiler(RichTextBox:=_SceneDescriptionTextBox, baseContext:=_BaseContext, TypeNamedTypeDictionary:=_RelativisticRayTracerTermContextBuilder.TypeDictionary)
        Try
            Dim compilerResult = _Compiler.GetResult

            _SceneDescriptionTextBox.Document = New FlowDocument(New Paragraph(New Run(compilerResult.CorrectedText)))

            _RayTracerPicture = compilerResult.Result
        Catch ex As CompilerException
            _ErrorTextBox.Text = ex.Message
            Return False
        End Try

        Return True
    End Function

    Private Sub SaveButton_Click(sender As System.Object, e As System.EventArgs) Handles _SaveButton.Click
        If _SavePictureDialog.ShowDialog Then
            Select Case _SavePictureDialog.FilterIndex
                Case 1
                    _ResultBitmap.Save(_SavePictureDialog.FileName, format:=System.Drawing.Imaging.ImageFormat.Png)
                Case 2
                    _ResultBitmap.Save(_SavePictureDialog.FileName, format:=System.Drawing.Imaging.ImageFormat.Bmp)
                Case Else
                    Throw New ArgumentOutOfRangeException("_SaveFileDialog.FilterIndex")
            End Select
        End If
    End Sub

    Private Sub CalculateNeededTimeButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _CalculateNeededTimeButton.Click
        If Not Me.TryCompileRayTracerDrawerAndShowErrors() Then Return
        If Not _CalculateTimeOptionsDialog.DialogResult Then Return

        Dim size = _RayTracerPicture.PictureSize

        Dim bitmap = New System.Drawing.Bitmap(size.Width, size.Height)

        Dim random = New Random

        Dim drawTimeStopwatch = New Stopwatch
        Dim testedPixelCount = 0

        Dim stopwatch = New Stopwatch
        stopwatch.Start()
        Do While If(_CalculateTimeOptionsDialog.Mode = CalculateTimeOptionsDialog.FixMode.Time,
                    stopwatch.ElapsedMilliseconds / 1000 < _CalculateTimeOptionsDialog.FixTestTime,
                    testedPixelCount < _CalculateTimeOptionsDialog.FixTestPixelCount)
            Dim randomX = random.Next(size.Width)
            Dim randomY = random.Next(size.Height)

            drawTimeStopwatch.Start()

            bitmap.SetPixel(randomX, randomY, _RayTracerPicture.GetPixelColor(randomX, randomY))

            drawTimeStopwatch.Stop()

            testedPixelCount += 1
        Loop

        stopwatch.Stop()

        _ResultImage.Source = New SimpleBitmap(bitmap).ToBitmapSource

        'experiment --> 
        Const factor = 3.4
        Dim ticksPerPixel = drawTimeStopwatch.ElapsedTicks / testedPixelCount * factor

        Dim timePerPixel = New TimeSpan(CLng(ticksPerPixel))
        _AverageNeededTimePerPixelLabel.Content = "Time per Pixel: " & timePerPixel.TotalMilliseconds.ToString & "ms"

        Dim picturePixelCount = size.Width * size.Height

        Dim overallTime = New TimeSpan(ticks:=CLng(ticksPerPixel * picturePixelCount))
        _TotalNeededTimeLabel.Content = "Overall time: " & overallTime.ToString
    End Sub

    Private _CalculateTimeOptionsDialog As New CalculateTimeOptionsDialog

    Private Sub CalculateNeededTimeOptionsButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _CalculateNeededTimeOptionsButton.Click
        _CalculateTimeOptionsDialog.ShowDialog()
    End Sub

    Private Sub RenderBackgroundWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles _RenderBackgroundWorker.DoWork
        Dim rayTracerDrawer = CType(e.Argument, RayTracerPicture(Of RadianceSpectrum))

        Dim resultBitmap = New System.Drawing.Bitmap(rayTracerDrawer.PictureSize.Width, rayTracerDrawer.PictureSize.Height)

        For bitmapX = 0 To rayTracerDrawer.PictureSize.Width - 1
            If _RenderBackgroundWorker.CancellationPending Then
                e.Cancel = True
                Return
            End If

            For bitmapY = 0 To rayTracerDrawer.PictureSize.Height - 1
                rayTracerDrawer.SetPixelColor(resultBitmap, bitmapX, bitmapY)
            Next
            _RenderBackgroundWorker.ReportProgress(CInt((bitmapX + 1) / rayTracerDrawer.PictureSize.Width * 100))
        Next

        e.Result = resultBitmap
    End Sub

    Private Sub RenderBackgroundWorker_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles _RenderBackgroundWorker.ProgressChanged
        _RenderProgressBar.Value = e.ProgressPercentage
    End Sub

    Private Sub RenderBackgroundWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles _RenderBackgroundWorker.RunWorkerCompleted
        If e.Error IsNot Nothing Then Throw e.Error

        _RenderStopwatch.Stop()

        _RenderProgressBar.Value = 0
        _RenderProgressBar.Visibility = Visibility.Collapsed
        _RenderCancelButton.Visibility = Visibility.Collapsed
        _RenderingTimeCalculationGroupBox.Visibility = Visibility.Collapsed

        _RenderButton.Visibility = Visibility.Visible
        _ResultTabItem.Visibility = Visibility.Visible

        Me.TaskbarItemInfo.ProgressState = Shell.TaskbarItemProgressState.None

        If e.Cancelled Then
            Me.RenderingTabItemsVisible = True
            Return
        End If

        _ResultBitmap = CType(e.Result, System.Drawing.Bitmap)

        _ResultImage.Source = New SimpleBitmap(_ResultBitmap).ToBitmapSource

        _TotalElapsedTimeLabel.Content = "Total elapsed time: " & _RenderStopwatch.Elapsed.ToString
        _AverageElapsedTimePerPixelLabel.Content = "Average elapsed time per pixel: " & (_RenderStopwatch.ElapsedMilliseconds / (_ResultBitmap.Size.Width * _ResultBitmap.Size.Height)).ToString & "ms"

        _ResultTabItem.IsSelected = True
    End Sub

    Private Sub RenderCancelButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _RenderCancelButton.Click
        _RenderBackgroundWorker.CancelAsync()
    End Sub

    Private Sub RenderProgressBar_ValueChanged(sender As Object, e As System.Windows.RoutedPropertyChangedEventArgs(Of Double)) Handles _RenderProgressBar.ValueChanged
        Me.TaskbarItemInfo.ProgressValue = e.NewValue / 100
    End Sub

    Private Sub RibbonWindow_Unloaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Unloaded
        Application.Current.Shutdown()
    End Sub

    Private Sub SceneDescriptionTextBox_TextChanged(sender As System.Object, e As System.Windows.Controls.TextChangedEventArgs) Handles _SceneDescriptionTextBox.TextChanged
        _SceneDescriptionTextBox.Popup.IsOpen = True
        Dim currentCharRect = _SceneDescriptionTextBox.Selection.Start.GetCharacterRect(LogicalDirection.Forward)

        _SceneDescriptionTextBox.Popup.VerticalOffset = -(_SceneDescriptionTextBox.ActualHeight - currentCharRect.Bottom)
        _SceneDescriptionTextBox.Popup.HorizontalOffset = currentCharRect.Left

        Dim listBoxItems = New List(Of ListBoxItem)
        For i = 0 To 3
            Dim listBoxItem = New ListBoxItem
            listBoxItem.Content = i

            Dim tooltip = New ToolTip
            tooltip.Content = "yo"
            tooltip.PlacementTarget = listBoxItem
            tooltip.Placement = Controls.Primitives.PlacementMode.Right
            tooltip.HorizontalOffset = 5
            
            listBoxItems.Add(listBoxItem)
        Next
        _SceneDescriptionTextBox.ItemList.ItemsSource = listBoxItems

        RaiseEvent SceneChanged()
    End Sub

    Private Sub MainWindow_SceneChanged() Handles Me.SceneChanged
        Me.TryCompileAndAdaptVisibilities()
    End Sub

    Private Sub TryCompileAndAdaptVisibilities()
        Me.RenderingTabItemsVisible = Me.TryCompileRayTracerDrawerAndShowErrors()
    End Sub

    Private Property RenderingTabItemsVisible As Boolean
        Get
            Return _RenderingTabItem.Visibility = Visibility.Visible
        End Get
        Set(value As Boolean)
            If value Then
                _RenderingTabItem.Visibility = Visibility.Visible
            Else
                _RenderingTabItem.Visibility = Visibility.Collapsed
            End If
        End Set
    End Property

    Private Property IsSceneDescriptionChangeable As Boolean
        Get
            Return _SceneDescriptionTextBox.IsEnabled
        End Get
        Set(value As Boolean)
            _SceneDescriptionTextBox.IsEnabled = value
        End Set
    End Property

    Private Sub CompileSceneButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles _CompileSceneButton.Click
        Me.TryCompileAndAdaptVisibilities()

        _SceneDescriptionTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline)
    End Sub

End Class
