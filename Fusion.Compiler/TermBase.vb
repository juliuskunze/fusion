Public MustInherit Class TermBase

    Protected ReadOnly _Term As String
    Protected ReadOnly _TrimmedTerm As String
    Protected ReadOnly _Context As TermContext
    Protected ReadOnly _Type As NamedType

    Public Sub New(term As String, context As TermContext, type As NamedType)
        If Not context.Types.Contains(NamedType.Real) Then Throw New InvalidOperationException("Type Real must be defined in this context.")
        If Not context.Types.Contains(NamedType.Vector3D) Then Throw New InvalidOperationException("Type Vector3D must be defined in this context.")

        _Term = term
        _TrimmedTerm = term.Trim
        _Context = context
        _Type = type
    End Sub

    Public Function TryGetConstantOrParameterExpression() As Expression
        If _TrimmedTerm = "" Then Throw New InvalidTermException(_Term, message:="Term must not be empty.")
        If Not IsValidVariableName(_TrimmedTerm) Then Return Nothing

        Dim constantExpression = Me.TryGetConstantExpression()
        If constantExpression IsNot Nothing Then Return constantExpression

        Dim parameterExpression = Me.TryGetParameterExpression()
        If parameterExpression IsNot Nothing Then Return parameterExpression

        Return Nothing
    End Function

    Private Function TryGetConstantExpression() As Expression
        Dim matchingConstant = _Context.TryParseConstant(_TrimmedTerm)
        If matchingConstant Is Nothing Then Return Nothing

        Me.CheckTypeMatch(type:=matchingConstant.Signature.Type)

        Return matchingConstant.Expression
    End Function

    Private Function TryGetParameterExpression() As Expression
        Dim matchingParameter = _Context.TryParseParameter(_TrimmedTerm)
        If matchingParameter Is Nothing Then Return Nothing

        Me.CheckTypeMatch(type:=matchingParameter.Type)

        Return matchingParameter.Expression
    End Function

    Public MustOverride Function GetExpression() As Expression

    Public Function GetDelegate() As System.Delegate
        Return Expression.Lambda(body:=Me.GetExpression, parameters:=_Context.Parameters.Select(Function(p) p.Expression)).Compile
    End Function

    Public Function GetDelegate(Of TDelegate)() As TDelegate
        Dim lambda = Expression.Lambda(Of TDelegate)(body:=Me.GetExpression, parameters:=_Context.Parameters.Select(Function(p) p.Expression))

        Return lambda.Compile
    End Function

    Public Function TryGetDelegate() As System.Delegate
        Try
            Return Me.GetDelegate
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function TryGetDelegate(Of TDelegate)() As TDelegate
        Try
            Return Me.GetDelegate(Of TDelegate)()
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Protected Function TryGetFunctionCall() As FunctionCall
        Try
            Return New FunctionCall(text:=_Term)
        Catch ex As ArgumentException
            Return Nothing
        End Try
    End Function

    Protected Sub CheckTypeMatch(type As NamedType)
        If Not _Type.SystemType.IsAssignableFrom(type.SystemType) Then Throw New InvalidTermException(Term:=_Term, message:="Type '" & type.Name & "' is not compatible to type '" & _Type.Name & "'.")
    End Sub

    Private Sub CheckDelegateTypeMatch(delegateType As DelegateType)
        _Type.Delegate.CheckIsAssignableFrom(delegateType)
    End Sub

End Class
