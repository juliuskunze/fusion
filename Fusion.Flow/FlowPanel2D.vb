Public Class FlowPanel2D
    Inherits CoordinateSystemToArray(Of FlowBox2D)
    Implements IFlowPanel2D(Of FlowBox2D)

    Private _CalculationCount As Integer = 0
    Public ReadOnly Property CalculationCount As Integer Implements IFlowPanel2D(Of FlowBox2D).CalculationCount
        Get
            Return _CalculationCount
        End Get
    End Property

    Public Sub New(lowerVertex As Vector2D, size As Vector2D, gridLength As Double, viscosity As Double, startVelocity As Vector2D)
        MyBase.New(lowerVertex:=lowerVertex, size:=size, gridLength:=gridLength)
        Me.Viscosity = viscosity

        setGeneralBoxes(startVelocity)
    End Sub

    Private Sub setGeneralBoxes(startVelocity As Vector2D)
        For columnIndex = 0 To Me.ColumnCount - 1
            For rowIndex = 0 To Me.RowCount - 1
                Me.Array(columnIndex, rowIndex) = New FlowBox2D(isWall:=False, density:=1, velocity:=startVelocity)
            Next
        Next
    End Sub

    Public Sub SetKeptVelocityBoxes()
        Dim keptVelocity = New Vector2D(0.1, 0)

        For rowIndex = 0 To Me.RowCount - 1
            Me.Array(1, rowIndex) = New StaticFlowBox2D(isWall:=False, keptDensity:=1.4, keptVelocity:=keptVelocity)
            Me.Array(Me.ColumnCount - 2, rowIndex) = New StaticFlowBox2D(isWall:=False, keptDensity:=0.8, keptVelocity:=keptVelocity)
        Next
    End Sub


    Public Sub SetBoundWall()
        Me.SetTopAndBottomWall()
        Me.SetLeftAndRightWall()
    End Sub

    Public Sub SetTopAndBottomWall()
        For columnIndex = 0 To Me.ColumnCount - 1
            Me.Array(columnIndex, 0).IsWall = True
            Me.Array(columnIndex, Me.RowCount - 1).IsWall = True
        Next
    End Sub

    Public Sub SetLeftAndRightWall()
        For rowIndex = 0 To Me.RowCount - 1
            Me.Array(0, rowIndex).IsWall = True
            Me.Array(Me.ColumnCount - 1, rowIndex).IsWall = True
        Next
    End Sub

    Public Sub SetWallFromPointSet(pointSet As IPointSet2D)
        For columnIndex = 0 To Me.ColumnCount - 1
            For rowIndex = 0 To Me.RowCount - 1
                If pointSet.Contains(Me.PointFromRowColumn(columnIndex, rowIndex)) Then
                    Me.Array(columnIndex, rowIndex).IsWall = True
                End If
            Next
        Next
    End Sub


    Public Sub NextTimeStep() Implements IFlowPanel2D(Of FlowBox2D).NextTimeStep
        Me.Array(35, 18).SmokeNew = 1

        For Each flowBox In Me.Array
            flowBox.NextTimeStep()
        Next

        setFNewFromFOldAndFEqMultiThreaded()

        _CalculationCount += 1
    End Sub

    Private Sub setFNewFromFOldAndFEq()
        For columnIndex = 0 To Me.ColumnCount - 1
            For rowIndex = 0 To Me.RowCount - 1
                setFNewFromFOldAndFEq(columnIndex, rowIndex)
            Next
        Next
    End Sub

    Private Sub setFNewFromFOldAndFEqMultiThreaded()
        Dim threads = New List(Of Threading.Thread)

        Dim threadCount As Integer = 2
        For threadIndex = 0 To threadCount - 1
            Dim thread = New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf setFNewFromFOldAndFEqMultiThreaded))

            Dim startIndexAndStep = New StartIndexAndStep(startIndex:=threadIndex, [step]:=threadCount)

            threads.Add(thread)
            thread.Start(startIndexAndStep)
        Next

        Do
            Threading.Thread.Sleep(millisecondsTimeout:=10)
        Loop While (From thread In threads Where thread.IsAlive).ToList.Count <> 0
    End Sub

    Private Sub setFNewFromFOldAndFEqMultiThreaded(startIndexAndStepObject As Object)
        Dim startIndexAndStep = CType(startIndexAndStepObject, StartIndexAndStep)
        For columnIndex = 0 To Me.ColumnCount - 1
            For rowIndex = startIndexAndStep.StartIndex To Me.RowCount - 1 Step startIndexAndStep.Step
                setFNewFromFOldAndFEq(columnIndex, rowIndex)
            Next
        Next
    End Sub

    Private Sub setFNewFromFOldAndFEq(columnIndex As Integer, rowIndex As Integer)
        Dim startFlowBox = Me.Array(columnIndex, rowIndex)
        If Not startFlowBox.IsWall Then
            For innerColumnIndex = 0 To 2
                For innerRowIndex = 0 To 2
                    Dim targetColumnIndex = (columnIndex + innerColumnIndex - 1 + Me.ColumnCount) Mod Me.ColumnCount
                    Dim targetRowIndex = (rowIndex + innerRowIndex - 1 + Me.RowCount) Mod Me.RowCount
                    Dim targetFlowBox = Me.Array(targetColumnIndex, targetRowIndex)
                    'Dim transportedF = startFlowBox.FOld(innerColumnIndex, innerRowIndex)
                    Dim transportedF = Me.Lambda * startFlowBox.FEq(innerColumnIndex, innerRowIndex) + (1 - Me.Lambda) * startFlowBox.FOld(innerColumnIndex, innerRowIndex)
                    Dim transportedSmoke = transportedF / startFlowBox.Density * startFlowBox.SmokeOld * 0.8
                    If targetFlowBox.IsWall Then
                        startFlowBox.FNew(-innerColumnIndex + 2, -innerRowIndex + 2) = transportedF
                        startFlowBox.SmokeNew += transportedSmoke
                    Else
                        targetFlowBox.FNew(innerColumnIndex, innerRowIndex) = transportedF
                        targetFlowBox.SmokeNew += transportedSmoke
                    End If
                Next
            Next
        End If
    End Sub

    Private _Viscosity As Double
    Public Property Viscosity As Double
        Get
            Return _Viscosity
        End Get
        Set(value As Double)
            _Viscosity = value
            _Lambda = 1 / (0.5 + 3 * value)
        End Set
    End Property

    Private _Lambda As Double
    Public ReadOnly Property Lambda As Double
        Get
            Return _Lambda
        End Get
    End Property

End Class
