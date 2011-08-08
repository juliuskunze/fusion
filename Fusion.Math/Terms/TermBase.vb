Public MustInherit Class TermBase

    Protected ReadOnly _Term As String
    Protected ReadOnly _TrimmedTerm As String
    Protected ReadOnly _Context As TermContext
    Protected ReadOnly _Type As NamedType

    Public Sub New(term As String, context As TermContext, type As NamedType)
        _Term = term
        _TrimmedTerm = term.Trim
        _Context = context
        _Type = Type
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

    Public MustOverride Function GetExpression() As Expression

    Public Function GetDelegate() As System.Delegate
        Return Expression.Lambda(body:=Me.GetExpression, parameters:=_Context.Parameters.Select(Function(p) p.ParameterExpression)).Compile
    End Function

    Public Function GetDelegate(Of TDelegate)() As TDelegate
        Dim lambda = Expression.Lambda(Of TDelegate)(body:=Me.GetExpression, parameters:=_Context.Parameters.Select(Function(p) p.ParameterExpression))

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

    Protected Function TryGetParameterExpression() As Expression
        Dim matchingParameters = From parameter In _Context.Parameters Where String.Equals(_TrimmedTerm, parameter.Name, StringComparison.OrdinalIgnoreCase)
        If Not matchingParameters.Any Then Return Nothing

        Dim matchingParameter = matchingParameters.Single
        Me.CheckTypeMatch(type:=matchingParameter.Type)

        Return matchingParameter.ParameterExpression
    End Function

    Protected Function TryGetConstantExpression() As Expression
        Dim matchingConstants = From constant In _Context.Constants Where String.Equals(_TrimmedTerm, constant.Name, StringComparison.OrdinalIgnoreCase)
        If Not matchingConstants.Any Then Return Nothing

        Dim matchingConstant = matchingConstants.Single
        Me.CheckTypeMatch(type:=matchingConstant.Type)

        Return matchingConstant.ConstantExpression
    End Function

    Protected Function TryGetFunctionCall() As FunctionCall
        Try
            Return New FunctionCall(functionCallText:=_Term)
        Catch ex As ArgumentException
            Return Nothing
        End Try
    End Function

    Protected Sub CheckTypeMatch(type As NamedType)
        If Not _Type.SystemType.IsAssignableFrom(type.SystemType) Then Throw New InvalidTermException(Term:=_Term, message:="Type '" & type.Name & "' is not compatible to type '" & _Type.Name & "'.")
    End Sub

End Class
