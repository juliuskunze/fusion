Public Class FrameTimer
    Implements IDisposable

    Private ReadOnly _Timer As Windows.Forms.Timer

    Public Sub New(framerate As Double, calcRate As Double, Optional fastMotion As Double = 1)
        _Timer = New Windows.Forms.Timer

        _Framerate = framerate
        Me.CalcRate = calcRate
        Me.Framerate = framerate

        _FastMotion = fastMotion

        AddHandler _Timer.Tick, AddressOf TickHandler
    End Sub


    Public Property Enabled() As Boolean
        Get
            Return _Timer.Enabled
        End Get
        Set(value As Boolean)
            _Timer.Enabled = value
        End Set
    End Property

    Public Sub Start()
        _Timer.Start()
    End Sub

    Public Sub [Stop]()
        _Timer.Stop()
    End Sub


    Private _Framerate As Double
    Public Property Framerate() As Double
        Get
            Return _Framerate
        End Get
        Set(value As Double)
            _Framerate = value

            Dim interval = CInt(1000 / _Framerate)
            _Timer.Interval = interval
            _Framerate = 1000 / interval

            _CalcsPerFrame = CInt(Me.CalcRate / Me.Framerate)
        End Set
    End Property

    Private _FastMotion As Double
    Public Property FastMotion() As Double
        Get
            Return _FastMotion
        End Get
        Set(value As Double)
            _FastMotion = value
        End Set
    End Property

    Public Property SlowMotion() As Double
        Get
            Return 1 / _FastMotion
        End Get
        Set(value As Double)
            _FastMotion = 1 / value
        End Set
    End Property

    Private _CalcsPerFrame As Integer
    Public Property CalcsPerFrame() As Integer
        Get
            Return _CalcsPerFrame
        End Get
        Set(value As Integer)
            _CalcsPerFrame = value
        End Set
    End Property

    Public Property CalcRate() As Double
        Get
            Return Me.Framerate * Me.CalcsPerFrame
        End Get
        Set(value As Double)
            Me.CalcsPerFrame = CInt(value / _Framerate)
            Me.Framerate = value / Me.CalcsPerFrame
        End Set
    End Property

    Public Property TimeStep() As Double
        Get
            Return Me.FastMotion / Me.CalcRate
        End Get
        Set(value As Double)
            Me.CalcRate = Me.FastMotion / Me.TimeStep
        End Set
    End Property


    Public Sub TickHandler(sender As Object, e As EventArgs)
        RaiseEvent FrameTick(Me, New FrameTickEventArgs(TimeStep:=Me.TimeStep, CalcsPerFrame:=Me.CalcsPerFrame))
    End Sub

    Public Event FrameTick(sender As Object, e As FrameTickEventArgs)


    Private _Disposed As Boolean
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not _Disposed Then
            If disposing Then
            End If

            _Timer.Dispose()

        End If
        _Disposed = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
