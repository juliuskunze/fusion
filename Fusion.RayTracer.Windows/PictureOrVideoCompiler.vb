Imports System.Windows.Controls.Primitives

Public Class PictureOrVideoCompiler

    Private WithEvents _CompilePictureMenuItem As MenuItem
    Private WithEvents _CompileVideoMenuItem As MenuItem

    Private WithEvents _PictureCompiler As RichCompiler(Of RayTracerPicture(Of RadianceSpectrum))
    Private WithEvents _VideoCompiler As RichCompiler(Of RayTracerVideo(Of RadianceSpectrum))

    Private ReadOnly _DescriptionBox As RichTextBox
    Private ReadOnly _HelpPopup As Popup
    Private ReadOnly _HelpListBox As ListBox
    Private ReadOnly _HelpScrollViewer As ScrollViewer
    Private ReadOnly _RelativisticRayTracerTermContextBuilder As RelativisticRayTracerTermContextBuilder

    Public Sub New(descriptionBox As RichTextBox,
                   helpPopup As Popup,
                   helpListBox As ListBox,
                   helpScrollViewer As ScrollViewer,
                   relativisticRayTracerTermContextBuilder As RelativisticRayTracerTermContextBuilder,
                   compilePictureMenuItem As MenuItem,
                   compileVideoMenuItem As MenuItem)
        _DescriptionBox = descriptionBox
        _HelpPopup = helpPopup
        _HelpListBox = helpListBox
        _HelpScrollViewer = helpScrollViewer
        _RelativisticRayTracerTermContextBuilder = relativisticRayTracerTermContextBuilder
        _CompilePictureMenuItem = compilePictureMenuItem
        _CompileVideoMenuItem = compileVideoMenuItem

        _PictureCompiler = Me.GetCompiler(Of RayTracerPicture(Of RadianceSpectrum))()
        _VideoCompiler = Me.GetCompiler(Of RayTracerVideo(Of RadianceSpectrum))()
        Me.Mode = CompileMode.Picture
    End Sub



    Public Property Mode As CompileMode
        Get
            Return If(_CompilePictureMenuItem.IsChecked, CompileMode.Picture, CompileMode.Video)
        End Get
        Set(value As CompileMode)
            Dim changed = Not (
            (value = CompileMode.Picture AndAlso _CompilePictureMenuItem.IsChecked) OrElse
            (value = CompileMode.Video AndAlso _CompileVideoMenuItem.IsChecked))

            _CompilePictureMenuItem.IsChecked = (value = CompileMode.Picture)
            _CompileVideoMenuItem.IsChecked = (value = CompileMode.Video)

            Select Case value
                Case CompileMode.Picture
                    _VideoCompiler.Deactivate()
                    _PictureCompiler.Activate()
                    _PictureCompiler.Compile()
                Case CompileMode.Video
                    _PictureCompiler.Deactivate()
                    _VideoCompiler.Activate()
                    _VideoCompiler.Compile()
            End Select
        End Set
    End Property

    Public Sub Compile()
        Select Case Me.Mode
            Case CompileMode.Picture
                _PictureCompiler.Compile()
            Case CompileMode.Video
                _VideoCompiler.Compile()
        End Select
    End Sub

    Private Function GetCompiler(Of TResult)() As RichCompiler(Of TResult)
        Return New RichCompiler(Of TResult)(RichTextBox:=_DescriptionBox,
                                            helpPopup:=_HelpPopup,
                                            helpListBox:=_HelpListBox,
                                            helpScrollViewer:=_HelpScrollViewer,
                                            baseContext:=_RelativisticRayTracerTermContextBuilder.TermContext,
                                            TypeNamedTypeDictionary:=_RelativisticRayTracerTermContextBuilder.TypeDictionary)
    End Function
    
    Public Sub Unfocus()
        If Me.Mode = CompileMode.Picture Then
            _PictureCompiler.Unfocus()
        Else
            _VideoCompiler.Unfocus()
        End If
    End Sub

    Public Property AutoCompile As Boolean
        Get
            Return _PictureCompiler.AutoCompile
        End Get
        Set(value As Boolean)
            _PictureCompiler.AutoCompile = value
            _VideoCompiler.AutoCompile = value
        End Set
    End Property

    Public Event PictureCompiled(e As CompilerResultEventArgs(Of RayTracerPicture(Of RadianceSpectrum)))
    Public Event VideoCompiled(e As CompilerResultEventArgs(Of RayTracerVideo(Of RadianceSpectrum)))

    Private Sub _PictureCompiler_Compiled(sender As Object, e As CompilerResultEventArgs(Of RayTracerPicture(Of RadianceSpectrum))) Handles _PictureCompiler.Compiled
        RaiseEvent PictureCompiled(e)
    End Sub

    Private Sub _VideoCompiler_Compiled(sender As Object, e As CompilerResultEventArgs(Of RayTracerVideo(Of RadianceSpectrum))) Handles _VideoCompiler.Compiled
        RaiseEvent VideoCompiled(e)
    End Sub

    Private Sub _CompileVideoMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _CompileVideoMenuItem.Click
        Me.Mode = CompileMode.Video

        e.Handled = True
    End Sub

    Private Sub CompilePictureMenuItem_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles _CompilePictureMenuItem.Click
        Me.Mode = CompileMode.Picture

        e.Handled = True
    End Sub

    Public ReadOnly Property ApplyingTextDecorations As Boolean
        Get
            If Me.Mode = CompileMode.Picture Then
                Return _PictureCompiler.ApplyingTextDecorations
            Else
                Return _VideoCompiler.ApplyingTextDecorations
            End If
        End Get
    End Property

End Class
