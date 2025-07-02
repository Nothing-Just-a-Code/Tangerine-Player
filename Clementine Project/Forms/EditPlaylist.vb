Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors.Controls
Imports Tangerine.TangerinePlaylist

Public Class EditPlaylist
    Public PList As TangerinePlaylist
    Private Sub EditPlaylist_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
        ReadPlaylist()
    End Sub

    Sub New(ByVal pl As TangerinePlaylist)
        InitializeComponent()
        PList = pl
    End Sub

    Private Sub ReadPlaylist()
        PLName.Text = PList.PlaylistName
        songscount.Text = $"{PList.List.Count.ToString()} song(s)."
        playlistlst.BeginUpdate()
        For Each item As TangerinePlaylist.PlaylistItem In PList.List
            Dim PItem As New ImageListBoxItem() With {.Value = item.SongName,
                .Tag = item}
            PItem.ImageOptions.Image = My.Resources.document_3_
            playlistlst.Items.Add(PItem)
        Next
        playlistlst.EndUpdate()
    End Sub

    Private Async Sub deletesongCM_ItemClick(sender As Object, e As ItemClickEventArgs) Handles deletesongCM.ItemClick
        PopupMenu1.HidePopup()
        Dim songitem As TangerinePlaylist.PlaylistItem = CType(CType(Me.playlistlst.SelectedItem, ImageListBoxItem).Tag, PlaylistItem)
        PList.List.Remove(songitem)
        playlistlst.Items.Remove(playlistlst.SelectedItem)
        songscount.Text = $"{PList.List.Count.ToString()} song(s)."
        playlistlst.Enabled = False
        Await SavePlaylistAsync(PList)
        playlistlst.Enabled = True
    End Sub

    Private Sub editnameCM_ItemClick(sender As Object, e As ItemClickEventArgs) Handles editnameCM.ItemClick
        PopupMenu1.HidePopup()
        If playlistlst.SelectedItem Is Nothing Then Exit Sub
        Dim rnplaylist As New RenamePlaylist()
        If rnplaylist.ShowDialog(Me) = DialogResult.OK Then Me.PLName.Text = rnplaylist.TextEdit1.Text
    End Sub

    Private Sub editurlCM_ItemClick(sender As Object, e As ItemClickEventArgs) Handles editurlCM.ItemClick
        PopupMenu1.HidePopup()
        If playlistlst.SelectedItem Is Nothing Then Exit Sub
        PopupMenu1.HidePopup()
        Dim songitem As PlaylistItem = CType(CType(Me.playlistlst.SelectedItem, ImageListBoxItem).Tag, PlaylistItem)
        Dim editurlForm As New ChangeSongURL(songitem)
        editurlForm.ShowDialog(Me)
    End Sub

    Private Sub playlistlst_SelectedIndexChanged(sender As Object, e As EventArgs) Handles playlistlst.SelectedIndexChanged

    End Sub

    Private Sub playlistlst_MouseDown(sender As Object, e As MouseEventArgs) Handles playlistlst.MouseDown
        If e.Button = MouseButtons.Right Then
            If playlistlst.SelectedItem IsNot Nothing Then
                PopupMenu1.ShowPopup(MousePosition)
            End If
        End If
    End Sub
End Class