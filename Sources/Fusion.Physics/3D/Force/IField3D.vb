Public Interface IField3D

    Function Field(particle As Particle3D, location As Vector3D) As Vector3D
    Function Force(field As Vector3D, particle As Particle3D) As Vector3D

    Function Potential(particle As Particle3D, location As Vector3D) As Double
    Function PotentialEnergy(potential As Double, particle As Particle3D) As Double

End Interface
