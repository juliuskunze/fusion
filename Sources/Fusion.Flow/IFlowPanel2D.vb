Public Interface IFlowPanel2D(Of T As IFlowBox2D)
    Inherits ICoordinateSystemToArray(Of T)

    ReadOnly Property CalculationCount As Integer
    Sub NextTimeStep()
End Interface
