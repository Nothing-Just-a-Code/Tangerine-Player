Public Class NewPlaylist

    Private Sub TextEdit1_TextChanged(sender As Object, e As EventArgs) Handles PLName.TextChanged
        CrtBtn.Enabled = Not IsEmpty(PLName.Text)
    End Sub

    Private Sub CrtBtn_Click(sender As Object, e As EventArgs) Handles CrtBtn.Click
        If PLName.Text.Equals("Now Playing", StringComparison.OrdinalIgnoreCase) Then
            MBox("Failed to Create", "Sorry but you cannot create a playlist with this name. Try with a different name.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf AllPlaylists.Any(Function(x) x.PlaylistName.Equals(PLName.Text, StringComparison.OrdinalIgnoreCase)) Then
            MBox("Playlist already Exist", "A playlist with this name is already exist.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            CType(Me.Owner, MainForm).AddNewPlaylist(Me.PLName.Text)
            Me.Close()
        End If

    End Sub
End Class