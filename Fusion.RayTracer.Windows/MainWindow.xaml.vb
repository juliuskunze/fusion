Imports System.Windows.Controls.Primitives

Public Class MainWindow

    Private ReadOnly _SaveDescriptionDialog As SaveFileDialog
    Private ReadOnly _OpenDescriptionDialog As OpenFileDialog

    Private WithEvents _PictureCompiler As RichCompiler(Of RayTracerPicture(Of RadianceSpectrum))
    Private WithEvents _Picture As RayTracerPicture(Of RadianceSpectrum)
    Private _ResultBitmap As System.Drawing.Bitmap

    Private WithEvents _VideoCompiler As RichCompiler(Of RayTracerVideo(Of RadianceSpectrum))
    Private WithEvents _Video As RayTracerVideo(Of RadianceSpectrum)

    Private _RenderStopwatch As Stopwatch
    Private WithEvents _RenderBackgroundWorker As ComponentModel.BackgroundWorker

    Private ReadOnly _RelativisticRayTracerTermContextBuilder As RelativisticRayTracerTermContextBuilder

    Private ReadOnly _DefaultInitialDirectory As String

    Private ReadOnly _SavePictureDialog As SaveFileDialog
    Private ReadOnly _SaveVideoDialog As SaveFileDialog

    Private ReadOnly _OpenVideoDirectoryDialog As System.Windows.Forms.FolderBrowserDialog

    Private _CurrentFileName As String

    Public Sub New(relativisticRayTracerTermContextBuilder As RelativisticRayTracerTermContextBuilder)
        Me.InitializeComponent()

        _RelativisticRayTracerTermContextBuilder = relativisticRayTracerTermContextBuilder
        _DefaultInitialDirectory = My.Computer.FileSystem.SpecialDirectories.Desktop

        _RenderBackgroundWorker = New ComponentModel.BackgroundWorker With {.WorkerReportsProgress = True, .WorkerSupportsCancellation = True}

        _SavePictureDialog = New SaveFileDialog With {
            .DefaultExt = ".png",
            .Filter = "Portable Network Graphics|*.png|Bitmap|*.bmp",
            .FileName = "Ray tracer picture",
            .InitialDirectory = _DefaultInitialDirectory}

        _SaveVideoDialog = New SaveFileDialog With {
            .DefaultExt = ".avi",
            .Filter = "Audio Video Interleave|*.avi",
            .FileName = "Ray tracer video",
            .InitialDirectory = _DefaultInitialDirectory}

        Dim filter = Me.GetFileExtensionFilter(CompileMode.Picture) & "|" & Me.GetFileExtensionFilter(CompileMode.Video)

        _SaveDescriptionDialog = New SaveFileDialog With {
            .FileName = "Ray tracer picture scene description",
            .InitialDirectory = _DefaultInitialDirectory}

        _OpenDescriptionDialog = New OpenFileDialog With {
            .Filter = filter,
            .InitialDirectory = _DefaultInitialDirectory}

        _OpenVideoDirectoryDialog = New System.Windows.Forms.FolderBrowserDialog With {.SelectedPath = _DefaultInitialDirectory}

        Me.Mode = CompileMode.Picture
    End Sub

    Private Function GetCompiler(Of TResult)() As RichCompiler(Of TResult)
        Return New RichCompiler(Of TResult)(RichTextBox:=_SceneDescriptionTextBox,
                                            autoCompletePopup:=_AutoCompletePopup,
                                            autoCompleteListBox:=_AutoCompleteListBox,
                                            autoCompleteScrollViewer:=_AutoCompleteScrollViewer,
                                            baseContext:=_RelativisticRayTracerTermContextBuilder.TermContext,
                                            TypeNamedTypeDictionary:=_RelativisticRayTracerTermContextBuilder.TypeDictionary)
    End Function

    Private Sub RenderButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _RenderButton.Click
        _RenderButton.Visibility = Visibility.Collapsed
        _RenderCancelButton.Visibility = Visibility.Visible
        _RenderProgressBar.Visibility = Visibility.Visible
        Me.TaskbarItemInfo.ProgressState = Shell.TaskbarItemProgressState.Normal

        _RenderStopwatch = Stopwatch.StartNew

        _RenderBackgroundWorker.RunWorkerAsync(_Picture)
    End Sub

    Private Sub Compiler_Compiled(sender As Object, e As CompilerResultEventArgs(Of RayTracerPicture(Of RadianceSpectrum))) Handles _PictureCompiler.Compiled
        OnCompiled(e, out_result:=_Picture)
    End Sub

    Private Sub Compiler_Compiled(sender As Object, e As CompilerResultEventArgs(Of RayTracerVideo(Of RadianceSpectrum))) Handles _VideoCompiler.Compiled
        OnCompiled(e, out_result:=_Video)
    End Sub

    Private Sub OnCompiled(Of TResult)(ByVal e As CompilerResultEventArgs(Of TResult), ByRef out_result As TResult)
        If e.CompilerResult.WasCompilationSuccessful Then
            out_result = e.CompilerResult.Result
        Else
            _ErrorTextBox.Text = e.CompilerResult.ErrorMessage
        End If

        Me.RenderingTabItemsVisible = e.CompilerResult.WasCompilationSuccessful
    End Sub

    Private Sub SaveButton_Click(sender As System.Object, e As System.EventArgs) Handles _SavePictureButton.Click
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
        If Not _CalculateTimeOptionsDialog.DialogResult Then Return

        Dim size = _Picture.PictureSize

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

            bitmap.SetPixel(randomX, randomY, _Picture.GetPixelColor(randomX, randomY))

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

    Private Sub MainWindow_Deactivated(sender As Object, e As EventArgs) Handles Me.Deactivated
        _PictureCompiler.Unfocus()
    End Sub

    Private Sub MainWindow_KeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles Me.KeyDown
        If Keyboard.IsKeyDown(Key.LeftCtrl) OrElse Keyboard.IsKeyDown(Key.RightCtrl) Then
            Select Case e.Key
                Case Key.S
                    Me.SaveDescription()

                    e.Handled = True
                Case Key.O
                    Me.ShowOpenDescriptionDialog()

                    e.Handled = True
                Case Key.F5
                    Me.Compile()

                    e.Handled = True
            End Select
        End If
    End Sub

    Private Sub Compile()
        Select Case Me.Mode
            Case CompileMode.Picture
                _PictureCompiler.Compile()
            Case CompileMode.Video
                _VideoCompiler.Compile()
        End Select
    End Sub

    Private Property RenderingTabItemsVisible As Boolean
        Get
            Return _PictureRenderingTabItem.Visibility = Visibility.Visible
        End Get
        Set(value As Boolean)
            If value Then
                _PictureRenderingTabItem.Visibility = Visibility.Visible
            Else
                _PictureRenderingTabItem.Visibility = Visibility.Collapsed
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

    Private Sub CompileMenuItem_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles _CompileMenuItem.Click
        _PictureCompiler.Compile()
    End Sub

    Private Enum CompileMode
        Picture
        Video
    End Enum

    Private Property Mode As CompileMode
        Get
            Return If(_CompilePictureMenuItem.IsChecked, CompileMode.Picture, CompileMode.Video)
        End Get
        Set(value As CompileMode)
            _CompilePictureMenuItem.IsChecked = (value = CompileMode.Picture)
            _CompileVideoMenuItem.IsChecked = (value = CompileMode.Video)

            Select Case value
                Case CompileMode.Picture
                    _PictureCompiler = Me.GetCompiler(Of RayTracerPicture(Of RadianceSpectrum))()
                    _PictureCompiler.Compile()
                Case CompileMode.Video
                    _VideoCompiler = Me.GetCompiler(Of RayTracerVideo(Of RadianceSpectrum))()
                    _VideoCompiler.Compile()
            End Select


        End Set
    End Property

    Private Sub SaveDescriptionMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _SaveDescriptionMenuItem.Click
        Me.SaveDescription()
    End Sub

    Private Sub SaveDescriptionAsMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _SaveDescriptionAsMenuItem.Click
        Me.ShowSaveDescriptionAsDialog()
    End Sub

    Private Sub ShowSaveDescriptionAsDialog()
        If IO.File.Exists(_CurrentFileName) Then _SaveDescriptionDialog.FileName = _CurrentFileName

        _SaveDescriptionDialog.DefaultExt = Me.GetDescriptionFileExtension

        If _SaveDescriptionDialog.ShowDialog(owner:=Me) Then
            Me.SaveDescription(_SaveDescriptionDialog.FileName)
        End If
    End Sub

    Private Sub SaveDescription()
        If IO.File.Exists(_CurrentFileName) Then
            Me.SaveDescription(_CurrentFileName)
        Else
            Me.ShowSaveDescriptionAsDialog()
        End If
    End Sub

    Private Sub SaveDescription(fileName As String)
        _CurrentFileName = fileName

        Dim description = New TextOnlyDocument(_SceneDescriptionTextBox.Document).Text

        Try
            Using streamWriter = New IO.StreamWriter(fileName)
                streamWriter.Write(description)
            End Using
        Catch ex As IO.IOException
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Function GetDescriptionFileExtension(mode As CompileMode) As String
        Select Case mode
            Case CompileMode.Picture
                Return ".pic"
            Case CompileMode.Video
                Return ".vid"
            Case Else
                Throw New ArgumentOutOfRangeException
        End Select
    End Function

    Private Function GetDescriptionFileExtension() As String
        Return Me.GetDescriptionFileExtension(Me.Mode)
    End Function

    Private Function GetMode(fileExtension As String) As CompileMode
        Select Case fileExtension
            Case ".pic" : Return CompileMode.Picture
            Case ".vid" : Return CompileMode.Video
            Case Else : Throw New ArgumentOutOfRangeException
        End Select
    End Function

    Private Function GetFileExtensionFilter() As String
        Return Me.GetFileExtensionFilter(Me.Mode)
    End Function

    Private Function GetFileExtensionFilter(mode As CompileMode) As String
        Select Case mode
            Case CompileMode.Picture : Return "Ray tracer picture scene description|*.pic"
            Case CompileMode.Video : Return "Ray tracer video scene description|*.vid"
            Case Else : Throw New ArgumentOutOfRangeException
        End Select
    End Function

    Private Sub OpenDescriptionSourceMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _OpenDescriptionMenuItem.Click
        Me.ShowOpenDescriptionDialog()
    End Sub

    Private Sub ShowOpenDescriptionDialog()
        If _OpenDescriptionDialog.ShowDialog(owner:=Me) Then
            _CurrentFileName = _OpenDescriptionDialog.FileName

            Dim text As String

            Try
                Using streamReader = New IO.StreamReader(_CurrentFileName)
                    text = streamReader.ReadToEnd()
                End Using
            Catch ex As IO.IOException
                MessageBox.Show(ex.Message)
                Return
            End Try

            Dim document = TextOnlyDocument.GetDocumentFromText(text:=text)

            _SceneDescriptionTextBox.Document = document

            Dim mode As CompileMode
            Try
                mode = Me.GetMode(fileExtension:=IO.Path.GetExtension(_CurrentFileName))
            Catch ex As ArgumentOutOfRangeException
                MessageBox.Show("Unknown file extension.")
                Return
            End Try

            Me.Mode = mode
        End If
    End Sub

    Private Sub CompilePictureMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _CompilePictureMenuItem.Click
        e.Handled = True
        Me.Mode = CompileMode.Picture
    End Sub

    Private Sub CompileVideoMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _CompileVideoMenuItem.Click
        e.Handled = True
        Me.Mode = CompileMode.Video
    End Sub

    Private Sub _AutoCompileMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _AutoCompileMenuItem.Click
        Dim autoCompile = _AutoCompileMenuItem.IsChecked

        _CompileMenuItem.IsEnabled = Not autoCompile
        _ErrorTextBox.IsEnabled = autoCompile

        If Me.Mode = CompileMode.Picture Then
            _PictureCompiler.AutoCompile = autoCompile
        Else
            _VideoCompiler.AutoCompile = autoCompile
        End If
    End Sub

End Class