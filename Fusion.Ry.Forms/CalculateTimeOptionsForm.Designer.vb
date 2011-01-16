<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CalculateTimeOptionsForm
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
        Me.fixTimeRadioButton = New System.Windows.Forms.RadioButton()
        Me.fixPixelCountRadioButton = New System.Windows.Forms.RadioButton()
        Me.fixTimeTextBox = New System.Windows.Forms.TextBox()
        Me.fixPixelCountTextBox = New System.Windows.Forms.TextBox()
        Me.okButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'fixTimeRadioButton
        '
        Me.fixTimeRadioButton.AutoSize = True
        Me.fixTimeRadioButton.Checked = True
        Me.fixTimeRadioButton.Location = New System.Drawing.Point(12, 13)
        Me.fixTimeRadioButton.Name = "fixTimeRadioButton"
        Me.fixTimeRadioButton.Size = New System.Drawing.Size(124, 21)
        Me.fixTimeRadioButton.TabIndex = 0
        Me.fixTimeRadioButton.TabStop = True
        Me.fixTimeRadioButton.Text = "Fix test time (s)"
        Me.fixTimeRadioButton.UseVisualStyleBackColor = True
        '
        'fixPixelRadioButton
        '
        Me.fixPixelCountRadioButton.AutoSize = True
        Me.fixPixelCountRadioButton.Location = New System.Drawing.Point(12, 41)
        Me.fixPixelCountRadioButton.Name = "fixPixelCountRadioButton"
        Me.fixPixelCountRadioButton.Size = New System.Drawing.Size(144, 21)
        Me.fixPixelCountRadioButton.TabIndex = 1
        Me.fixPixelCountRadioButton.Text = "Fix test pixel count"
        Me.fixPixelCountRadioButton.UseVisualStyleBackColor = True
        '
        'fixTimeTextBox
        '
        Me.fixTimeTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fixTimeTextBox.Location = New System.Drawing.Point(204, 12)
        Me.fixTimeTextBox.Name = "fixTimeTextBox"
        Me.fixTimeTextBox.Size = New System.Drawing.Size(100, 22)
        Me.fixTimeTextBox.TabIndex = 2
        Me.fixTimeTextBox.Text = "0,5"
        '
        'fixPixelCountTextBox
        '
        Me.fixPixelCountTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fixPixelCountTextBox.Enabled = False
        Me.fixPixelCountTextBox.Location = New System.Drawing.Point(204, 41)
        Me.fixPixelCountTextBox.Name = "fixPixelCountTextBox"
        Me.fixPixelCountTextBox.Size = New System.Drawing.Size(100, 22)
        Me.fixPixelCountTextBox.TabIndex = 3
        Me.fixPixelCountTextBox.Text = "1000"
        '
        'okButton
        '
        Me.okButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.okButton.Location = New System.Drawing.Point(204, 69)
        Me.okButton.Name = "okButton"
        Me.okButton.Size = New System.Drawing.Size(100, 29)
        Me.okButton.TabIndex = 4
        Me.okButton.Text = "Ok"
        Me.okButton.UseVisualStyleBackColor = True
        '
        'CalculateTimeOptionsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(316, 114)
        Me.Controls.Add(Me.okButton)
        Me.Controls.Add(Me.fixPixelCountTextBox)
        Me.Controls.Add(Me.fixTimeTextBox)
        Me.Controls.Add(Me.fixPixelCountRadioButton)
        Me.Controls.Add(Me.fixTimeRadioButton)
        Me.Name = "CalculateTimeOptionsForm"
        Me.Text = "Calculate time options"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents fixTimeRadioButton As System.Windows.Forms.RadioButton
    Private WithEvents fixPixelCountRadioButton As System.Windows.Forms.RadioButton
    Private WithEvents fixTimeTextBox As System.Windows.Forms.TextBox
    Private WithEvents fixPixelCountTextBox As System.Windows.Forms.TextBox
    Private WithEvents okButton As System.Windows.Forms.Button
End Class
