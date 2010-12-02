Public Class GravityField3D
    Implements IField3D

    Public Function Field(ByVal particle As Particle3D, ByVal location As Math.Vector3D) As Math.Vector3D Implements IField3D.Field
        Dim connection = location - particle.Location
        Dim distance = connection.Length

        Return -Constants.GravitationalConstant * particle.Mass * connection / distance ^ 3
    End Function

    Public Function Force(ByVal field As Math.Vector3D, ByVal particle As Particle3D) As Math.Vector3D Implements IField3D.Force
        Return field * particle.Mass
    End Function

    Public Function Potential(ByVal particle As Particle3D, ByVal location As Math.Vector3D) As Double Implements IField3D.Potential
        Dim connection = location - particle.Location
        Dim distance = connection.Length
        Return -Constants.GravitationalConstant * particle.Mass / distance
    End Function

    Public Function PotentialEnergy(ByVal potential As Double, ByVal particle As Particle3D) As Double Implements IField3D.PotentialEnergy
        Return potential * particle.Mass
    End Function
End Class