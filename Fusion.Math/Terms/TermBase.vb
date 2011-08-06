Public MustInherit Class TermBase(Of TResult)

    Protected ReadOnly _TermWithoutBlanks As String
    Protected ReadOnly _Context As TermContext

    Public Sub New(termWithoutBlanks As String, context As TermContext)
        _TermWithoutBlanks = termWithoutBlanks
        _Context = context
    End Sub

    Public Function TryGetConstantOrParameterExpression() As Expression
        If _TermWithoutBlanks = "" Then Throw New InvalidTermException(_TermWithoutBlanks, message:="Term must not be empty.")
        If Not IsValidVariableName(_TermWithoutBlanks) Then Return Nothing

        Dim constantExpression = Me.TryGetConstantExpression()
        If constantExpression IsNot Nothing Then Return constantExpression

        Dim parameterExpression = Me.TryGetParameterExpression()
        If parameterExpression IsNot Nothing Then Return parameterExpression

        Return Nothing
    End Function

    Public MustOverride Function GetExpression() As Expression

    Public Function GetDelegate() As System.Delegate
        Return Expression.Lambda(body:=Me.GetExpression, parameters:=_Context.Parameters).Compile
    End Function

    Public Function GetDelegate(Of TDelegate)() As TDelegate
        Dim lambda = Expression.Lambda(Of TDelegate)(body:=Me.GetExpression, parameters:=_Context.Parameters)

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
        Dim parameters = From parameter In _Context.Parameters Where String.Equals(_TermWithoutBlanks, parameter.Name, StringComparison.OrdinalIgnoreCase)
        If Not parameters.Any Then Return Nothing

        Return parameters.Single
    End Function

    Protected Function TryGetConstantExpression() As Expression
        Dim constants = From constant In _Context.Constants Where String.Equals(_TermWithoutBlanks, constant.Name, StringComparison.OrdinalIgnoreCase)
        If Not constants.Any Then Return Nothing

        Return constants.Single.ConstantExpression
    End Function

    Protected Function TryGetFunctionExpression() As NamedFunctionExpression
        Dim functions = From functionExpression In _Context.Functions Where _TermWithoutBlanks.StartsWith(functionExpression.Name, StringComparison.OrdinalIgnoreCase)
        If Not functions.Any Then Return Nothing

        Return functions.Single
    End Function

    Public ReadOnly Property ResultType As System.Type
        Get
            Return GetType(TResult)
        End Get
    End Property

End Class
