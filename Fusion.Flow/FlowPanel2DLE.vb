Public Class FlowPanel2DLE
    Inherits CoordinateSystemToArray(Of FlowBox2DLE)
    Implements IFlowPanel2D(Of FlowBox2DLE)

    Private _CalculationCount As Integer = 0
    Public ReadOnly Property CalculationCount As Integer Implements IFlowPanel2D(Of FlowBox2DLE).CalculationCount
        Get
            Return _CalculationCount
        End Get
    End Property

    Public Sub New(lowerVertex As Vector2D, size As Vector2D, gridLength As Double, startViscosity As Double, startVelocity As Vector2D)
        MyBase.New(lowerVertex:=lowerVertex, size:=size, gridLength:=gridLength)

        setGeneralBoxes(startVelocity, startViscosity)
    End Sub

    Private Sub setGeneralBoxes(startVelocity As Vector2D, startViscosity As Double)
        For columnIndex = 0 To Me.ColumnCount - 1
            For rowIndex = 0 To Me.RowCount - 1
                Me.Array(columnIndex, rowIndex) = New FlowBox2DLE(isWall:=False, density:=1, velocity:=startVelocity, viscosity:=startViscosity)
            Next
        Next
    End Sub

    Public Sub SetKeptVelocityBoxes(viscosity As Double)
        Dim keptVelocity = New Vector2D

        For rowIndex = 0 To Me.RowCount - 1
            Me.Array(1, rowIndex) = New StaticFlowBox2DLE(isWall:=False, keptDensity:=1.4, keptVelocity:=keptVelocity, viscosity:=viscosity)
            Me.Array(Me.ColumnCount - 2, rowIndex) = New StaticFlowBox2DLE(isWall:=False, keptDensity:=1, keptVelocity:=keptVelocity, viscosity:=viscosity)
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


    Public Sub NextTimeStep() Implements IFlowPanel2D(Of FlowBox2DLE).NextTimeStep
        For Each flowBox In Me.Array
            flowBox.NextTimeStep()
        Next

        setFNewFromFOldAndFEq()

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
                    Dim transportedF = startFlowBox.Lambda * startFlowBox.FEq(innerColumnIndex, innerRowIndex) + (1 - startFlowBox.Lambda) * startFlowBox.FOld(innerColumnIndex, innerRowIndex)
                    If targetFlowBox.IsWall Then
                        startFlowBox.FNew(-innerColumnIndex + 2, -innerRowIndex + 2) = transportedF
                    Else
                        targetFlowBox.FNew(innerColumnIndex, innerRowIndex) = transportedF
                    End If
                Next
            Next
        End If
    End Sub

End Class
