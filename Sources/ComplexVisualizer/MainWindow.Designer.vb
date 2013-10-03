<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainWindow
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
        Me._PictureBox = New System.Windows.Forms.PictureBox()
        Me._ComplexLabel = New System.Windows.Forms.Label()
        CType(Me._PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_PictureBox
        '
        Me._PictureBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._PictureBox.Location = New System.Drawing.Point(12, 12)
        Me._PictureBox.Name = "_PictureBox"
        Me._PictureBox.Size = New System.Drawing.Size(260, 225)
        Me._PictureBox.TabIndex = 0
        Me._PictureBox.TabStop = False
        '
        '_ComplexLabel
        '
        Me._ComplexLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me._ComplexLabel.AutoSize = True
        Me._ComplexLabel.Location = New System.Drawing.Point(12, 240)
        Me._ComplexLabel.Name = "_ComplexLabel"
        Me._ComplexLabel.Size = New System.Drawing.Size(0, 13)
        Me._ComplexLabel.TabIndex = 1
        '
        'MainWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.Controls.Add(Me._ComplexLabel)
        Me.Controls.Add(Me._PictureBox)
        Me.Name = "MainWindow"
        Me.Text = "Complex visualizer"
        CType(Me._PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents _PictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents _ComplexLabel As System.Windows.Forms.Label
End Class
