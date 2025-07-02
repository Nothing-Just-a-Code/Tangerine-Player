Imports Tangerine.TangerinePlaylist

Public Class ChangeSongURL
    Private SongItem As PlaylistItem

    Private Sub songurl_TextChanged(sender As Object, e As EventArgs) Handles songurl.TextChanged
        SimpleButton1.Enabled = Not IsEmpty(songurl.Text)
    End Sub

    Private Async Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        If Uri.IsWellFormedUriString(songurl.Text, UriKind.Absolute) Then
            songurl.Enabled = False
            'Check if Song URL is a valid Soundcloud URL
            Dim mainform = CType(CType(Me.Owner, EditPlaylist).Owner, MainForm)
            Dim isvalid As Boolean = Await mainform.SC.Tracks.IsUrlValidAsync(songurl.Text)
            If isvalid Then
                SongItem.SongURL = songurl.Text
                Await SavePlaylistAsync(CType(Me.Owner, EditPlaylist).PList)
                Me.Close()
            Else
                MBox("Invalid SoundCloud URL", "This link seems an invalud SoundCloud URL.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        Else
            MBox("Invalid Link", "The song URL seems invalid.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Sub New(ByVal pitem As PlaylistItem)
        InitializeComponent()
        SongItem = pitem
    End Sub
End Class