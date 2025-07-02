<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditPlaylist
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EditPlaylist))
        Me.PopupMenu1 = New DevExpress.XtraBars.PopupMenu(Me.components)
        Me.editnameCM = New DevExpress.XtraBars.BarSubItem()
        Me.editurlCM = New DevExpress.XtraBars.BarSubItem()
        Me.deletesongCM = New DevExpress.XtraBars.BarSubItem()
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
        Me.playlistlst = New DevExpress.XtraEditors.ImageListBoxControl()
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.songscount = New DevExpress.XtraEditors.LabelControl()
        Me.PLName = New DevExpress.XtraEditors.LabelControl()
        Me.Bar1 = New DevExpress.XtraBars.Bar()
        CType(Me.PopupMenu1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.playlistlst, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.playlistlst.SuspendLayout()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PopupMenu1
        '
        Me.PopupMenu1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.editnameCM), New DevExpress.XtraBars.LinkPersistInfo(Me.editurlCM), New DevExpress.XtraBars.LinkPersistInfo(Me.deletesongCM)})
        Me.PopupMenu1.Manager = Me.BarManager1
        Me.PopupMenu1.Name = "PopupMenu1"
        '
        'editnameCM
        '
        Me.editnameCM.AllowDrawArrow = DevExpress.Utils.DefaultBoolean.[False]
        Me.editnameCM.Caption = "Rename Playlist"
        Me.editnameCM.Id = 0
        Me.editnameCM.ImageOptions.Image = CType(resources.GetObject("editnameCM.ImageOptions.Image"), System.Drawing.Image)
        Me.editnameCM.Name = "editnameCM"
        '
        'editurlCM
        '
        Me.editurlCM.AllowDrawArrow = DevExpress.Utils.DefaultBoolean.[False]
        Me.editurlCM.Caption = "Edit Song URL"
        Me.editurlCM.Id = 1
        Me.editurlCM.ImageOptions.Image = CType(resources.GetObject("editurlCM.ImageOptions.Image"), System.Drawing.Image)
        Me.editurlCM.Name = "editurlCM"
        '
        'deletesongCM
        '
        Me.deletesongCM.AllowDrawArrow = DevExpress.Utils.DefaultBoolean.[False]
        Me.deletesongCM.Caption = "Delete Song"
        Me.deletesongCM.Id = 2
        Me.deletesongCM.ImageOptions.Image = CType(resources.GetObject("deletesongCM.ImageOptions.Image"), System.Drawing.Image)
        Me.deletesongCM.Name = "deletesongCM"
        '
        'BarManager1
        '
        Me.BarManager1.AllowCustomization = False
        Me.BarManager1.AllowQuickCustomization = False
        Me.BarManager1.CloseSubMenusOnMouseLeave = DevExpress.Utils.DefaultBoolean.[True]
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.editnameCM, Me.editurlCM, Me.deletesongCM})
        Me.BarManager1.MaxItemId = 3
        Me.BarManager1.UseAltKeyForMenu = False
        Me.BarManager1.UseF10KeyForMenu = False
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Manager = Me.BarManager1
        Me.barDockControlTop.Size = New System.Drawing.Size(670, 0)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 358)
        Me.barDockControlBottom.Manager = Me.BarManager1
        Me.barDockControlBottom.Size = New System.Drawing.Size(670, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlLeft.Manager = Me.BarManager1
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 358)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(670, 0)
        Me.barDockControlRight.Manager = Me.BarManager1
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 358)
        '
        'playlistlst
        '
        Me.playlistlst.Controls.Add(Me.barDockControlLeft)
        Me.playlistlst.Controls.Add(Me.barDockControlRight)
        Me.playlistlst.Controls.Add(Me.barDockControlBottom)
        Me.playlistlst.Controls.Add(Me.barDockControlTop)
        Me.playlistlst.Dock = System.Windows.Forms.DockStyle.Fill
        Me.playlistlst.Location = New System.Drawing.Point(0, 74)
        Me.playlistlst.Name = "playlistlst"
        Me.playlistlst.Size = New System.Drawing.Size(670, 358)
        Me.playlistlst.TabIndex = 0
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.songscount)
        Me.PanelControl1.Controls.Add(Me.PLName)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(670, 74)
        Me.PanelControl1.TabIndex = 1
        '
        'songscount
        '
        Me.songscount.Location = New System.Drawing.Point(12, 36)
        Me.songscount.Name = "songscount"
        Me.songscount.Size = New System.Drawing.Size(15, 13)
        Me.songscount.TabIndex = 1
        Me.songscount.Text = ". . ."
        '
        'PLName
        '
        Me.PLName.Appearance.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.PLName.Appearance.Options.UseFont = True
        Me.PLName.Location = New System.Drawing.Point(12, 12)
        Me.PLName.Name = "PLName"
        Me.PLName.Size = New System.Drawing.Size(17, 17)
        Me.PLName.TabIndex = 0
        Me.PLName.Text = ". . ."
        '
        'Bar1
        '
        Me.Bar1.BarName = "Custom 2"
        Me.Bar1.DockCol = 0
        Me.Bar1.DockRow = 0
        Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.Bar1.Text = "Custom 2"
        '
        'EditPlaylist
        '
        Me.ActiveGlowColor = System.Drawing.Color.Orange
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(670, 432)
        Me.Controls.Add(Me.playlistlst)
        Me.Controls.Add(Me.PanelControl1)
        Me.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Glow
        Me.IconOptions.Image = CType(resources.GetObject("EditPlaylist.IconOptions.Image"), System.Drawing.Image)
        Me.InactiveGlowColor = System.Drawing.Color.LightGray
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EditPlaylist"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "View/Edit Playlist"
        CType(Me.PopupMenu1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.playlistlst, System.ComponentModel.ISupportInitialize).EndInit()
        Me.playlistlst.ResumeLayout(False)
        Me.playlistlst.PerformLayout()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PopupMenu1 As DevExpress.XtraBars.PopupMenu
    Friend WithEvents playlistlst As DevExpress.XtraEditors.ImageListBoxControl
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents songscount As DevExpress.XtraEditors.LabelControl
    Friend WithEvents PLName As DevExpress.XtraEditors.LabelControl
    Friend WithEvents editnameCM As DevExpress.XtraBars.BarSubItem
    Friend WithEvents editurlCM As DevExpress.XtraBars.BarSubItem
    Friend WithEvents deletesongCM As DevExpress.XtraBars.BarSubItem
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
End Class
