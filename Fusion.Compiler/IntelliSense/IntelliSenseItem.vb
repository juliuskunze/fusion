Public Class IntelliSenseItem

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

    Public Sub New(signature As ISignature)
        Me.New(Name:=signature.Name, ToolTipText:=signature.GetSignatureString)
    End Sub

    Public Sub New(functionGroup As IGrouping(Of String, FunctionInstance))
        Me.New(Name:=functionGroup.Key, ToolTipText:=String.Join(separator:=Microsoft.VisualBasic.ControlChars.NewLine, values:=functionGroup.Select(Function(instance) instance.Signature.ToString)))
    End Sub

End Class
