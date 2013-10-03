<Serializable()>
Public Class EndNodes(Of NodeType)

    Public Sub New(node1 As NodeType, node2 As NodeType)
        Me.Node1 = node1
        Me.Node2 = node2
    End Sub

    Public Property Node1() As NodeType
    Public Property Node2() As NodeType
End Class