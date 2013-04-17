Imports System.IO
Imports System.Drawing

Public Class MainWindow
    Private Const _TitleBase = "Fusion Ray Tracer"

    Private WithEvents _SaveDescriptionDialog As SaveDescriptionDialog
    Private ReadOnly _OpenDescriptionDialog As OpenDescriptionDialog

    Private WithEvents _Compiler As PictureOrVideoCompiler

    Private WithEvents _Picture As RayTracerPicture(Of RadianceSpectrum)
    Private WithEvents _Video As RayTracerVideo(Of RadianceSpectrum)

    Private _ResultBitmap As Bitmap

    Private ReadOnly _SavePictureDialog As SavePictureDialog
    Private ReadOnly _SaveVideoDialog As SaveFileDialog

    Private WithEvents _PictureRenderer As PictureRenderer
    Private WithEvents _VideoRenderer As VideoRenderer

    Public Sub New(relativisticRayTracerTermContextBuilder As RelativisticRayTracerTermContextBuilder,
                   initialDirectory As DirectoryInfo,
                   Optional startupFile As FileInfo = Nothing)
        InitializeComponent()

        _SavePictureDialog = New SavePictureDialog(Owner:=Me, initalDirectory:=initialDirectory)
        _SaveVideoDialog = New SaveVideoDialog(Owner:=Me, initialDirectory:=initialDirectory)
        _SaveDescriptionDialog = New SaveDescriptionDialog(Owner:=Me, initialDirectory:=initialDirectory)
        _OpenDescriptionDialog = New OpenDescriptionDialog(Owner:=Me, initialDirectory:=initialDirectory)

        _Compiler = CreatePictureOrVideoCompiler(relativisticRayTracerTermContextBuilder)

        Mode = CompileMode.Picture

        InitDescription()

        AddHandler _CompileVideoMenuItem.Checked, AddressOf _CompileVideoMenuItem_Checked
        AddHandler _CompileVideoMenuItem.Unchecked, AddressOf _CompileVideoMenuItem_Unchecked
        AddHandler _CompilePictureMenuItem.Checked, AddressOf _CompilePictureMenuItem_Checked
        AddHandler _CompilePictureMenuItem.Unchecked, AddressOf _CompilePictureMenuItem_Unchecked

        If (startupFile IsNot Nothing) AndAlso New DescriptionOpener(startupFile).Check Then
            LoadDescription(startupFile)
        End If
    End Sub

    Private Function CreatePictureOrVideoCompiler(relativisticRayTracerTermContextBuilder As RelativisticRayTracerTermContextBuilder) As PictureOrVideoCompiler
        Return New PictureOrVideoCompiler(descriptionBox:=_DescriptionBox,
                                          helpPopup:=_HelpPopup,
                                          helpListBox:=_HelpListBox,
                                          helpScrollViewer:=_HelpScrollViewer,
                                          relativisticRayTracerTermContextBuilder:=relativisticRayTracerTermContextBuilder)
    End Function

    Private Sub RenderButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _RenderButton.Click
        Render()
    End Sub

    Private Sub Render()
        _RenderButton.Visibility = Visibility.Collapsed
        _RenderCancelButton.Visibility = Visibility.Visible
        _RenderProgressBar.Visibility = Visibility.Visible
        _RenderingLabel.Visibility = Visibility.Visible
        _RenderErrorLabel.Content = Nothing
        TaskbarItemInfo.ProgressState = Shell.TaskbarItemProgressState.Normal

        If Mode = CompileMode.Picture Then
            _PictureRenderer = New PictureRenderer(_Picture)
            _PictureRenderer.RunAsync()
        Else
            _VideoRenderer = New VideoRenderer(_Video, outputFile:=VideoOutputFile, definitionText:=Description)
            _VideoRenderer.RunAsync()
        End If
    End Sub

    Private Sub ResetRenderTabs()
        _EstimatedRenderTimePerPixelLabel.Visibility = Visibility.Collapsed
        _EstimatedTotalTimeLabel.Visibility = Visibility.Collapsed
        _TotalElapsedTimeLabel.Visibility = Visibility.Collapsed
        _AverageElapsedTimePerPixelLabel.Visibility = Visibility.Collapsed

        Const normalRenderToolTip = "Renders a picture or video based on the compiled scene."
        Const videoPathInvalidRenderToolTip = "Please select an output path before you render the scene."

        _VideoFileGrid.Visibility = If(Mode = CompileMode.Video, Visibility.Visible, Visibility.Collapsed)
        _RenderButton.IsEnabled = If(Mode = CompileMode.Picture, True, IsVideoOutputFileValid)
        _RenderButton.ToolTip = If(Mode = CompileMode.Picture, normalRenderToolTip, If(IsVideoOutputFileValid, normalRenderToolTip, videoPathInvalidRenderToolTip))
        _VideoRenderedLabel.Visibility = System.Windows.Visibility.Collapsed
        _RenderingLabel.Visibility = Visibility.Collapsed
        _RenderErrorLabel.Content = Nothing
    End Sub

    Private ReadOnly Property VideoOutputFile As FileInfo
        Get
            Return New FileInfo(_VideoFileBox.Text)
        End Get
    End Property

    Private Sub Compiler_Compiled(e As CompilerResultEventArgs(Of RayTracerPicture(Of RadianceSpectrum))) Handles _Compiler.PictureCompiled
        OnCompiled(e, out_result:=_Picture)
    End Sub

    Private Sub Compiler_Compiled(e As CompilerResultEventArgs(Of RayTracerVideo(Of RadianceSpectrum))) Handles _Compiler.VideoCompiled
        OnCompiled(e, out_result:=_Video)
    End Sub

    Private Sub OnCompiled(Of TResult)(e As CompilerResultEventArgs(Of TResult), ByRef out_result As TResult)
        If e.CompilerResult.WasCompilationSuccessful Then
            out_result = e.CompilerResult.Result
            _CompileLabel.Content = "Compilation succeeded. (Open the rendering tab above to continue.)"
            _ErrorTextBox.Text = ""
            SetRenderTabItemVisibility(True)
        Else
            _CompileLabel.Content = "Error:"
            _ErrorTextBox.Text = e.CompilerResult.ErrorMessage
            SetRenderTabItemVisibility(False)
        End If
    End Sub

    Private Sub SaveButton_Click(sender As System.Object, e As System.EventArgs) Handles _SavePictureButton.Click
        ShowSavePictureDialog()
    End Sub

    Private Sub ShowSavePictureDialog()
        If _SaveDescriptionDialog.IsFileAccepted Then
            _SavePictureDialog.File = New FileInfo(IO.Path.GetFileNameWithoutExtension(_SaveDescriptionDialog.File.Name))
        End If

        If _SavePictureDialog.Show Then
            Select Case _SavePictureDialog.File.Extension
                Case ".png"
                    _ResultBitmap.Save(_SavePictureDialog.File.FullName, format:=System.Drawing.Imaging.ImageFormat.Png)
                Case ".bmp"
                    _ResultBitmap.Save(_SavePictureDialog.File.FullName, format:=System.Drawing.Imaging.ImageFormat.Bmp)
                Case Else
                    Throw New ArgumentOutOfRangeException("_SaveFileDialog.FilterIndex")
            End Select
        End If
    End Sub

    Private Sub _EstimateRenderTimeButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _EstimateRenderTimeButton.Click
        EstimateRenderTime()
    End Sub

    Private Sub EstimateRenderTime()
        If Not _RenderTimeEstimationOptionsDialog.DialogResult Then Return

        Dim estimator = GetRenderTimeEstimator()
        Try
            Dim result = estimator.Run

            _EstimatedRenderTimePerPixelLabel.Visibility = Visibility.Visible
            _EstimatedRenderTimePerPixelLabel.Content = "Estimated average time per pixel: " & AveragePixelTimeString(result.TimePerPixel)
            _EstimatedTotalTimeLabel.Visibility = Visibility.Visible
            _EstimatedTotalTimeLabel.Content = "Estimated total time: " & TotalTimeString(result.TotalTime)
        Catch ex As Exception
            ShowRenderError(ex)
        End Try
    End Sub

    Private Shared Function TotalTimeString(totalTime As TimeSpan) As String
        If totalTime.Days = 0 Then
            If totalTime.Hours = 0 Then
                If totalTime.Minutes = 0 Then
                    If totalTime.Seconds = 0 Then
                        Return totalTime.TotalMilliseconds.ToString("0") & " milliseconds"
                    Else
                        Return totalTime.TotalSeconds.ToString("0.0 seconds")
                    End If
                Else
                    Return String.Format("{0} minutes {1} seconds", totalTime.Minutes, totalTime.Seconds)
                End If
            Else
                Return String.Format("{0} hours {1} minutes", totalTime.Hours, totalTime.Minutes)
            End If
        Else
            Return String.Format("{0} days {1} hours", totalTime.Days, totalTime.Hours)
        End If
        'Return String.Format("{0}{1:00}:{2:00}:{3:00}", If(totalTime.Days = 0, "", CStr(totalTime.Days) & " "), totalTime.Hours, totalTime.Minutes, totalTime.Seconds + CInt(totalTime.Milliseconds / 1000))
    End Function

    Private Shared Function AveragePixelTimeString(averagePixelTime As TimeSpan) As String
        Return String.Format("{0:0.000} milliseconds", averagePixelTime.TotalMilliseconds)
    End Function

    Private Function GetRenderTimeEstimator() As IRenderTimeEstimator
        If _Compiler.Mode = CompileMode.Picture Then
            Return New PictureRenderTimeEstimator(_Picture, options:=_RenderTimeEstimationOptionsDialog.Options)
        Else
            Return New VideoRenderTimeEstimator(video:=_Video, options:=_RenderTimeEstimationOptionsDialog.Options)
        End If
    End Function

    Private ReadOnly _RenderTimeEstimationOptionsDialog As New RenderTimeEstimationOptionsDialog

    Private Sub CalculateNeededTimeOptionsButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _EstimateRenderTimeOptionsButton.Click
        _RenderTimeEstimationOptionsDialog.ShowDialog()
    End Sub

    Private Sub RenderCancelButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _RenderCancelButton.Click
        CancelRenderAsync()
    End Sub

    Private Sub CancelRenderAsync()
        If Mode = CompileMode.Picture Then
            _PictureRenderer.CancelAsync()
        Else
            _VideoRenderer.CancelAsync()
        End If
    End Sub

    Private Sub RenderProgressBar_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles _RenderProgressBar.ValueChanged
        TaskbarItemInfo.ProgressValue = e.NewValue
    End Sub

    Private Sub MainWindow_Deactivated(sender As Object, e As EventArgs) Handles Me.Deactivated
        _Compiler.Unfocus()
    End Sub

    Private Sub MainWindow_Closing(sender As Object, e As ComponentModel.CancelEventArgs) Handles Me.Closing
        e.Cancel = Not TryCloseDescription()
    End Sub

    Private Sub MainWindow_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If Keyboard.IsKeyDown(Key.LeftCtrl) OrElse Keyboard.IsKeyDown(Key.RightCtrl) Then
            Select Case e.Key
                Case Key.N
                    TryCloseDescription()

                    e.Handled = True

                Case Key.S
                    TrySaveDescription()

                    e.Handled = True
                Case Key.O
                    ShowOpenDescriptionDialog()

                    e.Handled = True
            End Select
        Else
            Select Case e.Key
                Case Key.F1
                    ShowHelpWindow()

                    e.Handled = True
                Case Key.F2
                    CompileAndShowHelpMenuItem()

                    e.Handled = True
                Case Key.F4
                    _Compiler.AutoCompile = Not _Compiler.AutoCompile

                    e.Handled = True
                Case Key.F5
                    _Compiler.Compile()

                    e.Handled = True

            End Select
        End If
    End Sub

    Private Sub TrySaveDescription()
        _SaveDescriptionDialog.TrySave(Description)
    End Sub

    Private Sub SetRenderTabItemVisibility(visible As Boolean)
        _RenderingTabItem.Visibility = If(visible, Visibility.Visible, Visibility.Collapsed)

        If visible Then
            ResetRenderTabs()
        End If
    End Sub

    Private Sub CompileMenuItem_Click(sender As System.Object, e As RoutedEventArgs) Handles _CompileMenuItem.Click
        _Compiler.Compile()
    End Sub

    Private Sub NewMenuItem_Click(sender As Object, e As RoutedEventArgs) Handles _NewMenuItem.Click
        TryCloseDescription()
    End Sub

    Private Sub OpenMenuItem_Click(sender As Object, e As RoutedEventArgs) Handles _OpenMenuItem.Click
        ShowOpenDescriptionDialog()
    End Sub

    Private Sub SaveMenuItem_Click(sender As Object, e As RoutedEventArgs) Handles _SaveMenuItem.Click
        TrySaveDescription()
    End Sub

    Private Shared Sub SplicePicturesMenuItem_Click(sender As Object, e As RoutedEventArgs) Handles _SplicePicturesMenuItem.Click
        Dim window = New VideoSplicerWindow
        window.Show()
    End Sub

    Private Sub SaveAsMenuItem_Click(sender As Object, e As RoutedEventArgs) Handles _SaveAsMenuItem.Click
        _SaveDescriptionDialog.ShowAndTrySave(description:=Description)

        SetTitleByCurrentFile()
    End Sub

    Private Sub ShowOpenDescriptionDialog()
        If Not _OpenDescriptionDialog.Show Then Return
        If Not TryCloseDescription() Then Return

        LoadDescription(_OpenDescriptionDialog.File)
    End Sub

    Private Sub LoadDescription(file As FileInfo)
        Dim descriptionOpener = New DescriptionOpener(file)
        Dim sceneDescription = descriptionOpener.OpenDescription
        _SaveDescriptionDialog.File = file
        Mode = descriptionOpener.Mode
        _SaveDescriptionDialog.IsFileAccepted = True

        SetTitleByCurrentFile()

        _Compiler.LoadDocument(sceneDescription)
        _HasUnsavedChanges = False
    End Sub

    Private Sub SetTitleByCurrentFile()
        Title = _SaveDescriptionDialog.File.Name & " - " & _TitleBase
    End Sub

    Private Sub AutoCompileMenuItem_Click(sender As Object, e As RoutedEventArgs) Handles _AutoCompileMenuItem.Click
        AutoCompile = _AutoCompileMenuItem.IsChecked
    End Sub

    Private _HasUnsavedChanges As Boolean

    Private Sub ClearDescription()
        _Compiler.LoadDocument(description:="")
        InitDescription()
    End Sub

    Private Sub InitDescription()
        _SaveDescriptionDialog.IsFileAccepted = False
        Title = _TitleBase
        _HasUnsavedChanges = False
    End Sub

    Private Function TryCloseDescription() As Boolean
        If Not _HasUnsavedChanges Then
            ClearDescription()
            Return True
        End If

        Dim result = MessageBox.Show("Do you want to save the current description?", "Save?", MessageBoxButton.YesNoCancel)

        Select Case result
            Case MessageBoxResult.Yes
                TrySaveDescription()
                ClearDescription()
                Return True

            Case MessageBoxResult.No
                ClearDescription()
                Return True

            Case MessageBoxResult.Cancel
                Return False

            Case Else
                Throw New ArgumentOutOfRangeException("result")
        End Select
    End Function

    Private Sub SceneDescriptionTextBox_TextChanged(sender As Object, e As TextChangedEventArgs) Handles _DescriptionBox.TextChanged
        If Not IsLoaded OrElse _Compiler.ApplyingTextDecorations Then Return

        _HasUnsavedChanges = True
    End Sub

    Private WriteOnly Property AutoCompile As Boolean
        Set(value As Boolean)
            _Compiler.AutoCompile = value
            _CompileMenuItem.IsEnabled = Not value
            _ErrorTextBox.IsEnabled = value
            _AutoCompileMenuItem.IsChecked = value
        End Set
    End Property

    Private Sub _VideoOutputFileChangeButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _ChangeVideoFileButton.Click
        If IsVideoOutputFileValid() Then
            _SaveVideoDialog.File = VideoOutputFile
        End If
        If _SaveVideoDialog.Show Then
            _VideoFileBox.Text = _SaveVideoDialog.File.FullName
        End If
    End Sub

    Private Property Mode As CompileMode
        Get
            Return _Compiler.Mode
        End Get
        Set(value As CompileMode)
            _Compiler.Mode = value

            _CompilePictureMenuItem.IsChecked = (value = CompileMode.Picture)
            _CompileVideoMenuItem.IsChecked = (value = CompileMode.Video)

            _SaveDescriptionDialog.Mode = value

            Dim isVideo = (_Compiler.Mode = CompileMode.Video)

            _VideoFileGrid.Visibility = If(isVideo, Visibility.Visible, Visibility.Collapsed)
            SetRenderButtonEnabled()
        End Set
    End Property

    Private Sub _Renderer_ProgressChanged(e As ComponentModel.ProgressChangedEventArgs) Handles _PictureRenderer.ProgressChanged, _VideoRenderer.ProgressChanged
        _RenderProgressBar.Value = e.ProgressPercentage / 100
    End Sub

    Private Sub _PictureRenderer_Completed(e As RenderResultEventArgs(Of Bitmap)) Handles _PictureRenderer.Completed
        OnPictureRendered(e)
    End Sub

    Private Sub OnPictureRendered(ByVal e As RenderResultEventArgs(Of Bitmap))
        OnRendered(e)

        If Not e.WasSuccessful Then Return

        _ResultBitmap = e.Result
        _ResultImage.Source = New SimpleBitmap(_ResultBitmap).ToBitmapSource
        ShowAverageElapsedTimePerPixel(e.ElapsedTime.Divide(_ResultBitmap.Size.Width * _ResultBitmap.Size.Height))

        _ResultPictureTabItem.Visibility = Visibility.Visible
        _ResultPictureTabItem.IsSelected = True
    End Sub

    Private Sub ShowAverageElapsedTimePerPixel(time As TimeSpan)
        _AverageElapsedTimePerPixelLabel.Visibility = Visibility.Visible
        _AverageElapsedTimePerPixelLabel.Content = "Elapsed average time per pixel: " & AveragePixelTimeString(time)
    End Sub

    Private Sub _VideoRenderer_Completed(e As RenderResultEventArgs(Of Object)) Handles _VideoRenderer.Completed
        OnVideoRendered(e)
    End Sub

    Private Sub OnVideoRendered(e As RenderResultEventArgs(Of Object))
        OnRendered(e)

        If Not e.WasSuccessful Then Return

        Dim firstPictureSize = _Video.GetFrame(0).PictureSize
        
        ShowAverageElapsedTimePerPixel(e.ElapsedTime.Divide(_Video.FrameCount * firstPictureSize.Width * firstPictureSize.Height))
        _VideoRenderedLabel.Visibility = Visibility.Visible
    End Sub

    Private Sub OnRendered(Of TResult)(e As RenderResultEventArgs(Of TResult))
        _RenderProgressBar.Value = 0
        _RenderProgressBar.Visibility = Visibility.Collapsed
        _RenderCancelButton.Visibility = Visibility.Collapsed

        _RenderButton.Visibility = Visibility.Visible
        _RenderingLabel.Visibility = Visibility.Collapsed

        TaskbarItemInfo.ProgressState = Shell.TaskbarItemProgressState.None

        If e.Cancelled Then
            _RenderErrorLabel.Content = "Rendering cancelled."
            Return
        End If
        If e.Error IsNot Nothing Then
            ShowRenderError(e.Error)
        End If

        If e.WasSuccessful Then
            _TotalElapsedTimeLabel.Visibility = Visibility.Visible
            _TotalElapsedTimeLabel.Content = "Elapsed total time: " & TotalTimeString(e.ElapsedTime)
        End If
    End Sub

    Private Sub ShowRenderError(ex As Exception)
        _RenderErrorLabel.Content = "A render error occured: " & ex.Message
    End Sub

    Private Function IsVideoOutputFileValid() As Boolean
        Return _VideoFileBox.Text <> ""
    End Function

    Private Sub _VideoFileBox_TextChanged(sender As Object, e As TextChangedEventArgs) Handles _VideoFileBox.TextChanged
        SetRenderButtonEnabled()
    End Sub

    Private Sub SetRenderButtonEnabled()
        If Not Mode = CompileMode.Video Then Return

        _RenderButton.IsEnabled = IsVideoOutputFileValid()
    End Sub

    Private ReadOnly Property Description As String
        Get
            Return New TextOnlyDocument(_DescriptionBox.Document).Text
        End Get
    End Property

    Private Sub _SaveDescriptionDialog_Saved() Handles _SaveDescriptionDialog.Saved
        _HasUnsavedChanges = False
        _OpenDescriptionDialog.File = _SaveDescriptionDialog.File
    End Sub

    Private Sub _CompileVideoMenuItem_Checked(sender As Object, e As RoutedEventArgs)
        Mode = CompileMode.Video
    End Sub

    Private Sub _CompileVideoMenuItem_Unchecked(sender As Object, e As RoutedEventArgs)
        _CompilePictureMenuItem.IsChecked = True
    End Sub

    Private Sub _CompilePictureMenuItem_Checked(sender As Object, e As RoutedEventArgs)
        Mode = CompileMode.Picture
    End Sub

    Private Sub _CompilePictureMenuItem_Unchecked(sender As Object, e As RoutedEventArgs)
        _CompileVideoMenuItem.IsChecked = True
    End Sub

    Private Shared Sub _GeneralHelpMenuItem_Click(sender As Object, e As RoutedEventArgs) Handles _GeneralHelpMenuItem.Click
        ShowHelpWindow()
    End Sub

    Private Shared Sub ShowHelpWindow()
        Dim helpWindow = New HelpWindow
        helpWindow.Show()
    End Sub

    Private Sub _CompileAndShowHelpMenuItem_Click(sender As Object, e As RoutedEventArgs) Handles _CompileAndShowHelpMenuItem.Click
        CompileAndShowHelpMenuItem()
    End Sub

    Private Sub CompileAndShowHelpMenuItem()
        _Compiler.Compile(showHelp:=True)
    End Sub

    Private Sub _DescriptionTab_LostFocus(sender As System.Object, e As RoutedEventArgs) Handles _DescriptionTab.LostFocus
        _Compiler.Unfocus()
    End Sub
End Class