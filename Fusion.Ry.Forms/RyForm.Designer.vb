<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RyForm
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
        Me.startButton = New System.Windows.Forms.Button()
        Me.buttonPanel = New System.Windows.Forms.Panel()
        Me.customSizeTextBox = New System.Windows.Forms.TextBox()
        Me.customSizeRadioButton = New System.Windows.Forms.RadioButton()
        Me.windowSizeRadioButton = New System.Windows.Forms.RadioButton()
        Me.saveButton = New System.Windows.Forms.Button()
        Me.progressBar = New System.Windows.Forms.ProgressBar()
        Me.pictureBox = New System.Windows.Forms.PictureBox()
        Me.colorPanel = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.saveFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.VideoButton = New System.Windows.Forms.Button()
        Me.buttonPanel.SuspendLayout()
        CType(Me.pictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(3, 3)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(120, 28)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "Trace tha rays!"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'buttonPanel
        '
        Me.buttonPanel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.buttonPanel.Controls.Add(Me.VideoButton)
        Me.buttonPanel.Controls.Add(Me.customSizeTextBox)
        Me.buttonPanel.Controls.Add(Me.customSizeRadioButton)
        Me.buttonPanel.Controls.Add(Me.windowSizeRadioButton)
        Me.buttonPanel.Controls.Add(Me.saveButton)
        Me.buttonPanel.Controls.Add(Me.progressBar)
        Me.buttonPanel.Controls.Add(Me.startButton)
        Me.buttonPanel.Location = New System.Drawing.Point(546, 12)
        Me.buttonPanel.Name = "buttonPanel"
        Me.buttonPanel.Size = New System.Drawing.Size(126, 343)
        Me.buttonPanel.TabIndex = 2
        '
        'customSizeTextBox
        '
        Me.customSizeTextBox.Enabled = False
        Me.customSizeTextBox.Location = New System.Drawing.Point(24, 154)
        Me.customSizeTextBox.Name = "customSizeTextBox"
        Me.customSizeTextBox.Size = New System.Drawing.Size(99, 22)
        Me.customSizeTextBox.TabIndex = 10
        '
        'customSizeRadioButton
        '
        Me.customSizeRadioButton.AutoSize = True
        Me.customSizeRadioButton.Location = New System.Drawing.Point(3, 127)
        Me.customSizeRadioButton.Name = "customSizeRadioButton"
        Me.customSizeRadioButton.Size = New System.Drawing.Size(105, 21)
        Me.customSizeRadioButton.TabIndex = 9
        Me.customSizeRadioButton.Text = "Custom size"
        Me.customSizeRadioButton.UseVisualStyleBackColor = True
        '
        'windowSizeRadioButton
        '
        Me.windowSizeRadioButton.AutoSize = True
        Me.windowSizeRadioButton.Checked = True
        Me.windowSizeRadioButton.Location = New System.Drawing.Point(3, 100)
        Me.windowSizeRadioButton.Name = "windowSizeRadioButton"
        Me.windowSizeRadioButton.Size = New System.Drawing.Size(107, 21)
        Me.windowSizeRadioButton.TabIndex = 8
        Me.windowSizeRadioButton.TabStop = True
        Me.windowSizeRadioButton.Text = "Window size"
        Me.windowSizeRadioButton.UseVisualStyleBackColor = True
        '
        'saveButton
        '
        Me.saveButton.Enabled = False
        Me.saveButton.Location = New System.Drawing.Point(3, 66)
        Me.saveButton.Name = "saveButton"
        Me.saveButton.Size = New System.Drawing.Size(120, 28)
        Me.saveButton.TabIndex = 7
        Me.saveButton.Text = "Save..."
        Me.saveButton.UseVisualStyleBackColor = True
        '
        'progressBar
        '
        Me.progressBar.Location = New System.Drawing.Point(3, 37)
        Me.progressBar.Name = "progressBar"
        Me.progressBar.Size = New System.Drawing.Size(120, 23)
        Me.progressBar.TabIndex = 6
        '
        'pictureBox
        '
        Me.pictureBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pictureBox.BackColor = System.Drawing.Color.Black
        Me.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.pictureBox.Location = New System.Drawing.Point(12, 12)
        Me.pictureBox.Name = "pictureBox"
        Me.pictureBox.Size = New System.Drawing.Size(531, 474)
        Me.pictureBox.TabIndex = 3
        Me.pictureBox.TabStop = False
        '
        'colorPanel
        '
        Me.colorPanel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.colorPanel.Location = New System.Drawing.Point(549, 378)
        Me.colorPanel.Name = "colorPanel"
        Me.colorPanel.Size = New System.Drawing.Size(123, 108)
        Me.colorPanel.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(552, 358)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 17)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Color Panel"
        '
        'saveFileDialog
        '
        Me.saveFileDialog.DefaultExt = "bmp"
        Me.saveFileDialog.FileName = "ray tracing picture"
        '
        'VideoButton
        '
        Me.VideoButton.Location = New System.Drawing.Point(3, 182)
        Me.VideoButton.Name = "VideoButton"
        Me.VideoButton.Size = New System.Drawing.Size(128, 110)
        Me.VideoButton.TabIndex = 6
        Me.VideoButton.Text = "Trace'da'vid"
        Me.VideoButton.UseVisualStyleBackColor = True
        '
        'RyForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(684, 498)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.colorPanel)
        Me.Controls.Add(Me.pictureBox)
        Me.Controls.Add(Me.buttonPanel)
        Me.Name = "RyForm"
        Me.Text = "Ry"
        Me.buttonPanel.ResumeLayout(False)
        Me.buttonPanel.PerformLayout()
        CType(Me.pictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents buttonPanel As System.Windows.Forms.Panel
    Private WithEvents pictureBox As System.Windows.Forms.PictureBox
    Private WithEvents colorPanel As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Private WithEvents progressBar As System.Windows.Forms.ProgressBar
    Private WithEvents saveButton As System.Windows.Forms.Button
    Private WithEvents saveFileDialog As System.Windows.Forms.SaveFileDialog
    Private WithEvents customSizeTextBox As System.Windows.Forms.TextBox
    Private WithEvents customSizeRadioButton As System.Windows.Forms.RadioButton
    Private WithEvents windowSizeRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents VideoButton As System.Windows.Forms.Button

End Class
