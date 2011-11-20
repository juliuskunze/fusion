Public Class CompileHelpItem

    Private ReadOnly _Name As String
    Public ReadOnly Property Name As String
        Get
            Return _Name
        End Get
    End Property

    Private ReadOnly _ToolTipText As String
    Public ReadOnly Property ToolTipText As String
        Get
            Return _ToolTipText
        End Get
    End Property

    Public Sub New(name As String, toolTipText As String)
        _Name = name
        _ToolTipText = toolTipText
    End Sub

    Private Shared Function GetSignatureToolTipText(signature As ISignature) As String
        Return signature.GetSignatureString & If(signature.Description Is Nothing, "", Microsoft.VisualBasic.ControlChars.NewLine & signature.Description)
    End Function

    Public Sub New(signature As ISignature)
        Me.New(Name:=signature.Name, ToolTipText:=GetSignatureToolTipText(signature))
    End Sub

    Public Sub New(functionGroup As IGrouping(Of String, FunctionInstance))
        Me.New(Name:=functionGroup.Key, ToolTipText:=String.Join(separator:=Microsoft.VisualBasic.ControlChars.NewLine, values:=functionGroup.Select(Function(instance) GetSignatureToolTipText(instance.Signature))))
    End Sub

    Public Function ToListBoxItem() As ListBoxItem
        Dim listBoxItem = New ListBoxItem
        listBoxItem.Content = _Name
        listBoxItem.ToolTip = Me.GetTooltip(listBoxItem)

        Return listBoxItem
    End Function

    Private Function GetTooltip(listBoxItem As ListBoxItem) As TextToolTip
        Dim tooltip = New TextToolTip
        tooltip.Text = _ToolTipText
        tooltip.PlacementTarget = listBoxItem
        tooltip.Placement = Controls.Primitives.PlacementMode.Right
        Return tooltip
    End Function
End Class
