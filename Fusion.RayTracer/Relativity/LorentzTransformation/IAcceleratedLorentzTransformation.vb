Public Interface IAcceleratedLorentzTransformation
    ''' <summary>
    ''' The transformation from the inertial system into the accelerating system.
    ''' </summary>
    Function GetTransformationAtTime(acceleratedFrameTime As Double) As LorentzTransformation
End Interface