Imports System.Windows.Controls.Primitives

Public Class RichCompiler(Of TResult)

    Public Property TypeNamedTypeDictionary As TypeNamedTypeDictionary
    Private WithEvents _RichTextBox As RichTextBox
    Private WithEvents _AutoCompletePopup As Popup
    Private WithEvents _AutoCompleteListBox As ListBox
    Private WithEvents _AutoCompleteScrollViewer As ScrollViewer
    Private ReadOnly _BaseContext As TermContext

    Private _ApplyingTextDecorations As Boolean

    Private _TextOnlyDocument As TextOnlyDocument
    Private _LocatedString As LocatedString
    Private _CursorTextPointer As TextPointer
    Private _CurrentIdentifier As LocatedString

    Private _CurrentToolTip As ToolTip

    Public Sub New(richTextBox As RichTextBox,
                   autoCompletePopup As Popup,
                   autoCompleteListBox As ListBox,
                   autoCompleteScrollViewer As ScrollViewer,
                   baseContext As TermContext,
                   typeNamedTypeDictionary As TypeNamedTypeDictionary)
        _RichTextBox = richTextBox
        _TypeNamedTypeDictionary = typeNamedTypeDictionary
        _AutoCompletePopup = autoCompletePopup
        _AutoCompleteListBox = autoCompleteListBox
        _AutoCompleteScrollViewer = autoCompleteScrollViewer
        _BaseContext = baseContext

        _AutoCompletePopup.PlacementTarget = _RichTextBox
        _RichTextBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
        _RichTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible

        Dim pasteCommandBinding = New CommandBinding(ApplicationCommands.Paste, AddressOf OnPaste, AddressOf OnCanExecutePaste)
        _RichTextBox.CommandBindings.Add(pasteCommandBinding)
        
        Me.UpdateOnTextChanged()
    End Sub

    Private Sub UpdateOnTextChanged()
        _TextOnlyDocument = New TextOnlyDocument(_RichTextBox.Document)
        _LocatedString = _TextOnlyDocument.Text.ToLocated
        _CursorTextPointer = _RichTextBox.Selection.Start
        _CurrentIdentifier = _LocatedString.GetSurroundingIdentifier(_TextOnlyDocument.GetIndex(_CursorTextPointer))
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

    Public Sub Compile(Optional textChanged As Boolean = False)
        Dim filter = _CurrentIdentifier

        Dim compiler = New Compiler(Of TResult)(LocatedString:=_LocatedString, baseContext:=_BaseContext, TypeNamedTypeDictionary:=TypeNamedTypeDictionary, CursorPosition:=_TextOnlyDocument.GetIndex(_CursorTextPointer))

        Dim intelliSense As IntelliSense = Nothing
        Dim richCompilerResult As RichCompilerResult(Of TResult) = Nothing

        Try
            Dim compilerResult = compiler.Compile(textChanged:=textChanged)

            intelliSense = compilerResult.IntelliSense
            richCompilerResult = New RichCompilerResult(Of TResult)(compilerResult.Result)
        Catch ex As CompilerExceptionWithIntelliSense
            Dim locatedEx = TryCast(ex.InnerCompilerExcpetion, LocatedCompilerException)
            If locatedEx IsNot Nothing Then
                Me.UnderlineError(locatedEx.LocatedString, _TextOnlyDocument)
            Else
                Me.RemoveTextDecorations()
            End If

            intelliSense = ex.IntelliSense
            richCompilerResult = New RichCompilerResult(Of TResult)(ex.InnerCompilerExcpetion.Message)
        Finally
            Me.ShowIntelliSense(intelliSense)

            RaiseEvent Compiled(Me, New CompilerResultEventArgs(Of TResult)(richCompilerResult))
        End Try
    End Sub

    Private Sub ShowIntelliSense(intelliSense As IntelliSense)
        If intelliSense.IsEmpty Then
            Me.CloseAutoCompletePopup()
        Else
            Dim currentIdentifierStartCharRectangle = _TextOnlyDocument.GetTextPointer(_CurrentIdentifier.StartIndex).GetCharacterRect(LogicalDirection.Forward)

            _AutoCompletePopup.VerticalOffset = -(_RichTextBox.ActualHeight - currentIdentifierStartCharRectangle.Bottom)
            _AutoCompletePopup.HorizontalOffset = currentIdentifierStartCharRectangle.Left

            Dim intelliSenseItems = intelliSense.GetItems
            Dim listBoxItems = intelliSenseItems.Select(Function(intelliSenseItem) intelliSenseItem.ToListBoxItem)

            _AutoCompleteListBox.ItemsSource = listBoxItems

            If intelliSenseItems.Any Then
                Me.ReopenAutoCompletePopup()

                _AutoCompleteListBox.SelectedIndex = 0

                For i = 0 To intelliSenseItems.Count - 1
                    Dim intelliSenseItem = intelliSenseItems(i)
                    If intelliSenseItem.Name.StartsWith(intelliSense.Filter) Then
                        _AutoCompleteListBox.SelectedIndex = i
                        Exit For
                    End If
                Next
            Else
                Me.CloseAutoCompletePopup()
            End If
        End If
    End Sub

    Private Sub UnderlineError(locatedString As LocatedString, textOnlyDocument As TextOnlyDocument)
        _ApplyingTextDecorations = True

        Dim length = If(locatedString.Length > 0, locatedString.Length, If(locatedString.ContainingAnalizedString.ToLocated.ContainsCharIndex(locatedString.EndIndex + 1), 1, 0))

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


        'y = 0.7 for default font
        Dim brush = New DrawingBrush(brushPattern) With {.TileMode = TileMode.Tile, .Viewport = New Rect(0, -1, 9, 3), .ViewportUnits = BrushMappingMode.Absolute}

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

        Me.UpdateOnTextChanged()

        Me.Compile(textChanged:=True)
    End Sub

    Public Event Compiled(sender As Object, e As CompilerResultEventArgs(Of TResult))

    Private Sub RichTextBox_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles _RichTextBox.PreviewKeyDown
        If _AutoCompletePopup.IsOpen Then
            Select Case e.Key
                Case Key.Down
                    If _AutoCompleteListBox.SelectedIndex < _AutoCompleteListBox.Items.Count - 1 Then _AutoCompleteListBox.SelectedIndex += 1

                    e.Handled = True
                Case Key.Up
                    If _AutoCompleteListBox.SelectedIndex > 0 Then _AutoCompleteListBox.SelectedIndex -= 1

                    e.Handled = True
                Case Key.Escape
                    Me.CloseAutoCompletePopup()

                    e.Handled = True
                Case Key.Tab
                    Me.CloseAutoCompletePopupAndUpdateSource()

                    e.Handled = True
            End Select
        Else
            Select Case e.Key
                Case Key.Tab
                    Me.InsertText(New String(" "c, 4))

                    e.Handled = True
            End Select
        End If


    End Sub

    Private Sub AutoCompleteListBox_GotFocus(sender As Object, e As System.Windows.RoutedEventArgs) Handles _AutoCompleteListBox.GotFocus
        _RichTextBox.Focus()

        Dim listBoxItem = TryCast(e.OriginalSource, ListBoxItem)
        listBoxItem.IsSelected = True
    End Sub



    Private Sub AutoCompleteListBox_PreviewMouseDown(sender As Object, e As MouseButtonEventArgs) Handles _AutoCompleteListBox.PreviewMouseDown
        If e.LeftButton <> MouseButtonState.Pressed AndAlso e.RightButton <> MouseButtonState.Pressed Then Return

        Dim tb = TryCast(e.OriginalSource, TextBlock)
        If tb Is Nothing Then Return

        If e.ClickCount = 2 Then
            Me.CloseAutoCompletePopupAndUpdateSource()
            e.Handled = True
        End If
    End Sub

    Private Sub CloseAutoCompletePopupAndUpdateSource()
        Me.CloseAutoCompletePopup()

        Me.InsertText(CStr(DirectCast(_AutoCompleteListBox.SelectedItem, ListBoxItem).Content))
    End Sub

    Private Sub InsertText(text As String)
        Dim clipboardCopy = Clipboard.GetDataObject.GetData(GetType(String))

        Clipboard.SetDataObject(text)

        Dim rangeToReplace = _TextOnlyDocument.GetTextRange(_CurrentIdentifier)
        _RichTextBox.Selection.Select(rangeToReplace.Start, rangeToReplace.End)

        _RichTextBox.Paste()


        Clipboard.SetDataObject(If(clipboardCopy Is Nothing, "", clipboardCopy))
    End Sub

    Private Sub ReopenAutoCompletePopup()
        Me.CloseAutoCompletePopup()
        _AutoCompletePopup.IsOpen = True
    End Sub

    Private Sub CloseAutoCompletePopup()
        Me.CloseCurrentToolTipIfNotNull()

        _AutoCompletePopup.IsOpen = False
    End Sub

    Private Sub CloseCurrentToolTipIfNotNull()
        If _CurrentToolTip Is Nothing Then Return

        _CurrentToolTip.IsOpen = False
    End Sub

    Private Sub ReopenCurrentToolTipIfNotNull()
        If _CurrentToolTip Is Nothing Then Return

        _CurrentToolTip.IsOpen = False
        Me.OpenCurrentToolTip()
    End Sub


    Public Sub Deactivate()
        Me.CloseAutoCompletePopup()
    End Sub

    Private Sub AutoCompleteListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles _AutoCompleteListBox.SelectionChanged
        Me.CloseCurrentToolTipIfNotNull()

        Dim selectedItem = CType(_AutoCompleteListBox.SelectedItem, ListBoxItem)
        If selectedItem Is Nothing Then Return

        selectedItem.BringIntoView()
        _CurrentToolTip = DirectCast(selectedItem.ToolTip, ToolTip)

        Me.OpenCurrentToolTip()
    End Sub

    Private Sub OpenCurrentToolTip()
        _CurrentToolTip.HorizontalOffset = If(_AutoCompleteScrollViewer.ComputedVerticalScrollBarVisibility = Visibility.Visible, 22, 5)

        _CurrentToolTip.IsOpen = True
    End Sub

    Private Sub AutoCompleteScrollViewer_ScrollChanged(sender As Object, e As System.Windows.Controls.ScrollChangedEventArgs) Handles _AutoCompleteScrollViewer.ScrollChanged
        Me.ReopenCurrentToolTipIfNotNull()
    End Sub

End Class
