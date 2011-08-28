Imports System.Windows.Controls.Primitives

Public Class AutoCompleteTextBox
    Inherits RichTextBox

    Private _Loaded As Boolean

    Public ReadOnly Property Popup() As Popup
        Get
            Return TryCast(Me.Template.FindName("PART_Popup", Me), Popup)
        End Get
    End Property

    Public ReadOnly Property ItemList() As ListBox
        Get
            Return TryCast(Me.Template.FindName("PART_ItemList", Me), ListBox)
        End Get
    End Property

    Public ReadOnly Property Host() As ScrollViewer
        Get
            Return TryCast(Me.Template.FindName("PART_ContentHost", Me), ScrollViewer)
        End Get
    End Property

    Public ReadOnly Property TextBoxView() As UIElement
        Get
            For Each o In LogicalTreeHelper.GetChildren(Host)
                Return TryCast(o, UIElement)
            Next

            Return Nothing
        End Get
    End Property

    Public Overrides Sub OnApplyTemplate()
        MyBase.OnApplyTemplate()
        _loaded = True
        AddHandler Me.KeyDown, AddressOf AutoCompleteTextBox_PreviewKeyDown
        AddHandler Me.PreviewKeyDown, AddressOf AutoCompleteTextBox_PreviewKeyDown
        AddHandler Me.ItemList.PreviewMouseDown, AddressOf ItemList_PreviewMouseDown
        AddHandler Me.ItemList.KeyDown, AddressOf ItemList_KeyDown
        AddHandler Me.ItemList.SelectionChanged, AddressOf ItemList_SelectionChanged
    End Sub

    Private Sub AutoCompleteTextBox_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If e.Key <> Key.Down OrElse ItemList.Items.Count <= 0 OrElse (TypeOf e.OriginalSource Is ListBoxItem) Then Return

        Me.ItemList.Focus()
        Me.ItemList.SelectedIndex = 0
        Dim listboxItem = TryCast(ItemList.ItemContainerGenerator.ContainerFromIndex(ItemList.SelectedIndex), ListBoxItem)
        listboxItem.Focus()
        e.Handled = True
    End Sub

    Private Sub AutoCompleteTextBox_KeyDown(sender As Object, e As KeyEventArgs)
        If Not {Key.Tab, Key.Enter}.Contains(e.Key) Then Return

        Me.ClosePopupAndUpdateSource()
    End Sub

    Private Sub ItemList_KeyDown(sender As Object, e As KeyEventArgs)
        If Not TypeOf e.OriginalSource Is ListBoxItem Then Return

        'Dim tb As ListBoxItem = TryCast(e.OriginalSource, ListBoxItem)

        If Not {Key.Tab, Key.Enter}.Contains(e.Key) Then Return

        Me.ClosePopupAndUpdateSource()
    End Sub

    Private Sub ItemList_PreviewMouseDown(sender As Object, e As MouseButtonEventArgs)
        If e.LeftButton <> MouseButtonState.Pressed Then Return

        Dim tb = TryCast(e.OriginalSource, TextBlock)
        If tb Is Nothing Then Return

        If e.ClickCount = 2 Then
            Me.ClosePopupAndUpdateSource()
            e.Handled = True
        End If
    End Sub

    Private Sub ClosePopupAndUpdateSource()
        Me.Popup.IsOpen = False

        Dim selected = CStr(DirectCast(Me.ItemList.SelectedItem, ListBoxItem).Content)

        Me.AppendText(selected)

        Me.Selection.Select(Me.Document.ContentEnd, Me.Document.ContentEnd)
    End Sub

    Private Sub ItemList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        CType(CType(e.AddedItems(0), ListBoxItem).ToolTip, ToolTip).IsOpen = True
    End Sub

End Class
