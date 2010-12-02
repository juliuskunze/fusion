Public Class Visualizer2D

    Private Const _metersPerInch As Double = 0.0254

    Private _screenMap As AffineMap2D

    Private _projectionMap As AffineMap2D
    Public Property ProjectionMap() As AffineMap2D
        Get
            Return _projectionMap
        End Get
        Set(ByVal value As AffineMap2D)
            _projectionMap = value
            _finalMap = generateFinalMap()
            _inverseMap = generateInverseMap()
            RaiseEvent MapChanged(Me, Nothing)
        End Set
    End Property

    Private _finalMap As AffineMap2D
    Public ReadOnly Property Map() As AffineMap2D
        Get
            Return _finalMap
        End Get
    End Property

    Private Function generateScreenMap() As AffineMap2D
        Dim screenMap = AffineMap2D.Identity
        screenMap = screenMap.Before(LinearMap2D.Scaling(Graphics.DpiX / _metersPerInch, Graphics.DpiY / _metersPerInch))
        screenMap = screenMap.Before(LinearMap2D.Reflection(axisAngle:=0))
        screenMap.TranslationVector = New Vector2D(Graphics.VisibleClipBounds.Size) / 2
        Return screenMap
    End Function

    Private Function generateFinalMap() As AffineMap2D
        Return _screenMap.After(_projectionMap)
    End Function

    Private Function generateInverseMap() As AffineMap2D
        Return Me.Map.Inverse
    End Function

    Private _inverseMap As AffineMap2D
    Public ReadOnly Property InverseMap() As AffineMap2D
        Get
            Return _inverseMap
        End Get
    End Property

    Public Event MapChanged(ByVal sender As Object, ByVal e As EventArgs)

    Public Sub New(ByVal graphics As Graphics)
        Me.Graphics = graphics
        Me.ProjectionMap = AffineMap2D.Identity
        Me.Traces = False
    End Sub

    Private _graphics As Graphics
    Public Property Graphics As Graphics
        Get
            Return _graphics
        End Get
        Set(ByVal value As Graphics)
            _graphics = value

            _screenMap = generateScreenMap()

            Dim bufferedGraphicsContext = New BufferedGraphicsContext
            _buffer = bufferedGraphicsContext.Allocate(_graphics, Drawing.Rectangle.Round(_graphics.VisibleClipBounds))
            _bufferedGraphics = _buffer.Graphics()
            _bufferedGraphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        End Set
    End Property

    Private _buffer As BufferedGraphics

    Private _bufferedGraphics As Graphics
    Public ReadOnly Property DrawingGraphics As Graphics
        Get
            Return _bufferedGraphics
        End Get
    End Property

    Public Property Traces As Boolean

    Public Property BackColor As Color

    Public Sub Render(ByVal DrawAction As Action)
        If Not Traces Then
            Me.DrawingGraphics.Clear(Me.BackColor)
        End If

        DrawAction()

        _buffer.Render()
    End Sub

    Public Function GenerateCircleRect(ByVal center As Vector2D, ByVal radius As Double) As RectangleF
        Return GenerateScreenRadiusCircleRect(center, Me.ProjectionMap.LinearMap.ZoomOut * radius)
    End Function

    Public Function GenerateScreenRadiusCircleRect(ByVal center As Vector2D, ByVal screenRadius As Double) As RectangleF
        Dim xScreenRadiusInDots = screenRadius * Graphics.DpiX / _metersPerInch
        Dim yScreenRadiusInDots = screenRadius * Graphics.DpiY / _metersPerInch

        Dim screenRadiusDiagonalVector = New Vector2D(xScreenRadiusInDots, yScreenRadiusInDots)

        Dim lowerCorner = Me.Map.Apply(center) - screenRadiusDiagonalVector
        Dim upperCorner = Me.Map.Apply(center) + screenRadiusDiagonalVector

        Return New RectangleF(lowerCorner.ToPointF, (upperCorner - lowerCorner).ToSizeF)
    End Function

End Class
