Imports System.IO

Public Class MainWindow

    Private ReadOnly _SaveDescriptionDialog As SaveFileDialog
    Private ReadOnly _OpenDescriptionDialog As OpenFileDialog

    Private WithEvents _Compiler As PictureOrVideoCompiler

    Private WithEvents _Picture As RayTracerPicture(Of RadianceSpectrum)
    Private WithEvents _Video As RayTracerVideo(Of RadianceSpectrum)

    Private _ResultBitmap As System.Drawing.Bitmap

    Private _RenderStopwatch As Stopwatch
    Private WithEvents _RenderBackgroundWorker As ComponentModel.BackgroundWorker

    Private ReadOnly _DefaultInitialDirectory As String

    Private ReadOnly _SavePictureDialog As SaveFileDialog
    Private ReadOnly _SaveVideoDialog As SaveFileDialog

    Private ReadOnly _OpenVideoDirectoryDialog As System.Windows.Forms.FolderBrowserDialog

    Private _CurrentFile As IO.FileInfo

    Public Sub New(relativisticRayTracerTermContextBuilder As RelativisticRayTracerTermContextBuilder)
        Me.InitializeComponent()

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

        Dim saveFilter = Me.GetFileExtensionFilter(CompileMode.Picture) & "|" & Me.GetFileExtensionFilter(CompileMode.Video)
        Dim openFilter = "All Files|*.*|" & saveFilter

        _SaveDescriptionDialog = New SaveFileDialog With {
            .Filter = saveFilter,
            .InitialDirectory = _DefaultInitialDirectory}

        _OpenDescriptionDialog = New OpenFileDialog With {
            .Filter = openFilter,
            .InitialDirectory = _DefaultInitialDirectory}

        _OpenVideoDirectoryDialog = New System.Windows.Forms.FolderBrowserDialog With {.SelectedPath = _DefaultInitialDirectory}

        Me.CreateNewDescription()

        _Compiler = New PictureOrVideoCompiler(descriptionBox:=_DescriptionBox,
                                               helpPopup:=_HelpPopup,
                                               helpListBox:=_HelpListBox,
                                               helpScrollViewer:=_HelpScrollViewer,
                                               relativisticRayTracerTermContextBuilder:=relativisticRayTracerTermContextBuilder,
                                               compilePictureMenuItem:=_CompilePictureMenuItem,
                                               compileVideoMenuItem:=_CompileVideoMenuItem)

    End Sub

    Private Sub RenderButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _RenderButton.Click
        _RenderButton.Visibility = Visibility.Collapsed
        _RenderCancelButton.Visibility = Visibility.Visible
        _RenderProgressBar.Visibility = Visibility.Visible
        Me.TaskbarItemInfo.ProgressState = Shell.TaskbarItemProgressState.Normal

        _RenderStopwatch = Stopwatch.StartNew

        _RenderBackgroundWorker.RunWorkerAsync(_Picture)
    End Sub

    Private Sub Compiler_Compiled(e As CompilerResultEventArgs(Of RayTracerPicture(Of RadianceSpectrum))) Handles _Compiler.PictureCompiled
        OnCompiled(e, out_result:=_Picture)
    End Sub

    Private Sub Compiler_Compiled(e As CompilerResultEventArgs(Of RayTracerVideo(Of RadianceSpectrum))) Handles _Compiler.VideoCompiled
        OnCompiled(e, out_result:=_Video)
    End Sub

    Private Sub OnCompiled(Of TResult)(ByVal e As CompilerResultEventArgs(Of TResult), ByRef out_result As TResult)
        If e.CompilerResult.WasCompilationSuccessful Then
            out_result = e.CompilerResult.Result
            _CompileLabel.Content = "Compilation succeeded."
            _ErrorTextBox.Text = ""
        Else
            _CompileLabel.Content = "Error:"
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
        _Compiler.Unfocus()
    End Sub

    Private Sub MainWindow_KeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles Me.KeyDown
        If Keyboard.IsKeyDown(Key.LeftCtrl) OrElse Keyboard.IsKeyDown(Key.RightCtrl) Then
            Select Case e.Key
                Case Key.N
                    Me.TryCloseCurrentDescription()

                    e.Handled = True

                Case Key.S
                    Me.TrySaveOrSaveAs()

                    e.Handled = True
                Case Key.O
                    Me.ShowOpenDescriptionDialog()

                    e.Handled = True
            End Select
        Else
            Select Case e.Key
                Case Key.F5
                    _Compiler.Compile()

                    e.Handled = True
                Case Key.F4
                    _Compiler.AutoCompile = Not _Compiler.AutoCompile

                    e.Handled = True
            End Select
        End If
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
            Return _DescriptionBox.IsEnabled
        End Get
        Set(value As Boolean)
            _DescriptionBox.IsEnabled = value
        End Set
    End Property

    Private Sub CompileMenuItem_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles _CompileMenuItem.Click
        _Compiler.Compile()
    End Sub

    Private Sub NewMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _NewMenuItem.Click
        Me.TryCloseCurrentDescription()
    End Sub

    Private Sub OpenMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _OpenMenuItem.Click
        Me.ShowOpenDescriptionDialog()
    End Sub

    Private Sub SaveMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _SaveMenuItem.Click
        Me.TrySaveOrSaveAs()
    End Sub

    Private Sub SaveAsMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _SaveAsMenuItem.Click
        Me.ShowSaveAsDialog()
    End Sub

    Private Sub ShowSaveAsDialog()
        _SaveDescriptionDialog.FileName = If(_CurrentFile Is Nothing, "Ray tracer picture scene description", _CurrentFile.Name)
        _SaveDescriptionDialog.FilterIndex = Me.GetCurrentFilterIndex()
        _SaveDescriptionDialog.DefaultExt = Me.GetDescriptionFileExtension

        If Not _SaveDescriptionDialog.ShowDialog(owner:=Me) Then Return

        Me.Save(New IO.FileInfo(_SaveDescriptionDialog.FileName))
    End Sub

    Private Sub TrySaveOrSaveAs()
        If IO.File.Exists(_CurrentFile.FullName) Then
            Me.Save(_CurrentFile)
        Else
            Me.ShowSaveAsDialog()
        End If
    End Sub

    Private Sub Save(targetFile As IO.FileInfo)
        _CurrentFile = targetFile

        Dim description = New TextOnlyDocument(_DescriptionBox.Document).Text

        Try
            Using streamWriter = New IO.StreamWriter(_CurrentFile.FullName)
                streamWriter.Write(description)
            End Using
        Catch ex As IO.IOException
            MessageBox.Show(ex.Message)
            Return
        End Try

        _HasUnsavedChanges = False
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
        Return Me.GetDescriptionFileExtension(_Compiler.Mode)
    End Function

    Private Function GetMode(fileExtension As String) As CompileMode
        Select Case fileExtension
            Case ".pic" : Return CompileMode.Picture
            Case ".vid" : Return CompileMode.Video
            Case Else : Throw New ArgumentOutOfRangeException
        End Select
    End Function

    Private Function GetCurrentFilterIndex() As Integer
        Return GetFilterIndex(_Compiler.Mode)
    End Function

    Private Shared Function GetFilterIndex(compileMode As CompileMode) As Integer
        Select Case compileMode
            Case compileMode.Picture : Return 1
            Case compileMode.Video : Return 2
            Case Else : Throw New ArgumentOutOfRangeException
        End Select
    End Function

    Private Function GetCurrentFileExtensionFilter() As String
        Return Me.GetFileExtensionFilter(_Compiler.Mode)
    End Function

    Private Function GetFileExtensionFilter(mode As CompileMode) As String
        Select Case mode
            Case CompileMode.Picture : Return "Ray tracer picture scene description (*.pic)|*.pic"
            Case CompileMode.Video : Return "Ray tracer video scene description (*.vid)|*.vid"
            Case Else : Throw New ArgumentOutOfRangeException
        End Select
    End Function

    Private Sub ShowOpenDescriptionDialog()
        If _OpenDescriptionDialog.ShowDialog(owner:=Me) Then
            If Me.TryCloseCurrentDescription() Then
                Me.TryOpenFile(f:=New FileInfo(_OpenDescriptionDialog.FileName))
            End If
        End If
    End Sub

    Private Sub TryOpenFile(f As FileInfo)
        Dim text As String

        Dim mode As CompileMode
        Try
            mode = Me.GetMode(fileExtension:=f.Extension)
        Catch ex As ArgumentOutOfRangeException
            mode = CompileMode.Picture
        End Try

        _Compiler.Mode = mode

        Try
            Using streamReader = New IO.StreamReader(f.FullName)
                text = streamReader.ReadToEnd()
            End Using
        Catch ex As IO.IOException
            MessageBox.Show(ex.Message)
            Return
        End Try

        Me.LoadDescription(f:=f, text:=text)

        _HasUnsavedChanges = False
    End Sub

    Private Sub AutoCompileMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _AutoCompileMenuItem.Click
        Me.AutoCompile = _AutoCompileMenuItem.IsChecked
    End Sub

    Private _HasUnsavedChanges As Boolean

    Private Sub CreateNewDescription()
        _HasUnsavedChanges = False
        Me.LoadDescription(f:=Nothing, text:="")
    End Sub

    Private Sub LoadDescription(f As IO.FileInfo, text As String)
        _CurrentFile = f
        Const titleBase = "Fusion Ray Tracer"
        Me.Title = If(_CurrentFile Is Nothing, titleBase, _CurrentFile.Name & " - " & titleBase)
        _DescriptionBox.Document = TextOnlyDocument.GetDocumentFromText(text)
    End Sub

    Private Function TryCloseCurrentDescription() As Boolean
        If Not _HasUnsavedChanges Then
            Me.CreateNewDescription()
            Return True
        End If

        Dim result = MessageBox.Show("Do you want to save the current description?", "Save?", MessageBoxButton.YesNoCancel)

        Select Case result
            Case MessageBoxResult.Yes
                If _CurrentFile IsNot Nothing Then
                    Me.Save(_CurrentFile)
                Else
                    Me.ShowSaveAsDialog()
                End If
                Me.CreateNewDescription()
                Return True

            Case MessageBoxResult.No
                Me.CreateNewDescription()
                Return True

            Case MessageBoxResult.Cancel
                Return False

            Case Else
                Throw New ArgumentOutOfRangeException("result")
        End Select
    End Function

    Private Sub SceneDescriptionTextBox_TextChanged(sender As Object, e As System.Windows.Controls.TextChangedEventArgs) Handles _DescriptionBox.TextChanged
        If Not Me.IsLoaded OrElse _Compiler.ApplyingTextDecorations Then Return

        _HasUnsavedChanges = True
    End Sub

    Private Property AutoCompile As Boolean
        Get
            Return _Compiler.AutoCompile
        End Get
        Set(value As Boolean)
            _Compiler.AutoCompile = value
            _CompileMenuItem.IsEnabled = Not value
            _ErrorTextBox.IsEnabled = value
            _AutoCompileMenuItem.IsChecked = value
        End Set
    End Property


            

End Class