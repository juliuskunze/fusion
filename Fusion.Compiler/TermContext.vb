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

    Private _GroupedFunctions As IEnumerable(Of IGrouping(Of String, FunctionInstance))
    Public ReadOnly Property GroupedFunctionsAndFunctionParameters As IEnumerable(Of IGrouping(Of String, FunctionInstance))
        Get
            If _GroupedFunctions Is Nothing Then
                Dim parameterFunctions =
                        From x In Me.Parameters
                        Where x.Signature.Type.IsFunctionType
                        Select x.ToFunctionInstance
                Dim constantFunctions =
                        From x In Me.Constants
                        Where x.Signature.Type.IsFunctionType
                        Select x.ToFunctionInstance

                _GroupedFunctions = _Functions.Concat(parameterFunctions).Concat(constantFunctions).GroupBy(Function(instance) instance.Signature.Name, comparer:=StringComparer.OrdinalIgnoreCase).ToArray
            End If

            Return _GroupedFunctions
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

    Private Shared ReadOnly _Default As New TermContext(Constants:={New ConstantInstance(Of Boolean)("true", True, TypeDictionary.Default, "A Boolean value that passes a conditional test."),
                                                                    New ConstantInstance(Of Boolean)("false", False, TypeDictionary.Default, "A Boolean value that fails a conditional test."),
                                                                    New ConstantInstance(Of Double)("pi", System.Math.PI, TypeDictionary.Default, "= 3.14159..."),
                                                                    New ConstantInstance(Of Double)("e", System.Math.E, TypeDictionary.Default, "= 2.71828...")
                                                                   },
                                                        Functions:={New FunctionInstance(Of Func(Of Double, Double))("Sqrt", Function(x As Double) System.Math.Sqrt(x), TypeDictionary.Default, "The square root of a real number."),
                                                                    New FunctionInstance(Of Func(Of Double, Double))("Exp", Function(x) System.Math.Exp(x), TypeDictionary.Default, "E raised to the specified power."),
                                                                    New FunctionInstance(Of Func(Of Double, Double))("Sin", Function(x) System.Math.Sin(x), TypeDictionary.Default, "The sine of a specified angle."),
                                                                    New FunctionInstance(Of Func(Of Double, Double))("Cos", Function(x) System.Math.Cos(x), TypeDictionary.Default, "The cosine of a specified angle."),
                                                                    New FunctionInstance(Of Func(Of Double, Double))("Tan", Function(x) System.Math.Tan(x), TypeDictionary.Default, "The tangent of a specified angle."),
                                                                    New FunctionInstance(Of Func(Of Double, Double))("Arcsin", Function(x) System.Math.Asin(x), TypeDictionary.Default, "The arcsine of a specified number."),
                                                                    New FunctionInstance(Of Func(Of Double, Double))("Arccos", Function(x) System.Math.Acos(x), TypeDictionary.Default, "The arccosine of a specified number."),
                                                                    New FunctionInstance(Of Func(Of Double, Double))("Abs", Function(x) System.Math.Abs(x), TypeDictionary.Default, "The absolute value of the specified number."),
                                                                    New FunctionInstance(Of Func(Of Double, Double, Double))("Max", Function(a, b) System.Math.Max(a, b), TypeDictionary.Default, "The maximum value of the specified numbers."),
                                                                    New FunctionInstance(Of Func(Of Double, Double, Double))("Min", Function(a, b) System.Math.Min(a, b), TypeDictionary.Default, "The minimum value of the specified numbers."),
                                                                    New FunctionInstance(Of Func(Of Vector3D, Double))("X", Function(v) v.X, TypeDictionary.Default, "The x-component of a vector."),
                                                                    New FunctionInstance(Of Func(Of Vector3D, Double))("Y", Function(v) v.Y, TypeDictionary.Default, "The y-component of a vector."),
                                                                    New FunctionInstance(Of Func(Of Vector3D, Double))("Z", Function(v) v.Z, TypeDictionary.Default, "The z-component of a vector."),
                                                                    New FunctionInstance(Of Func(Of Vector3D, Vector3D, Double))("ScalarProduct", Function(u, v) u * v, TypeDictionary.Default, "The scalar product of two vectors."),
                                                                    New FunctionInstance(Of Func(Of Vector3D, Vector3D, Vector3D))("VectorProduct", Function(u, v) u.CrossProduct(v), TypeDictionary.Default, "The vector product of two vectors.")
                                                                   },
                                                        Types:=NamedTypes.Default)

    Public Shared ReadOnly Property [Default] As TermContext
        Get
            Return _Default
        End Get
    End Property

    Public Function Merge(second As TermContext) As TermContext
        For Each newConstantSignature In second._Constants.Select(Function(c) c.Signature).Concat(second._Parameters.Select(Function(p) p.Signature))
            For Each constantSignature In _Constants.Select(Function(c) c.Signature).Concat(_Parameters.Select(Function(p) p.Signature))
                newConstantSignature.CheckForConflicts(constantSignature)
            Next
        Next

        For Each newFunctionSignature In second._Functions.Select(Function(f) f.Signature)
            For Each functionSignature In _Functions.Select(Function(f) f.Signature)
                newFunctionSignature.CheckForConflicts(functionSignature)
            Next
        Next

        Return New TermContext(Constants:=_Constants.Concat(second._Constants),
                               Functions:=_Functions.Concat(second._Functions),
                               Parameters:=_Parameters.Concat(second._Parameters),
                               Types:=_Types.Merge(second.Types))
    End Function

    Public Function ParseSingleFunctionWithName(name As LocatedString) As FunctionInstance
        Dim matchingGroup = Me.GetMatchingFunctionGroup(name)
        If matchingGroup.Count > 1 Then Throw New LocatedCompilerException(name, String.Format("There are multiple definitions for function with name '{0}'.", name))

        Return matchingGroup.Single
    End Function

    Public Function ParseFunction(functionCall As FunctionCall) As FunctionInstance
        Dim matchingFunctionGroup = Me.GetMatchingFunctionGroup(functionCall.FunctionName)
        Dim matchingFunctions = matchingFunctionGroup.Where(Function(instance) instance.Signature.FunctionType.Parameters.Count = functionCall.Arguments.Count)
        If Not matchingFunctions.Any Then Throw New LocatedCompilerException(functionCall.LocatedString, String.Format("Function '{0}' with parameter count {1} not defined in this context.", functionCall.FunctionName, functionCall.Arguments.Count))
        Return matchingFunctions.Single
    End Function

    Private Function GetMatchingFunctionGroup(functionName As LocatedString) As IGrouping(Of String, FunctionInstance)
        Dim matchingFunctionGroups = Me.GroupedFunctionsAndFunctionParameters.Where(Function(group) CompilerTools.IdentifierEquals(group.Key, functionName.ToString))
        If Not matchingFunctionGroups.Any Then Throw New LocatedCompilerException(functionName, String.Format("Function '{0}' not defined in this context.", functionName))
        Return matchingFunctionGroups.Single
    End Function

    Public Function TryParseConstant(name As String) As ConstantInstance
        Dim matchingConstants = From constant In Me.Constants Where CompilerTools.IdentifierEquals(name, constant.Signature.Name)
        If Not matchingConstants.Any Then Return Nothing

        Return matchingConstants.Single
    End Function

    Public Function TryParseParameter(name As String) As NamedParameter
        Dim matchingParameters = From parameter In Me.Parameters Where CompilerTools.IdentifierEquals(name, parameter.Signature.Name)
        If Not matchingParameters.Any Then Return Nothing

        Return matchingParameters.Single
    End Function
End Class
