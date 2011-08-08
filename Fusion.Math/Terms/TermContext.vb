Public Class TermContext

    Private ReadOnly _Types As NamedTypes
    Public ReadOnly Property Types As NamedTypes
        Get
            Return _Types
        End Get
    End Property

    Private ReadOnly _Constants As IEnumerable(Of ConstExpression)
    Public ReadOnly Property Constants As IEnumerable(Of ConstExpression)
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

    Private ReadOnly _Functions As IEnumerable(Of FunctionExpression)
    Public ReadOnly Property Functions As IEnumerable(Of FunctionExpression)
        Get
            Return _Functions
        End Get
    End Property

    Public Sub New(constants As IEnumerable(Of ConstExpression),
                   parameters As IEnumerable(Of NamedParameter),
                   functions As IEnumerable(Of FunctionExpression))
        Me.New(constants, parameters, functions, Types:=NamedTypes.DefaultTypes)
    End Sub

    Public Sub New(constants As IEnumerable(Of ConstExpression),
                    parameters As IEnumerable(Of NamedParameter),
                    functions As IEnumerable(Of FunctionExpression),
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
            Return New TermContext(Constants:={New ConstExpression(New ConstantDeclaration("Pi", NamedType.Real), System.Math.PI),
                                               New ConstExpression(New ConstantDeclaration("E", NamedType.Real), System.Math.E)},
                                   Parameters:={},
                                   Functions:={New FunctionExpression("Sqrt", New FunctionType(NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), FunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Sqrt")),
                                               New FunctionExpression("Exp", New FunctionType(NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), FunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Exp")),
                                               New FunctionExpression("Sin", New FunctionType(NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), FunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Sin")),
                                               New FunctionExpression("Cos", New FunctionType(NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), FunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Cos")),
                                               New FunctionExpression("Tan", New FunctionType(NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), FunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Tan")),
                                               New FunctionExpression("Asin", New FunctionType(NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), FunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Asin")),
                                               New FunctionExpression("Acos", New FunctionType(NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), FunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Acos"))})
        End Get
    End Property

    Public Function Merge(second As TermContext) As TermContext
        Return New TermContext(Constants:=_Constants.Concat(second._Constants),
                               Functions:=_Functions.Concat(second._Functions),
                               Parameters:=_Parameters.Concat(second._Parameters))
    End Function

End Class
