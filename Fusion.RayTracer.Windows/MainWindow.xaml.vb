Imports System.Windows.Controls.Primitives

Public Class MainWindow

    Private WithEvents _RayTracerPicture As RayTracerPicture(Of RadianceSpectrum)
    Private _ResultBitmap As System.Drawing.Bitmap
    Private _RenderStopwatch As Stopwatch

    Private WithEvents _RenderBackgroundWorker As ComponentModel.BackgroundWorker

    Private Shared ReadOnly _RelativisticRayTracerTermContextBuilder As New RelativisticRayTracerTermContextBuilder
    Private Shared ReadOnly _BaseContext As TermContext = _RelativisticRayTracerTermContextBuilder.TermContext

    Private WithEvents _PictureCompiler As RichCompiler(Of RayTracerPicture(Of RadianceSpectrum))
    Private WithEvents _VideoCompiler As RichCompiler(Of RayTracerVideo(Of RadianceSpectrum))

    Private Shared ReadOnly _DefaultInitialDirectory As String = My.Computer.FileSystem.SpecialDirectories.Desktop

    Private ReadOnly _SavePictureDialog As SaveFileDialog
    Private ReadOnly _SaveVideoDialog As SaveFileDialog

    Private ReadOnly _SaveDescriptionDialog As SaveFileDialog
    Private ReadOnly _OpenDescriptionDialog As OpenFileDialog

    Private ReadOnly _OpenVideoDirectoryDialog As System.Windows.Forms.FolderBrowserDialog

    Private _CurrentFileName As String

    Public Sub New()
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

        Me.InitializeComponent()

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

        Dim filter = Me.GetFilter(CompileMode.Picture) & "|" & Me.GetFilter(CompileMode.Video)

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
        Dim compiler = New RichCompiler(Of TResult)(RichTextBox:=_SceneDescriptionTextBox,
                                                    autoCompletePopup:=_AutoCompletePopup,
                                                    autoCompleteListBox:=_AutoCompleteListBox,
                                                    baseContext:=_BaseContext,
                                                    TypeNamedTypeDictionary:=_RelativisticRayTracerTermContextBuilder.TypeDictionary)

        compiler.Compile()


        Return compiler
    End Function

    Private Sub RenderButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _RenderButton.Click
        _RenderButton.Visibility = Visibility.Collapsed
        _RenderCancelButton.Visibility = Visibility.Visible
        _RenderProgressBar.Visibility = Visibility.Visible
        Me.TaskbarItemInfo.ProgressState = Shell.TaskbarItemProgressState.Normal

        _RenderStopwatch = Stopwatch.StartNew

        _RenderBackgroundWorker.RunWorkerAsync(_RayTracerPicture)
    End Sub

    Private Sub Compiler_Compiled(sender As Object, e As CompilerResultEventArgs(Of RayTracerPicture(Of RadianceSpectrum))) Handles _PictureCompiler.Compiled
        If e.CompilerResult.WasCompilationSuccessful Then
            _RayTracerPicture = e.CompilerResult.Result
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

    Private Sub MainWindow_Deactivated(sender As Object, e As EventArgs) Handles Me.Deactivated
        _PictureCompiler.Deactivate()
    End Sub

    Private Sub RibbonWindow_Unloaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Unloaded
        Application.Current.Shutdown()
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

    Private Sub CompileMenuItem_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles _CompileMenuItem.Click
        'Dim a = New VideoRenderer(24, 500, 500)

        _PictureCompiler.Compile()
    End Sub

    Private Sub CompilePictureMenuItem_Checked(sender As Object, e As System.Windows.RoutedEventArgs) Handles _CompilePictureMenuItem.Checked
        If Not MyBase.IsLoaded Then Return

        Me.Mode = CompileMode.Picture
    End Sub

    Private Sub CompileVideoMenuItem_Checked(sender As Object, e As System.Windows.RoutedEventArgs) Handles _CompileVideoMenuItem.Checked
        If Not MyBase.IsLoaded Then Return

        Me.Mode = CompileMode.Video
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

            Select value
                Case CompileMode.Picture
                    _PictureCompiler = Me.GetCompiler(Of RayTracerPicture(Of RadianceSpectrum))()
                Case CompileMode.Video
                    _VideoCompiler = Me.GetCompiler(Of RayTracerVideo(Of RadianceSpectrum))()
            End Select
        End Set
    End Property

    Private Sub SaveDescriptionMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _SaveDescriptionMenuItem.Click
        If IO.File.Exists(_CurrentFileName) Then
            Me.SaveDescription(_CurrentFileName)
        Else
            Me.SaveDescriptionAs()
        End If
    End Sub

    Private Sub SaveDescriptionAsMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _SaveDescriptionAsMenuItem.Click
        Me.SaveDescriptionAs()
    End Sub

    Private Sub SaveDescriptionAs()
        If IO.File.Exists(_CurrentFileName) Then _SaveDescriptionDialog.FileName = _CurrentFileName

        _SaveDescriptionDialog.DefaultExt = Me.GetDescriptionFileExtension

        If _SaveDescriptionDialog.ShowDialog(owner:=Me) Then
            Me.SaveDescription(fileName:=_SaveDescriptionDialog.FileName)
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

    Private Function GetFilter() As String
        Return Me.GetFilter(Me.Mode)
    End Function

    Private Function GetFilter(mode As CompileMode) As String
        Select Case mode
            Case CompileMode.Picture : Return "Ray tracer picture scene description|*.pic"
            Case CompileMode.Video : Return "Ray tracer video scene description|*.vid"
            Case Else : Throw New ArgumentOutOfRangeException
        End Select
    End Function

    Private Sub OpenDescriptionSourceMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _OpenDescriptionMenuItem.Click
        Me.OpenDescription()
    End Sub

    Private Sub OpenDescription()
        If _OpenDescriptionDialog.ShowDialog(owner:=Me) Then
            Dim text As String

            Try
                Using streamReader = New IO.StreamReader(_OpenDescriptionDialog.FileName)
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
                mode = Me.GetMode(fileExtension:=IO.Path.GetExtension(_OpenDescriptionDialog.FileName))
            Catch ex As ArgumentOutOfRangeException
                MessageBox.Show("Unknown file extension.")
                Return
            End Try

            Me.Mode = mode
        End If
    End Sub

End Class
