Public Class NamedExpression

    Private ReadOnly _Name As String
    Public ReadOnly Property Name As String
        Get
            Return _Name
        End Get
    End Property

    Private ReadOnly _Expression As Expression
    Public ReadOnly Property Expression As Expression
        Get
            Return _Expression
        End Get
    End Property

    Public Sub New(ByVal name As String, ByVal expression As Expression)
        _Name = name
        _Expression = expression
    End Sub

    Public Shared Function GetSystemMathMethodCallExpression(ByVal name As String) As Expression
        Return Expressions.Expression.Call(method:=GetType(System.Math).GetMethod(name:=name))
    End Function

End Class