Public Class ConstantAcceleratedTransformationTests
    <Test>
    Public Sub Test()
        Dim acceleration = 10
        Dim time = 2

        Dim transformation = New ConstantAccelerationLorentzTransformation(acceleration:=acceleration)

        Dim currentTransformation = transformation.GetTransformation(acceleratedFrameTime:=0)

    End Sub
End Class