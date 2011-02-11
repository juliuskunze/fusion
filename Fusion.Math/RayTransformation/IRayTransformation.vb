''' <summary>
''' Transforms a ray of a stationary reference frame into one which relativly moves
''' with a constant velocity in x-direction to the first.
''' </summary>
''' The origins of the reference frames are equal, their times are equal.
''' <remarks></remarks>
Public Interface IRayTransformation

    Property RelativeXVelocityInC As Double

    ''' <summary>
    ''' The ray in the moved reference frame.
    ''' </summary>
    ''' <param name="ray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function TransformedRay(ByVal ray As Ray) As Ray

End Interface
