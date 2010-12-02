<Serializable()>
Public Class ElectricField3D
    Implements IField3D

    Public Function Potential(ByVal particle As Particle3D, ByVal location As Vector3D) As Double Implements IField3D.Potential
        Dim connection = particle.Location - location
        Dim distance = connection.Length

        Return +(1 / (4 * PI * ElectricConstant)) * particle.Charge / distance
    End Function

    Public Function Energy(ByVal potential As Double, ByVal particle As Particle3D) As Double Implements IField3D.PotentialEnergy
        Return potential * particle.Charge
    End Function


    Public Function Field(ByVal particle As Particle3D, ByVal location As Math.Vector3D) As Math.Vector3D Implements IField3D.Field
        Dim connection = location - particle.Location
        Dim distance = connection.Length

        Return +(1 / (4 * PI * ElectricConstant)) * particle.Charge * connection / distance ^ 3
    End Function

    Public Function Force(ByVal field As Math.Vector3D, ByVal particle As Particle3D) As Math.Vector3D Implements IField3D.Force
        Return field * particle.Charge
    End Function

End Class