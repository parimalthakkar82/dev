<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMDI
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.KafkaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuProducer = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConnectServerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuConsumer = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConnectServerToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuKakaCommand = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGetTopicList = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WindowsMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.CascadeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TileVerticalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TileHorizontalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ArrangeIconsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.MenuStrip.SuspendLayout()
        Me.StatusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip
        '
        Me.MenuStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.KafkaToolStripMenuItem, Me.ToolsMenu, Me.WindowsMenu})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.MdiWindowListItem = Me.WindowsMenu
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Size = New System.Drawing.Size(843, 28)
        Me.MenuStrip.TabIndex = 5
        Me.MenuStrip.Text = "MenuStrip"
        '
        'KafkaToolStripMenuItem
        '
        Me.KafkaToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuProducer, Me.mnuConsumer, Me.mnuKakaCommand})
        Me.KafkaToolStripMenuItem.Name = "KafkaToolStripMenuItem"
        Me.KafkaToolStripMenuItem.Size = New System.Drawing.Size(60, 24)
        Me.KafkaToolStripMenuItem.Text = "Kafka"
        '
        'mnuProducer
        '
        Me.mnuProducer.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConnectServerToolStripMenuItem})
        Me.mnuProducer.Name = "mnuProducer"
        Me.mnuProducer.Size = New System.Drawing.Size(167, 26)
        Me.mnuProducer.Text = "Producer"
        '
        'ConnectServerToolStripMenuItem
        '
        Me.ConnectServerToolStripMenuItem.Name = "ConnectServerToolStripMenuItem"
        Me.ConnectServerToolStripMenuItem.Size = New System.Drawing.Size(191, 26)
        Me.ConnectServerToolStripMenuItem.Text = "Connect Server"
        '
        'mnuConsumer
        '
        Me.mnuConsumer.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConnectServerToolStripMenuItem1})
        Me.mnuConsumer.Name = "mnuConsumer"
        Me.mnuConsumer.Size = New System.Drawing.Size(167, 26)
        Me.mnuConsumer.Text = "Consumer"
        '
        'ConnectServerToolStripMenuItem1
        '
        Me.ConnectServerToolStripMenuItem1.Name = "ConnectServerToolStripMenuItem1"
        Me.ConnectServerToolStripMenuItem1.Size = New System.Drawing.Size(191, 26)
        Me.ConnectServerToolStripMenuItem1.Text = "Connect Server"
        '
        'mnuKakaCommand
        '
        Me.mnuKakaCommand.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGetTopicList})
        Me.mnuKakaCommand.Name = "mnuKakaCommand"
        Me.mnuKakaCommand.Size = New System.Drawing.Size(167, 26)
        Me.mnuKakaCommand.Text = "Commands"
        '
        'mnuGetTopicList
        '
        Me.mnuGetTopicList.Name = "mnuGetTopicList"
        Me.mnuGetTopicList.Size = New System.Drawing.Size(181, 26)
        Me.mnuGetTopicList.Text = "Get Topic List"
        '
        'ToolsMenu
        '
        Me.ToolsMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OptionsToolStripMenuItem})
        Me.ToolsMenu.Name = "ToolsMenu"
        Me.ToolsMenu.Size = New System.Drawing.Size(58, 24)
        Me.ToolsMenu.Text = "&Tools"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(224, 26)
        Me.OptionsToolStripMenuItem.Text = "&Options"
        '
        'WindowsMenu
        '
        Me.WindowsMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CascadeToolStripMenuItem, Me.TileVerticalToolStripMenuItem, Me.TileHorizontalToolStripMenuItem, Me.CloseAllToolStripMenuItem, Me.ArrangeIconsToolStripMenuItem})
        Me.WindowsMenu.Name = "WindowsMenu"
        Me.WindowsMenu.Size = New System.Drawing.Size(84, 24)
        Me.WindowsMenu.Text = "&Windows"
        '
        'CascadeToolStripMenuItem
        '
        Me.CascadeToolStripMenuItem.Name = "CascadeToolStripMenuItem"
        Me.CascadeToolStripMenuItem.Size = New System.Drawing.Size(190, 26)
        Me.CascadeToolStripMenuItem.Text = "&Cascade"
        '
        'TileVerticalToolStripMenuItem
        '
        Me.TileVerticalToolStripMenuItem.Name = "TileVerticalToolStripMenuItem"
        Me.TileVerticalToolStripMenuItem.Size = New System.Drawing.Size(190, 26)
        Me.TileVerticalToolStripMenuItem.Text = "Tile &Vertical"
        '
        'TileHorizontalToolStripMenuItem
        '
        Me.TileHorizontalToolStripMenuItem.Name = "TileHorizontalToolStripMenuItem"
        Me.TileHorizontalToolStripMenuItem.Size = New System.Drawing.Size(190, 26)
        Me.TileHorizontalToolStripMenuItem.Text = "Tile &Horizontal"
        '
        'CloseAllToolStripMenuItem
        '
        Me.CloseAllToolStripMenuItem.Name = "CloseAllToolStripMenuItem"
        Me.CloseAllToolStripMenuItem.Size = New System.Drawing.Size(190, 26)
        Me.CloseAllToolStripMenuItem.Text = "C&lose All"
        '
        'ArrangeIconsToolStripMenuItem
        '
        Me.ArrangeIconsToolStripMenuItem.Name = "ArrangeIconsToolStripMenuItem"
        Me.ArrangeIconsToolStripMenuItem.Size = New System.Drawing.Size(190, 26)
        Me.ArrangeIconsToolStripMenuItem.Text = "&Arrange Icons"
        '
        'StatusStrip
        '
        Me.StatusStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 532)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Padding = New System.Windows.Forms.Padding(1, 0, 19, 0)
        Me.StatusStrip.Size = New System.Drawing.Size(843, 26)
        Me.StatusStrip.TabIndex = 7
        Me.StatusStrip.Text = "StatusStrip"
        '
        'ToolStripStatusLabel
        '
        Me.ToolStripStatusLabel.Name = "ToolStripStatusLabel"
        Me.ToolStripStatusLabel.Size = New System.Drawing.Size(49, 20)
        Me.ToolStripStatusLabel.Text = "Status"
        '
        'frmMDI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(843, 558)
        Me.Controls.Add(Me.MenuStrip)
        Me.Controls.Add(Me.StatusStrip)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmMDI"
        Me.Text = "frmMDI"
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ArrangeIconsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WindowsMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CascadeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TileVerticalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TileHorizontalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents ToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents MenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents ToolsMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents KafkaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents mnuProducer As ToolStripMenuItem
    Friend WithEvents mnuConsumer As ToolStripMenuItem
    Friend WithEvents mnuKakaCommand As ToolStripMenuItem
    Friend WithEvents mnuGetTopicList As ToolStripMenuItem
    Friend WithEvents ConnectServerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConnectServerToolStripMenuItem1 As ToolStripMenuItem
End Class
