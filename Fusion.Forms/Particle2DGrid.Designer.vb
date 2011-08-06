<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Particle2DGrid
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.grid = New System.Windows.Forms.DataGridView()
        Me.nameGridColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.massGridColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.chargeGridColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.locationGridColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.velocityGridColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colorGridColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.radiusGridColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.changeColumn = New System.Windows.Forms.DataGridViewButtonColumn()
        CType(Me.grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grid
        '
        Me.grid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.nameGridColumn, Me.massGridColumn, Me.chargeGridColumn, Me.locationGridColumn, Me.velocityGridColumn, Me.colorGridColumn, Me.radiusGridColumn, Me.changeColumn})
        Me.grid.Location = New System.Drawing.Point(0, 0)
        Me.grid.Name = "grid"
        Me.grid.Size = New System.Drawing.Size(100, 100)
        Me.grid.TabIndex = 0
        '
        'nameGridColumn
        '
        Me.nameGridColumn.HeaderText = "Name"
        Me.nameGridColumn.Name = "nameGridColumn"
        '
        'massGridColumn
        '
        Me.massGridColumn.HeaderText = "Mass"
        Me.massGridColumn.Name = "massGridColumn"
        '
        'chargeGridColumn
        '
        Me.chargeGridColumn.HeaderText = "Charge"
        Me.chargeGridColumn.Name = "chargeGridColumn"
        '
        'locationGridColumn
        '
        Me.locationGridColumn.HeaderText = "Location"
        Me.locationGridColumn.Name = "locationGridColumn"
        '
        'velocityGridColumn
        '
        Me.velocityGridColumn.HeaderText = "Velocity"
        Me.velocityGridColumn.Name = "velocityGridColumn"
        '
        'colorGridColumn
        '
        Me.colorGridColumn.HeaderText = "Color"
        Me.colorGridColumn.Name = "colorGridColumn"
        '
        'radiusGridColumn
        '
        Me.radiusGridColumn.HeaderText = "Radius"
        Me.radiusGridColumn.Name = "radiusGridColumn"
        '
        'changeColumn
        '
        Me.changeColumn.HeaderText = "Change"
        Me.changeColumn.Name = "changeColumn"
        Me.changeColumn.ReadOnly = True
        Me.changeColumn.Text = "Change..."
        '
        'Particle2DGrid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grid)
        Me.Name = "Particle2DGrid"
        Me.Size = New System.Drawing.Size(100, 100)
        CType(Me.grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grid As System.Windows.Forms.DataGridView
    Friend WithEvents nameGridColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents massGridColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chargeGridColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents locationGridColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents velocityGridColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colorGridColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents radiusGridColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents changeColumn As System.Windows.Forms.DataGridViewButtonColumn

End Class
