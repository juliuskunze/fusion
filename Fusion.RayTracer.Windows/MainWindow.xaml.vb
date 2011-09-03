Imports System.Windows.Controls.Primitives

Public Class MainWindow

    Private WithEvents _RayTracerPicture As RayTracerPicture(Of RadianceSpectrum)
    Private _ResultBitmap As System.Drawing.Bitmap
    Private _RenderStopwatch As Stopwatch

    Private WithEvents _RenderBackgroundWorker As ComponentModel.BackgroundWorker

    Private ReadOnly _RelativisticRayTracerTermContextBuilder As New RelativisticRayTracerTermContextBuilder
    Private ReadOnly _BaseContext As TermContext = _RelativisticRayTracerTermContextBuilder.TermContext
    Private WithEvents _Compiler As RichCompiler(Of RayTracerPicture(Of RadianceSpectrum))

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

        AddHandler Me.KeyDown, AddressOf AutoCompleteTextBox_PreviewKeyDown
        AddHandler Me.PreviewKeyDown, AddressOf AutoCompleteTextBox_PreviewKeyDown
        AddHandler _AutoCompletitionListBox.PreviewMouseDown, AddressOf ItemListBox_PreviewMouseDown
        AddHandler _AutoCompletitionListBox.KeyDown, AddressOf ItemListBox_KeyDown
        AddHandler _AutoCompletitionListBox.SelectionChanged, AddressOf ItemList_SelectionChanged

        _Compiler = New RichCompiler(Of RayTracerPicture(Of RadianceSpectrum))(RichTextBox:=_SceneDescriptionTextBox,
                                                                               autoCompletePopup:=_AutoCompletitionPopup,
                                                                               autoCompleteListBox:=_AutoCompletitionListBox,
                                                                               baseContext:=_BaseContext,
                                                                               TypeNamedTypeDictionary:=_RelativisticRayTracerTermContextBuilder.TypeDictionary)

        _SceneDescriptionTextBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
        _SceneDescriptionTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible

        _AutoCompletitionPopup.PlacementTarget = _SceneDescriptionTextBox
    End Sub

    Private Sub RenderButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _RenderButton.Click
        _RenderButton.Visibility = Visibility.Collapsed
        _RenderCancelButton.Visibility = Visibility.Visible
        _RenderProgressBar.Visibility = Visibility.Visible
        Me.TaskbarItemInfo.ProgressState = Shell.TaskbarItemProgressState.Normal

        _RenderStopwatch = Stopwatch.StartNew

        _RenderBackgroundWorker.RunWorkerAsync(_RayTracerPicture)
    End Sub

    Private Sub Compiler_Compiled(sender As Object, e As CompilerResultEventArgs(Of RayTracerPicture(Of RadianceSpectrum))) Handles _Compiler.Compiled
        If e.CompilerResult.WasCompilationSuccessful Then
            _RayTracerPicture = e.CompilerResult.Result
        Else
            _ErrorTextBox.Text = e.CompilerResult.ErrorMessage
        End If

        Me.RenderingTabItemsVisible = e.CompilerResult.WasCompilationSuccessful
    End Sub

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
        _Compiler.Compile()
    End Sub

    Private _Loaded As Boolean

    Private Sub AutoCompleteTextBox_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If TypeOf e.OriginalSource Is ListBoxItem Then Return

        If e.Key <> Key.Down OrElse _AutoCompletitionListBox.Items.Count = 0 Then Return

        _AutoCompletitionListBox.Focus()
        _AutoCompletitionListBox.SelectedIndex = 0
        Dim listboxItem = TryCast(_AutoCompletitionListBox.ItemContainerGenerator.ContainerFromIndex(_AutoCompletitionListBox.SelectedIndex), ListBoxItem)
        listboxItem.Focus()
        e.Handled = True
    End Sub

    Private Sub AutoCompleteTextBox_KeyDown(sender As Object, e As KeyEventArgs)
        Select Case e.Key
            Case Key.Escape
                Me.ClosePopup()
                e.Handled = True
        End Select
    End Sub

    Private Sub ItemListBox_KeyDown(sender As Object, e As KeyEventArgs)
        If Not TypeOf e.OriginalSource Is ListBoxItem Then Return

        Select Case e.Key
            Case Key.Tab, Key.Enter
                Me.ClosePopupAndUpdateSource()
            Case Key.Escape
                Me.ClosePopup()
                e.Handled = True
        End Select
    End Sub

    Private Sub ItemListBox_PreviewMouseDown(sender As Object, e As MouseButtonEventArgs)
        If e.LeftButton <> MouseButtonState.Pressed Then Return

        Dim tb = TryCast(e.OriginalSource, TextBlock)
        If tb Is Nothing Then Return

        If e.ClickCount = 2 Then
            Me.ClosePopupAndUpdateSource()
            e.Handled = True
        End If
    End Sub

    Private Sub ClosePopupAndUpdateSource()
        Me.ClosePopup()
        Dim selected = CStr(DirectCast(_AutoCompletitionListBox.SelectedItem, ListBoxItem).Content)

        _SceneDescriptionTextBox.AppendText(selected)
    End Sub

    Private Sub ClosePopup()
        _AutoCompletitionPopup.IsOpen = False
        For Each listBoxItemObj In _AutoCompletitionListBox.Items
            Dim listBoxItem = CType(listBoxItemObj, ListBoxItem)
            Dim toolTip = CType(listBoxItem.ToolTip, ToolTip)
            toolTip.IsOpen = False
        Next
    End Sub

    Private Sub ItemList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        For Each listBoxItemObj In _AutoCompletitionListBox.Items
            Dim listBoxItem = CType(listBoxItemObj, ListBoxItem)
            Dim toolTip = CType(listBoxItem.ToolTip, ToolTip)
            toolTip.IsOpen = listBoxItem.IsSelected
        Next
    End Sub

    '_SceneDescriptionTextBox.Popup.IsOpen = True

    'Dim currentCharRect = _SceneDescriptionTextBox.Selection.Start.GetCharacterRect(LogicalDirection.Forward)

    '_SceneDescriptionTextBox.Popup.VerticalOffset = -(_SceneDescriptionTextBox.ActualHeight - currentCharRect.Bottom)
    '_SceneDescriptionTextBox.Popup.HorizontalOffset = currentCharRect.Left

    '_SceneDescriptionTextBox.ListBox.ItemsSource = listBoxItems

End Class
