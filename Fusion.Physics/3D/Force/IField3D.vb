Public Interface IField3D

    Function Field(ByVal particle As Particle3D, ByVal location As Vector3D) As Vector3D
    Function Force(ByVal field As Vector3D, ByVal particle As Particle3D) As Vector3D

    Function Potential(ByVal particle As Particle3D, ByVal location As Vector3D) As Double
    Function PotentialEnergy(ByVal potential As Double, ByVal particle As Particle3D) As Double

End Interface
