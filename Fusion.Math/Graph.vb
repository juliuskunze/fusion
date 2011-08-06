<Serializable()>
Public Class Graph(Of NodeType As Class, EdgeType As IEdge(Of NodeType))

    Private _Nodes As List(Of NodeType)
    Private _Edges As List(Of EdgeType)

    Public Sub New()
        _Nodes = New List(Of NodeType)
        _Edges = New List(Of EdgeType)
    End Sub

    Public ReadOnly Property Nodes() As ObjectModel.ReadOnlyCollection(Of NodeType)
        Get
            Return _Nodes.AsReadOnly
        End Get
    End Property

    Public ReadOnly Property Edges() As ObjectModel.ReadOnlyCollection(Of EdgeType)
        Get
            Return _Edges.AsReadOnly
        End Get
    End Property

    Public Sub AddNode(node As NodeType)
        _Nodes.Add(node)
    End Sub

    Public Sub AddNodes(nodes As IEnumerable(Of NodeType))
        For Each node In nodes
            _Nodes.Add(node)
        Next
    End Sub

    Public Sub AddEdge(edge As EdgeType)
        If _Nodes.Contains(edge.EndNodes.Node1) AndAlso _
           _Nodes.Contains(edge.EndNodes.Node2) Then
            _Edges.Add(edge)
        Else
            Throw New ArgumentException("The start or end node of the edge to add was not found.")
        End If
    End Sub

    Public Sub RemoveEdge(edge As EdgeType)
        _Edges.Remove(edge)
    End Sub

    Public Sub RemoveEdgeAt(index As Integer)
        _Edges.RemoveAt(index)
    End Sub

    Public Function IsEndpoint(node As NodeType, edge As EdgeType) As Boolean
        Return node Is edge.EndNodes.Node1 OrElse _
               node Is edge.EndNodes.Node2
    End Function

    Public Sub RemoveNode(node As NodeType)
        For Each edge In _Edges
            If IsEndpoint(node, edge) Then
                _Edges.Remove(edge)
            End If
        Next
        _Nodes.Remove(node)
    End Sub

    Public Function Degree(node As NodeType) As Integer
        Degree = 0
        For Each edge In _Edges
            If IsEndpoint(node, edge) Then
                Degree += 1
            End If
        Next
        Return Degree
    End Function

End Class
