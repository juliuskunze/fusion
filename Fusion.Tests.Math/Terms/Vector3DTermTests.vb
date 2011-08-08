Public Class Vector3DTermTests

    <Test()>
    Public Sub Test()
        Assert.That(New Term(Term:="(1;2;a)", Type:=NamedType.Vector3D, userContext:=New TermContext(constants:={},
                                                                                                     parameters:={New NamedParameter(name:="a", Type:=NamedType.Real)},
                                                                                                     Functions:={})).GetDelegate(Of Func(Of Double, Vector3D)).Invoke(3.0) = New Vector3D(1, 2, 3))
    End Sub

End Class
