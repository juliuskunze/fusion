Public Class GravityField3D
    Implements IField3D

    Public Function Field(particle As Particle3D, location As Math.Vector3D) As Math.Vector3D Implements IField3D.Field
        Dim connection = location - particle.Location
        Dim distance = connection.Length

        Return -Constants.GravitationalConstant * particle.Mass * connection / distance ^ 3
    End Function

    Public Function Force(field As Math.Vector3D, particle As Particle3D) As Math.Vector3D Implements IField3D.Force
        Return field * particle.Mass
    End Function

    Public Function Potential(particle As Particle3D, location As Math.Vector3D) As Double Implements IField3D.Potential
        Dim connection = location - particle.Location
        Dim distance = connection.Length
        Return -Constants.GravitationalConstant * particle.Mass / distance
    End Function

    Public Function PotentialEnergy(potential As Double, particle As Particle3D) As Double Implements IField3D.PotentialEnergy
        Return potential * particle.Mass
    End Function
End Class