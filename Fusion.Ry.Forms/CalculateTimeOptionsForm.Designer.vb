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
        Me._fixTimeRadioButton = New System.Windows.Forms.RadioButton()
        Me._fixPixelCountRadioButton = New System.Windows.Forms.RadioButton()
        Me._FixTimeTextBox = New System.Windows.Forms.TextBox()
        Me._FixPixelCountTextBox = New System.Windows.Forms.TextBox()
        Me._okButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'fixTimeRadioButton
        '
        Me._FixTimeRadioButton.AutoSize = True
        Me._FixTimeRadioButton.Checked = True
        Me._FixTimeRadioButton.Location = New System.Drawing.Point(12, 13)
        Me._FixTimeRadioButton.Name = "_FixTimeRadioButton"
        Me._FixTimeRadioButton.Size = New System.Drawing.Size(124, 21)
        Me._FixTimeRadioButton.TabIndex = 0
        Me._FixTimeRadioButton.TabStop = True
        Me._FixTimeRadioButton.Text = "Fix test time (s)"
        Me._FixTimeRadioButton.UseVisualStyleBackColor = True
        '
        'fixPixelRadioButton
        '
        Me._FixPixelCountRadioButton.AutoSize = True
        Me._FixPixelCountRadioButton.Location = New System.Drawing.Point(12, 41)
        Me._FixPixelCountRadioButton.Name = "_FixPixelCountRadioButton"
        Me._FixPixelCountRadioButton.Size = New System.Drawing.Size(144, 21)
        Me._FixPixelCountRadioButton.TabIndex = 1
        Me._FixPixelCountRadioButton.Text = "Fix test pixel count"
        Me._FixPixelCountRadioButton.UseVisualStyleBackColor = True
        '
        'fixTimeTextBox
        '
        Me._FixTimeTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._FixTimeTextBox.Location = New System.Drawing.Point(204, 12)
        Me._FixTimeTextBox.Name = "_FixTimeTextBox"
        Me._FixTimeTextBox.Size = New System.Drawing.Size(100, 22)
        Me._FixTimeTextBox.TabIndex = 2
        Me._FixTimeTextBox.Text = "0,5"
        '
        'fixPixelCountTextBox
        '
        Me._FixPixelCountTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._FixPixelCountTextBox.Enabled = False
        Me._FixPixelCountTextBox.Location = New System.Drawing.Point(204, 41)
        Me._FixPixelCountTextBox.Name = "_FixPixelCountTextBox"
        Me._FixPixelCountTextBox.Size = New System.Drawing.Size(100, 22)
        Me._FixPixelCountTextBox.TabIndex = 3
        Me._FixPixelCountTextBox.Text = "1000"
        '
        'okButton
        '
        Me._okButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._okButton.Location = New System.Drawing.Point(204, 69)
        Me._okButton.Name = "_OkButton"
        Me._okButton.Size = New System.Drawing.Size(100, 29)
        Me._okButton.TabIndex = 4
        Me._okButton.Text = "Ok"
        Me._okButton.UseVisualStyleBackColor = True
        '
        'CalculateTimeOptionsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(316, 114)
        Me.Controls.Add(Me._okButton)
        Me.Controls.Add(Me._FixPixelCountTextBox)
        Me.Controls.Add(Me._FixTimeTextBox)
        Me.Controls.Add(Me._FixPixelCountRadioButton)
        Me.Controls.Add(Me._FixTimeRadioButton)
        Me.Name = "CalculateTimeOptionsForm"
        Me.Text = "Calculate time options"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents _FixTimeRadioButton As System.Windows.Forms.RadioButton
    Private WithEvents _FixPixelCountRadioButton As System.Windows.Forms.RadioButton
    Private WithEvents _FixTimeTextBox As System.Windows.Forms.TextBox
    Private WithEvents _FixPixelCountTextBox As System.Windows.Forms.TextBox
    Private WithEvents _OkButton As System.Windows.Forms.Button
End Class
