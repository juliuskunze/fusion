Imports System.Windows.Controls.Primitives

Public Class RichCompiler(Of TResult)

    Public Property TypeNamedTypeDictionary As TypeNamedTypeDictionary
    Private WithEvents _RichTextBox As RichTextBox
    Private ReadOnly _AutoCompletePopup As Popup
    Private ReadOnly _BaseContext As TermContext

    Private _ApplyingTextDecorations As Boolean

    Public Sub New(richTextBox As RichTextBox,
                   autoCompletePopup As Popup,
                   autoCompleteListBox As ListBox,
                   baseContext As TermContext,
                   typeNamedTypeDictionary As TypeNamedTypeDictionary)
        _RichTextBox = richTextBox
        _TypeNamedTypeDictionary = typeNamedTypeDictionary
        _AutoCompletePopup = autoCompletePopup
        _BaseContext = baseContext

        Dim pasteCommandBinding = New CommandBinding(ApplicationCommands.Paste, AddressOf OnPaste, AddressOf OnCanExecutePaste)
        _RichTextBox.CommandBindings.Add(pasteCommandBinding)
    End Sub

    Private Sub OnPaste(sender As Object, e As ExecutedRoutedEventArgs)
        If sender IsNot _RichTextBox Then Return

        Dim dataObject = Clipboard.GetDataObject
        If dataObject Is Nothing Then Return

        Dim clipboardString = CType(dataObject.GetData(GetType(String)), String)

        e.Handled = True

        If clipboardString IsNot Nothing Then
            Clipboard.SetDataObject(clipboardString, True)

            _RichTextBox.Paste()
            _RichTextBox.InvalidateVisual()
        End If
    End Sub

    Private Sub OnCanExecutePaste(target As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = target Is _RichTextBox
    End Sub


    Public Sub Compile()
        Dim textOnlyDocument = New TextOnlyDocument(_RichTextBox.Document)

        Dim compiler = New Compiler(Of TResult)(Text:=textOnlyDocument.Text, baseContext:=_BaseContext, TypeNamedTypeDictionary:=TypeNamedTypeDictionary, cursorPosition:=textOnlyDocument.GetIndex(_RichTextBox.Selection.Start))

        Try
            Dim compilerResult = compiler.GetResult

            RaiseEvent Compiled(Me, New CompilerResultEventArgs(Of TResult)(New RichCompilerResult(Of TResult)(compilerResult.Result)))
        Catch ex As CompilerExceptionWithCursorTermContext
            Dim locatedEx = TryCast(ex.InnerCompilerExcpetion, LocatedCompilerException)
            If locatedEx IsNot Nothing Then
                UnderlineError(locatedEx.LocatedString, textOnlyDocument)
            Else
                RemoveTextDecorations()
            End If

            RaiseEvent Compiled(Me, New CompilerResultEventArgs(Of TResult)(New RichCompilerResult(Of TResult)(ex.InnerCompilerExcpetion.Message)))
        End Try
    End Sub

    Private Sub UnderlineError(ByVal locatedString As LocatedString, textOnlyDocument As TextOnlyDocument)
        _ApplyingTextDecorations = True

        Dim length = If(locatedString.Length > 0, locatedString.Length, If(locatedString.ContainingAnalizedString.ToLocated.Contains(locatedString.EndIndex + 1), 1, 0))

        Dim errorLocation = textOnlyDocument.GetTextRange(startIndex:=locatedString.StartIndex, length:=length)
        Dim startTextPointer = _RichTextBox.Document.ContentStart
        Dim endTextPointer = _RichTextBox.Document.ContentEnd

        Dim beforeError = New TextRange(startTextPointer, errorLocation.Start)
        Dim errorRange = New TextRange(errorLocation.Start, errorLocation.End)
        Dim afterError = New TextRange(errorLocation.End, endTextPointer)

        beforeError.ApplyPropertyValue(Inline.TextDecorationsProperty, _NormalTextDecorations)
        errorRange.ApplyPropertyValue(Inline.TextDecorationsProperty, _ErrorTextDecorations)
        afterError.ApplyPropertyValue(Inline.TextDecorationsProperty, _NormalTextDecorations)

        _ApplyingTextDecorations = False
    End Sub

    Private Sub RemoveTextDecorations()
        Dim documentRange = New TextRange(_RichTextBox.Document.ContentStart, _RichTextBox.Document.ContentEnd)
        documentRange.ApplyPropertyValue(Inline.TextDecorationsProperty, _NormalTextDecorations)
    End Sub

    '!!!Public Function GetCorrectedText() As String
    '    Return MyBase..TrimEnd({" "c, Microsoft.VisualBasic.ControlChars.Tab, ";"c}).ToString
    'End Function

    Private Shared ReadOnly _ErrorTextDecorations As TextDecorationCollection = CreateErrorTextDecorations()
    Private Shared Function CreateErrorTextDecorations() As TextDecorationCollection
        Dim vector = New Vector(0, 10)

        Dim geometry = New StreamGeometry()
        Using context = geometry.Open
            context.BeginFigure(New Point, False, False)
            context.PolyLineTo({New Point(0.75, 0.75), New Point(1.5, 0), New Point(2.25, 0.75), New Point(3, 0)}, True, True)
        End Using

        Dim brushPattern = New GeometryDrawing With {.Pen = New Pen(Brushes.Blue, 0.2), .Geometry = geometry}


        Dim brush = New DrawingBrush(brushPattern) With {.TileMode = TileMode.Tile, .Viewport = New Rect(0, 0.7, 9, 3), .ViewportUnits = BrushMappingMode.Absolute}

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

    Private Sub RichTextBox_TextChanged(sender As System.Object, e As System.Windows.Controls.TextChangedEventArgs) Handles _RichTextBox.TextChanged
        If _ApplyingTextDecorations Then Return

        Me.Compile()
    End Sub

    Public Event Compiled(sender As Object, e As CompilerResultEventArgs(Of TResult))

End Class
