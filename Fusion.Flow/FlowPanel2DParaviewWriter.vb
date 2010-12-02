Public Class FlowPanel2DParaviewWriter(Of T As IFlowBox2D)
    Public Property FlowPanel As IFlowPanel2D(Of T)
    Public Property Directory As String
    Public Property Name As String


    Public Sub New(ByVal flowPanel As IFlowPanel2D(Of T), ByVal directory As String, ByVal name As String)
        Me.FlowPanel = flowPanel
        Me.Directory = directory
        Me.Name = name
    End Sub

    Private Function getString() As String
        Dim writer = New IO.StringWriter

        writer.WriteLine("# vtk DataFile Version 2.0")
        writer.WriteLine("Simulation Data")
        writer.WriteLine("ASCII")
        writer.WriteLine("DATASET STRUCTURED_POINTS")
        writer.WriteLine("DIMENSIONS " & Me.FlowPanel.ColumnCount.ToString & " " & Me.FlowPanel.RowCount.ToString & " 1")
        writer.WriteLine("ORIGIN 0 0 0")
        writer.WriteLine("SPACING 1 1 1")
        writer.WriteLine("POINT_DATA " & (Me.FlowPanel.ColumnCount * Me.FlowPanel.RowCount).ToString)
        writer.WriteLine("VECTORS Velocity float")
        For rowIndex = 0 To Me.FlowPanel.RowCount - 1
            For columnIndex = 0 To Me.FlowPanel.ColumnCount - 1
                Dim flowBox = Me.FlowPanel.Array(columnIndex, rowIndex)
                writer.WriteLine(flowBox.Velocity.X.ToString & " " & flowBox.Velocity.Y.ToString & " 0")
            Next
        Next

        writer.WriteLine("SCALARS Density float")
        writer.WriteLine("LOOKUP_TABLE default")

        For rowIndex = 0 To Me.FlowPanel.RowCount - 1
            For columnIndex = 0 To Me.FlowPanel.ColumnCount - 1
                Dim flowBox = Me.FlowPanel.Array(columnIndex, rowIndex)
                writer.WriteLine(flowBox.Density.ToString)
            Next
        Next

        Dim writeString = writer.ToString.Replace(",", ".")

        Return writeString
    End Function

    Public Sub WriteToTextFile()
        IO.File.WriteAllText(Me.Directory & "\" & Me.Name & Me.FlowPanel.CalculationCount.ToString & ".vtk", getString)
    End Sub

End Class
