<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SphereParticleControl
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
        Me.lblMass = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cldColor = New System.Windows.Forms.ColorDialog()
        Me.btnChangeColor = New System.Windows.Forms.Button()
        Me.pbxColor = New System.Windows.Forms.PictureBox()
        Me.lbl5 = New System.Windows.Forms.Label()
        Me.vbxRadius = New Fusion.Forms.ValueBox()
        Me.vbxCharge = New Fusion.Forms.ValueBox()
        Me.vbxMass = New Fusion.Forms.ValueBox()
        Me.v2bLocation = New Fusion.Forms.Vector2DBox()
        Me.v2bVelocity = New Fusion.Forms.Vector2DBox()
        CType(Me.pbxColor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblMass
        '
        Me.lblMass.AutoSize = True
        Me.lblMass.Location = New System.Drawing.Point(0, 4)
        Me.lblMass.Name = "lblMass"
        Me.lblMass.Size = New System.Drawing.Size(32, 13)
        Me.lblMass.TabIndex = 2
        Me.lblMass.Text = "Mass"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(0, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Charge"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(0, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Location"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(0, 82)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(44, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Velocity"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(0, 134)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(31, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Color"
        '
        'btnChangeColor
        '
        Me.btnChangeColor.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnChangeColor.Location = New System.Drawing.Point(222, 129)
        Me.btnChangeColor.Name = "btnChangeColor"
        Me.btnChangeColor.Size = New System.Drawing.Size(81, 23)
        Me.btnChangeColor.TabIndex = 6
        Me.btnChangeColor.Text = "Change..."
        Me.btnChangeColor.UseVisualStyleBackColor = True
        '
        'pbxColor
        '
        Me.pbxColor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbxColor.Location = New System.Drawing.Point(48, 130)
        Me.pbxColor.Name = "pbxColor"
        Me.pbxColor.Size = New System.Drawing.Size(168, 22)
        Me.pbxColor.TabIndex = 12
        Me.pbxColor.TabStop = False
        '
        'lbl5
        '
        Me.lbl5.AutoSize = True
        Me.lbl5.Location = New System.Drawing.Point(0, 108)
        Me.lbl5.Name = "lbl5"
        Me.lbl5.Size = New System.Drawing.Size(40, 13)
        Me.lbl5.TabIndex = 13
        Me.lbl5.Text = "Radius"
        '
        'vbxRadius
        '
        Me.vbxRadius.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vbxRadius.Location = New System.Drawing.Point(48, 104)
        Me.vbxRadius.Name = "vbxRadius"
        Me.vbxRadius.Size = New System.Drawing.Size(255, 20)
        Me.vbxRadius.TabIndex = 5
        '
        'vbxCharge
        '
        Me.vbxCharge.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vbxCharge.Location = New System.Drawing.Point(48, 26)
        Me.vbxCharge.Name = "vbxCharge"
        Me.vbxCharge.Size = New System.Drawing.Size(255, 20)
        Me.vbxCharge.TabIndex = 2
        '
        'vbxMass
        '
        Me.vbxMass.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vbxMass.Location = New System.Drawing.Point(48, 0)
        Me.vbxMass.Name = "vbxMass"
        Me.vbxMass.Size = New System.Drawing.Size(255, 20)
        Me.vbxMass.TabIndex = 1
        '
        'v2bLocation
        '
        Me.v2bLocation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.v2bLocation.Location = New System.Drawing.Point(48, 52)
        Me.v2bLocation.Name = "v2bLocation"
        Me.v2bLocation.Size = New System.Drawing.Size(255, 20)
        Me.v2bLocation.TabIndex = 3
        '
        'v2bVelocity
        '
        Me.v2bVelocity.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.v2bVelocity.Location = New System.Drawing.Point(48, 78)
        Me.v2bVelocity.Name = "v2bVelocity"
        Me.v2bVelocity.Size = New System.Drawing.Size(255, 20)
        Me.v2bVelocity.TabIndex = 4
        '
        'SphereParticleControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.v2bVelocity)
        Me.Controls.Add(Me.v2bLocation)
        Me.Controls.Add(Me.vbxRadius)
        Me.Controls.Add(Me.lbl5)
        Me.Controls.Add(Me.pbxColor)
        Me.Controls.Add(Me.btnChangeColor)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.vbxCharge)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.vbxMass)
        Me.Controls.Add(Me.lblMass)
        Me.Name = "SphereParticleControl"
        Me.Size = New System.Drawing.Size(303, 152)
        CType(Me.pbxColor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblMass As System.Windows.Forms.Label
    Friend WithEvents vbxMass As Fusion.Forms.ValueBox
    Friend WithEvents vbxCharge As Fusion.Forms.ValueBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cldColor As System.Windows.Forms.ColorDialog
    Friend WithEvents btnChangeColor As System.Windows.Forms.Button
    Friend WithEvents pbxColor As System.Windows.Forms.PictureBox
    Friend WithEvents vbxRadius As Fusion.Forms.ValueBox
    Friend WithEvents lbl5 As System.Windows.Forms.Label
    Friend WithEvents v2bLocation As Fusion.Forms.Vector2DBox
    Friend WithEvents v2bVelocity As Fusion.Forms.Vector2DBox
End Class
