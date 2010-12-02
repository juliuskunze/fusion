Public Class FieldSingleForceGenerator2D
    Implements ISingleForceGenerator2D

    Public Property Field As IField2D

    Public Sub New(ByVal field As IField2D)
        Me.Field = field
    End Sub

    Public Function Force(ByVal particle As Particle2D) As Math.Vector2D Implements ISingleForceGenerator2D.Force
        Return Me.Field.Field(particle.Location)
    End Function

End Class
