Public Class Field2DDrawer
    Implements IDrawer2D

    Private _ArrowGridDrawer As Field2DArrowGridDrawer
    Public ReadOnly Property ArrowGridDrawer As Field2DArrowGridDrawer
        Get
            Return _ArrowGridDrawer
        End Get
    End Property

    Private _FieldlineDrawer As Field2DFieldlineDrawer
    Public ReadOnly Property FieldlineDrawer As Field2DFieldlineDrawer
        Get
            Return _FieldlineDrawer
        End Get
    End Property

    Private _ColorDrawer As Field2DColorAreaDrawer
    Public ReadOnly Property ColorDrawer As Field2DColorAreaDrawer
        Get
            Return _ColorDrawer
        End Get
    End Property

    Public Property Field As ParticleField2D

    Public Sub New(ByVal visualizer As Visualizer2D, ByVal particles As IEnumerable(Of Particle2D))
        Me.New(visualizer, New ParticleField2D(New Electric2D, particles))
    End Sub

    Public Sub New(ByVal visualizer As Visualizer2D, ByVal field As ParticleField2D)
        Me.Visualizer = visualizer
        Me.Field = field

        _ArrowGridDrawer = New Field2DArrowGridDrawer(Me.Visualizer, field)
        _FieldlineDrawer = New Field2DFieldlineDrawer(Me.Visualizer, field)
        _ColorDrawer = New Field2DColorAreaDrawer(Me.Visualizer, field)

        Me.VisualizationType = VisualizationTypes.Fieldlines
    End Sub

    Public Enum VisualizationTypes
        ArrowGrid
        Fieldlines
        Color
    End Enum
    Public Property VisualizationType() As VisualizationTypes

    Public Sub Draw() Implements IDrawer2D.Draw
        If Me.Field.ForceType IsNot Nothing Then
            Select Case VisualizationType
                Case VisualizationTypes.ArrowGrid
                    Me.ArrowGridDrawer.Draw()
                Case VisualizationTypes.Fieldlines
                    Me.FieldlineDrawer.Draw()
                Case VisualizationTypes.Color
                    Me.ColorDrawer.Draw()
            End Select
        End If
    End Sub

    Public Property Visualizer As Visualizer2D Implements IDrawer2D.Visualizer
End Class
