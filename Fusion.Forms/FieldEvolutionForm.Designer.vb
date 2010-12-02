<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FieldEvolutionForm
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
        Me.pictureBox = New System.Windows.Forms.PictureBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.startEvolutionButton = New System.Windows.Forms.Button()
        Me.fitnessLabel = New System.Windows.Forms.Label()
        CType(Me.pictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pictureBox
        '
        Me.pictureBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pictureBox.Location = New System.Drawing.Point(12, 12)
        Me.pictureBox.Name = "pictureBox"
        Me.pictureBox.Size = New System.Drawing.Size(160, 249)
        Me.pictureBox.TabIndex = 0
        Me.pictureBox.TabStop = False
        '
        'startButton
        '
        Me.startButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.startButton.Location = New System.Drawing.Point(178, 12)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(102, 23)
        Me.startButton.TabIndex = 1
        Me.startButton.Text = "Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'startEvolutionButton
        '
        Me.startEvolutionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.startEvolutionButton.Location = New System.Drawing.Point(178, 41)
        Me.startEvolutionButton.Name = "startEvolutionButton"
        Me.startEvolutionButton.Size = New System.Drawing.Size(102, 23)
        Me.startEvolutionButton.TabIndex = 2
        Me.startEvolutionButton.Text = "Start Evolution"
        Me.startEvolutionButton.UseVisualStyleBackColor = True
        '
        'fitnessLabel
        '
        Me.fitnessLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fitnessLabel.AutoSize = True
        Me.fitnessLabel.Location = New System.Drawing.Point(182, 76)
        Me.fitnessLabel.Name = "fitnessLabel"
        Me.fitnessLabel.Size = New System.Drawing.Size(0, 13)
        Me.fitnessLabel.TabIndex = 3
        '
        'FieldEvolutionForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 273)
        Me.Controls.Add(Me.fitnessLabel)
        Me.Controls.Add(Me.startEvolutionButton)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.pictureBox)
        Me.KeyPreview = True
        Me.Name = "FieldEvolutionForm"
        Me.Text = "Field Evolution"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.pictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents startButton As System.Windows.Forms.Button
    Friend WithEvents startEvolutionButton As System.Windows.Forms.Button
    Friend WithEvents fitnessLabel As System.Windows.Forms.Label
End Class
