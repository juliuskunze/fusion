Public Class NamedFunctionDefinition
    Inherits DefinitionBase

    Public Sub New(definition As String, userContext As TermContext)
        MyBase.New(WithoutBlanks(definition), userContext)
    End Sub

    Public Function GetNamedFunctionExpression() As NamedFunctionExpression
        Dim parts = _DefinitionWithoutBlanks.Split("="c)
        If parts.Count <> 2 Then Throw New ArgumentException("Function definition invalid.")

        Dim functionAndParameterNamesString = parts.First

        Dim functionNameDefinition = TryGetFunctionAndArgumentNames(functionAndParameterNamesString:=functionAndParameterNamesString)
        Dim parameters = functionNameDefinition.ParameterNames.Select(Function(name) Expression.Parameter(type:=GetType(Double), name:=name)).ToArray

        Dim term = New Term(term:=parts.Last, userContext:=_UserContext.Merge(New TermContext(constants:={}, parameters:=parameters, Functions:={})))
        Dim lambdaFunction = Expression.Lambda(body:=term.GetExpression, parameters:=parameters)
        
        Return New NamedFunctionExpression(name:=functionNameDefinition.FunctionName, expressionBuilder:=Function(arguments) Expression.Invoke(lambdaFunction, arguments:=arguments))
    End Function

    Private Function TryGetFunctionAndArgumentNames(functionAndParameterNamesString As String) As FunctionNameDefinition
        For functionNameLength = functionAndParameterNamesString.Length - 1 To 0 Step -1
            Dim functionName = functionAndParameterNamesString.Substring(0, functionNameLength)

            If Not functionName.IsValidVariableName Then Continue For

            Dim parameterString = functionAndParameterNamesString.Substring(startIndex:=functionNameLength, length:=functionAndParameterNamesString.Length - functionNameLength)
            If Not parameterString.IsInBrackets Then Throw New ArgumentException("Invalid definition.")

            Dim parameterStringWithoutBracktes = parameterString.Substring(1, parameterString.Length - 2)
            Dim parameterNames = parameterStringWithoutBracktes.Split(","c)

            Return New FunctionNameDefinition(functionName:=functionName, parameterNames:=parameterNames)
        Next

        Throw New ArgumentException("Function definition invalid.")
    End Function

    Private Class FunctionNameDefinition

        Private ReadOnly _FunctionName As String
        Public ReadOnly Property FunctionName As String
            Get
                Return _FunctionName
            End Get
        End Property

        Private ReadOnly _ParameterNames As IEnumerable(Of String)
        Public ReadOnly Property ParameterNames As IEnumerable(Of String)
            Get
                Return _ParameterNames
            End Get
        End Property

        Public Sub New(functionName As String, parameterNames As IEnumerable(Of String))
            _FunctionName = functionName
            _ParameterNames = parameterNames
        End Sub

    End Class

End Class
