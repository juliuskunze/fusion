Imports System.Windows.Controls.Primitives

Public Class PictureOrVideoCompiler

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
                   relativisticRayTracerTermContextBuilder As RelativisticRayTracerTermContextBuilder)
        _DescriptionBox = descriptionBox
        _HelpPopup = helpPopup
        _HelpListBox = helpListBox
        _HelpScrollViewer = helpScrollViewer
        _RelativisticRayTracerTermContextBuilder = relativisticRayTracerTermContextBuilder

        _PictureCompiler = Me.GetCompiler(Of RayTracerPicture(Of RadianceSpectrum))()
        _VideoCompiler = Me.GetCompiler(Of RayTracerVideo(Of RadianceSpectrum))()
    End Sub

    Private _Mode As CompileMode
    Public Property Mode As CompileMode
        Get
            Return _Mode
        End Get
        Set(value As CompileMode)
            _Mode = value

            Select Case value
                Case CompileMode.Picture
                    _VideoCompiler.Deactivate()
                    _PictureCompiler.ActivateAndCompile()
                Case CompileMode.Video
                    _PictureCompiler.Deactivate()
                    _VideoCompiler.ActivateAndCompile()
            End Select
        End Set
    End Property

    Public Sub Compile(Optional showHelp As Boolean = False)
        Select Case Me.Mode
            Case CompileMode.Picture
                _PictureCompiler.Compile(showHelp:=showHelp)
            Case CompileMode.Video
                _VideoCompiler.Compile(showHelp:=showHelp)
        End Select
    End Sub

    Private Function GetCompiler(Of TResult)() As RichCompiler(Of TResult)
        Return New RichCompiler(Of TResult)(RichTextBox:=_DescriptionBox,
                                            helpListPopup:=_HelpPopup,
                                            helpListBox:=_HelpListBox,
                                            helpScrollViewer:=_HelpScrollViewer,
                                            baseContext:=_RelativisticRayTracerTermContextBuilder.TermContext,
                                            typeDictionary:=_RelativisticRayTracerTermContextBuilder.TypeDictionary)
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

    Public ReadOnly Property ApplyingTextDecorations As Boolean
        Get
            If Me.Mode = CompileMode.Picture Then
                Return _PictureCompiler.ApplyingTextDecorations
            Else
                Return _VideoCompiler.ApplyingTextDecorations
            End If
        End Get
    End Property

    Public Sub LoadDocument(description As String)
        If Me.Mode = CompileMode.Picture Then
            _PictureCompiler.LoadDocument(description:=description)
        Else
            _VideoCompiler.LoadDocument(description:=description)
        End If
    End Sub

End Class

