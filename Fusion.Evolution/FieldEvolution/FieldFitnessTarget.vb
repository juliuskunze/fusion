Public Class FieldFitnessTarget
    Implements IFitness(Of ParticleField2D)

    Public Property Location As Vector2D
    Public Property TargetField As Vector2D
    Public Property Weight As Double

    Public Sub New(location As Vector2D, targetField As Vector2D, Optional weight As Double = 1)
        Me.Location = location
        Me.TargetField = targetField
        Me.Weight = weight
    End Sub

    Public Function Fitness(solution As Physics.ParticleField2D) As Double Implements IFitness(Of Physics.ParticleField2D).Fitness
        Return -Weight * (solution.Field(Location) - Me.TargetField).Length
    End Function
End Class
