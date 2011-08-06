<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FlowForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.pictureBox = New System.Windows.Forms.PictureBox()
        Me.restartButton = New System.Windows.Forms.Button()
        Me.frameRateTextBox = New System.Windows.Forms.TextBox()
        Me.feqLabel = New System.Windows.Forms.Label()
        Me.calculationRateTextBox = New System.Windows.Forms.TextBox()
        Me.calculationCountLabel = New System.Windows.Forms.Label()
        Me.frameCountLabel = New System.Windows.Forms.Label()
        Me.panel = New System.Windows.Forms.Panel()
        Me.refreshButton = New System.Windows.Forms.Button()
        Me.calculationRateLabel = New System.Windows.Forms.Label()
        Me.startStopButton = New System.Windows.Forms.Button()
        Me.frameRateLabel = New System.Windows.Forms.Label()
        Me.testLabel = New System.Windows.Forms.Label()
        CType(Me.pictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panel.SuspendLayout()
        Me.SuspendLayout()
        '
        'pictureBox
        '
        Me.pictureBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pictureBox.Location = New System.Drawing.Point(16, 15)
        Me.pictureBox.Margin = New System.Windows.Forms.Padding(4)
        Me.pictureBox.Name = "pictureBox"
        Me.pictureBox.Size = New System.Drawing.Size(533, 483)
        Me.pictureBox.TabIndex = 0
        Me.pictureBox.TabStop = False
        '
        'restartButton
        '
        Me.restartButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.restartButton.Location = New System.Drawing.Point(3, 3)
        Me.restartButton.Name = "restartButton"
        Me.restartButton.Size = New System.Drawing.Size(251, 30)
        Me.restartButton.TabIndex = 1
        Me.restartButton.Text = "Restart"
        Me.restartButton.UseVisualStyleBackColor = True
        '
        'frameRateTextBox
        '
        Me.frameRateTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.frameRateTextBox.Location = New System.Drawing.Point(120, 112)
        Me.frameRateTextBox.Name = "frameRateTextBox"
        Me.frameRateTextBox.Size = New System.Drawing.Size(134, 22)
        Me.frameRateTextBox.TabIndex = 2
        Me.frameRateTextBox.Text = "10"
        '
        'feqLabel
        '
        Me.feqLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.feqLabel.AutoSize = True
        Me.feqLabel.Location = New System.Drawing.Point(207, 104)
        Me.feqLabel.Name = "feqLabel"
        Me.feqLabel.Size = New System.Drawing.Size(0, 17)
        Me.feqLabel.TabIndex = 3
        '
        'calculationRateTextBox
        '
        Me.calculationRateTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.calculationRateTextBox.Location = New System.Drawing.Point(120, 137)
        Me.calculationRateTextBox.Name = "calculationRateTextBox"
        Me.calculationRateTextBox.Size = New System.Drawing.Size(134, 22)
        Me.calculationRateTextBox.TabIndex = 4
        Me.calculationRateTextBox.Text = "10"
        '
        'calculationCountLabel
        '
        Me.calculationCountLabel.AutoSize = True
        Me.calculationCountLabel.Location = New System.Drawing.Point(3, 72)
        Me.calculationCountLabel.Name = "calculationCountLabel"
        Me.calculationCountLabel.Size = New System.Drawing.Size(120, 17)
        Me.calculationCountLabel.TabIndex = 5
        Me.calculationCountLabel.Text = "Calculation count:"
        '
        'frameCountLabel
        '
        Me.frameCountLabel.AutoSize = True
        Me.frameCountLabel.Location = New System.Drawing.Point(3, 89)
        Me.frameCountLabel.Name = "frameCountLabel"
        Me.frameCountLabel.Size = New System.Drawing.Size(95, 17)
        Me.frameCountLabel.TabIndex = 6
        Me.frameCountLabel.Text = "Frame count: "
        '
        'panel
        '
        Me.panel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panel.Controls.Add(Me.testLabel)
        Me.panel.Controls.Add(Me.refreshButton)
        Me.panel.Controls.Add(Me.calculationRateLabel)
        Me.panel.Controls.Add(Me.startStopButton)
        Me.panel.Controls.Add(Me.frameRateLabel)
        Me.panel.Controls.Add(Me.restartButton)
        Me.panel.Controls.Add(Me.frameCountLabel)
        Me.panel.Controls.Add(Me.frameRateTextBox)
        Me.panel.Controls.Add(Me.calculationCountLabel)
        Me.panel.Controls.Add(Me.calculationRateTextBox)
        Me.panel.Location = New System.Drawing.Point(556, 15)
        Me.panel.Name = "panel"
        Me.panel.Size = New System.Drawing.Size(257, 249)
        Me.panel.TabIndex = 7
        '
        'refreshButton
        '
        Me.refreshButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.refreshButton.Location = New System.Drawing.Point(3, 165)
        Me.refreshButton.Name = "refreshButton"
        Me.refreshButton.Size = New System.Drawing.Size(251, 30)
        Me.refreshButton.TabIndex = 10
        Me.refreshButton.Text = "Refresh"
        Me.refreshButton.UseVisualStyleBackColor = True
        '
        'calculationRateLabel
        '
        Me.calculationRateLabel.AutoSize = True
        Me.calculationRateLabel.Location = New System.Drawing.Point(3, 140)
        Me.calculationRateLabel.Name = "calculationRateLabel"
        Me.calculationRateLabel.Size = New System.Drawing.Size(110, 17)
        Me.calculationRateLabel.TabIndex = 9
        Me.calculationRateLabel.Text = "Calculation rate:"
        '
        'startStopButton
        '
        Me.startStopButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.startStopButton.Location = New System.Drawing.Point(3, 39)
        Me.startStopButton.Name = "startStopButton"
        Me.startStopButton.Size = New System.Drawing.Size(251, 30)
        Me.startStopButton.TabIndex = 8
        Me.startStopButton.Text = "Start/Stop"
        Me.startStopButton.UseVisualStyleBackColor = True
        '
        'frameRateLabel
        '
        Me.frameRateLabel.AutoSize = True
        Me.frameRateLabel.Location = New System.Drawing.Point(3, 112)
        Me.frameRateLabel.Name = "frameRateLabel"
        Me.frameRateLabel.Size = New System.Drawing.Size(81, 17)
        Me.frameRateLabel.TabIndex = 7
        Me.frameRateLabel.Text = "Frame rate:"
        '
        'testLabel
        '
        Me.testLabel.AutoSize = True
        Me.testLabel.Location = New System.Drawing.Point(3, 198)
        Me.testLabel.Name = "testLabel"
        Me.testLabel.Size = New System.Drawing.Size(36, 17)
        Me.testLabel.TabIndex = 11
        Me.testLabel.Text = "Test"
        '
        'FlowForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(825, 512)
        Me.Controls.Add(Me.panel)
        Me.Controls.Add(Me.feqLabel)
        Me.Controls.Add(Me.pictureBox)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "FlowForm"
        Me.Text = "Fusion Flow"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.pictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panel.ResumeLayout(False)
        Me.panel.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents restartButton As System.Windows.Forms.Button
    Friend WithEvents frameRateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents feqLabel As System.Windows.Forms.Label
    Friend WithEvents calculationRateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents calculationCountLabel As System.Windows.Forms.Label
    Friend WithEvents frameCountLabel As System.Windows.Forms.Label
    Friend WithEvents panel As System.Windows.Forms.Panel
    Friend WithEvents calculationRateLabel As System.Windows.Forms.Label
    Friend WithEvents startStopButton As System.Windows.Forms.Button
    Friend WithEvents frameRateLabel As System.Windows.Forms.Label
    Friend WithEvents refreshButton As System.Windows.Forms.Button
    Friend WithEvents testLabel As System.Windows.Forms.Label
End Class
