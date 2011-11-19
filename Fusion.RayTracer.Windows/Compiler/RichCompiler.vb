Imports System.Windows.Controls.Primitives

Public Class RichCompiler(Of TResult)

    Private WithEvents _RichTextBox As RichTextBox
    Private WithEvents _HelpListPopup As Popup
    Private WithEvents _HelpListBox As ListBox
    Private WithEvents _HelpScrollViewer As ScrollViewer
    Private _ItemToolTip As ToolTip
    Private _OpenedFunctionToolTip As ToolTip

    Private _ApplyingTextDecorations As Boolean
    Public ReadOnly Property ApplyingTextDecorations As Boolean
        Get
            Return _ApplyingTextDecorations
        End Get
    End Property

    Private _TextOnlyDocument As TextOnlyDocument

    Private _Compiler As Compiler(Of TResult)

    Private _AutoCompile As Boolean
    Public Property AutoCompile As Boolean
        Get
            Return _AutoCompile
        End Get
        Set(value As Boolean)
            If value Then
                Me.ActivateAutoCompile()
            Else
                Me.DeactivateAutoCompile()
            End If
        End Set
    End Property

    Public Sub New(richTextBox As RichTextBox,
                   helpListPopup As Popup,
                   helpListBox As ListBox,
                   helpScrollViewer As ScrollViewer,
                   baseContext As TermContext,
                   typeNamedTypeDictionary As TypeNamedTypeDictionary,
                   Optional autoCompile As Boolean = True)
        _RichTextBox = richTextBox
        _HelpListPopup = helpListPopup
        _HelpListBox = helpListBox
        _HelpScrollViewer = helpScrollViewer
        _AutoCompile = autoCompile

        _HelpListPopup.PlacementTarget = _RichTextBox
        _RichTextBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible
        _RichTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible


        _Compiler = New Compiler(Of TResult)(baseContext:=baseContext, typeNamedTypeDictionary:=typeNamedTypeDictionary)
        Me.UpdateOnTextChanged()

        Dim pasteCommandBinding = New CommandBinding(ApplicationCommands.Paste, AddressOf OnPaste, AddressOf OnCanExecutePaste)
        _RichTextBox.CommandBindings.Add(pasteCommandBinding)

        Me.AddHandlersIfNeeded()

        _OpenedFunctionToolTip = New ToolTip With
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
        Me.UpdateAndCompile()

        Me.AddHandlersIfNeeded()
    End Sub

    Private Sub UpdateAndCompile(Optional showHelp As Boolean = False)
        Me.UpdateOnTextChanged()
        Me.Compile(showHelp:=showHelp)
    End Sub

    Public Sub DeactivateAutoCompile()
        _AutoCompile = False

        Me.Unfocus()
        Me.RemoveUnderline()
    End Sub

    Public Sub Deactivate()
        Me.RemoveHandlersIfNeeded()
    End Sub

    Private _HandlersAdded As Boolean

    Public Sub ActivateAndCompile(Optional showHelp As Boolean = False)
        Me.UpdateAndCompile(showHelp:=showHelp)
        Me.AddHandlersIfNeeded()
    End Sub

    Private Sub UpdateOnTextChanged()
        _TextOnlyDocument = New TextOnlyDocument(_RichTextBox.Document)
        _Compiler.Update(newLocatedString:=Me.GetText,
                         newSelection:=Me.GetSelection)
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
        Dim compileHelp As CompileHelp = Nothing
        Dim richCompilerResult As RichCompilerResult(Of TResult) = Nothing

        Try
            Dim compilerResult = _Compiler.Compile(withHelp:=showHelp)

            compileHelp = compilerResult.CompileHelp
            richCompilerResult = New RichCompilerResult(Of TResult)(compilerResult.Result)

            Me.RemoveUnderline()
        Catch ex As CompilerExceptionWithHelp
            Dim locatedEx = TryCast(ex.InnerCompilerException, LocatedCompilerException)
            If locatedEx IsNot Nothing Then
                Me.UnderlineError(locatedEx.LocatedString, _TextOnlyDocument)
            Else
                Me.RemoveUnderline()
            End If

            compileHelp = ex.CompileHelp
            richCompilerResult = New RichCompilerResult(Of TResult)(ex.InnerCompilerException.Message)

        Catch ex As Reflection.TargetInvocationException
            compileHelp = compileHelp.Empty
            richCompilerResult = New RichCompilerResult(Of TResult)(ex.InnerException.Message)
            Me.RemoveUnderline()

        Catch ex As Exception
            compileHelp = compileHelp.Empty
            richCompilerResult = New RichCompilerResult(Of TResult)(ex.Message)
            Me.RemoveUnderline()

        Finally
            Me.ShowHelp(compileHelp)

            RaiseEvent Compiled(Me, New CompilerResultEventArgs(Of TResult)(richCompilerResult))
        End Try
    End Sub

    Private Sub ShowHelp(compileHelp As CompileHelp)
        If compileHelp.IsEmpty Then
            Me.CloseHelp()
            Return
        End If

        Me.TryShowOpenedFunctionHelp(compileHelp)
        Me.TryShowHelpList(compileHelp)
    End Sub

    Private Sub TryShowOpenedFunctionHelp(compileHelp As CompileHelp)
        Dim help = compileHelp.TryGetInnermostOpenFunctionHelp
        If help Is Nothing Then
            Me.CloseFunctionToolTip()
            Return
        End If

        _OpenedFunctionToolTip.Content = help.ToolTipText
        _OpenedFunctionToolTip.IsOpen = True

        Dim functionStartCharRect = _TextOnlyDocument.GetCharacterRect(index:=compileHelp.InnermostCalledFunction.Location.StartIndex)
        Dim currentIdentifierStartCharRect = _TextOnlyDocument.GetCharacterRect(_Compiler.CurrentIdentifierIfDefined.Location.StartIndex)

        _OpenedFunctionToolTip.VerticalOffset = currentIdentifierStartCharRect.Bottom - _RichTextBox.ActualHeight
        _OpenedFunctionToolTip.HorizontalOffset = functionStartCharRect.Left
    End Sub

    Private Sub TryShowHelpList(ByVal compileHelp As CompileHelp)
        Dim helpItems = compileHelp.GetItems
        If Not helpItems.Any Then
            Me.CloseHelpList()
            Return
        End If

        Me.ShowHelpList(helpItems)
    End Sub

    Private Sub ShowHelpList(helpItems As IEnumerable(Of CompileHelpItem))
        Dim listBoxItems = helpItems.Select(Function(helpItem) helpItem.ToListBoxItem)

        _HelpListBox.ItemsSource = listBoxItems

        Dim currentIdentifierStartCharRect = _TextOnlyDocument.GetCharacterRect(_Compiler.CurrentIdentifierIfDefined.Location.StartIndex)

        Dim extraVerticalOffset = If(_OpenedFunctionToolTip.IsOpen, _OpenedFunctionToolTip.ActualHeight, 0)
        _HelpListPopup.VerticalOffset = currentIdentifierStartCharRect.Bottom - _RichTextBox.ActualHeight + extraVerticalOffset
        _HelpListPopup.HorizontalOffset = currentIdentifierStartCharRect.Left

        Me.ReopenHelpList()

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

        Dim length = If(locatedString.Length > 0, locatedString.Length, If(locatedString.ContainingAnalizedString.ToLocated.Location.ContainsCharIndex(locatedString.Location.EndIndex + 1), 1, 0))

        Dim errorLocation = textOnlyDocument.GetTextRange(startIndex:=locatedString.Location.StartIndex, length:=length)
        Dim startTextPointer = _RichTextBox.Document.ContentStart
        Dim endTextPointer = _RichTextBox.Document.ContentEnd

        Dim beforeError = New Documents.TextRange(startTextPointer, errorLocation.Start)
        Dim errorRange = New Documents.TextRange(errorLocation.Start, errorLocation.End)
        Dim afterError = New Documents.TextRange(errorLocation.End, endTextPointer)

        beforeError.ApplyPropertyValue(Inline.TextDecorationsProperty, _NormalTextDecorations)
        errorRange.ApplyPropertyValue(Inline.TextDecorationsProperty, _ErrorTextDecorations)
        afterError.ApplyPropertyValue(Inline.TextDecorationsProperty, _NormalTextDecorations)

        _ApplyingTextDecorations = False
    End Sub

    Private Sub RemoveUnderline()
        _ApplyingTextDecorations = True

        Dim documentRange = New Documents.TextRange(_RichTextBox.Document.ContentStart, _RichTextBox.Document.ContentEnd)
        documentRange.ApplyPropertyValue(Inline.TextDecorationsProperty, _NormalTextDecorations)

        _ApplyingTextDecorations = False
    End Sub

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

    Private Sub RichTextBox_TextChanged(sender As System.Object, e As System.Windows.Controls.TextChangedEventArgs)
        If _ApplyingTextDecorations Then Return
        If _AutoCompile Then
            Me.UpdateAndCompile(showHelp:=True)
        Else
            Me.RemoveUnderline()
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
                    Me.CloseHelpList()

                    e.Handled = True
                Case Key.Tab
                    Me.CloseHelpAndAutoComplete()

                    e.Handled = True
            End Select
        ElseIf _OpenedFunctionToolTip.IsOpen Then
            Select Case e.Key
                Case Key.Escape
                    Me.CloseFunctionToolTip()
            End Select
        Else
            Select Case e.Key
                Case Key.Tab
                    Me.InsertText(New String(" "c, 4), textToReplace:=_Compiler.Selection)

                    e.Handled = True
            End Select
        End If


    End Sub

    Private Sub AutoCompleteListBox_GotFocus(sender As Object, e As System.Windows.RoutedEventArgs)
        _RichTextBox.Focus()

        Dim listBoxItem = TryCast(e.OriginalSource, ListBoxItem)
        listBoxItem.IsSelected = True
    End Sub

    Private Sub AutoCompleteListBox_PreviewMouseDown(sender As Object, e As MouseButtonEventArgs)
        If e.LeftButton <> MouseButtonState.Pressed AndAlso e.RightButton <> MouseButtonState.Pressed Then Return

        Dim tb = TryCast(e.OriginalSource, TextBlock)
        If tb Is Nothing Then Return

        If e.ClickCount = 2 Then
            Me.CloseHelpAndAutoComplete()
            e.Handled = True
        End If
    End Sub

    Private Sub CloseHelpAndAutoComplete()
        Me.CloseHelp()
        Me.AutoCompleteText(CStr(DirectCast(_HelpListBox.SelectedItem, ListBoxItem).Content))
    End Sub

    Private Sub AutoCompleteText(text As String)
        Me.InsertText(text:=text, textToReplace:=_Compiler.CurrentIdentifierIfDefined.Location)
    End Sub

    Private Sub InsertText(text As String, textToReplace As TextLocation)
        Dim clipboardCopy = Clipboard.GetDataObject.GetData(GetType(String))

        Clipboard.SetDataObject(text)

        Dim rangeToReplace = _TextOnlyDocument.GetTextRange(textToReplace)
        _RichTextBox.Selection.Select(rangeToReplace.Start, rangeToReplace.End)

        _RichTextBox.Paste()

        Clipboard.SetDataObject(If(clipboardCopy Is Nothing, "", clipboardCopy))
    End Sub

    Private Sub ReopenHelpList()
        Me.CloseHelpList()
        _HelpListPopup.IsOpen = True
    End Sub

    Private Sub CloseHelpList()
        Me.CloseItemToolTipIfNotNull()

        _HelpListPopup.IsOpen = False
    End Sub

    Private Sub CloseHelp()
        Me.CloseHelpList()
        Me.CloseFunctionToolTip()
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
        Me.OpenItemToolTip()
    End Sub

    Public Sub Unfocus()
        Me.CloseHelp()
    End Sub

    Private Sub BringSelectedIntoViewAndReopenTooltip()
        Me.CloseItemToolTipIfNotNull()

        Dim selectedItem = DirectCast(_HelpListBox.SelectedItem, ListBoxItem)
        If selectedItem Is Nothing Then Return

        _ItemToolTip = DirectCast(selectedItem.ToolTip, ToolTip)

        Me.ReopenCurrentToolTipIfNotNull()

        _ReopenItemToolTipOnScroll = True

        _HelpListBox.ScrollIntoView(selectedItem)
    End Sub

    Private Sub OpenItemToolTip()
        _ItemToolTip.HorizontalOffset = If(_HelpScrollViewer.ComputedVerticalScrollBarVisibility = Visibility.Visible, 22, 5)
        _ItemToolTip.IsOpen = True
    End Sub

    Private _ReopenItemToolTipOnScroll As Boolean

    Private Sub AutoCompleteScrollViewer_ScrollChanged(sender As Object, e As System.Windows.Controls.ScrollChangedEventArgs)
        If _ReopenItemToolTipOnScroll Then
            _ReopenItemToolTipOnScroll = False

            Me.ReopenCurrentToolTipIfNotNull()
        End If
    End Sub

    Private Sub AutoCompleteListBox_SelectionChanged(sender As Object, e As System.Windows.Controls.SelectionChangedEventArgs)
        Me.BringSelectedIntoViewAndReopenTooltip()
    End Sub

    Public Sub LoadDocument(description As String)
        Me.Deactivate()

        _RichTextBox.Document = TextOnlyDocument.GetDocumentFromText(description)

        Me.ActivateAndCompile(showHelp:=False)
    End Sub

End Class
