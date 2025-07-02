<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class NewPlaylist
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(NewPlaylist))
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.CrtBtn = New DevExpress.XtraEditors.SimpleButton()
        Me.PLName = New DevExpress.XtraEditors.TextEdit()
        CType(Me.PLName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabelControl1
        '
        Me.LabelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.LabelControl1.Location = New System.Drawing.Point(12, 12)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(344, 39)
        Me.LabelControl1.TabIndex = 2
        Me.LabelControl1.Text = "A playlist allows you to organize multiple tracks into a collection that can be p" &
    "layed sequentially providing seamless control over continuous music playback."
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Segoe UI Semibold", 8.75!, System.Drawing.FontStyle.Bold)
        Me.LabelControl2.Appearance.Options.UseFont = True
        Me.LabelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.LabelControl2.Location = New System.Drawing.Point(12, 78)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(103, 15)
        Me.LabelControl2.TabIndex = 3
        Me.LabelControl2.Text = "Enter Playlist Name"
        '
        'CrtBtn
        '
        Me.CrtBtn.Enabled = False
        Me.CrtBtn.ImageOptions.Image = CType(resources.GetObject("CrtBtn.ImageOptions.Image"), System.Drawing.Image)
        Me.CrtBtn.Location = New System.Drawing.Point(270, 133)
        Me.CrtBtn.Name = "CrtBtn"
        Me.CrtBtn.Size = New System.Drawing.Size(86, 23)
        Me.CrtBtn.TabIndex = 1
        Me.CrtBtn.Text = "Create"
        '
        'PLName
        '
        Me.PLName.Location = New System.Drawing.Point(121, 72)
        Me.PLName.Name = "PLName"
        Me.PLName.Size = New System.Drawing.Size(214, 28)
        Me.PLName.TabIndex = 0
        '
        'NewPlaylist
        '
        Me.AcceptButton = Me.CrtBtn
        Me.ActiveGlowColor = System.Drawing.Color.Orange
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(368, 168)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.CrtBtn)
        Me.Controls.Add(Me.PLName)
        Me.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Glow
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.IconOptions.Image = CType(resources.GetObject("NewPlaylist.IconOptions.Image"), System.Drawing.Image)
        Me.InactiveGlowColor = System.Drawing.Color.Gray
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "NewPlaylist"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Create New Playlist"
        CType(Me.PLName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PLName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents CrtBtn As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
End Class
