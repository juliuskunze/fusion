Public Class TermContext

    Private ReadOnly _Types As NamedTypes
    Public ReadOnly Property Types As NamedTypes
        Get
            Return _Types
        End Get
    End Property

    Private ReadOnly _Constants As IEnumerable(Of ConstantInstance)
    Public ReadOnly Property Constants As IEnumerable(Of ConstantInstance)
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

    Private ReadOnly _Functions As IEnumerable(Of FunctionInstance)
    Public ReadOnly Property Functions As IEnumerable(Of FunctionInstance)
        Get
            Return _Functions
        End Get
    End Property

    Public Sub New(Optional constants As IEnumerable(Of ConstantInstance) = Nothing,
                   Optional parameters As IEnumerable(Of NamedParameter) = Nothing,
                   Optional functions As IEnumerable(Of FunctionInstance) = Nothing,
                   Optional types As NamedTypes = Nothing)
        _Constants = If(constants Is Nothing, Enumerable.Empty(Of ConstantInstance), constants)
        _Parameters = If(parameters Is Nothing, Enumerable.Empty(Of NamedParameter), parameters)
        _Functions = If(functions Is Nothing, Enumerable.Empty(Of FunctionInstance), functions)
        _Types = If(types Is Nothing, NamedTypes.Empty, types)
    End Sub

    Public Shared ReadOnly Property [Default] As TermContext
        Get
            Return New TermContext(Constants:={New ConstantInstance(New ConstantSignature("Pi", NamedType.Real), System.Math.PI),
                                               New ConstantInstance(New ConstantSignature("E", NamedType.Real), System.Math.E)},
                                   Functions:={New FunctionInstance("Sqrt", New DelegateType(NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), FunctionInstance.GetSystemMathFunctionExpressionBuilder(name:="Sqrt")),
                                               New FunctionInstance("Exp", New DelegateType(NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), FunctionInstance.GetSystemMathFunctionExpressionBuilder(name:="Exp")),
                                               New FunctionInstance("Sin", New DelegateType(NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), FunctionInstance.GetSystemMathFunctionExpressionBuilder(name:="Sin")),
                                               New FunctionInstance("Cos", New DelegateType(NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), FunctionInstance.GetSystemMathFunctionExpressionBuilder(name:="Cos")),
                                               New FunctionInstance("Tan", New DelegateType(NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), FunctionInstance.GetSystemMathFunctionExpressionBuilder(name:="Tan")),
                                               New FunctionInstance("Asin", New DelegateType(NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), FunctionInstance.GetSystemMathFunctionExpressionBuilder(name:="Asin")),
                                               New FunctionInstance("Acos", New DelegateType(NamedType.Real, {New NamedParameter(name:="x", Type:=NamedType.Real)}), FunctionInstance.GetSystemMathFunctionExpressionBuilder(name:="Acos"))},
                                   Types:=NamedTypes.Default)
        End Get
    End Property

    Public Function Merge(second As TermContext) As TermContext
        Return New TermContext(Constants:=_Constants.Concat(second._Constants),
                               Functions:=_Functions.Concat(second._Functions),
                               Parameters:=_Parameters.Concat(second._Parameters),
                               Types:=_Types.Merge(second.Types))
    End Function

End Class
