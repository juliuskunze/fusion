Imports System.Windows.Controls.Primitives

Public Class AutoCompleteTextBox
    Inherits TextBox

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
        'Dim text = TryCast(tb.Content, String)
        'Me.Text = text

        If Not {Key.Tab, Key.Enter}.Contains(e.Key) Then Return

        Me.ClosePopupAndUpdateSource()
    End Sub

    Private Sub ItemList_PreviewMouseDown(sender As Object, e As MouseButtonEventArgs)

        If e.LeftButton <> MouseButtonState.Pressed Then Return

        Dim toolTip = New ToolTip
        toolTip.Content = "yo"
        toolTip.Placement = PlacementMode.Bottom
        toolTip.IsOpen = True

        Dim tb As TextBlock = TryCast(e.OriginalSource, TextBlock)
        If tb Is Nothing Then Return
        tb.ToolTip = toolTip

        If e.ClickCount = 2 Then
            Me.ClosePopupAndUpdateSource()
        End If
        e.Handled = True
    End Sub

    Private Sub ClosePopupAndUpdateSource()
        Me.Popup.IsOpen = False

        Dim selected = CStr(Me.ItemList.SelectedItem)

        Me.Text &= selected

        Me.SelectionStart = Me.Text.Length
    End Sub

End Class
