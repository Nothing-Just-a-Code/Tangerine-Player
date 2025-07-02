<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Credits
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Credits))
        Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
        Me.XtraTabPage1 = New DevExpress.XtraTab.XtraTabPage()
        Me.SimpleButton3 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.XtraTabPage2 = New DevExpress.XtraTab.XtraTabPage()
        Me.ver = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.SimpleButton4 = New DevExpress.XtraEditors.SimpleButton()
        Me.updsts = New DevExpress.XtraEditors.LabelControl()
        CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabControl1.SuspendLayout()
        Me.XtraTabPage1.SuspendLayout()
        Me.XtraTabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'XtraTabControl1
        '
        Me.XtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.XtraTabControl1.Location = New System.Drawing.Point(0, 0)
        Me.XtraTabControl1.Name = "XtraTabControl1"
        Me.XtraTabControl1.SelectedTabPage = Me.XtraTabPage1
        Me.XtraTabControl1.Size = New System.Drawing.Size(473, 296)
        Me.XtraTabControl1.TabIndex = 0
        Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.XtraTabPage1, Me.XtraTabPage2})
        '
        'XtraTabPage1
        '
        Me.XtraTabPage1.Controls.Add(Me.SimpleButton3)
        Me.XtraTabPage1.Controls.Add(Me.SimpleButton2)
        Me.XtraTabPage1.Controls.Add(Me.SimpleButton1)
        Me.XtraTabPage1.Controls.Add(Me.LabelControl1)
        Me.XtraTabPage1.ImageOptions.Image = CType(resources.GetObject("XtraTabPage1.ImageOptions.Image"), System.Drawing.Image)
        Me.XtraTabPage1.Name = "XtraTabPage1"
        Me.XtraTabPage1.Size = New System.Drawing.Size(471, 265)
        Me.XtraTabPage1.Text = "About"
        '
        'SimpleButton3
        '
        Me.SimpleButton3.Location = New System.Drawing.Point(11, 231)
        Me.SimpleButton3.Name = "SimpleButton3"
        Me.SimpleButton3.Size = New System.Drawing.Size(449, 23)
        Me.SimpleButton3.TabIndex = 3
        Me.SimpleButton3.Text = "Newtonsoft.Json  -  Read/Write JSON"
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Location = New System.Drawing.Point(224, 202)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(236, 23)
        Me.SimpleButton2.TabIndex = 2
        Me.SimpleButton2.Text = "SoundCloudExplode  -  SoundCloud SDK"
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Location = New System.Drawing.Point(11, 202)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(204, 23)
        Me.SimpleButton1.TabIndex = 1
        Me.SimpleButton1.Text = "LibVLCSharp  -  Audio Engine"
        '
        'LabelControl1
        '
        Me.LabelControl1.AllowHtmlString = True
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Appearance.Options.UseFont = True
        Me.LabelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.LabelControl1.Location = New System.Drawing.Point(11, 23)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(449, 60)
        Me.LabelControl1.TabIndex = 0
        Me.LabelControl1.Text = resources.GetString("LabelControl1.Text")
        '
        'XtraTabPage2
        '
        Me.XtraTabPage2.Controls.Add(Me.updsts)
        Me.XtraTabPage2.Controls.Add(Me.SimpleButton4)
        Me.XtraTabPage2.Controls.Add(Me.ver)
        Me.XtraTabPage2.Controls.Add(Me.LabelControl2)
        Me.XtraTabPage2.ImageOptions.Image = CType(resources.GetObject("XtraTabPage2.ImageOptions.Image"), System.Drawing.Image)
        Me.XtraTabPage2.Name = "XtraTabPage2"
        Me.XtraTabPage2.Size = New System.Drawing.Size(471, 265)
        Me.XtraTabPage2.Text = "Version"
        '
        'ver
        '
        Me.ver.Location = New System.Drawing.Point(83, 15)
        Me.ver.Name = "ver"
        Me.ver.Size = New System.Drawing.Size(13, 13)
        Me.ver.TabIndex = 1
        Me.ver.Text = "na"
        '
        'LabelControl2
        '
        Me.LabelControl2.Location = New System.Drawing.Point(11, 15)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(66, 13)
        Me.LabelControl2.TabIndex = 0
        Me.LabelControl2.Text = "Your version:"
        '
        'SimpleButton4
        '
        Me.SimpleButton4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.SimpleButton4.Location = New System.Drawing.Point(0, 242)
        Me.SimpleButton4.Name = "SimpleButton4"
        Me.SimpleButton4.Size = New System.Drawing.Size(471, 23)
        Me.SimpleButton4.TabIndex = 2
        Me.SimpleButton4.Text = "Download Now"
        Me.SimpleButton4.Visible = False
        '
        'updsts
        '
        Me.updsts.Appearance.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.updsts.Appearance.Options.UseFont = True
        Me.updsts.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.updsts.Location = New System.Drawing.Point(11, 68)
        Me.updsts.Name = "updsts"
        Me.updsts.Size = New System.Drawing.Size(433, 30)
        Me.updsts.TabIndex = 3
        Me.updsts.Text = "Checking for Update..." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please wait."
        '
        'Credits
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(473, 296)
        Me.Controls.Add(Me.XtraTabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.IconOptions.Image = CType(resources.GetObject("Credits.IconOptions.Image"), System.Drawing.Image)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Credits"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Credits      -  Tangerine Player"
        CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabControl1.ResumeLayout(False)
        Me.XtraTabPage1.ResumeLayout(False)
        Me.XtraTabPage2.ResumeLayout(False)
        Me.XtraTabPage2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents XtraTabPage1 As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents XtraTabPage2 As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton3 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ver As DevExpress.XtraEditors.LabelControl
    Friend WithEvents SimpleButton4 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents updsts As DevExpress.XtraEditors.LabelControl
End Class
