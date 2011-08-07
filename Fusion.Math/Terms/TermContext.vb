Public Class TermContext

    Private ReadOnly _Types As IEnumerable(Of NamedType)
    Public ReadOnly Property Types As IEnumerable(Of NamedType)
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

    Private ReadOnly _Parameters As IEnumerable(Of ParameterExpression)
    Public ReadOnly Property Parameters As IEnumerable(Of ParameterExpression)
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
                parameters As IEnumerable(Of ParameterExpression),
                functions As IEnumerable(Of NamedFunctionExpression))
        Me.New(constants, parameters, functions, Types:=NamedType.DefaultTypes)
    End Sub

    Public Sub New(constants As IEnumerable(Of NamedConstantExpression),
                    parameters As IEnumerable(Of ParameterExpression),
                    functions As IEnumerable(Of NamedFunctionExpression),
                    types As IEnumerable(Of NamedType))
        _Constants = constants
        _Parameters = parameters
        _Functions = functions
        _Types = types
    End Sub

    Public Shared ReadOnly Property Empty As TermContext
        Get
            Return New TermContext(Constants:={}, Parameters:={}, Functions:={}, Types:={NamedType.Real})
        End Get
    End Property

    Public Shared ReadOnly Property DefaultContext As TermContext
        Get
            Return New TermContext(Constants:={New NamedConstantExpression("Pi", NamedType.Real, System.Math.PI),
                                               New NamedConstantExpression("E", NamedType.Real, System.Math.E)},
                                   Parameters:={},
                                   Functions:={New NamedFunctionExpression("Sqrt", NamedType.Real, NamedFunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Sqrt")),
                                               New NamedFunctionExpression("Exp", NamedType.Real, NamedFunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Exp")),
                                               New NamedFunctionExpression("Sin", NamedType.Real, NamedFunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Sin")),
                                               New NamedFunctionExpression("Cos", NamedType.Real, NamedFunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Cos")),
                                               New NamedFunctionExpression("Tan", NamedType.Real, NamedFunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Tan")),
                                               New NamedFunctionExpression("Asin", NamedType.Real, NamedFunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Asin")),
                                               New NamedFunctionExpression("Acos", NamedType.Real, NamedFunctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Acos"))})
        End Get
    End Property

    Public Function Merge(second As TermContext) As TermContext
        Return New TermContext(Constants:=_Constants.Concat(second._Constants),
                               Functions:=_Functions.Concat(second._Functions),
                               Parameters:=_Parameters.Concat(second._Parameters))
    End Function

End Class
