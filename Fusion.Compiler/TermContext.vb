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


    Private Shared ReadOnly _DefaultTypeNamedTypeDictionary As New TypeNamedTypeDictionary(NamedTypes.Default)

    Private Shared ReadOnly _Default As New TermContext(Constants:={New ConstantInstance(Of Double)("Pi", System.Math.PI, _DefaultTypeNamedTypeDictionary),
                                                                    New ConstantInstance(Of Double)("E", System.Math.E, _DefaultTypeNamedTypeDictionary)
                                                                   },
                                                        Functions:={New FunctionInstance(Of Func(Of Double, Double))("Sqrt", Function(x) System.Math.Sqrt(x), _DefaultTypeNamedTypeDictionary),
                                                                    New FunctionInstance(Of Func(Of Double, Double))("Exp", Function(x) System.Math.Exp(x), _DefaultTypeNamedTypeDictionary),
                                                                    New FunctionInstance(Of Func(Of Double, Double))("Sin", Function(x) System.Math.Sin(x), _DefaultTypeNamedTypeDictionary),
                                                                    New FunctionInstance(Of Func(Of Double, Double))("Cos", Function(x) System.Math.Cos(x), _DefaultTypeNamedTypeDictionary),
                                                                    New FunctionInstance(Of Func(Of Double, Double))("Tan", Function(x) System.Math.Tan(x), _DefaultTypeNamedTypeDictionary),
                                                                    New FunctionInstance(Of Func(Of Double, Double))("Asin", Function(x) System.Math.Asin(x), _DefaultTypeNamedTypeDictionary),
                                                                    New FunctionInstance(Of Func(Of Double, Double))("Acos", Function(x) System.Math.Acos(x), _DefaultTypeNamedTypeDictionary),
                                                                    New FunctionInstance(Of Func(Of Double, Double))("Abs", Function(x) System.Math.Abs(x), _DefaultTypeNamedTypeDictionary),
                                                                    New FunctionInstance(Of Func(Of Double, Double, Double))("Max", Function(a, b) System.Math.Max(a, b), _DefaultTypeNamedTypeDictionary),
                                                                    New FunctionInstance(Of Func(Of Double, Double, Double))("Min", Function(a, b) System.Math.Min(a, b), _DefaultTypeNamedTypeDictionary)
                                              },
                                   Types:=NamedTypes.Default)

    Public Shared ReadOnly Property [Default] As TermContext
        Get
            Return _Default
        End Get
    End Property

    Public Function Merge(second As TermContext) As TermContext
        Return New TermContext(Constants:=_Constants.Concat(second._Constants),
                               Functions:=_Functions.Concat(second._Functions),
                               Parameters:=_Parameters.Concat(second._Parameters),
                               Types:=_Types.Merge(second.Types))
    End Function

    Public Function ParseFunction(ByVal name As String) As FunctionInstance
        Dim matchingParameters =
                From parameter In Me.Parameters
                Where parameter.Type.IsDelegate AndAlso name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase)
                Select parameter.ToFunctionInstance

        Dim matchingFunctions =
                From functionInstance In Me.Functions
                Where name.Equals(functionInstance.Name, StringComparison.OrdinalIgnoreCase)

        Dim matchingFunctionsAndParameters = matchingFunctions.Concat(matchingParameters)

        If Not matchingFunctionsAndParameters.Any Then Throw New ArgumentException("Function '" & name & "' is not defined in this context.")

        Return matchingFunctionsAndParameters.Single
    End Function

    Public Function TryParseConstant(name As String) As ConstantInstance
        Dim matchingConstants = From constant In Me.Constants Where String.Equals(name, constant.Signature.Name, StringComparison.OrdinalIgnoreCase)
        If Not matchingConstants.Any Then Return Nothing

        Return matchingConstants.Single
    End Function

    Public Function TryParseParameter(name As String) As NamedParameter
        Dim matchingParameters = From parameter In Me.Parameters Where String.Equals(name, parameter.Name, StringComparison.OrdinalIgnoreCase)
        If Not matchingParameters.Any Then Return Nothing

        Return matchingParameters.Single
    End Function

End Class
