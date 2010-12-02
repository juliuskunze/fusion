Public Class FrameTimer
    Implements IDisposable

    Private _timer As Windows.Forms.Timer

    Public Sub New(ByVal framerate As Double, ByVal calcRate As Double, Optional ByVal fastMotion As Double = 1)
        _timer = New Windows.Forms.Timer

        _framerate = framerate
        Me.CalcRate = calcRate
        Me.Framerate = framerate

        _fastMotion = fastMotion

        AddHandler _timer.Tick, AddressOf TickHandler
    End Sub


    Public Property Enabled() As Boolean
        Get
            Return _timer.Enabled
        End Get
        Set(ByVal value As Boolean)
            _timer.Enabled = value
        End Set
    End Property

    Public Sub Start()
        _timer.Start()
    End Sub

    Public Sub [Stop]()
        _timer.Stop()
    End Sub


    Private _framerate As Double
    Public Property Framerate() As Double
        Get
            Return _framerate
        End Get
        Set(ByVal value As Double)
            _framerate = value

            Dim interval = CInt(1000 / _framerate)
            _timer.Interval = interval
            _framerate = 1000 / interval

            _calcsPerFrame = CInt(Me.CalcRate / Me.Framerate)
        End Set
    End Property

    Private _fastMotion As Double
    Public Property FastMotion() As Double
        Get
            Return _fastMotion
        End Get
        Set(ByVal value As Double)
            _fastMotion = value
        End Set
    End Property

    Public Property SlowMotion() As Double
        Get
            Return 1 / _fastMotion
        End Get
        Set(ByVal value As Double)
            _fastMotion = 1 / value
        End Set
    End Property

    Private _calcsPerFrame As Integer
    Public Property CalcsPerFrame() As Integer
        Get
            Return _calcsPerFrame
        End Get
        Set(ByVal value As Integer)
            _calcsPerFrame = value
        End Set
    End Property

    Public Property CalcRate() As Double
        Get
            Return Me.Framerate * Me.CalcsPerFrame
        End Get
        Set(ByVal value As Double)
            Me.CalcsPerFrame = CInt(value / _framerate)
            Me.Framerate = value / Me.CalcsPerFrame
        End Set
    End Property

    Public Property TimeStep() As Double
        Get
            Return Me.FastMotion / Me.CalcRate
        End Get
        Set(ByVal value As Double)
            Me.CalcRate = Me.FastMotion / Me.TimeStep
        End Set
    End Property


    Public Sub TickHandler(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent FrameTick(Me, New FrameTickEventArgs(TimeStep:=Me.TimeStep, CalcsPerFrame:=Me.CalcsPerFrame))
    End Sub

    Public Event FrameTick(ByVal sender As Object, ByVal e As FrameTickEventArgs)


    Private _disposed As Boolean
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not _disposed Then
            If disposing Then
            End If

            _timer.Dispose()

        End If
        _disposed = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

End Class
