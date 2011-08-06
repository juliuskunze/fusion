Public Class Vector3DTerm
    Inherits TermBase(Of Vector3D)

    Public Sub New(term As String, userContext As TermContext)
        MyBase.New(termWithoutBlanks:=term.WithoutBlanks,
                   context:=userContext)
    End Sub

    Public Overrides Function GetExpression() As System.Linq.Expressions.Expression
        Dim baseExpression = MyBase.TryGetConstantOrParameterExpression
        If baseExpression IsNot Nothing Then Return baseExpression

        Dim componentString = _TermWithoutBlanks

        If _TermWithoutBlanks.First = "("c AndAlso _TermWithoutBlanks.Last = ")"c OrElse
           _TermWithoutBlanks.First = "["c AndAlso _TermWithoutBlanks.Last = "]"c OrElse
           _TermWithoutBlanks.First = "{"c AndAlso _TermWithoutBlanks.Last = "}"c OrElse
           _TermWithoutBlanks.First = "<"c AndAlso _TermWithoutBlanks.Last = ">"c Then
            componentString = _TermWithoutBlanks.Remove(_TermWithoutBlanks.Length - 1, 1).Remove(0, 1)
        End If

        Dim components = componentString.Split(";"c, "|"c, ","c)

        If components.Count <> 3 Then Throw New InvalidTermException(_TermWithoutBlanks, "The component count of a 3D-vector must be 3.")

        Dim xExpression = New Term(Term:=components(0), userContext:=_Context).GetExpression
        Dim yExpression = New Term(Term:=components(1), userContext:=_Context).GetExpression
        Dim zExpression = New Term(Term:=components(2), userContext:=_Context).GetExpression

        If xExpression.Type <> GetType(Double) OrElse
           yExpression.Type <> GetType(Double) OrElse
           zExpression.Type <> GetType(Double) Then Throw New InvalidTermException(_TermWithoutBlanks, message:="The components of a vector must be real numbers.")

        Return NamedConstantExpression.GetFunctionExpressionBuilder(Of Vector3DConstructor)(userFunction:=Function(x, y, z) New Vector3D(x, y, z)).Invoke(arguments:={xExpression, yExpression, zExpression})
    End Function

    Private Delegate Function Vector3DConstructor(x As Double, y As Double, z As Double) As Vector3D

End Class
