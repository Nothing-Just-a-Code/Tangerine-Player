Imports DevExpress.XtraTab

Public Class Credits
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        OpenLink("https://code.videolan.org/videolan/LibVLCSharp")
    End Sub

    Private Sub OpenLink(ByVal url As String)
        Process.Start(New ProcessStartInfo() With {.FileName = url, .UseShellExecute = True})
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        OpenLink("https://github.com/jerry08/SoundCloudExplode")
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        OpenLink("https://www.newtonsoft.com/json")
    End Sub

    Private Async Sub XtraTabControl1_SelectedPageChanged(sender As Object, e As TabPageChangedEventArgs) Handles XtraTabControl1.SelectedPageChanged
        If e.Page.Name = "XtraTabPage2" Then
            ver.Text = GetCurrentVersion()
            Dim newver As Boolean = Await IsNewVersionAvailable()
            If newver = True Then
                CType(Me.Owner, MainForm).ShowAlert("New Version Available", "A new is available to download.", My.Resources.tangerine_1_)
                SimpleButton4.Visible = True
                updsts.Text = "A new version is available to download. Click the button below to Download the latest version."
            Else
                CType(Me.Owner, MainForm).ShowAlert("No Update Available", "You're using the latest version.", MainForm.AlertType.Information)
                updsts.Text = $"Nice!{Environment.NewLine}You're using the latest version of Tangerine Player."
            End If
        End If
    End Sub

    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        OpenLink("https://github.com/Nothing-Just-a-Code/Tangerine-Player/releases")
    End Sub
End Class