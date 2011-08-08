Public Class TermContext

    Private ReadOnly _Types As NamedTypes
    Public ReadOnly Property Types As NamedTypes
        Get
            Return _Types
        End Get
    End Property

    Private ReadOnly _Constants As IEnumerable(Of NamedConstant)
    Public ReadOnly Property Constants As IEnumerable(Of NamedConstant)
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

    Private ReadOnly _Functions As IEnumerable(Of NamedFunction)
    Public ReadOnly Property Functions As IEnumerable(Of NamedFunction)
        Get
            Return _Functions
        End Get
    End Property

    Public Sub New(constants As IEnumerable(Of NamedConstant),
                   parameters As IEnumerable(Of NamedParameter),
                   functions As IEnumerable(Of NamedFunction))
        Me.New(constants, parameters, functions, Types:=NamedTypes.DefaultTypes)
    End Sub

    Public Sub New(constants As IEnumerable(Of NamedConstant),
                    parameters As IEnumerable(Of NamedParameter),
                    functions As IEnumerable(Of NamedFunction),
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
            Return New TermContext(Constants:={New NamedConstant("Pi", NamedType.Real, System.Math.PI),
                                               New NamedConstant("E", NamedType.Real, System.Math.E)},
                                   Parameters:={},
                                   Functions:={New NamedFunction(New NamedDelegateType("Sqrt", NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), NamedFunction.GetSystemMathFunctionExpressionBuilder(name:="Sqrt")),
                                               New NamedFunction(New NamedDelegateType("Exp", NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), NamedFunction.GetSystemMathFunctionExpressionBuilder(name:="Exp")),
                                               New NamedFunction(New NamedDelegateType("Sin", NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), NamedFunction.GetSystemMathFunctionExpressionBuilder(name:="Sin")),
                                               New NamedFunction(New NamedDelegateType("Cos", NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), NamedFunction.GetSystemMathFunctionExpressionBuilder(name:="Cos")),
                                               New NamedFunction(New NamedDelegateType("Tan", NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), NamedFunction.GetSystemMathFunctionExpressionBuilder(name:="Tan")),
                                               New NamedFunction(New NamedDelegateType("Asin", NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), NamedFunction.GetSystemMathFunctionExpressionBuilder(name:="Asin")),
                                               New NamedFunction(New NamedDelegateType("Acos", NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), NamedFunction.GetSystemMathFunctionExpressionBuilder(name:="Acos"))})
        End Get
    End Property

    Public Function Merge(second As TermContext) As TermContext
        Return New TermContext(Constants:=_Constants.Concat(second._Constants),
                               Functions:=_Functions.Concat(second._Functions),
                               Parameters:=_Parameters.Concat(second._Parameters))
    End Function

End Class
