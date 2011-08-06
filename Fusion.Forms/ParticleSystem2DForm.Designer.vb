<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ParticleSystem2DForm
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.pictureBox = New System.Windows.Forms.PictureBox()
        Me.menuStrip = New System.Windows.Forms.MenuStrip()
        Me.fileMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.newMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.openMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.openExampleMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.dipoleMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.moonEarthSystemMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.saveMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.closeMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mapMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.showAllMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.timeMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.startStopMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.openFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.saveFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.statusStrip = New System.Windows.Forms.StatusStrip()
        Me.statusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.modesGroupBox = New System.Windows.Forms.GroupBox()
        Me.selectAndViewRadioButton = New System.Windows.Forms.RadioButton()
        Me.dragAndViewRadioButton = New System.Windows.Forms.RadioButton()
        Me.addForceButton = New System.Windows.Forms.CheckBox()
        Me.addParticleButton = New System.Windows.Forms.Button()
        Me.changeButton = New System.Windows.Forms.Button()
        Me.startStopButton = New System.Windows.Forms.Button()
        Me.splitContainer = New System.Windows.Forms.SplitContainer()
        Me.energyLabel = New System.Windows.Forms.Label()
        Me.momentumLabel = New System.Windows.Forms.Label()
        CType(Me.pictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.menuStrip.SuspendLayout()
        Me.statusStrip.SuspendLayout()
        Me.modesGroupBox.SuspendLayout()
        CType(Me.splitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContainer.Panel1.SuspendLayout()
        Me.splitContainer.Panel2.SuspendLayout()
        Me.splitContainer.SuspendLayout()
        Me.SuspendLayout()
        '
        'pictureBox
        '
        Me.pictureBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pictureBox.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.pictureBox.Location = New System.Drawing.Point(0, 0)
        Me.pictureBox.Name = "pictureBox"
        Me.pictureBox.Size = New System.Drawing.Size(390, 452)
        Me.pictureBox.TabIndex = 0
        Me.pictureBox.TabStop = False
        '
        'menuStrip
        '
        Me.menuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.fileMenuItem, Me.mapMenuItem, Me.timeMenuItem})
        Me.menuStrip.Location = New System.Drawing.Point(0, 0)
        Me.menuStrip.Name = "menuStrip"
        Me.menuStrip.Size = New System.Drawing.Size(554, 24)
        Me.menuStrip.TabIndex = 2
        Me.menuStrip.Text = "MenuStrip1"
        '
        'fileMenuItem
        '
        Me.fileMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.newMenuItem, Me.openMenuItem, Me.openExampleMenuItem, Me.saveMenuItem, Me.closeMenuItem})
        Me.fileMenuItem.Name = "fileMenuItem"
        Me.fileMenuItem.Size = New System.Drawing.Size(108, 20)
        Me.fileMenuItem.Text = "Particle system file"
        '
        'newMenuItem
        '
        Me.newMenuItem.Name = "newMenuItem"
        Me.newMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.newMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.newMenuItem.Text = "New"
        '
        'openMenuItem
        '
        Me.openMenuItem.Name = "openMenuItem"
        Me.openMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.openMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.openMenuItem.Text = "Open..."
        '
        'openExampleMenuItem
        '
        Me.openExampleMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.dipoleMenuItem, Me.moonEarthSystemMenuItem})
        Me.openExampleMenuItem.Name = "openExampleMenuItem"
        Me.openExampleMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.openExampleMenuItem.Text = "Open example"
        '
        'dipoleMenuItem
        '
        Me.dipoleMenuItem.Name = "dipoleMenuItem"
        Me.dipoleMenuItem.Size = New System.Drawing.Size(177, 22)
        Me.dipoleMenuItem.Text = "Dipole"
        '
        'moonEarthSystemMenuItem
        '
        Me.moonEarthSystemMenuItem.Name = "moonEarthSystemMenuItem"
        Me.moonEarthSystemMenuItem.Size = New System.Drawing.Size(177, 22)
        Me.moonEarthSystemMenuItem.Text = "Moon earth system"
        '
        'saveMenuItem
        '
        Me.saveMenuItem.Enabled = False
        Me.saveMenuItem.Name = "saveMenuItem"
        Me.saveMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.saveMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.saveMenuItem.Text = "Save..."
        '
        'closeMenuItem
        '
        Me.closeMenuItem.Enabled = False
        Me.closeMenuItem.Name = "closeMenuItem"
        Me.closeMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.closeMenuItem.Text = "Close"
        '
        'mapMenuItem
        '
        Me.mapMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.showAllMenuItem})
        Me.mapMenuItem.Name = "mapMenuItem"
        Me.mapMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.mapMenuItem.Text = "Map"
        '
        'showAllMenuItem
        '
        Me.showAllMenuItem.Name = "showAllMenuItem"
        Me.showAllMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.showAllMenuItem.Text = "Show all particles"
        '
        'timeMenuItem
        '
        Me.timeMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.startStopMenuItem})
        Me.timeMenuItem.Name = "timeMenuItem"
        Me.timeMenuItem.Size = New System.Drawing.Size(41, 20)
        Me.timeMenuItem.Text = "Time"
        '
        'startStopMenuItem
        '
        Me.startStopMenuItem.Name = "startStopMenuItem"
        Me.startStopMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.startStopMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.startStopMenuItem.Text = "Start"
        '
        'statusStrip
        '
        Me.statusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.statusLabel})
        Me.statusStrip.Location = New System.Drawing.Point(0, 481)
        Me.statusStrip.Name = "statusStrip"
        Me.statusStrip.Size = New System.Drawing.Size(554, 22)
        Me.statusStrip.TabIndex = 7
        Me.statusStrip.Text = "StatusStrip1"
        '
        'statusLabel
        '
        Me.statusLabel.Name = "statusLabel"
        Me.statusLabel.Size = New System.Drawing.Size(54, 17)
        Me.statusLabel.Text = "Welcome!"
        '
        'modesGroupBox
        '
        Me.modesGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.modesGroupBox.Controls.Add(Me.selectAndViewRadioButton)
        Me.modesGroupBox.Controls.Add(Me.dragAndViewRadioButton)
        Me.modesGroupBox.Location = New System.Drawing.Point(3, 119)
        Me.modesGroupBox.Name = "modesGroupBox"
        Me.modesGroupBox.Size = New System.Drawing.Size(155, 66)
        Me.modesGroupBox.TabIndex = 8
        Me.modesGroupBox.TabStop = False
        Me.modesGroupBox.Text = "Modes"
        '
        'selectAndViewRadioButton
        '
        Me.selectAndViewRadioButton.AutoSize = True
        Me.selectAndViewRadioButton.Location = New System.Drawing.Point(6, 42)
        Me.selectAndViewRadioButton.Name = "selectAndViewRadioButton"
        Me.selectAndViewRadioButton.Size = New System.Drawing.Size(101, 17)
        Me.selectAndViewRadioButton.TabIndex = 1
        Me.selectAndViewRadioButton.Text = "Select and view"
        Me.selectAndViewRadioButton.UseVisualStyleBackColor = True
        '
        'dragAndViewRadioButton
        '
        Me.dragAndViewRadioButton.AutoSize = True
        Me.dragAndViewRadioButton.Checked = True
        Me.dragAndViewRadioButton.Location = New System.Drawing.Point(6, 19)
        Me.dragAndViewRadioButton.Name = "dragAndViewRadioButton"
        Me.dragAndViewRadioButton.Size = New System.Drawing.Size(94, 17)
        Me.dragAndViewRadioButton.TabIndex = 0
        Me.dragAndViewRadioButton.TabStop = True
        Me.dragAndViewRadioButton.Text = "Drag and view"
        Me.dragAndViewRadioButton.UseVisualStyleBackColor = True
        '
        'addForceButton
        '
        Me.addForceButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.addForceButton.Appearance = System.Windows.Forms.Appearance.Button
        Me.addForceButton.Location = New System.Drawing.Point(3, 61)
        Me.addForceButton.Name = "addForceButton"
        Me.addForceButton.Size = New System.Drawing.Size(155, 23)
        Me.addForceButton.TabIndex = 6
        Me.addForceButton.Text = "Add force..."
        Me.addForceButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.addForceButton.UseVisualStyleBackColor = True
        '
        'addParticleButton
        '
        Me.addParticleButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.addParticleButton.Enabled = False
        Me.addParticleButton.Location = New System.Drawing.Point(3, 32)
        Me.addParticleButton.Name = "addParticleButton"
        Me.addParticleButton.Size = New System.Drawing.Size(155, 23)
        Me.addParticleButton.TabIndex = 4
        Me.addParticleButton.Text = "Add particle..."
        Me.addParticleButton.UseVisualStyleBackColor = True
        '
        'changeButton
        '
        Me.changeButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.changeButton.Enabled = False
        Me.changeButton.Location = New System.Drawing.Point(3, 90)
        Me.changeButton.Name = "changeButton"
        Me.changeButton.Size = New System.Drawing.Size(155, 23)
        Me.changeButton.TabIndex = 3
        Me.changeButton.Text = "Change particle..."
        Me.changeButton.UseVisualStyleBackColor = True
        '
        'startStopButton
        '
        Me.startStopButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.startStopButton.Enabled = False
        Me.startStopButton.Location = New System.Drawing.Point(3, 3)
        Me.startStopButton.Name = "startStopButton"
        Me.startStopButton.Size = New System.Drawing.Size(155, 23)
        Me.startStopButton.TabIndex = 1
        Me.startStopButton.Text = "Start"
        Me.startStopButton.UseVisualStyleBackColor = True
        '
        'splitContainer
        '
        Me.splitContainer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.splitContainer.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.splitContainer.Location = New System.Drawing.Point(0, 27)
        Me.splitContainer.Name = "splitContainer"
        '
        'splitContainer.Panel1
        '
        Me.splitContainer.Panel1.Controls.Add(Me.pictureBox)
        Me.splitContainer.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No
        '
        'splitContainer.Panel2
        '
        Me.splitContainer.Panel2.Controls.Add(Me.energyLabel)
        Me.splitContainer.Panel2.Controls.Add(Me.momentumLabel)
        Me.splitContainer.Panel2.Controls.Add(Me.startStopButton)
        Me.splitContainer.Panel2.Controls.Add(Me.modesGroupBox)
        Me.splitContainer.Panel2.Controls.Add(Me.changeButton)
        Me.splitContainer.Panel2.Controls.Add(Me.addParticleButton)
        Me.splitContainer.Panel2.Controls.Add(Me.addForceButton)
        Me.splitContainer.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.splitContainer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.splitContainer.Size = New System.Drawing.Size(554, 451)
        Me.splitContainer.SplitterDistance = 389
        Me.splitContainer.TabIndex = 9
        '
        'energyLabel
        '
        Me.energyLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.energyLabel.AutoSize = True
        Me.energyLabel.Location = New System.Drawing.Point(6, 419)
        Me.energyLabel.Name = "energyLabel"
        Me.energyLabel.Size = New System.Drawing.Size(0, 13)
        Me.energyLabel.TabIndex = 10
        '
        'momentumLabel
        '
        Me.momentumLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.momentumLabel.AutoSize = True
        Me.momentumLabel.Location = New System.Drawing.Point(6, 436)
        Me.momentumLabel.Name = "momentumLabel"
        Me.momentumLabel.Size = New System.Drawing.Size(0, 13)
        Me.momentumLabel.TabIndex = 9
        '
        'ParticleSystem2DForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(554, 503)
        Me.Controls.Add(Me.splitContainer)
        Me.Controls.Add(Me.statusStrip)
        Me.Controls.Add(Me.menuStrip)
        Me.KeyPreview = True
        Me.Name = "ParticleSystem2DForm"
        Me.Text = "Fusion Physics"
        CType(Me.pictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.menuStrip.ResumeLayout(False)
        Me.menuStrip.PerformLayout()
        Me.statusStrip.ResumeLayout(False)
        Me.statusStrip.PerformLayout()
        Me.modesGroupBox.ResumeLayout(False)
        Me.modesGroupBox.PerformLayout()
        Me.splitContainer.Panel1.ResumeLayout(False)
        Me.splitContainer.Panel2.ResumeLayout(False)
        Me.splitContainer.Panel2.PerformLayout()
        CType(Me.splitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContainer.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents menuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents fileMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents openMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents closeMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents saveMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents openExampleMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dipoleMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents moonEarthSystemMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mapMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents showAllMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents timeMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents startStopMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents newMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents openFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents saveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents statusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents statusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents modesGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents selectAndViewRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents dragAndViewRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents addForceButton As System.Windows.Forms.CheckBox
    Friend WithEvents addParticleButton As System.Windows.Forms.Button
    Friend WithEvents changeButton As System.Windows.Forms.Button
    Friend WithEvents startStopButton As System.Windows.Forms.Button
    Friend WithEvents splitContainer As System.Windows.Forms.SplitContainer
    Friend WithEvents energyLabel As System.Windows.Forms.Label
    Friend WithEvents momentumLabel As System.Windows.Forms.Label

End Class
