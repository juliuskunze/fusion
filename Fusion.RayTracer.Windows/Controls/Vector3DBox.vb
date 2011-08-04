Public Class Vector3DBox
    Inherits ValidatingTextBoxBase(Of Vector3D?)

    Protected Overrides Function Convert(ByVal text As String) As Math.Vector3D?
        Try
            Return New Vector3D(text)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class
