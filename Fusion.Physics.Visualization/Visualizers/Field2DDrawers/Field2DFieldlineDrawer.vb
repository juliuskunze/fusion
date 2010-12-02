Public Class Field2DFieldlineDrawer
    Implements IDrawer2D

    Public Property FieldlinesPerCharge As Double = 2000000.0
    Public Property MaxFieldlineStepCount As Integer = 100
    Public Property FieldlineStepLength As Double = 0.01
    Public Property ArrowsPerFieldStrengthAndFieldlineLength As Double = 0

    Public Sub New(ByVal visualizer As Visualizer2D, ByVal field As ParticleField2D)
        Me.FieldlinePen = New Pen(Color.Beige)
        Me.Visualizer = visualizer
        Me.Field = field
    End Sub

    Public Property Visualizer As Visualizer2D Implements IDrawer2D.Visualizer

    Public Property Field As ParticleField2D

    Public Sub Draw() Implements IDrawer2D.Draw
        For Each particle In Me.Field.Particles
            If particle.Charge > 0 Then
                Dim particleRadius As Double
                If TypeOf particle Is SphereParticle2D Then
                    particleRadius = DirectCast(particle, SphereParticle2D).Radius
                Else
                    particleRadius = 0
                End If

                Dim fieldLineNumberAroundParticle As Integer = CInt(Round(_FieldlinesPerCharge * particle.Charge, MidpointRounding.ToEven))

                For fieldLineIndex = 0 To fieldLineNumberAroundParticle - 1
                    Dim origin = particle.Location + Vector2D.FromLengthAndArgument(particleRadius, 2 * PI / fieldLineNumberAroundParticle * fieldLineIndex)
                    drawFieldline(origin:=origin)
                Next
            End If
        Next
    End Sub

    Private _fieldLinePen As Pen
    Public Property FieldlinePen() As Pen
        Get
            Return _fieldLinePen
        End Get
        Set(ByVal value As Pen)
            _fieldLinePen = value
            _fieldlinePenWithArrow = CType(FieldlinePen.Clone, Pen)
            _fieldlinePenWithArrow.EndCap = Drawing2D.LineCap.ArrowAnchor
        End Set
    End Property

    Private _fieldlinePenWithArrow As Pen

    Private Sub drawFieldline(ByVal origin As Vector2D)
        Dim currentFieldlineLocation = origin
        Dim nextFieldlineLocation As Vector2D

        Dim fieldStrengthSum As Double = 0
        Dim distanceSum As Double = 0

        For fieldLineStepIndex = 1 To Me.MaxFieldlineStepCount
            distanceSum += _FieldlineStepLength

            Dim nearestNegativeParticle = Me.Field.Particles.NearestNegativeParticle(currentFieldlineLocation)

            Dim fieldLineCaught As Boolean
            Dim radiusOfNearestParticle As Double

            If nearestNegativeParticle Is Nothing Then
                fieldLineCaught = False
            Else
                Dim distanceToNearestParticle = (nearestNegativeParticle.Location - currentFieldlineLocation).Length

                If TypeOf nearestNegativeParticle Is SphereParticle2D Then
                    Dim sphereParticle = DirectCast(nearestNegativeParticle, SphereParticle2D)
                    radiusOfNearestParticle = sphereParticle.Radius
                Else
                    radiusOfNearestParticle = 0
                End If
                fieldLineCaught = (distanceToNearestParticle < radiusOfNearestParticle + Me.FieldlineStepLength)
            End If

            If fieldLineCaught Then
                Dim radiusVector = currentFieldlineLocation - nearestNegativeParticle.Location
                radiusVector.Length = radiusOfNearestParticle

                nextFieldlineLocation = nearestNegativeParticle.Location + radiusVector
            Else
                Dim fieldStrength As Vector2D = Field.Field(currentFieldlineLocation)
                fieldStrengthSum += fieldStrength.Length

                Dim stepVector = fieldStrength
                stepVector.Length = _FieldlineStepLength

                nextFieldlineLocation = currentFieldlineLocation + stepVector
            End If

            Dim lastStep As Boolean = (fieldLineStepIndex >= Me.MaxFieldlineStepCount) OrElse fieldLineCaught
            Dim arrowNeeded As Boolean = (Me.ArrowsPerFieldStrengthAndFieldlineLength * fieldStrengthSum * distanceSum > 1) OrElse lastStep

            Dim finalFieldlinePen As Pen

            If arrowNeeded Then
                finalFieldlinePen = _fieldlinePenWithArrow

                distanceSum = 0
                fieldStrengthSum = 0
            Else
                finalFieldlinePen = Me.FieldlinePen
            End If

            Me.Visualizer.DrawingGraphics.DrawLine(finalFieldlinePen, Me.Visualizer.Map.Apply(currentFieldlineLocation).ToPointF, Me.Visualizer.Map.Apply(nextFieldlineLocation).ToPointF)

            If lastStep Then
                Exit For
            End If

            currentFieldlineLocation = nextFieldlineLocation
        Next

    End Sub


End Class
