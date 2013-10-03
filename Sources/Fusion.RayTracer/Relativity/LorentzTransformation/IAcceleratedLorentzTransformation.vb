Public Interface IAcceleratedLorentzTransformation
    ''' <summary>
    ''' The transformation from the inertial system the inertial system of the accelerating system at the specified point of time.
    ''' </summary>
    Function InertialToAcceleratedInertial(acceleratedFrameTime As Double) As LorentzTransformation
End Interface