Public Class Vector3DTerm
    Inherits TermBase(Of Vector3D)

    Public Sub New(ByVal term As String, ByVal userContext As TermContext)
        Me.New(termWithoutBlanks:=New String((term.Where(Function(c) Not Char.IsWhiteSpace(c))).ToArray),
               context:=TermContext.DefaultContext.Merge(userContext), obsolete_signatureDifferParameter:=False)
    End Sub

    Private Sub New(ByVal termWithoutBlanks As String,
                    ByVal context As TermContext, ByVal obsolete_signatureDifferParameter As Boolean)
        MyBase.New(Term:=termWithoutBlanks, context:=context)
    End Sub

    Public Overrides Function GetExpression() As System.Linq.Expressions.Expression
        Dim baseExpression = MyBase.TryGetConstantOrParameterExpression
        If baseExpression IsNot Nothing Then Return baseExpression

        Dim componentString = _Term

        If _Term.First = "("c AndAlso _Term.Last = ")"c OrElse
           _Term.First = "["c AndAlso _Term.Last = "]"c OrElse
           _Term.First = "{"c AndAlso _Term.Last = "}"c OrElse
           _Term.First = "<"c AndAlso _Term.Last = ">"c Then
            componentString = _Term.Remove(_Term.Length - 1, 1).Remove(0, 1)
        End If

        Dim components = componentString.Split(";"c, "|"c, ","c)

        If components.Count <> 3 Then Throw New InvalidTermException(_Term, "The component count of a 3D-vector must be 3.")

        Dim xExpression = New Term(termWithoutBlanks:=components(0), context:=_Context, obsolete_signatureDifferParameter:=False).GetExpression
        Dim yExpression = New Term(termWithoutBlanks:=components(1), context:=_Context, obsolete_signatureDifferParameter:=False).GetExpression
        Dim zExpression = New Term(termWithoutBlanks:=components(2), context:=_Context, obsolete_signatureDifferParameter:=False).GetExpression

        If xExpression.Type <> GetType(Double) OrElse
           yExpression.Type <> GetType(Double) OrElse
           zExpression.Type <> GetType(Double) Then Throw New InvalidTermException(_Term, message:="The components of a vector must be real numbers.")

        Return NamedConstantExpression.GetFunctionExpressionBuilder(Of Vector3DConstructor)(userFunction:=Function(x, y, z) New Vector3D(x, y, z)).Invoke(arguments:={xExpression, yExpression, zExpression})
    End Function

    Private Delegate Function Vector3DConstructor(ByVal x As Double, ByVal y As Double, ByVal z As Double) As Vector3D

End Class
