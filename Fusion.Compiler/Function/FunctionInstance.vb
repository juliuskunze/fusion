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
                                                              lambdaExpression As Expression(Of TDelegate),
                                                              typeDictionary As TypeDictionary,
                                                              Optional description As String = Nothing) As FunctionInstance
        Return New FunctionInstance(Of TDelegate)(name,
                                                  lambdaExpression,
                                                  typeDictionary,
                                                  description)
    End Function
End Class

Public Class FunctionInstance(Of TDelegate)
    Inherits FunctionInstance

    Public Sub New(name As String,
                   lambdaExpression As Expression(Of TDelegate),
                   typeDictionary As TypeDictionary,
                   Optional description As String = Nothing)
        Me.New(Signature:=New FunctionSignature(name:=name, FunctionType:=GetFunctionType(lambdaExpression, typeDictionary), description:=description), lambdaExpression:=lambdaExpression)
    End Sub

    Public Sub New(signature As FunctionSignature,
                   lambdaExpression As Expression(Of TDelegate))
        MyBase.New(signature:=signature, InvokableExpression:=lambdaExpression)
    End Sub

    Private Shared Function GetFunctionType(lambdaExpression As Expression(Of TDelegate), typeDictionary As TypeDictionary) As FunctionType
        Dim namedResultType = typeDictionary.GetNamedType(lambdaExpression.ReturnType)

        Dim namedParameters = lambdaExpression.Parameters.Select(Function(parameterExpression)
                                                                     Dim namedType = typeDictionary.GetNamedType(parameterExpression.Type)

                                                                     Return New NamedParameter(Name:=parameterExpression.Name, Type:=namedType)
                                                                 End Function)

        Return New FunctionType(resultType:=namedResultType, parameters:=namedParameters)
    End Function
End Class