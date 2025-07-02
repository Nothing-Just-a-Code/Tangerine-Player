Public Class RenamePlaylist

    Private Sub TextEdit1_TextChanged(sender As Object, e As EventArgs) Handles TextEdit1.TextChanged
        Me.SimpleButton1.Enabled = Not IsEmpty(TextEdit1.Text)
    End Sub

    Private Async Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        If AllPlaylists.Any(Function(x) x.PlaylistName.Equals(TextEdit1.Text, StringComparison.OrdinalIgnoreCase)) Then
            MBox("Name Already Exist", "A playlist with this name is already exist.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            TextEdit1.Enabled = False
            'CType(Me.Owner, EditPlaylist).PList.PlaylistName = TextEdit1.Text
            'Await SavePlaylistAsync(CType(Me.Owner, EditPlaylist).PList)
            Await Utilities.RenamePlaylist(TextEdit1.Text, CType(Me.Owner, EditPlaylist).PList)
            MBox("Playlist Renamed", "Playlist has been renamed successfully.", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub RenamePlaylist_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
    End Sub
End Class