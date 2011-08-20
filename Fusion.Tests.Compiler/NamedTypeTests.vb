Public Class NamedTypeTests

    <Test()>
    Public Sub NamedDelegateTypeFromText()
        Dim delegateType = NamedType.NamedDelegateTypeFromText("delegate Real WaveLengthFunction(Real wavelength)", typeContext:=NamedTypes.Default)

        Assert.That(delegateType.IsDelegate)
        Assert.AreSame(delegateType.Delegate.ResultType, NamedType.Real)
        Assert.AreEqual(delegateType.Name, "WaveLengthFunction")

        Dim parameter = delegateType.Delegate.Parameters.Single

        Assert.AreSame(parameter.Type, NamedType.Real)
        Assert.AreEqual(parameter.Name, "wavelength")
    End Sub

    <Test()>
    Public Sub IsAssignableFrom()
        Dim typeContext = New NamedTypes(NamedTypes.Default.Concat({New NamedType(name:="Object", systemType:=GetType(Object))}))

        Dim delegateType1 = NamedType.NamedDelegateTypeFromText("delegate Object DelegateType1(Real x)", typeContext:=typeContext)
        Dim delegateType2 = NamedType.NamedDelegateTypeFromText("delegate Real DelegateType2(Object x)", typeContext:=typeContext)

        delegateType1.CheckIsAssignableFrom(delegateType2)

        Try
            delegateType2.CheckIsAssignableFrom(delegateType1)
            Assert.Fail()
        Catch ex As ArgumentException
            Assert.AreEqual("Type 'Object' is not assignable to type 'Real'.", ex.Message)
        End Try
    End Sub

    Delegate Function D1(x As Integer) As Object

    Private Function D2(x As Object) As Integer
        Dim a As D1 = AddressOf D2

        Return Nothing
    End Function

End Class
