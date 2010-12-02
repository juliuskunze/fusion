<Serializable()>
Public Class Graph(Of NodeType As Class, EdgeType As IEdge(Of NodeType))

    Private _nodes As List(Of NodeType)
    Private _edges As List(Of EdgeType)

    Public Sub New()
        _nodes = New List(Of NodeType)
        _edges = New List(Of EdgeType)
    End Sub

    Public ReadOnly Property Nodes() As ObjectModel.ReadOnlyCollection(Of NodeType)
        Get
            Return _nodes.AsReadOnly
        End Get
    End Property

    Public ReadOnly Property Edges() As ObjectModel.ReadOnlyCollection(Of EdgeType)
        Get
            Return _edges.AsReadOnly
        End Get
    End Property

    Public Sub AddNode(ByVal node As NodeType)
        Me._nodes.Add(node)
    End Sub

    Public Sub AddNodes(ByVal nodes As IEnumerable(Of NodeType))
        For Each node In nodes
            Me._nodes.Add(node)
        Next
    End Sub

    Public Sub AddEdge(ByVal edge As EdgeType)
        If _nodes.Contains(edge.EndNodes.Node1) AndAlso _
           _nodes.Contains(edge.EndNodes.Node2) Then
            _edges.Add(edge)
        Else
            Throw New ArgumentException("The start or end node of the edge to add was not found.")
        End If
    End Sub

    Public Sub RemoveEdge(ByVal edge As EdgeType)
        _edges.Remove(edge)
    End Sub

    Public Sub RemoveEdgeAt(ByVal index As Integer)
        _edges.RemoveAt(index)
    End Sub

    Public Function IsEndpoint(ByVal node As NodeType, ByVal edge As EdgeType) As Boolean
        Return node Is edge.EndNodes.Node1 OrElse _
               node Is edge.EndNodes.Node2
    End Function

    Public Sub RemoveNode(ByVal node As NodeType)
        For Each edge In _edges
            If IsEndpoint(node, edge) Then
                _edges.Remove(edge)
            End If
        Next
        _nodes.Remove(node)
    End Sub

    Public Function Degree(ByVal node As NodeType) As Integer
        Degree = 0
        For Each edge In _edges
            If IsEndpoint(node, edge) Then
                Degree += 1
            End If
        Next
        Return Degree
    End Function

End Class
