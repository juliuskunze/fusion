Public Class Particle2DGrid
    Public Sub LoadParticleList(particleList As List(Of Particle2D))
        For Each particle In particleList
            addRow(particle)
        Next
    End Sub

    Private Sub addRow(particle As Particle2D)
        Dim gridRow As New DataGridViewRow
        'gridRow.Cells(gridColums.Name).Value = particle.Name
        gridRow.Cells(gridColums.Mass).Value = particle.Mass
        gridRow.Cells(gridColums.Charge).Value = particle.Charge
        gridRow.Cells(gridColums.Location).Value = particle.Location
        gridRow.Cells(gridColums.Velocity).Value = particle.Velocity
        If TypeOf particle Is SphereParticle2D Then
            Dim sphereParticle = DirectCast(particle, SphereParticle2D)
            gridRow.Cells(gridColums.Radius).Value = sphereParticle.Radius
        End If
        gridRow.Cells(gridColums.Color).Value = particle.Color

        grid.Rows.Add(gridRow)
    End Sub

    Private Sub grid_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grid.CellContentClick
    End Sub

    Private Enum gridColums As INteger
        Name
        Mass
        Charge
        Location
        Velocity
        Radius
        Color
    End Enum
End Class
