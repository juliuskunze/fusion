Public Class ViewController2D

    Public Sub New(ByVal visualizer As Visualizer2D, Optional ByVal zoomInFactorPerZoomStep As Double = 1.2, Optional ByVal rotationAnglePerDeltaInPixels As Double = PI / 100)
        Me.ZoomInFactorPerZoomStep = zoomInFactorPerZoomStep
        Me.ZoomCenterMode = CenterModes.AtMouseLocation

        Me.RotationAnglePerDeltaInPixels = rotationAnglePerDeltaInPixels
        Me.RotateCenterMode = CenterModes.AtMidpoint
        Me.Visualizer = visualizer
    End Sub

    Public Property Visualizer() As Visualizer2D
    Public Property ScreenMouseLocation() As Vector2D
    Public ReadOnly Property SimulationMouseLocation() As Vector2D
        Get
            Return Me.Visualizer.InverseMap.Apply(Me.ScreenMouseLocation)
        End Get
    End Property

    Public Enum CenterModes
        AtMouseLocation
        AtMidpoint
    End Enum
    Public Property ZoomCenterMode() As CenterModes
    Public Property RotateCenterMode As CenterModes

    Public Property ZoomInFactorPerZoomStep() As Double
    Public Property RotationAnglePerDeltaInPixels As Double

    Public Sub Relocate(ByVal oldSimulationMouseLocation As Vector2D)
        Me.Visualizer.ProjectionMap = Me.Visualizer.ProjectionMap.
            After(AffineMap2D.Translation(Me.SimulationMouseLocation - oldSimulationMouseLocation))
    End Sub

    Public Sub Zoom(ByVal zoomSteps As Double)
        Dim zoomCenter As Vector2D
        Select Case Me.ZoomCenterMode
            Case CenterModes.AtMidpoint
                zoomCenter = New Vector2D(Me.Visualizer.Graphics.VisibleClipBounds.Size) / 2
            Case CenterModes.AtMouseLocation
                zoomCenter = Me.SimulationMouseLocation
        End Select

        Me.Visualizer.ProjectionMap = Me.Visualizer.ProjectionMap.
            After(AffineMap2D.Scaling(Me.ZoomInFactorPerZoomStep ^ zoomSteps).At(zoomCenter))
    End Sub

    Private _RotationCenterSimulationLocation As Vector2D

    Public Sub StartRotate()
        _RotationCenterSimulationLocation = Me.SimulationMouseLocation
    End Sub

    Public Sub Rotate(ByVal oldScreenMouseLocation As Vector2D)
        Dim xDeltaInPixels = Me.ScreenMouseLocation.X - oldScreenMouseLocation.X
        Dim rotationAngle = xDeltaInPixels * RotationAnglePerDeltaInPixels

        Dim zoomCenter As Vector2D
        Select Case Me.RotateCenterMode
            Case CenterModes.AtMidpoint
                zoomCenter = Me.Visualizer.InverseMap.Apply(New Vector2D(Me.Visualizer.Graphics.VisibleClipBounds.Size) / 2)
            Case CenterModes.AtMouseLocation
                zoomCenter = _RotationCenterSimulationLocation
        End Select

        Me.Visualizer.ProjectionMap = Me.Visualizer.ProjectionMap.
            After(AffineMap2D.Rotation(rotationAngle).At(zoomCenter))
    End Sub

    Public Sub CenterSimulationLocation(ByVal simulationLocation As Vector2D)
        Me.Visualizer.ProjectionMap = Me.Visualizer.ProjectionMap.
            Before(AffineMap2D.Translation(-Me.Visualizer.ProjectionMap.Apply(simulationLocation)))
    End Sub

End Class
