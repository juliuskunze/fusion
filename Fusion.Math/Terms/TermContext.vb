Public Class TermContext

    Private ReadOnly _Types As NamedTypes
    Public ReadOnly Property Types As NamedTypes
        Get
            Return _Types
        End Get
    End Property

    Private ReadOnly _Constants As IEnumerable(Of NamedConstantExpression)
    Public ReadOnly Property Constants As IEnumerable(Of NamedConstantExpression)
        Get
            Return _Constants
        End Get
    End Property

    Private ReadOnly _Parameters As IEnumerable(Of NamedParameter)
    Public ReadOnly Property Parameters As IEnumerable(Of NamedParameter)
        Get
            Return _Parameters
        End Get
    End Property

    Private ReadOnly _Functions As IEnumerable(Of NamedFUnctionExpression)
    Public ReadOnly Property Functions As IEnumerable(Of NamedFUnctionExpression)
        Get
            Return _Functions
        End Get
    End Property

    Public Sub New(constants As IEnumerable(Of NamedConstantExpression),
                   parameters As IEnumerable(Of NamedParameter),
                   functions As IEnumerable(Of NamedFunctionExpression))
        Me.New(constants, parameters, functions, Types:=NamedTypes.DefaultTypes)
    End Sub

    Public Sub New(constants As IEnumerable(Of NamedConstantExpression),
                    parameters As IEnumerable(Of NamedParameter),
                    functions As IEnumerable(Of NamedFunctionExpression),
                    types As NamedTypes)
        _Constants = constants
        _Parameters = parameters
        _Functions = functions
        _Types = types
    End Sub

    Public Shared ReadOnly Property Empty As TermContext
        Get
            Return New TermContext(Constants:={}, Parameters:={}, Functions:={}, Types:=New NamedTypes({NamedType.Real}))
        End Get
    End Property

    Public Shared ReadOnly Property DefaultContext As TermContext
        Get
            Return New TermContext(Constants:={New NamedConstantExpression("Pi", NamedType.Real, System.Math.PI),
                                               New NamedConstantExpression("E", NamedType.Real, System.Math.E)},
                                   Parameters:={},
                                   Functions:={New NamedFunctionExpression(New NamedDelegateType("Sqrt", NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), NamedFunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Sqrt")),
                                               New NamedFunctionExpression(New NamedDelegateType("Exp", NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), NamedFunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Exp")),
                                               New NamedFunctionExpression(New NamedDelegateType("Sin", NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), NamedFunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Sin")),
                                               New NamedFunctionExpression(New NamedDelegateType("Cos", NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), NamedFunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Cos")),
                                               New NamedFunctionExpression(New NamedDelegateType("Tan", NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), NamedFunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Tan")),
                                               New NamedFunctionExpression(New NamedDelegateType("Asin", NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), NamedFunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Asin")),
                                               New NamedFunctionExpression(New NamedDelegateType("Acos", NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), NamedFunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Acos"))})
        End Get
    End Property

    Public Function Merge(second As TermContext) As TermContext
        Return New TermContext(Constants:=_Constants.Concat(second._Constants),
                               Functions:=_Functions.Concat(second._Functions),
                               Parameters:=_Parameters.Concat(second._Parameters))
    End Function

End Class
