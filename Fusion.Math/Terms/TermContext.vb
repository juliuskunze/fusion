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
    
    Private ReadOnly _Functions As IEnumerable(Of NamedExpression)
    Public ReadOnly Property Functions As IEnumerable(Of NamedExpression)
        Get
            Return _Functions
        End Get
    End Property

    Public Sub New(ByVal constants As IEnumerable(Of NamedConstantExpression),
                   ByVal parameters As IEnumerable(Of ParameterExpression),
                   ByVal functions As IEnumerable(Of NamedExpression))
        _Constants = constants
        _Parameters = parameters
        _Functions = functions
    End Sub

    Public Shared ReadOnly Property DefaultContext As TermContext
        Get
            Return New TermContext(Constants:={New NamedConstantExpression("Pi", System.Math.PI),
                                               New NamedConstantExpression("E", System.Math.E)},
                                   Parameters:={},
                                   Functions:={New NamedExpression("Sqrt", NamedExpression.GetSystemMathFunctionExpressionBuilder(name:="Sqrt")),
                                               New NamedExpression("Exp", NamedExpression.GetSystemMathFunctionExpressionBuilder(name:="Exp")),
                                               New NamedExpression("Sin", NamedExpression.GetSystemMathFunctionExpressionBuilder(name:="Sin")),
                                               New NamedExpression("Cos", NamedExpression.GetSystemMathFunctionExpressionBuilder(name:="Cos")),
                                               New NamedExpression("Tan", NamedExpression.GetSystemMathFunctionExpressionBuilder(name:="Tan")),
                                               New NamedExpression("Asin", NamedExpression.GetSystemMathFunctionExpressionBuilder(name:="Asin")),
                                               New NamedExpression("Acos", NamedExpression.GetSystemMathFunctionExpressionBuilder(name:="Acos"))})
        End Get
    End Property

    Public Function Merge(ByVal second As TermContext) As TermContext
        Return New TermContext(Constants:=_Constants.Concat(second._Constants),
                               Functions:=_Functions.Concat(second._Functions),
                               Parameters:=_Parameters.Concat(second._Parameters))
    End Function

End Class
