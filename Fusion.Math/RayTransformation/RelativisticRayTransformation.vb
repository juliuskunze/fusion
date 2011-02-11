Public Class RelativisticRayTransformation
    Implements IRayTransformation

    Public Sub New(ByVal relativeXVelocityInC As Double)
        Me.RelativeXVelocityInC = RelativeXVelocityInC
    End Sub

    Public Property RelativeXVelocityInC As Double Implements IRayTransformation.RelativeXVelocityInC

    Public Function TransformedRay(ByVal ray As Ray) As Ray Implements IRayTransformation.TransformedRay

    End Function
End Class
