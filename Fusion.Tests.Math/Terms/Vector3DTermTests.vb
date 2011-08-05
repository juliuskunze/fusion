Public Class Vector3DTermTests

    <Test()>
    Public Sub Test()
        Assert.That(New Vector3DTerm(Term:="(1;2;a)", userContext:=New TermContext(constants:={},
                                                                                   parameters:={Expression.Parameter(GetType(Double), "a")},
                                                                                   Functions:={})).GetDelegate(Of Func(Of Double, Vector3D)).Invoke(3.0) = New Vector3D(1, 2, 3))
    End Sub

End Class
