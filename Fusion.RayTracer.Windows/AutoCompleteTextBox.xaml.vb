Imports System.Windows.Controls.Primitives

Public Class AutoCompleteTextBox
    Inherits RichTextBox

    Private _Loaded As Boolean

    Public ReadOnly Property Popup() As Popup
        Get
            Return TryCast(Me.Template.FindName("PART_Popup", Me), Popup)
        End Get
    End Property

    Public ReadOnly Property ListBox() As ListBox
        Get
            Return TryCast(Me.Template.FindName("PART_ItemListBox", Me), ListBox)
        End Get
    End Property

    Public ReadOnly Property Host() As ScrollViewer
        Get
            Return TryCast(Me.Template.FindName("PART_ContentHost", Me), ScrollViewer)
        End Get
    End Property

    Public ReadOnly Property TextBoxView() As UIElement
        Get
            For Each o In LogicalTreeHelper.GetChildren(Me.Host)
                Return TryCast(o, UIElement)
            Next

            Return Nothing
        End Get
    End Property

    Public Overrides Sub OnApplyTemplate()
        MyBase.OnApplyTemplate()
        AddHandler Me.KeyDown, AddressOf AutoCompleteTextBox_PreviewKeyDown
        AddHandler Me.PreviewKeyDown, AddressOf AutoCompleteTextBox_PreviewKeyDown
        AddHandler Me.ListBox.PreviewMouseDown, AddressOf ItemListBox_PreviewMouseDown
        AddHandler Me.ListBox.KeyDown, AddressOf ItemListBox_KeyDown
        AddHandler Me.ListBox.SelectionChanged, AddressOf ItemList_SelectionChanged
        _Loaded = True
    End Sub

    Private Sub AutoCompleteTextBox_PreviewKeyDown(sender As Object, e As KeyEventArgs)
        If TypeOf e.OriginalSource Is ListBoxItem Then Return

        If e.Key <> Key.Down OrElse Me.ListBox.Items.Count = 0 Then Return

        Me.ListBox.Focus()
        Me.ListBox.SelectedIndex = 0
        Dim listboxItem = TryCast(ListBox.ItemContainerGenerator.ContainerFromIndex(ListBox.SelectedIndex), ListBoxItem)
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
        Dim selected = CStr(DirectCast(Me.ListBox.SelectedItem, ListBoxItem).Content)

        Me.AppendText(selected)

        Me.Selection.Select(Me.Document.ContentEnd, Me.Document.ContentEnd)
    End Sub

    Private Sub ClosePopup()
        Me.Popup.IsOpen = False
        For Each listBoxItemObj In Me.ListBox.Items
            Dim listBoxItem = CType(listBoxItemObj, ListBoxItem)
            Dim toolTip = CType(listBoxItem.ToolTip, ToolTip)
            toolTip.IsOpen = False
        Next
    End Sub

    Private Sub ItemList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        For Each listBoxItemObj In Me.ListBox.Items
            Dim listBoxItem = CType(listBoxItemObj, ListBoxItem)
            Dim toolTip = CType(listBoxItem.ToolTip, ToolTip)
            toolTip.IsOpen = listBoxItem.IsSelected
        Next

    End Sub

End Class
