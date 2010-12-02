Public Class LinkedPointSets3D
    Inherits List(Of IPointSet3D)
    Implements IPointSet3D

    Public Property LinkOperator As Func(Of Boolean, Boolean, Boolean)

    Public Sub New(ByVal linkOperator As Func(Of Boolean, Boolean, Boolean))
        Me.LinkOperator = linkOperator
    End Sub

    Public Overloads Function Contains(ByVal point As Vector3D) As Boolean Implements IPointSet3D.Contains
        Dim result = True

        For Each pointSet In Me
            result = Me.LinkOperator.Invoke(result, pointSet.Contains(point))
        Next

        Return result
    End Function

End Class