Imports System.Windows.Controls.Primitives

Public Class RichCompiler(Of TResult)

    Private WithEvents _RichTextBox As RichTextBox
    Private WithEvents _HelpListPopup As Popup
    Private WithEvents _HelpListBox As ListBox
    Private WithEvents _HelpScrollViewer As ScrollViewer
    Private _ItemToolTip As TextToolTip
    Private ReadOnly _OpenedFunctionToolTip As TextToolTip

    Private _ApplyingTextDecorations As Boolean
    Public ReadOnly Property ApplyingTextDecorations As Boolean
        Get
            Return _ApplyingTextDecorations
        End Get
    End Property

    Private _TextOnlyDocument As TextOnlyDocument

    Private ReadOnly _Compiler As Compiler(Of TResult)

    Private _AutoCompile As Boolean
    Public Property AutoCompile As Boolean
        Get
            Return _AutoCompile
        End Get
        Set(value As Boolean)
            If value Then
                ActivateAutoCompile()
            Else
                DeactivateAutoCompile()
            End If
        End Set
    End Property

    Public Sub New(richTextBox As RichTextBox,
                   helpListPopup As Popup,
                   helpListBox As ListBox,
                   helpScrollViewer As ScrollViewer,
                   baseContext As TermContext,
                   typeDictionary As TypeDictionary,
                   Optional autoCompile As Boolean = True)
        _RichTextBox = richTextBox
        _HelpListPopup = helpListPopup
        _HelpListBox = helpListBox
        _HelpScrollViewer = helpScrollViewer
        _AutoCompile = autoCompile
        _HelpListPopup.PlacementTarget = _RichTextBox
        _RichTextBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
        _RichTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible

        _Compiler = New Compiler(Of TResult)(baseContext:=baseContext, typeDictionary:=typeDictionary)
        UpdateOnTextOrSelectionChanged()

        Dim pasteCommandBinding = New CommandBinding(ApplicationCommands.Paste, AddressOf OnPaste, AddressOf OnCanExecutePaste)
        _RichTextBox.CommandBindings.Add(pasteCommandBinding)

        AddHandlersIfNeeded()

        _OpenedFunctionToolTip = New TextToolTip With
            {
            .Placement = PlacementMode.Bottom,
            .PlacementTarget = _RichTextBox,
            .StaysOpen = True
            }
    End Sub

    Private Sub AddHandlersIfNeeded()
        If _HandlersAdded Then Return

        AddHandler _HelpListBox.SelectionChanged, AddressOf AutoCompleteListBox_SelectionChanged
        AddHandler _HelpScrollViewer.ScrollChanged, AddressOf AutoCompleteScrollViewer_ScrollChanged
        AddHandler _HelpListBox.PreviewMouseDown, AddressOf AutoCompleteListBox_PreviewMouseDown
        AddHandler _HelpListBox.GotFocus, AddressOf AutoCompleteListBox_GotFocus
        AddHandler _RichTextBox.TextChanged, AddressOf RichTextBox_TextChanged
        AddHandler _RichTextBox.PreviewKeyDown, AddressOf RichTextBox_PreviewKeyDown

        _HandlersAdded = True
    End Sub

    Private Sub RemoveHandlersIfNeeded()
        If Not _HandlersAdded Then Return

        RemoveHandler _HelpListBox.SelectionChanged, AddressOf AutoCompleteListBox_SelectionChanged
        RemoveHandler _HelpScrollViewer.ScrollChanged, AddressOf AutoCompleteScrollViewer_ScrollChanged
        RemoveHandler _HelpListBox.PreviewMouseDown, AddressOf AutoCompleteListBox_PreviewMouseDown
        RemoveHandler _HelpListBox.GotFocus, AddressOf AutoCompleteListBox_GotFocus
        RemoveHandler _RichTextBox.TextChanged, AddressOf RichTextBox_TextChanged
        RemoveHandler _RichTextBox.PreviewKeyDown, AddressOf RichTextBox_PreviewKeyDown

        _HandlersAdded = False
    End Sub

    Public Sub ActivateAutoCompile()
        _AutoCompile = True
        UpdateAndCompile()

        AddHandlersIfNeeded()
    End Sub

    Private Sub UpdateAndCompile(Optional showHelp As Boolean = False)
        UpdateOnTextOrSelectionChanged()
        Compile(showHelp:=showHelp)
    End Sub

    Public Sub DeactivateAutoCompile()
        _AutoCompile = False

        Unfocus()
        RemoveUnderline()
    End Sub

    Public Sub Deactivate()
        RemoveHandlersIfNeeded()
    End Sub

    Private _HandlersAdded As Boolean

    Public Sub ActivateAndCompile(Optional showHelp As Boolean = False)
        UpdateAndCompile(showHelp:=showHelp)
        AddHandlersIfNeeded()
    End Sub

    Private Sub UpdateOnTextOrSelectionChanged()
        _TextOnlyDocument = New TextOnlyDocument(_RichTextBox.Document)
        _Compiler.Update(newLocatedString:=GetText,
                         newSelection:=GetSelection)
    End Sub

    Private Function GetText() As LocatedString
        Return _TextOnlyDocument.Text.ToLocated
    End Function

    Private Function GetSelection() As TextLocation
        Dim startIndex = _TextOnlyDocument.GetIndex(_RichTextBox.Selection.Start)
        Dim endIndex = _TextOnlyDocument.GetIndex(_RichTextBox.Selection.End)

        Return New TextLocation(startIndex:=startIndex, length:=startIndex - endIndex)
    End Function

    Private Sub OnPaste(sender As Object, e As ExecutedRoutedEventArgs)
        If sender IsNot _RichTextBox Then Return

        Dim dataObject = Clipboard.GetDataObject
        If dataObject Is Nothing Then Return

        Dim clipboardString = CType(dataObject.GetData(GetType(String)), String)

        e.Handled = True

        If clipboardString IsNot Nothing Then
            Clipboard.SetDataObject(clipboardString, copy:=True)

            _RichTextBox.Paste()
            _RichTextBox.InvalidateVisual()
        End If
    End Sub

    Private Sub OnCanExecutePaste(target As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = target Is _RichTextBox
    End Sub

    Public Sub Compile(Optional showHelp As Boolean = False)
        Dim compilerHelp As CompilerHelp = Nothing
        Dim richCompilerResult As RichCompilerResult(Of TResult) = Nothing

        Try
            Dim compilerResult = _Compiler.Compile(withHelp:=showHelp)

            compilerHelp = compilerResult.CompilerHelp
            richCompilerResult = New RichCompilerResult(Of TResult)(compilerResult.Result)

            RemoveUnderline()
        Catch ex As CompilerExceptionWithHelp
            Dim locatedEx = TryCast(ex.InnerCompilerException, LocatedCompilerException)
            If locatedEx IsNot Nothing Then
                UnderlineError(locatedEx.LocatedString, _TextOnlyDocument)
            Else
                RemoveUnderline()
            End If

            compilerHelp = ex.CompilerHelp
            richCompilerResult = New RichCompilerResult(Of TResult)(ex.InnerCompilerException.Message)

        Catch ex As Reflection.TargetInvocationException
            compilerHelp = compilerHelp.Empty
            richCompilerResult = New RichCompilerResult(Of TResult)(ex.InnerException.Message)
            RemoveUnderline()

        Catch ex As Exception
            compilerHelp = compilerHelp.Empty
            richCompilerResult = New RichCompilerResult(Of TResult)(ex.Message)
            RemoveUnderline()

        Finally
            Me.ShowHelp(compilerHelp)

            RaiseEvent Compiled(Me, New CompilerResultEventArgs(Of TResult)(richCompilerResult))
        End Try
    End Sub

    Private Sub ShowHelp(compilerHelp As CompilerHelp)
        If compilerHelp.IsEmpty Then
            CloseHelp()
            Return
        End If

        TryShowOpenedFunctionHelp(compilerHelp)
        TryShowHelpList(compilerHelp)
    End Sub

    Private Sub TryShowOpenedFunctionHelp(compilerHelp As CompilerHelp)
        Dim help = compilerHelp.TryGetInnermostOpenFunctionHelp
        If help Is Nothing Then
            CloseFunctionToolTip()
            Return
        End If

        _OpenedFunctionToolTip.Text = help.ToolTipText
        _OpenedFunctionToolTip.IsOpen = True

        Dim functionStartCharRect = _TextOnlyDocument.GetCharacterRect(index:=compilerHelp.InnermostCalledFunction.Location.StartIndex)
        Dim currentIdentifierStartCharRect = _TextOnlyDocument.GetCharacterRect(_Compiler.CurrentIdentifierIfDefined.Location.StartIndex)

        _OpenedFunctionToolTip.VerticalOffset = currentIdentifierStartCharRect.Bottom - _RichTextBox.ActualHeight
        _OpenedFunctionToolTip.HorizontalOffset = functionStartCharRect.Left
    End Sub

    Private Sub TryShowHelpList(compilerHelp As CompilerHelp)
        Dim helpItems = compilerHelp.GetItems
        If Not helpItems.Any Then
            CloseHelpList()
            Return
        End If

        ShowHelpList(helpItems)
    End Sub

    Private Sub ShowHelpList(helpItems As IEnumerable(Of CompilerHelpItem))
        Dim listBoxItems = helpItems.Select(Function(helpItem) helpItem.ToListBoxItem)

        _HelpListBox.ItemsSource = listBoxItems

        Dim currentIdentifierStartCharRect = _TextOnlyDocument.GetCharacterRect(_Compiler.CurrentIdentifierIfDefined.Location.StartIndex)

        Dim extraVerticalOffset = If(_OpenedFunctionToolTip.IsOpen, _OpenedFunctionToolTip.ActualHeight, 0)
        _HelpListPopup.VerticalOffset = currentIdentifierStartCharRect.Bottom - _RichTextBox.ActualHeight + extraVerticalOffset
        _HelpListPopup.HorizontalOffset = currentIdentifierStartCharRect.Left

        ReopenHelpList()

        Dim selectedIndex = 0

        For index = 0 To helpItems.Count - 1
            Dim helpItem = helpItems(index)
            If helpItem.Name.StartsWith(_Compiler.CurrentIdentifierIfDefined.ToString, StringComparison.InvariantCultureIgnoreCase) Then
                selectedIndex = index

                Exit For
            End If
        Next

        _HelpListBox.SelectedIndex = selectedIndex
    End Sub

    Private Sub UnderlineError(locatedString As LocatedString, textOnlyDocument As TextOnlyDocument)
        _ApplyingTextDecorations = True

        Dim length = If(locatedString.Length > 0, locatedString.Length, If(locatedString.ContainingAnalyzedString.ToLocated.Location.ContainsCharIndex(locatedString.Location.EndIndex + 1), 1, 0))

        Dim errorLocation = textOnlyDocument.GetTextRange(startIndex:=locatedString.Location.StartIndex, length:=length)
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

    Private Sub RemoveUnderline()
        _ApplyingTextDecorations = True

        Dim documentRange = New TextRange(_RichTextBox.Document.ContentStart, _RichTextBox.Document.ContentEnd)
        documentRange.ApplyPropertyValue(Inline.TextDecorationsProperty, _NormalTextDecorations)

        _ApplyingTextDecorations = False
    End Sub

    Private Shared ReadOnly _ErrorTextDecorations As TextDecorationCollection = CreateErrorTextDecorations()
    Private Shared Function CreateErrorTextDecorations() As TextDecorationCollection
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

    Private Sub RichTextBox_TextChanged(sender As System.Object, e As TextChangedEventArgs)
        If _ApplyingTextDecorations Then Return
        If _AutoCompile Then
            UpdateAndCompile(showHelp:=True)
        Else
            RemoveUnderline()
        End If
    End Sub

    Public Event Compiled(sender As Object, e As CompilerResultEventArgs(Of TResult))

    Private Sub RichTextBox_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If _HelpListPopup.IsOpen Then
            Select Case e.Key
                Case Key.Down
                    If _HelpListBox.SelectedIndex < _HelpListBox.Items.Count - 1 Then _HelpListBox.SelectedIndex += 1

                    e.Handled = True
                Case Key.Up
                    If _HelpListBox.SelectedIndex > 0 Then _HelpListBox.SelectedIndex -= 1

                    e.Handled = True
                Case Key.Escape
                    CloseHelpList()

                    e.Handled = True
                Case Key.Tab
                    CloseHelpAndInsertSuggestion()

                    e.Handled = True
            End Select
        ElseIf _OpenedFunctionToolTip.IsOpen Then
            Select Case e.Key
                Case Key.Escape
                    CloseFunctionToolTip()
            End Select
        Else
            Select Case e.Key
                Case Key.Tab
                    InsertText(New String(" "c, 4), textToReplace:=_Compiler.Selection)

                    e.Handled = True
            End Select
        End If


    End Sub

    Private Sub AutoCompleteListBox_GotFocus(sender As Object, e As RoutedEventArgs)
        _RichTextBox.Focus()

        Dim listBoxItem = TryCast(e.OriginalSource, ListBoxItem)
        listBoxItem.IsSelected = True
    End Sub

    Private Sub AutoCompleteListBox_PreviewMouseDown(sender As Object, e As MouseButtonEventArgs)
        If e.LeftButton <> MouseButtonState.Pressed AndAlso e.RightButton <> MouseButtonState.Pressed Then Return

        Dim tb = TryCast(e.OriginalSource, TextBlock)
        If tb Is Nothing Then Return

        If e.ClickCount = 2 Then
            CloseHelpAndInsertSuggestion()
            e.Handled = True
        End If
    End Sub

    Private Sub CloseHelpAndInsertSuggestion()
        CloseHelp()
        CompleteIdentifierByHelp(CStr(DirectCast(_HelpListBox.SelectedItem, ListBoxItem).Content))
    End Sub

    Private Sub CompleteIdentifierByHelp(text As String)
        InsertText(text:=text, textToReplace:=_Compiler.CurrentIdentifierIfDefined.Location)
        CloseHelp()
    End Sub

    Private Sub InsertText(text As String, textToReplace As TextLocation)
        Dim replaceStartPosition = _TextOnlyDocument.GetTextRange(textToReplace).Start
        replaceStartPosition.DeleteTextInRun(textToReplace.Length)
        replaceStartPosition.InsertTextInRun(text)

        Dim finalCoursorPosition = replaceStartPosition.GetPositionAtOffset(text.Length)
        _RichTextBox.Selection.Select(finalCoursorPosition, finalCoursorPosition)
    End Sub

    Private Sub ReopenHelpList()
        CloseHelpList()
        _HelpListPopup.IsOpen = True
    End Sub

    Private Sub CloseHelpList()
        CloseItemToolTipIfNotNull()

        _HelpListPopup.IsOpen = False
    End Sub

    Private Sub CloseHelp()
        CloseHelpList()
        CloseFunctionToolTip()
    End Sub

    Private Sub CloseFunctionToolTip()
        _OpenedFunctionToolTip.IsOpen = False
    End Sub

    Private Sub CloseItemToolTipIfNotNull()
        If _ItemToolTip Is Nothing Then Return

        _ItemToolTip.IsOpen = False
    End Sub

    Private Sub ReopenCurrentToolTipIfNotNull()
        If _ItemToolTip Is Nothing Then Return

        _ItemToolTip.IsOpen = False
        OpenItemToolTip()
    End Sub

    Public Sub Unfocus()
        CloseHelp()
    End Sub

    Private Sub BringSelectedIntoViewAndReopenTooltip()
        CloseItemToolTipIfNotNull()

        Dim selectedItem = DirectCast(_HelpListBox.SelectedItem, ListBoxItem)
        If selectedItem Is Nothing Then Return

        _ItemToolTip = DirectCast(selectedItem.ToolTip, TextToolTip)

        ReopenCurrentToolTipIfNotNull()

        _ReopenItemToolTipOnScroll = True

        _HelpListBox.ScrollIntoView(selectedItem)
    End Sub

    Private Sub OpenItemToolTip()
        _ItemToolTip.HorizontalOffset = If(_HelpScrollViewer.ComputedVerticalScrollBarVisibility = Visibility.Visible, 22, 5)
        _ItemToolTip.IsOpen = True
    End Sub

    Private _ReopenItemToolTipOnScroll As Boolean

    Private Sub AutoCompleteScrollViewer_ScrollChanged(sender As Object, e As ScrollChangedEventArgs)
        If _ReopenItemToolTipOnScroll Then
            _ReopenItemToolTipOnScroll = False

            ReopenCurrentToolTipIfNotNull()
        End If
    End Sub

    Private Sub AutoCompleteListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        BringSelectedIntoViewAndReopenTooltip()
    End Sub

    Public Sub LoadDocument(description As String)
        Deactivate()

        _RichTextBox.Document = TextOnlyDocument.GetDocumentFromText(description)

        ActivateAndCompile(showHelp:=False)
    End Sub

    Private Sub _RichTextBox_SelectionChanged(sender As Object, e As RoutedEventArgs) Handles _RichTextBox.SelectionChanged
        UpdateOnTextOrSelectionChanged()
    End Sub
End Class