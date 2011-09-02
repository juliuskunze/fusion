Public Class RelativisticRayTracerPictureCompiler
    Inherits Compiler(Of RayTracerPicture(Of RadianceSpectrum))
    
    Private ReadOnly _TextOnlyDocument As TextOnlyDocument
    Private ReadOnly _RichTextBox As RichTextBox

    Public Sub New(richTextBox As RichTextBox, baseContext As TermContext, typeNamedTypeDictionary As TypeNamedTypeDictionary)
        Me.New(TextOnlyDocument:=New TextOnlyDocument(richTextBox.Document), richTextBox:=richTextBox, baseContext:=baseContext, typeNamedTypeDictionary:=typeNamedTypeDictionary)
    End Sub

    Private Sub New(textOnlyDocument As TextOnlyDocument, richTextBox As RichTextBox, baseContext As TermContext, typeNamedTypeDictionary As TypeNamedTypeDictionary)
        MyBase.New(Text:=textOnlyDocument.Text, baseContext:=baseContext, typeNamedTypeDictionary:=typeNamedTypeDictionary)
        _TextOnlyDocument = textOnlyDocument
        _RichTextBox = richTextBox
    End Sub

    Public Function CompileAndShowErrors() As CompilerResult(Of RayTracerPicture(Of RadianceSpectrum))
        Try
            Return New CompilerResult(Of RayTracerPicture(Of RadianceSpectrum))(MyBase.GetResult)
        Catch ex As CompilerException
            Dim locatedEx = TryCast(ex, LocatedCompilerException)
            If locatedEx IsNot Nothing Then UnderlineError(locatedEx.LocatedString)

            Return New CompilerResult(Of RayTracerPicture(Of RadianceSpectrum))(ex.Message)
        End Try
    End Function

    Private Sub UnderlineError(ByVal locatedString As LocatedString)
        Dim errorLocation = _TextOnlyDocument.GetTextRange(startIndex:=locatedString.StartIndex, length:=locatedString.Length)
        Dim startTextPointer = _RichTextBox.Document.ContentStart
        Dim endTextPointer = _RichTextBox.Document.ContentEnd

        Dim beforeError = New TextRange(startTextPointer, errorLocation.Start)
        Dim errorRange = New TextRange(errorLocation.Start, errorLocation.End)
        Dim afterError = New TextRange(errorLocation.End, endTextPointer)

        beforeError.ApplyPropertyValue(Inline.TextDecorationsProperty, _NormalTextDecorations)
        errorRange.ApplyPropertyValue(Inline.TextDecorationsProperty, _ErrorTextDecorations)
        afterError.ApplyPropertyValue(Inline.TextDecorationsProperty, _NormalTextDecorations)
    End Sub

    Public Function GetCorrectedText() As String
        Return _LocatedString.TrimEnd({" "c, Microsoft.VisualBasic.ControlChars.Tab, ";"c}).ToString
    End Function

    Private Shared ReadOnly _ErrorTextDecorations As TextDecorationCollection = CreateErrorTextDecorations()
    Private Shared Function CreateErrorTextDecorations() As TextDecorationCollection
        Dim geometry = New StreamGeometry()
        Using context = geometry.Open
            context.BeginFigure(New Point(0.0, 0.0), False, False)
            context.PolyLineTo({New Point(0.75, 0.75), New Point(1.5, 0.0), New Point(2.25, 0.75), New Point(3.0, 0.0)}, True, True)
        End Using

        Dim brushPattern = New GeometryDrawing() With {.Pen = New Pen(Brushes.Blue, 0.2), .Geometry = geometry}


        Dim brush = New DrawingBrush(brushPattern) With {.TileMode = TileMode.Tile, .Viewport = New Rect(0.0, 1.5, 9.0, 3.0), .ViewportUnits = BrushMappingMode.Absolute}

        Dim pen = New Pen(brush, 3.0)
        pen.Freeze()

        Dim textDecoration = New TextDecoration With {.Pen = pen}

        Dim collection = New TextDecorationCollection({textDecoration})
        collection.Freeze()

        Return collection
    End Function

    Private Shared ReadOnly _NormalTextDecorations As TextDecorationCollection = CreateNormalTextDecorations()
    Private Shared Function CreateNormalTextDecorations() As TextDecorationCollection
        Dim collection = New TextDecorationCollection({})
        collection.Freeze()

        Return collection
    End Function

End Class
