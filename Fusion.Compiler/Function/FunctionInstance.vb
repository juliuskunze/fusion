Public Class FunctionInstance

    Private ReadOnly _Signature As FunctionSignature
    Public ReadOnly Property Signature As FunctionSignature
        Get
            Return _Signature
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

    Public Sub New(signature As FunctionSignature, invokableExpression As Expression)
        _Signature = signature
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
        Me.New(Signature:=New FunctionSignature(name:=name, DelegateType:=GetDelegateType(lambdaExpression, typeNamedTypeDictionary)), lambdaExpression:=lambdaExpression)
    End Sub

    Public Sub New(signature As FunctionSignature,
                   lambdaExpression As Expressions.Expression(Of TDelegate))
        MyBase.New(signature:=signature, InvokableExpression:=lambdaExpression)
    End Sub

    Private Shared Function GetDelegateType( lambdaExpression As Expression(Of TDelegate),  typeNamedTypeDictionary As TypeNamedTypeDictionary) As DelegateType
        Dim namedResultType = typeNamedTypeDictionary.GetNamedType(lambdaExpression.ReturnType)

        Dim namedParameters = lambdaExpression.Parameters.Select(Function(parameterExpression)
                                                                     Dim namedType = typeNamedTypeDictionary.GetNamedType(parameterExpression.Type)

                                                                     Return New NamedParameter(Name:=parameterExpression.Name, Type:=namedType)
                                                                 End Function)

        Return New DelegateType(resultType:=namedResultType, parameters:=namedParameters)
    End Function

End Class