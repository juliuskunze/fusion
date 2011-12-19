﻿Imports System.IO
Imports System.Drawing

Public Class MainWindow

    Private Const _TitleBase = "Fusion Ray Tracer"

    Private WithEvents _SaveDescriptionDialog As SaveDescriptionDialog
    Private ReadOnly _OpenDescriptionDialog As OpenDescriptionDialog

    Private WithEvents _Compiler As PictureOrVideoCompiler

    Private WithEvents _Picture As RayTracerPicture(Of RadianceSpectrum)
    Private WithEvents _Video As RayTracerVideo(Of RadianceSpectrum)

    Private _ResultBitmap As System.Drawing.Bitmap

    Private ReadOnly _SavePictureDialog As SavePictureDialog
    Private ReadOnly _SaveVideoDialog As SaveFileDialog

    Private WithEvents _PictureRenderer As PictureRenderer
    Private WithEvents _VideoRenderer As VideoRenderer

    Public Sub New(relativisticRayTracerTermContextBuilder As RelativisticRayTracerTermContextBuilder,
                   initialDirectory As DirectoryInfo)
        Me.InitializeComponent()

        _SavePictureDialog = New SavePictureDialog(Owner:=Me, initalDirectory:=initialDirectory)
        _SaveVideoDialog = New SaveVideoDialog(Owner:=Me, initialDirectory:=initialDirectory)
        _SaveDescriptionDialog = New SaveDescriptionDialog(Owner:=Me, initialDirectory:=initialDirectory)
        _OpenDescriptionDialog = New OpenDescriptionDialog(Owner:=Me, initialDirectory:=initialDirectory)

        _Compiler = Me.CreatePictureOrVideoCompiler(relativisticRayTracerTermContextBuilder)

        Me.Mode = CompileMode.Picture

        Me.InitDescription()
        
        AddHandler _CompileVideoMenuItem.Checked, AddressOf _CompileVideoMenuItem_Checked
        AddHandler _CompileVideoMenuItem.Unchecked, AddressOf _CompileVideoMenuItem_Unchecked
        AddHandler _CompilePictureMenuItem.Checked, AddressOf _CompilePictureMenuItem_Checked
        AddHandler _CompilePictureMenuItem.Unchecked, AddressOf _CompilePictureMenuItem_Unchecked
    End Sub

    Private Function CreatePictureOrVideoCompiler(relativisticRayTracerTermContextBuilder As RelativisticRayTracerTermContextBuilder) As PictureOrVideoCompiler
        Return New PictureOrVideoCompiler(descriptionBox:=_DescriptionBox,
                                          helpPopup:=_HelpPopup,
                                          helpListBox:=_HelpListBox,
                                          helpScrollViewer:=_HelpScrollViewer,
                                          relativisticRayTracerTermContextBuilder:=relativisticRayTracerTermContextBuilder)
    End Function

    Private Sub RenderButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _RenderButton.Click
        Me.Render()
    End Sub

    Private Sub Render()
        _RenderButton.Visibility = Visibility.Collapsed
        _RenderCancelButton.Visibility = Visibility.Visible
        _RenderProgressBar.Visibility = Visibility.Visible
        _RenderingLabel.Visibility = Visibility.Visible
        _RenderErrorLabel.Content = Nothing
        Me.TaskbarItemInfo.ProgressState = Shell.TaskbarItemProgressState.Normal

        If Me.Mode = CompileMode.Picture Then
            _PictureRenderer = New PictureRenderer(_Picture)
            _PictureRenderer.RunAsync()
        Else
            _VideoRenderer = New VideoRenderer(_Video, outputFile:=Me.VideoOutputFile)
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

        _VideoFileGrid.Visibility = If(Me.Mode = CompileMode.Video, Visibility.Visible, Visibility.Collapsed)
        _RenderButton.IsEnabled = If(Me.Mode = CompileMode.Picture, True, Me.IsVideoOutputFileValid)
        _RenderButton.ToolTip = If(Me.Mode = CompileMode.Picture, normalRenderToolTip, If(IsVideoOutputFileValid, normalRenderToolTip, videoPathInvalidRenderToolTip))
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
            _CompileLabel.Content = "Compilation succeeded."
            _ErrorTextBox.Text = ""
            Me.SetRenderTabItemVisibility(True)
        Else
            _CompileLabel.Content = "Error:"
            _ErrorTextBox.Text = e.CompilerResult.ErrorMessage
            Me.SetRenderTabItemVisibility(False)
        End If
    End Sub

    Private Sub SaveButton_Click(sender As System.Object, e As System.EventArgs) Handles _SavePictureButton.Click
        Me.ShowSavePictureDialog()
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
        Me.EstimateRenderTime()
    End Sub

    Private Sub EstimateRenderTime()
        If Not _RenderTimeEstimationOptionsDialog.DialogResult Then Return

        Dim estimator = Me.GetRenderTimeEstimator
        Try
            Dim result = estimator.Run

            _EstimatedRenderTimePerPixelLabel.Visibility = Visibility.Visible
            _EstimatedRenderTimePerPixelLabel.Content = "Estimated average time per pixel: " & AveragePixelTimeString(result.TimePerPixel)
            _EstimatedTotalTimeLabel.Visibility = Visibility.Visible
            _EstimatedTotalTimeLabel.Content = "Estimated total time: " & TotalTimeString(result.TotalTime)
        Catch ex As Exception
            Me.ShowRenderError(ex)
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

    Private _RenderTimeEstimationOptionsDialog As New RenderTimeEstimationOptionsDialog

    Private Sub CalculateNeededTimeOptionsButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _EstimateRenderTimeOptionsButton.Click
        _RenderTimeEstimationOptionsDialog.ShowDialog()
    End Sub

    Private Sub RenderCancelButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _RenderCancelButton.Click
        Me.CancelRenderAsync()
    End Sub

    Private Sub CancelRenderAsync()
        If Me.Mode = CompileMode.Picture Then
            _PictureRenderer.CancelAsync()
        Else
            _VideoRenderer.CancelAsync()
        End If
    End Sub

    Private Sub RenderProgressBar_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles _RenderProgressBar.ValueChanged
        Me.TaskbarItemInfo.ProgressValue = e.NewValue
    End Sub

    Private Sub MainWindow_Deactivated(sender As Object, e As EventArgs) Handles Me.Deactivated
        _Compiler.Unfocus()
    End Sub

    Private Sub MainWindow_Closing(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        e.Cancel = Not Me.TryCloseDescription()
    End Sub

    Private Sub MainWindow_KeyDown(sender As Object, e As Input.KeyEventArgs) Handles Me.KeyDown
        If Keyboard.IsKeyDown(Key.LeftCtrl) OrElse Keyboard.IsKeyDown(Key.RightCtrl) Then
            Select Case e.Key
                Case Key.N
                    Me.TryCloseDescription()

                    e.Handled = True

                Case Key.S
                    Me.TrySaveDescription()

                    e.Handled = True
                Case Key.O
                    Me.ShowOpenDescriptionDialog()

                    e.Handled = True
            End Select
        Else
            Select Case e.Key
                Case Key.F1
                    Me.ShowHelpWindow()

                    e.Handled = True
                Case Key.F2
                    Me.CompileAndShowHelpMenuItem()

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
        _SaveDescriptionDialog.TrySave(Me.Description)
    End Sub

    Private Sub SetRenderTabItemVisibility(visible As Boolean)
        _RenderingTabItem.Visibility = If(visible, Visibility.Visible, Visibility.Collapsed)

        If visible Then
            Me.ResetRenderTabs()
        End If
    End Sub

    Private Property IsSceneDescriptionChangeable As Boolean
        Get
            Return _DescriptionBox.IsEnabled
        End Get
        Set(value As Boolean)
            _DescriptionBox.IsEnabled = value
        End Set
    End Property

    Private Sub CompileMenuItem_Click(sender As System.Object, e As RoutedEventArgs) Handles _CompileMenuItem.Click
        _Compiler.Compile()
    End Sub

    Private Sub NewMenuItem_Click(sender As Object, e As RoutedEventArgs) Handles _NewMenuItem.Click
        Me.TryCloseDescription()
    End Sub

    Private Sub OpenMenuItem_Click(sender As Object, e As RoutedEventArgs) Handles _OpenMenuItem.Click
        Me.ShowOpenDescriptionDialog()
    End Sub

    Private Sub SaveMenuItem_Click(sender As Object, e As RoutedEventArgs) Handles _SaveMenuItem.Click
        Me.TrySaveDescription()
    End Sub

    Private Sub SaveAsMenuItem_Click(sender As Object, e As RoutedEventArgs) Handles _SaveAsMenuItem.Click
        _SaveDescriptionDialog.ShowAndTrySave(description:=Me.Description)

        Me.SetTitleByCurrentFile()
    End Sub

    Private Sub ShowOpenDescriptionDialog()
        If Not _OpenDescriptionDialog.Show Then Return
        If Not Me.TryCloseDescription Then Return

        Dim description = _OpenDescriptionDialog.OpenDescription()

        _SaveDescriptionDialog.File = _OpenDescriptionDialog.File
        Me.Mode = _OpenDescriptionDialog.Mode
        _SaveDescriptionDialog.IsFileAccepted = True

        Me.SetTitleByCurrentFile()

        _Compiler.LoadDocument(description)
        _HasUnsavedChanges = False
    End Sub

    Private Sub SetTitleByCurrentFile()
        Me.Title = _SaveDescriptionDialog.File.Name & " - " & _TitleBase
    End Sub

    Private Sub AutoCompileMenuItem_Click(sender As Object, e As RoutedEventArgs) Handles _AutoCompileMenuItem.Click
        Me.AutoCompile = _AutoCompileMenuItem.IsChecked
    End Sub

    Private _HasUnsavedChanges As Boolean

    Private Sub ClearDescription()
        _Compiler.LoadDocument(description:="")
        Me.InitDescription()
    End Sub

    Private Sub InitDescription()
        _SaveDescriptionDialog.IsFileAccepted = False
        Me.Title = _TitleBase
        _HasUnsavedChanges = False
    End Sub

    Private Function TryCloseDescription() As Boolean
        If Not _HasUnsavedChanges Then
            Me.ClearDescription()
            Return True
        End If

        Dim result = MessageBox.Show("Do you want to save the current description?", "Save?", MessageBoxButton.YesNoCancel)

        Select Case result
            Case MessageBoxResult.Yes
                Me.TrySaveDescription()
                Me.ClearDescription()
                Return True

            Case MessageBoxResult.No
                Me.ClearDescription()
                Return True

            Case MessageBoxResult.Cancel
                Return False

            Case Else
                Throw New ArgumentOutOfRangeException("result")
        End Select
    End Function

    Private Sub SceneDescriptionTextBox_TextChanged(sender As Object, e As Controls.TextChangedEventArgs) Handles _DescriptionBox.TextChanged
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

    Private Sub _VideoOutputFileChangeButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _ChangeVideoFileButton.Click
        If Me.IsVideoOutputFileValid Then
            _SaveVideoDialog.File = Me.VideoOutputFile
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
            Me.SetRenderButtonEnabled()
        End Set
    End Property

    Private Sub _Renderer_ProgressChanged(e As ComponentModel.ProgressChangedEventArgs) Handles _PictureRenderer.ProgressChanged, _VideoRenderer.ProgressChanged
        _RenderProgressBar.Value = e.ProgressPercentage / 100
    End Sub

    Private Sub _PictureRenderer_Completed(e As RenderResultEventArgs(Of System.Drawing.Bitmap)) Handles _PictureRenderer.Completed
        OnPictureRendered(e)
    End Sub

    Private Sub OnPictureRendered(ByVal e As RenderResultEventArgs(Of Bitmap))
        Me.OnRendered(e)

        If Not e.WasSuccessful Then Return

        _ResultBitmap = e.Result
        _ResultImage.Source = New SimpleBitmap(_ResultBitmap).ToBitmapSource
        Me.ShowAverageElapsedTimePerPixel(e.ElapsedTime.Divide(_ResultBitmap.Size.Width * _ResultBitmap.Size.Height))

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
        Me.OnRendered(e)

        If Not e.WasSuccessful Then Return

        Dim firstPictureSize = _Video.GetFrame(0).PictureSize

        Me.ShowAverageElapsedTimePerPixel(e.ElapsedTime.Divide(_Video.FrameCount * firstPictureSize.Width * firstPictureSize.Height))
        _VideoRenderedLabel.Visibility = System.Windows.Visibility.Visible
    End Sub

    Private Sub OnRendered(Of TResult)(e As RenderResultEventArgs(Of TResult))
        _RenderProgressBar.Value = 0
        _RenderProgressBar.Visibility = Visibility.Collapsed
        _RenderCancelButton.Visibility = Visibility.Collapsed

        _RenderButton.Visibility = Visibility.Visible
        _RenderingLabel.Visibility = Visibility.Collapsed

        Me.TaskbarItemInfo.ProgressState = Shell.TaskbarItemProgressState.None

        If e.Cancelled Then
            _RenderErrorLabel.Content = "Rendering cancelled."
            Return
        End If
        If e.Error IsNot Nothing Then
            Me.ShowRenderError(e.Error)
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

    Private Sub _VideoFileBox_TextChanged(sender As Object, e As Controls.TextChangedEventArgs) Handles _VideoFileBox.TextChanged
        Me.SetRenderButtonEnabled()
    End Sub

    Private Sub SetRenderButtonEnabled()
        If Not Me.Mode = CompileMode.Video Then Return

        _RenderButton.IsEnabled = Me.IsVideoOutputFileValid
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
        Me.Mode = CompileMode.Video
    End Sub

    Private Sub _CompileVideoMenuItem_Unchecked(sender As Object, e As RoutedEventArgs)
        _CompilePictureMenuItem.IsChecked = True
    End Sub

    Private Sub _CompilePictureMenuItem_Checked(sender As Object, e As RoutedEventArgs)
        Me.Mode = CompileMode.Picture
    End Sub

    Private Sub _CompilePictureMenuItem_Unchecked(sender As Object, e As RoutedEventArgs)
        _CompileVideoMenuItem.IsChecked = True
    End Sub

    Private Sub _GeneralHelpMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _GeneralHelpMenuItem.Click
        Me.ShowHelpWindow()
    End Sub

    Private Sub ShowHelpWindow()
        Dim helpWindow = New HelpWindow
        helpWindow.Show()
    End Sub

    Private Sub _CompileAndShowHelpMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _CompileAndShowHelpMenuItem.Click
        Me.CompileAndShowHelpMenuItem()
    End Sub

    Private Sub CompileAndShowHelpMenuItem()
        _Compiler.Compile(showHelp:=True)
    End Sub
End Class