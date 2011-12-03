Public Class NamedTypeTests

    <Test()>
    Public Sub NamedDelegateTypeFromText()
        Dim delegateType = NamedType.NamedFunctionTypeFromString("FunctionType Real WavelengthFunction(Real wavelength)".ToLocated, typeContext:=NamedTypes.Default)

        Assert.That(delegateType.IsFunctionType)
        Assert.AreSame(delegateType.[Function].ResultType, NamedType.Real)
        Assert.AreEqual(delegateType.Name, "WavelengthFunction")

        Dim parameter = delegateType.[Function].Parameters.Single

        Assert.AreSame(parameter.Signature.Type, NamedType.Real)
        Assert.AreEqual(parameter.Signature.Name, "wavelength")
    End Sub

    <Test()>
    Public Sub IsAssignableFrom()
        Dim typeContext = New NamedTypes(NamedTypes.Default.Concat({New NamedType(name:="Object", systemType:=GetType(Object))}))

        Dim delegateType1 = NamedType.NamedFunctionTypeFromString("FunctionType Object FunctionType1(Real x)".ToLocated, typeContext:=typeContext)
        Dim delegateType2 = NamedType.NamedFunctionTypeFromString("FunctionType Real FunctionType2(Object x)".ToLocated, typeContext:=typeContext)

        delegateType1.CheckIsAssignableFrom(delegateType2)

        Try
            delegateType2.CheckIsAssignableFrom(delegateType1)
            Assert.Fail()
        Catch ex As CompilerException
            Assert.AreEqual("Type 'FunctionType1' is not assignable to type 'FunctionType2'.", ex.Message)
        End Try
    End Sub

    Delegate Function D1(x As Integer) As Object

    Private Function D2(x As Object) As Integer
        Dim a As D1 = AddressOf D2

        Return Nothing
    End Function

    <Test()>
    Public Sub NamedWithTypeArguments()
        Dim setType = New NamedType("Set", GetType(IEnumerable(Of )))
        Dim stringSetType = setType.MakeGenericType({New NamedType("Text", GetType(String))})

        Assert.AreEqual(expected:="Set[Text]", actual:=stringSetType.NameWithTypeArguments)
    End Sub

End Class
