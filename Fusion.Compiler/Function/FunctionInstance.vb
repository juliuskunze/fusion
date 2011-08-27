Public Class FunctionInstance

    Private ReadOnly _Name As String
    Public ReadOnly Property Name As String
        Get
            Return _Name
        End Get
    End Property

    Private ReadOnly _DelegateType As DelegateType
    Public ReadOnly Property DelegateType As DelegateType
        Get
            Return _DelegateType
        End Get
    End Property

    Private ReadOnly _CallExpressionBuilder As FunctionCallExpressionBuilder
    Public ReadOnly Property CallExpressionBuilder As FunctionCallExpressionBuilder
        Get
            Return _CallExpressionBuilder
        End Get
    End Property

    Public ReadOnly Property CallExpression(arguments As IEnumerable(Of Expression)) As Expression
        Get
            Return _CallExpressionBuilder.Run(arguments)
        End Get
    End Property

    Private ReadOnly _InvokableExpression As Expression
    Public ReadOnly Property InvokableExpression As Expression
        Get
            Return _InvokableExpression
        End Get
    End Property

    Public Sub New(name As String, delegateType As DelegateType, invokableExpression As Expression)
        _Name = name
        _DelegateType = delegateType
        _InvokableExpression = invokableExpression
        _CallExpressionBuilder = New FunctionCallExpressionBuilder(invokableExpression:=invokableExpression)
    End Sub

    Public Shared Function FromLambdaExpression(Of TDelegate)(name As String,
                                                              lambdaExpression As Expressions.Expression(Of TDelegate),
                                                              typeNamedTypeDictionary As TypeNamedTypeDictionary) As FunctionInstance
        Return New FunctionInstance(Of TDelegate)(name, lambdaExpression, typeNamedTypeDictionary)
    End Function

End Class

Public Class FunctionInstance(Of TDelegate)
    Inherits FunctionInstance

    Public Sub New(name As String,
                   lambdaExpression As Expressions.Expression(Of TDelegate),
                   typeNamedTypeDictionary As TypeNamedTypeDictionary)
        Me.New(name:=name, Type:=GetDelegateType(lambdaExpression, typeNamedTypeDictionary), lambdaExpression:=lambdaExpression)
    End Sub

    Public Sub New(name As String,
                   type As DelegateType,
                   lambdaExpression As Expressions.Expression(Of TDelegate))
        MyBase.New(name:=name, DelegateType:=type, InvokableExpression:=lambdaExpression)
    End Sub

    Private Shared Function GetDelegateType(ByVal lambdaExpression As Expression(Of TDelegate), ByVal typeNamedTypeDictionary As TypeNamedTypeDictionary) As DelegateType
        Dim namedResultType = typeNamedTypeDictionary.GetNamedType(lambdaExpression.ReturnType)

        Dim namedParameters = lambdaExpression.Parameters.Select(Function(parameterExpression)
                                                                     Dim namedType = typeNamedTypeDictionary.GetNamedType(parameterExpression.Type)

                                                                     Return New NamedParameter(Name:=parameterExpression.Name, Type:=namedType)
                                                                 End Function)

        Return New DelegateType(resultType:=namedResultType, parameters:=namedParameters)
    End Function

End Class