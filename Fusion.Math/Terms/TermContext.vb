Public Class TermContext

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
                    functions As IEnumerable(Of NamedFUnctionExpression))
        _Constants = constants
        _Parameters = parameters
        _Functions = functions
    End Sub

    Public Shared ReadOnly Property DefaultContext As TermContext
        Get
            Return New TermContext(Constants:={New NamedConstantExpression("Pi", System.Math.PI),
                                               New NamedConstantExpression("E", System.Math.E)},
                                   Parameters:={},
                                   Functions:={New NamedFUnctionExpression("Sqrt", NamedFUnctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Sqrt")),
                                               New NamedFUnctionExpression("Exp", NamedFUnctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Exp")),
                                               New NamedFUnctionExpression("Sin", NamedFUnctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Sin")),
                                               New NamedFUnctionExpression("Cos", NamedFUnctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Cos")),
                                               New NamedFUnctionExpression("Tan", NamedFUnctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Tan")),
                                               New NamedFUnctionExpression("Asin", NamedFUnctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Asin")),
                                               New NamedFUnctionExpression("Acos", NamedFUnctionExpression.GetSystemMathFunctionExpressionBuilder(name:="Acos"))})
        End Get
    End Property

    Public Function Merge(second As TermContext) As TermContext
        Return New TermContext(Constants:=_Constants.Concat(second._Constants),
                               Functions:=_Functions.Concat(second._Functions),
                               Parameters:=_Parameters.Concat(second._Parameters))
    End Function

End Class
