Public Class Visualizer2D
    Private Const _MetersPerInch As Double = 0.0254

    Private _ScreenMap As AffineMap2D

    Private _ProjectionMap As AffineMap2D
    Public Property ProjectionMap() As AffineMap2D
        Get
            Return _ProjectionMap
        End Get
        Set(value As AffineMap2D)
            _ProjectionMap = value
            _FinalMap = generateFinalMap()
            _InverseMap = generateInverseMap()
            RaiseEvent MapChanged(Me, Nothing)
        End Set
    End Property

    Private _FinalMap As AffineMap2D
    Public ReadOnly Property Map() As AffineMap2D
        Get
            Return _FinalMap
        End Get
    End Property

    Private Function generateScreenMap() As AffineMap2D
        Dim screenMap = AffineMap2D.Identity
        screenMap = screenMap.Before(LinearMap2D.Scaling(Graphics.DpiX / _MetersPerInch, Graphics.DpiY / _MetersPerInch))
        screenMap = screenMap.Before(LinearMap2D.Reflection(axisAngle:=0))
        screenMap.TranslationVector = New Vector2D(Graphics.VisibleClipBounds.Size) / 2
        Return screenMap
    End Function

    Private Function generateFinalMap() As AffineMap2D
        Return _ScreenMap.After(_ProjectionMap)
    End Function

    Private Function generateInverseMap() As AffineMap2D
        Return Me.Map.Inverse
    End Function

    Private _InverseMap As AffineMap2D
    Public ReadOnly Property InverseMap() As AffineMap2D
        Get
            Return _InverseMap
        End Get
    End Property

    Public Event MapChanged(sender As Object, e As EventArgs)

    Public Sub New(graphics As Graphics)
        Me.Graphics = graphics
        Me.ProjectionMap = AffineMap2D.Identity
        Me.Traces = False
    End Sub

    Private _Graphics As Graphics
    Public Property Graphics As Graphics
        Get
            Return _Graphics
        End Get
        Set(value As Graphics)
            _Graphics = value

            _ScreenMap = generateScreenMap()

            Dim bufferedGraphicsContext = New BufferedGraphicsContext
            _Buffer = bufferedGraphicsContext.Allocate(_Graphics, Drawing.Rectangle.Round(_Graphics.VisibleClipBounds))
            _BufferedGraphics = _Buffer.Graphics()
            _BufferedGraphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        End Set
    End Property

    Private _Buffer As BufferedGraphics

    Private _BufferedGraphics As Graphics
    Public ReadOnly Property DrawingGraphics As Graphics
        Get
            Return _BufferedGraphics
        End Get
    End Property

    Public Property Traces As Boolean

    Public Property BackColor As Color

    Public Sub Render(DrawAction As Action)
        If Not Traces Then
            Me.DrawingGraphics.Clear(Me.BackColor)
        End If

        DrawAction()

        _Buffer.Render()
    End Sub

    Public Function GenerateCircleRect(center As Vector2D, radius As Double) As RectangleF
        Return GenerateScreenRadiusCircleRect(center, Me.ProjectionMap.LinearMap.ZoomOut * radius)
    End Function

    Public Function GenerateScreenRadiusCircleRect(center As Vector2D, screenRadius As Double) As RectangleF
        Dim xScreenRadiusInDots = screenRadius * Graphics.DpiX / _MetersPerInch
        Dim yScreenRadiusInDots = screenRadius * Graphics.DpiY / _MetersPerInch

        Dim screenRadiusDiagonalVector = New Vector2D(xScreenRadiusInDots, yScreenRadiusInDots)

        Dim lowerCorner = Me.Map.Apply(center) - screenRadiusDiagonalVector
        Dim upperCorner = Me.Map.Apply(center) + screenRadiusDiagonalVector

        Return New RectangleF(lowerCorner.ToPointF, (upperCorner - lowerCorner).ToSizeF)
    End Function
End Class
