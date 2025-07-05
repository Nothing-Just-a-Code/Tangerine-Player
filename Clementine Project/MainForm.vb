Imports System.ComponentModel
Imports System.Timers
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Alerter
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraTab
Imports DevExpress.XtraWaitForm
Imports LibVLCSharp.Shared
Imports Newtonsoft.Json
Imports SoundCloudExplode
Imports SoundCloudExplode.Common
Imports SoundCloudExplode.Utils
Imports System.Threading.Tasks

Public Class MainForm
    Public WithEvents SC As SoundCloudClient
    Public WithEvents MP As MediaPlayer
    Private WithEvents LibVlc As LibVLC
    Public NowPlaying As TangerinePlaylist
    Private SearchItems As New List(Of SearchTrackItem)()
    Private WithEvents VolumeSaveTimer As New System.Timers.Timer(1000)
    Public Property IsPaused As Boolean = False
    Public Property IsSeeking As Boolean = False
    Public Property CurrentSongID As Long
    Private Async Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
        TC.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False
        Bar1.Visible = False
        If Await InitClementine() Then
            StartupLoader.Hide()
            Bar1.Visible = True
            MusicPanel.Visible = True
            TC.ShowTabHeader = DevExpress.Utils.DefaultBoolean.Default
            NowPlaying = New TangerinePlaylist() With {.PlaylistName = "Now Playing", .List = New List(Of TangerinePlaylist.PlaylistItem)()}
            Await GetorRefreshPlaylists()
            Me.searchtracks_lst.DataSource = SearchItems
            Await Task.Run(Sub() InitConfig())
        Else
            ShowAlert("Cannot Run Application", "Failed to initialized Tangerine Player.", AlertType.Error)
            TLogger.WriteLog("Failed to start application. Engine initializing failed. Re-install application to fix.")
            Process.GetCurrentProcess().Kill()
        End If
    End Sub

    Sub New()
        InitializeComponent()
        StartupLoader.Dock = DockStyle.Fill
    End Sub

    Private Sub InitConfig()
        'volume
        Me.VolBar.Position = TConfig.Volume

        'Repeatmode
        Select Case TConfig.RepeatMode
            Case TangerineConfig.RepeatType.AllSongs
                Me.RptBtn.SvgImage = My.Resources.RepeatAll
                Me.RptBtn.ToolTip = "Repeat All"
                Exit Select
            Case TangerineConfig.RepeatType.NoRepeat
                Me.RptBtn.SvgImage = My.Resources.RepeatOff
                Me.RptBtn.ToolTip = "Repeat Off"
                Exit Select
            Case TangerineConfig.RepeatType.Single
                Me.RptBtn.SvgImage = My.Resources.RepeatOne
                Me.RptBtn.ToolTip = "Repeat One"
                Exit Select
        End Select

        'Audio Channel
        MP?.SetChannel(TConfig.ChannelOutput)

        'network caching
        Select Case TConfig.Caching
            Case TangerineConfig.CachingType.Low
                MP.NetworkCaching = 700
            Case TangerineConfig.CachingType.Medium
                MP.NetworkCaching = 1800
            Case TangerineConfig.CachingType.High
                MP.NetworkCaching = 3000
        End Select

        'Equalizer
        If TConfig.EnableEqualizer = True Then
            EqL = New Equalizer(TConfig.Equalizer)
            MP.SetEqualizer(EqL)
        Else
            EqL = New Equalizer()
        End If
    End Sub

    Private Async Function InitClementine() As Task(Of Boolean)
        Me.SC = New SoundCloudClient()
        Me.LibVlc = New LibVLC(enableDebugLogs:=False)
        Me.MP = New MediaPlayer(Me.LibVlc)
        Try
            Await SC.InitializeAsync()
            Core.Initialize(Utilities.GetLibVlcEngineDirectory())
            Return SC.IsInitialized
        Catch vlcex As VLCException
            TLogger.WriteLog(vlcex)
            Return False
        Catch ex As Exception
            TLogger.WriteLog(ex)
            Return False
        End Try
    End Function

    Public Async Sub AddSongToNowPlaying(ByVal media As TangerineMedia)
        If Not NowPlaying.SongExist(media.SongID) Then
            If media.SongArtURL Is Nothing Then
                NowPlaying.AddSong(songurl:=media.SongURL, songname:=media.SongName, "", media.SongID)
            Else
                NowPlaying.AddSong(songurl:=media.SongURL, songname:=media.SongName, media.SongArtURL, media.SongID)
            End If
            Dim index As Integer = nowplayinglist.Items.Add(media)
            If Not IsEmpty(media.SongArtURL) Then nowplayinglist.Items.Item(index).ImageOptions.ImageKey = Await GetOrSetSongArt(media.SongID.ToString(), media.SongArtURL)
            nowplayinglist.Items.Item(index).Tag = media
            Await Task.Delay(200)
        End If
    End Sub

    Public Sub RemoveSongFromNowPlaying(ByVal song As TangerineMedia)
        NowPlaying.RemoveSong(song.SongID)
        Dim LItem As ImageListBoxItem = CType(nowplayinglist.Items.Cast(Of Object)().FirstOrDefault(Function(x) CType(x, TangerineMedia).SongID = song.SongID), ImageListBoxItem)
        If LItem IsNot Nothing Then nowplayinglist.Items.Remove(item:=LItem)
    End Sub

    Private Async Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles MuteBtn.Click
        Await ToggleMuteAsync()
    End Sub

    Private Sub MP_TimeChanged(sender As Object, e As MediaPlayerTimeChangedEventArgs) Handles MP.TimeChanged
        If songlen.InvokeRequired Then
            songlen.Invoke(New EventHandler(Of MediaPlayerTimeChangedEventArgs)(AddressOf MP_TimeChanged), New Object() {sender, e})
            Return
        End If
        Me.songlen.Value = CInt(TimeSpan.FromMilliseconds(e.Time).TotalSeconds)
        Me.SongLenLbl.Text = TimeSpan.FromMilliseconds(e.Time).ToString("hh\:mm\:ss")
    End Sub

    Public Async Function IsSongURLValid(ByVal url As String) As Task(Of Boolean)
        Try
            Dim result = Await SC.GetUrlKindAsync(url)
            If result = Kind.Track Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Async Function PlaySong(ByVal songurl As String) As Task
        If InvokeRequired Then
            Invoke(New MethodInvoker(Async Function()
                                         Await Task.Run(Function() PlaySong(songurl))
                                     End Function))
            Return
        End If
        Await Task.Run(Async Function()
                           Dim isValid As Boolean = Await IsSongURLValid(songurl)
                           If isValid Then
                               Dim song = Await SC.Tracks.GetAsync(songurl)
                               Dim down As String = Await SC.Tracks.GetDownloadUrlAsync(song)
                               Dim Tmed As TangerineMedia
                               If song.ArtworkUrl Is Nothing Then
                                   Tmed = New TangerineMedia(songurl, song.Title, "", song.Id)
                               Else
                                   Tmed = New TangerineMedia(songurl, song.Title, song.ArtworkUrl.ToString(), song.Id)
                               End If
                               Dim med As New Media(Me.LibVlc, mrl:=down, FromType.FromLocation)
                               'Parse Media
                               Await med.Parse(MediaParseOptions.ParseNetwork, 30000, Nothing)

                               AddSongToNowPlaying(Tmed)
                               songlen.Properties.Maximum = CInt(TimeSpan.FromMilliseconds(med.Duration).TotalSeconds)
                               SongDur.Text = TimeSpan.FromMilliseconds(med.Duration).ToString("hh\:mm\:ss")
                               MP.Play(med)
                               CurSong.Text = Tmed.SongName
                               CurrentSongID = song.Id
                               songart.Image = Await LoadImageFromURL(Tmed.SongArtURL)
                               ShowAlert(Tmed.SongName, $"Playing", songart.Image)
                           Else
                               ShowAlert("Invalid Song", "The song you added is invalid or has invalid link.", AlertType.Error)
                           End If
                       End Function)
    End Function

    Private Sub AlertControl1_HtmlElementMouseClick(sender As Object, e As AlertHtmlElementMouseEventArgs) Handles AlertControl1.HtmlElementMouseClick
        If e.Element.ClassName = "header-button" OrElse e.Element.ClassName = "close-button" Then e.HtmlPopup.Close()
    End Sub

    Public Sub ShowAlert(ByVal title As String, ByVal text As String, Optional ByVal type As AlertType = AlertType.None)
        If TConfig.ShowNotifications = False Then Exit Sub
        Select Case type
            Case AlertType.None
                AlertControl1.Show(Me, title, text, ImageCollection2.Images.Item("tangerine"))
            Case AlertType.Error
                AlertControl1.Show(Me, title, text, ImageCollection2.Images.Item("error"))
            Case AlertType.Information
                AlertControl1.Show(Me, title, text, ImageCollection2.Images.Item("information"))
            Case AlertType.Success
                AlertControl1.Show(Me, title, text, ImageCollection2.Images.Item("success"))
            Case AlertType.Warning
                AlertControl1.Show(Me, title, text, ImageCollection2.Images.Item("warning"))
        End Select
    End Sub

    Public Sub ShowAlert(ByVal title As String, ByVal text As String, ByVal image As Image)
        If TConfig.ShowNotifications = False Then Exit Sub
        AlertControl1.Show(Me, title, text, image)
    End Sub

    Enum AlertType As Integer
        [Error]
        Success
        Warning
        Information
        None
    End Enum

    Private Sub ProgressBarControl1_PositionChanged(sender As Object, e As EventArgs) Handles VolBar.PositionChanged
        If InvokeRequired Then
            Invoke(New EventHandler(AddressOf ProgressBarControl1_PositionChanged), New Object() {sender, e})
            Return
        End If
        If VolumeSaveTimer.Enabled = True Then VolumeSaveTimer.Stop()
        VolumeSaveTimer.Start()
    End Sub

    Private Sub ChangeProgress(e As MouseEventArgs)
        If InvokeRequired Then
            Invoke(Sub()
                       If e.Button = Windows.Forms.MouseButtons.Left Then
                           Dim mousepos = Math.Min(Math.Max(e.X, 0), VolBar.ClientSize.Width)
                           Dim value = CInt(VolBar.Properties.Minimum + (VolBar.Properties.Maximum - VolBar.Properties.Minimum) * mousepos / VolBar.ClientSize.Width)
                           If value > VolBar.Position And value < VolBar.Properties.Maximum Then
                               VolBar.Position = value + 1
                               VolBar.Position = value
                           Else
                               VolBar.Position = value
                           End If
                       End If
                   End Sub)
        Else
            If e.Button = Windows.Forms.MouseButtons.Left Then
                Dim mousepos = Math.Min(Math.Max(e.X, 0), VolBar.ClientSize.Width)
                Dim value = CInt(VolBar.Properties.Minimum + (VolBar.Properties.Maximum - VolBar.Properties.Minimum) * mousepos / VolBar.ClientSize.Width)
                If value > VolBar.Position And value < VolBar.Properties.Maximum Then
                    VolBar.Position = value + 1
                    VolBar.Position = value
                Else
                    VolBar.Position = value
                End If
            End If
        End If
    End Sub

    Private Sub VolBar_MouseDown(sender As Object, e As MouseEventArgs) Handles VolBar.MouseDown
        If VolBar.InvokeRequired Then
            VolBar.Invoke(New MouseEventHandler(AddressOf VolBar_MouseDown), New Object() {sender, e})
            Return
        End If
        ChangeProgress(e)
    End Sub

    Private Sub VolBar_MouseMove(sender As Object, e As MouseEventArgs) Handles VolBar.MouseMove
        If VolBar.InvokeRequired Then
            VolBar.Invoke(New MouseEventHandler(AddressOf VolBar_MouseMove), New Object() {sender, e})
            Return
        End If
        ChangeProgress(e)
    End Sub

    Public Async Function GetOrSetSongArt(ByVal songid As String, ByVal arturl As String) As Task(Of String)
        If nowplayingimglst.Images.Keys.Contains(songid) Then
            Return songid
        Else
            Dim img As Image = Await Utilities.LoadImageFromURL(arturl)
            nowplayingimglst.AddImage(img, songid)
            Return songid
        End If
    End Function

    Public Async Function GetorRefreshPlaylists() As Task
        Await Task.Run(Sub()
                           If IO.Directory.Exists(Utilities.PlaylistPath) Then
                               If Not AllPlaylists.Count = 0 Then
                                   AllPlaylists.Clear()
                                   playlists_lst.Items.Clear()
                                   BarSubItem4.ClearLinks()
                               End If
                               playlists_lst.BeginUpdate()
                               Dim playlistindex As Integer = 0
                               For Each item As String In IO.Directory.GetFiles(PlaylistPath, "*.tpl")
                                   Dim PL As TangerinePlaylist = JsonConvert.DeserializeObject(Of TangerinePlaylist)(IO.File.ReadAllText(item, System.Text.Encoding.UTF8))
                                   AllPlaylists.Add(PL)
                                   Dim PLItem As New ImageListBoxItem() With {
                                       .Value = PL.PlaylistName,
                                       .Tag = PL}
                                   PLItem.ImageOptions.ImageIndex = 0
                                   playlists_lst.Items.Add(PLItem)
                                   Dim PLMenu As New BarSubItem() With {.Caption = PL.PlaylistName,
                                   .Tag = PL,
                                   .Name = $"playlist_{playlistindex}"}
                                   PLMenu.ImageOptions.Image = My.Resources.cd_burning
                                   PLMenu.AllowDrawArrow = DevExpress.Utils.DefaultBoolean.False
                                   BarSubItem4.AddItem(PLMenu)
                                   playlistindex += 1
                               Next
                               playlists_lst.EndUpdate()
                           End If
                       End Sub)
    End Function


    Private Async Sub searchtrack_KeyPress(sender As Object, e As KeyPressEventArgs) Handles searchtrack.KeyPress
        If e.KeyChar = ChrW(13) Then
            e.Handled = True
            Dim loadScreen As New ProgressPanel() With {.Caption = "Searching.", .Description = "Please wait...", .Dock = DockStyle.Fill, .ContentAlignment = ContentAlignment.MiddleCenter}
            loadScreen.Parent = searchtracks_lst
            If Not SearchItems.Count = 0 Then SearchItems.Clear()
            searchtrack.Enabled = False
            Dim Trackenum = SC.Search.GetTracksAsync(Me.searchtrack.Text).GetAsyncEnumerator()
            If Await Trackenum.MoveNextAsync() = False Then
                ShowAlert("No Tracks Found", "Please try with different keyword.", My.Resources.cancel_1_)
                Return
            End If
            searchtracks_lst.BeginUpdate()
            Try
                Dim placeholderImage = My.Resources.tangerine_1_

                While Await Trackenum.MoveNextAsync()
                    Dim song = Trackenum.Current

                    If song.Kind = Kind.Track Then
                        ' Create item with placeholder image
                        Dim IT As New SearchTrackItem() With {
                            .AuthorName = If(Not IsEmpty(song.User.FullName), song.User.FullName, "<no name>"),
                            .Duration = TimeSpanToWords(TimeSpan.FromMilliseconds(song.Duration)),
                            .SongID = song.Id,
                            .SongName = song.Title,
                            .SongURL = song.Uri.ToString(),
                            .PermaLink = song.PermalinkUrl.ToString(),
                            .ArtImage = placeholderImage,
                            .ArtImageURL = If(song.ArtworkUrl IsNot Nothing, song.ArtworkUrl.ToString(), Nothing)
                        }

                        ' Add to list
                        SearchItems.Add(IT)

                        ' Start async task to load image in background
                        If IT.ArtImageURL IsNot Nothing Then
                            Task.Run(Async Function()
                                         Dim image = Await LoadImageFromURL(IT.ArtImageURL)
                                         If image IsNot Nothing Then
                                             IT.ArtImage = image
                                             searchtracks_lst.Invoke(Sub()
                                                                         ' Optionally refresh UI item here
                                                                         searchtracks_lst.Refresh()
                                                                     End Sub)
                                         End If
                                     End Function)
                        End If
                    End If
                End While
            Finally
                Trackenum.DisposeAsync()
                searchtracks_lst.EndUpdate()
                searchtrack.Enabled = True
                loadScreen.Dispose()
            End Try

        End If
    End Sub

    Private Async Sub VolumeSaveTimer_Elapsed(sender As Object, e As ElapsedEventArgs) Handles VolumeSaveTimer.Elapsed
        If MP IsNot Nothing Then
            Await ChangeVolumeAsync(VolBar.Position)
            TConfig.Volume = Me.MP.Volume
            Await SaveConfig()
            VolumeSaveTimer.Stop()
        Else
            VolumeSaveTimer.Stop()
        End If

    End Sub

    Private Sub SearchListCM_BeforePopup(sender As Object, e As CancelEventArgs) Handles SearchListCM.BeforePopup
        If Me.SearchItems.Count = 0 OrElse searchtracks_lst.SelectedItem Is Nothing Then
            e.Cancel = True
            Exit Sub
        Else
            Dim songitem = CType(Me.searchtracks_lst.SelectedItem, SearchTrackItem)
            Me.Songname_txt.Caption = $"<b>{songitem.SongName}</b>"
            If songitem.ArtImage Is Nothing Then
                Me.Songname_txt.ImageOptions.Image = Nothing
            Else
                Me.Songname_txt.ImageOptions.Image = ResizeImage(songitem.ArtImage, 20, 20)
            End If
            AddToPlaylistMenu.ClearLinks()
            'Add Now Playing in Menu
            Dim submenuNP As New DevExpress.XtraBars.BarSubItem() With {.Caption = "Now Playing", .Tag = NowPlaying, .AllowDrawArrow = DevExpress.Utils.DefaultBoolean.False}
            submenuNP.ImageOptions.Image = My.Resources.album
            AddHandler submenuNP.ItemClick, New ItemClickEventHandler(AddressOf AddToNowPlayingPlaylist)
            AddToPlaylistMenu.AddItem(submenuNP)

            'Add Playlists in Menu
            If Not AllPlaylists.Count = 0 Then
                For Each PL As TangerinePlaylist In AllPlaylists
                    Dim submenu As New DevExpress.XtraBars.BarSubItem() With {.Caption = PL.PlaylistName, .Tag = PL, .AllowDrawArrow = DevExpress.Utils.DefaultBoolean.False}
                    submenu.ImageOptions.Image = My.Resources.album
                    AddHandler submenu.ItemClick, New ItemClickEventHandler(AddressOf AddToPlaylist)
                    AddToPlaylistMenu.AddItem(submenu)
                Next
            End If
        End If
    End Sub

    Private Async Sub PlaySongMenu_ItemClick(sender As Object, e As ItemClickEventArgs) Handles PlaySongMenu.ItemClick
        SearchListCM.HidePopup()
        Dim sItem As SearchTrackItem = CType(searchtracks_lst.SelectedItem, SearchTrackItem)
        Await PlaySong(sItem.SongURL)
        Me.TC.SelectedTabPageIndex = 0
    End Sub

    Private Sub OpenInBrowserMenu_ItemClick(sender As Object, e As ItemClickEventArgs) Handles OpenInBrowserMenu.ItemClick
        SearchListCM.HidePopup()
        Dim sItem As SearchTrackItem = CType(searchtracks_lst.SelectedItem, SearchTrackItem)
        Process.Start(New ProcessStartInfo() With {.FileName = sItem.PermaLink, .UseShellExecute = True})
    End Sub

    Private Sub RemoveFromListMenu_ItemClick(sender As Object, e As ItemClickEventArgs) Handles RemoveFromListMenu.ItemClick
        SearchListCM.HidePopup()
        Dim sItem As SearchTrackItem = CType(searchtracks_lst.SelectedItem, SearchTrackItem)
        SearchItems.Remove(sItem)
    End Sub

    Private Sub CopyNameMenu_ItemClick(sender As Object, e As ItemClickEventArgs) Handles CopyNameMenu.ItemClick
        SearchListCM.HidePopup()
        Dim sItem As SearchTrackItem = CType(searchtracks_lst.SelectedItem, SearchTrackItem)
        Clipboard.SetText(sItem.SongName)
    End Sub

    Private Sub ClearAllMenu_ItemClick(sender As Object, e As ItemClickEventArgs) Handles ClearAllMenu.ItemClick
        SearchListCM.HidePopup()
        SearchItems.Clear()
    End Sub

    Private Sub AddToNowPlayingPlaylist(ByVal sender As Object, ByVal e As ItemClickEventArgs)
        SearchListCM.HidePopup()
        Dim sItem As SearchTrackItem = CType(searchtracks_lst.SelectedItem, SearchTrackItem)
        If NowPlaying.SongExist(sItem.SongID) Then
            ShowAlert("Song Already Exist", "This song is already exist in this playlist.", type:=AlertType.Error)
        Else
            If IsEmpty(sItem.ArtImageURL) Then
                Dim TMed As New TangerineMedia(sItem.SongURL, sItem.SongName, "", sItem.SongID)
                AddSongToNowPlaying(TMed)
                ShowAlert("Song Added", "The song has been added in this playlist.", type:=AlertType.Success)
            Else
                Dim TMed As New TangerineMedia(sItem.SongURL, sItem.SongName, sItem.ArtImageURL, sItem.SongID)
                AddSongToNowPlaying(TMed)
                ShowAlert("Song Added", "The song has been added in this playlist.", type:=AlertType.Success)
            End If
        End If
    End Sub



    Private Async Sub AddToPlaylist(ByVal sender As Object, ByVal e As ItemClickEventArgs)
        SearchListCM.HidePopup()
        Dim sItem As SearchTrackItem = CType(searchtracks_lst.SelectedItem, SearchTrackItem)
        Dim PL As TangerinePlaylist = CType(CType(e.Item, BarSubItem).Tag, TangerinePlaylist)
        If PL Is Nothing OrElse sItem Is Nothing Then Return
        If PL.SongExist(sItem.SongID) Then
            ShowAlert("Song Already Exist", "This song is already exist in this playlist.", type:=AlertType.Error)
        Else
            If IsEmpty(sItem.ArtImageURL) Then
                PL.AddSong(sItem.SongURL, sItem.SongName, "", sItem.SongID)
            Else
                PL.AddSong(sItem.SongURL, sItem.SongName, sItem.ArtImageURL, sItem.SongID)
            End If
            Await Utilities.SavePlaylistAsync(PL)
            ShowAlert("Song Added", "The song has been added in this playlist.", type:=AlertType.Success)
        End If
    End Sub

    Public Async Function ChangePlaylist(ByVal pl As TangerinePlaylist) As Task
        If pl.List.Count = 0 Then
            ShowAlert("Cannot Switch", "This playlist is empty.", type:=AlertType.Error)
            Exit Function
        End If
        Dim Waiter As New ProgressPanel With {
            .Caption = "Switching Playlist.", .Description = "It will take a few minute...", .Dock = DockStyle.Fill, .ContentAlignment = ContentAlignment.MiddleCenter,
            .Parent = nowplayinglist
        }
        NowPlaying = New TangerinePlaylist()
        NowPlaying = pl
        For Each item As TangerinePlaylist.PlaylistItem In pl.List
            Await Task.Run(Sub()
                               Dim med As New TangerineMedia(item.SongURL, item.SongName, item.ArtURL, item.SongID)
                               AddSongToNowPlaying(med)
                           End Sub)
        Next
        Waiter.Dispose()
    End Function

    Private Async Sub songlen_MouseDown(sender As Object, e As MouseEventArgs) Handles songlen.MouseDown
        Await SeekBegin()
    End Sub

    Private Async Sub songlen_MouseUp(sender As Object, e As MouseEventArgs) Handles songlen.MouseUp
        Await SeekEnd()
    End Sub

    Private Sub MP_LengthChanged(sender As Object, e As MediaPlayerLengthChangedEventArgs) Handles MP.LengthChanged
        If songlen.InvokeRequired Then
            songlen.Invoke(New EventHandler(Of MediaPlayerLengthChangedEventArgs)(AddressOf MP_LengthChanged), New Object() {sender, e})
            Return
        End If
        If MP IsNot Nothing Then Me.songlen.Properties.Maximum = CInt(e.Length / 1000)
    End Sub

    Private Async Sub songlen_ValueChanged(sender As Object, e As EventArgs) Handles songlen.ValueChanged
        If InvokeRequired Then
            Invoke(New EventHandler(AddressOf songlen_ValueChanged), New Object() {sender, e})
            Return
        End If
        Await Seeking(songlen.Value * 1000)
    End Sub

    Private Sub MP_Playing(sender As Object, e As EventArgs) Handles MP.Playing
        If InvokeRequired Then
            Invoke(New EventHandler(AddressOf MP_Playing), New Object() {sender, e})
            Return
        End If
        PPBtn.ImageOptions.SvgImage = My.Resources.Pause
    End Sub

    Private Sub MP_Paused(sender As Object, e As EventArgs) Handles MP.Paused
        If InvokeRequired Then
            Invoke(New EventHandler(AddressOf MP_Paused), New Object() {sender, e})
            Return
        End If
        PPBtn.ImageOptions.SvgImage = My.Resources.Play
    End Sub

    Private Async Sub PPBtn_Click(sender As Object, e As EventArgs) Handles PPBtn.Click
        If IsPaused = False Then
            Await PauseAsync()
        ElseIf ispaused = True Then
            Await ResumeAsync()
        End If
    End Sub

#Region "MEDIA STATES"
    Public Async Function PauseAsync() As Task
        If MP IsNot Nothing AndAlso MP.Media IsNot Nothing AndAlso MP.IsPlaying Then
            Await Task.Run(Sub()
                               If MP.CanPause Then MP.SetPause(True)
                               IsPaused = True
                           End Sub)
        End If
    End Function

    Public Async Function ResumeAsync() As Task
        If MP IsNot Nothing AndAlso MP.Media IsNot Nothing AndAlso IsPaused = True Then
            Await Task.Run(Sub()
                               MP.SetPause(False)
                               IsPaused = False
                           End Sub)
        End If
    End Function

    Public Async Function StopAsync() As Task
        If MP IsNot Nothing AndAlso MP.Media IsNot Nothing Then
            Await Task.Run(Sub()
                               MP.Stop()
                               Me.songlen.Value = 0
                           End Sub)
        End If
    End Function

    Public Async Function ChangeVolumeAsync(ByVal vol As Integer) As Task
        If MP IsNot Nothing Then
            Await Task.Run(Sub() MP.Volume = vol)
        End If
    End Function

    Private Async Function SeekBegin() As Task
        If MP IsNot Nothing AndAlso MP.IsPlaying Then
            Await Task.Run(Sub()
                               If MP.CanPause Then MP.SetPause(True)
                               IsSeeking = True
                           End Sub)
        End If
    End Function

    Private Async Function SeekEnd() As Task
        If MP IsNot Nothing AndAlso IsSeeking = True Then
            Await Task.Run(Sub()
                               MP.SetPause(False)
                               IsSeeking = False
                           End Sub)
        End If
    End Function

    Public Async Function Seeking(ByVal time As Long) As Task
        If MP IsNot Nothing AndAlso MP.Media IsNot Nothing AndAlso IsSeeking = True Then
            Await Task.Run(Sub()
                               MP.Time = time
                           End Sub)
        End If
    End Function

    Public Async Function ToggleMuteAsync() As Task
        If MP IsNot Nothing Then
            Await Task.Run(Sub()
                               MP.ToggleMute()
                           End Sub)
        End If
    End Function

#End Region

    Private Sub MP_Muted(sender As Object, e As EventArgs) Handles MP.Muted
        If InvokeRequired Then
            Invoke(New EventHandler(AddressOf MP_Muted), New Object() {sender, e})
            Return
        End If
        VolBar.Enabled = False
        MuteBtn.ImageOptions.SvgImage = My.Resources.Mute
    End Sub

    Private Sub MP_Unmuted(sender As Object, e As EventArgs) Handles MP.Unmuted
        If InvokeRequired Then
            Invoke(New EventHandler(AddressOf MP_Unmuted), New Object() {sender, e})
            Return
        End If
        VolBar.Enabled = True
        MuteBtn.ImageOptions.SvgImage = My.Resources.Volume2
    End Sub

    Private Async Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            Me.Hide()
            If MP IsNot Nothing AndAlso MP.IsPlaying Then
                Await StopAsync()
            End If
            MP?.Dispose()
            LibVlc.Dispose()
        Catch ex As Exception
            TLogger.WriteLog(ex)
            Process.GetCurrentProcess().Kill()
        End Try
    End Sub

    Private Async Sub RptBtn_Click(sender As Object, e As EventArgs) Handles RptBtn.Click
        If InvokeRequired Then
            Invoke(New EventHandler(AddressOf RptBtn_Click), New Object() {sender, e})
            Return
        End If
        If TConfig.RepeatMode = TangerineConfig.RepeatType.AllSongs Then
            TConfig.RepeatMode = TangerineConfig.RepeatType.NoRepeat
            Me.RptBtn.SvgImage = My.Resources.RepeatOff
            Me.RptBtn.ToolTip = "Repeat Off"
            Await SaveConfig()
        ElseIf TConfig.RepeatMode = TangerineConfig.RepeatType.NoRepeat Then
            TConfig.RepeatMode = TangerineConfig.RepeatType.Single
            Me.RptBtn.SvgImage = My.Resources.RepeatOne
            Me.RptBtn.ToolTip = "Repeat One"
            Await SaveConfig()
        ElseIf TConfig.RepeatMode = TangerineConfig.RepeatType.Single Then
            TConfig.RepeatMode = TangerineConfig.RepeatType.AllSongs
            Me.RptBtn.SvgImage = My.Resources.RepeatAll
            Me.RptBtn.ToolTip = "Repeat All"
            Await SaveConfig()
        End If
    End Sub

    Private Sub crtPLMenu_ItemClick(sender As Object, e As ItemClickEventArgs) Handles crtPLMenu.ItemClick
        Bar1.Manager.CloseMenus()
        NewPlaylist.Show(Me)
    End Sub

    Private Async Sub OnChangePlaylistClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
        Await ChangePlaylist(CType(CType(e.Item, BarSubItem).Tag, TangerinePlaylist))
    End Sub

    Public Async Sub AddNewPlaylist(ByVal name As String)
        Dim PList As New TangerinePlaylist() With {
    .PlaylistName = name,
    .CreatedOn = DateTime.Now,
    .List = New List(Of TangerinePlaylist.PlaylistItem)()}
        AllPlaylists.Add(PList)
        Await SavePlaylistAsync(PList)
        ShowAlert(name, "A new playlist has been created.", MainForm.AlertType.Success)
        Dim menuitem As New BarSubItem() With {.Caption = name, .Tag = PList, .AllowDrawArrow = DevExpress.Utils.DefaultBoolean.False}
        menuitem.ImageOptions.Image = My.Resources.cd_burning
        BarSubItem4.AddItem(menuitem)
        Dim PLItem As New ImageListBoxItem() With {
    .Value = PList.PlaylistName,
    .Tag = PList}
        PLItem.ImageOptions.ImageIndex = 0
        playlists_lst.Items.Add(PLItem)
    End Sub

    Private Sub PlaylistCM_BeforePopup(sender As Object, e As CancelEventArgs) Handles PlaylistCM.BeforePopup
        If playlists_lst.Items.Count = 0 Then e.Cancel = True
    End Sub

    Private Async Sub PlayPlaylistMenu_ItemClick(sender As Object, e As ItemClickEventArgs) Handles PlayPlaylistMenu.ItemClick
        PlaylistCM.HidePopup()
        Dim PLITem As TangerinePlaylist = TryCast(CType(playlists_lst.SelectedItem, ImageListBoxItem).Tag, TangerinePlaylist)
        If PLITem Is Nothing Then
            MBox("Invalid or Deleted Playlist", "Error while playing. This playlist seems invalid or deleted. Please restart Tangerine Player to refresh the playlists.", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If PLITem.List.Count = 0 Then
            ShowAlert("Cannot Play", "This playlist is empty.", type:=AlertType.Error)
            Exit Sub
        Else
            Await StopAsync()
            Await ChangePlaylist(PLITem)
            Await PlaySong(PLITem.List.First().SongURL)
        End If
    End Sub

    Private Sub MP_EncounteredError(sender As Object, e As EventArgs) Handles MP.EncounteredError
        If InvokeRequired Then
            Invoke(New EventHandler(AddressOf MP_EncounteredError), New Object() {sender, e})
            Return
        End If
        TLogger.WriteLog($"Error while playing/streaming media: {MP?.Media?.Mrl}")
    End Sub

    Private Async Sub MP_EndReached(sender As Object, e As EventArgs) Handles MP.EndReached
        If InvokeRequired Then
            Invoke(New EventHandler(AddressOf MP_EndReached), New Object() {sender, e})
            Return
        End If
        Select Case TConfig.RepeatMode
            Case TangerineConfig.RepeatType.AllSongs
                If CurrentSongID = Nothing Then Exit Sub
                Dim lastsong = NowPlaying.List.Last()
                If CurrentSongID = lastsong.SongID Then
                    ShowAlert("Playlist End Reached", "Playing from starting..", type:=AlertType.Information)
                    Await PlaySong(NowPlaying.List.First().SongURL)
                Else
                    Dim CurSong = NowPlaying.List.FirstOrDefault(Function(x) x.SongID = CurrentSongID)
                    If CurSong IsNot Nothing Then
                        Dim songindex As Integer = NowPlaying.List.IndexOf(CurSong)
                        Await PlaySong(NowPlaying.List.Item(songindex + 1).SongURL)
                    End If
                End If
            Case TangerineConfig.RepeatType.Single
                Await StopAsync()
                MP.Play()
            Case TangerineConfig.RepeatType.NoRepeat
                Dim CurSong = NowPlaying.List.FirstOrDefault(Function(x) x.SongID = CurrentSongID)
                If CurSong IsNot Nothing Then
                    Dim songindex As Integer = NowPlaying.List.IndexOf(CurSong)
                    If songindex = NowPlaying.List.Count - 1 Then
                        Await StopAsync()
                        ShowAlert("Playlist Ended", "Playlist ended and stopped.")
                    Else
                        Await PlaySong(NowPlaying.List.Item(songindex + 1).SongURL)
                    End If
                End If
        End Select
    End Sub

    Private Sub LibVlc_Log(sender As Object, e As LogEventArgs) Handles LibVlc.Log
        If e.Level = LogLevel.Error Then TLogger.WriteLog(e.FormattedLog)
    End Sub

    Private Sub DeletePlaylistMenu_ItemClick(sender As Object, e As ItemClickEventArgs) Handles DeletePlaylistMenu.ItemClick
        PlaylistCM.HidePopup()
        If MBox("Deleting Playlist", "Do you really want to delete this playlist? This action cannot be undone.", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Dim PL As TangerinePlaylist = CType(CType(playlists_lst.SelectedItem, ImageListBoxItem).Tag, TangerinePlaylist)
            'remove from menu
            Dim removeitems As New List(Of BarSubItemLink)()
            For Each item As BarSubItemLink In BarSubItem4.ItemLinks
                If item.Caption.Equals(PL.PlaylistName, StringComparison.OrdinalIgnoreCase) Then
                    removeitems.Add(item)
                    Exit For
                End If
            Next
            For Each baritem In removeitems
                baritem.Dispose()
            Next
            playlists_lst.Items.Remove(playlists_lst.SelectedItem)
            AllPlaylists.Remove(PL)
            Dim PLPath As String = IO.Path.Combine(PlaylistPath, $"{PL.PlaylistName}.tpl")
            If IO.File.Exists(PLPath) Then IO.File.Delete(PLPath)
            ShowAlert("Playlist Deleted", "Playlist has been deleted.", type:=AlertType.Success)
        End If
    End Sub

    Private Sub CopyPLNameMenu_ItemClick(sender As Object, e As ItemClickEventArgs) Handles CopyPLNameMenu.ItemClick
        PlaylistCM.HidePopup()
        Clipboard.SetText(CType(playlists_lst.SelectedItem, ImageListBoxItem).Value.ToString())
    End Sub

    Private Sub ViewEditPlaylistMenu_ItemClick(sender As Object, e As ItemClickEventArgs) Handles ViewEditPlaylistMenu.ItemClick
        PlaylistCM.HidePopup()
        Dim EditForm As New EditPlaylist(CType(CType(Me.playlists_lst.SelectedItem, ImageListBoxItem).Tag, TangerinePlaylist))
        EditForm.ShowDialog(Me)
    End Sub

    Private Sub BarSubItem6_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarSubItem6.ItemClick
        Settings.Show(Me)
        Dim eq As New Equalizer()
    End Sub

    Private Sub chkforupdate_ItemClick(sender As Object, e As ItemClickEventArgs) Handles chkforupdate.ItemClick
        BarSubItem12.Manager.CloseMenus()
        Credits.Show(Me)
        Credits.XtraTabControl1.SelectedTabPageIndex = 1
    End Sub

    Private Sub creds_ItemClick(sender As Object, e As ItemClickEventArgs) Handles creds.ItemClick
        BarSubItem12.Manager.CloseMenus()
        Credits.Show(Me)
    End Sub

    Private Sub bugreport_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bugreport.ItemClick
        BarSubItem12.Manager.CloseMenus()
        Process.Start(New ProcessStartInfo() With {.FileName = "https://github.com/Nothing-Just-a-Code/Tangerine-Player/issues/new/choose",
                      .UseShellExecute = True})
    End Sub

    Private Sub BarSubItem2_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarSubItem2.ItemClick
        BarSubItem1.Manager.CloseMenus()
        Process.Start(New ProcessStartInfo() With {
                      .FileName = TLogger.LogFile,
                      .UseShellExecute = True})
    End Sub

    Private Async Sub BarSubItem3_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarSubItem3.ItemClick
        BarSubItem1.Manager.CloseMenus()
        If MP IsNot Nothing AndAlso MP.IsPlaying Then Await StopAsync()
        Me.Close()
    End Sub

    Private Async Sub NextBtn_Click(sender As Object, e As EventArgs) Handles NextBtn.Click
        If CurrentSongID = Nothing Then Exit Sub
        Dim CurSong = NowPlaying.List.FirstOrDefault(Function(x) x.SongID = CurrentSongID)
        Dim songindex As Integer = NowPlaying.List.IndexOf(CurSong)
        If songindex = NowPlaying.List.Count - 1 Then
            ShowAlert("Cannot Play Next Song", "It's a last song of this playlist.")
        Else
            If MP.IsPlaying Then Await StopAsync()
            Await PlaySong(NowPlaying.List.Item(songindex + 1).SongURL)
        End If
    End Sub

    Private Async Sub PrevBtn_Click(sender As Object, e As EventArgs) Handles PrevBtn.Click
        If CurrentSongID = Nothing Then Exit Sub
        Dim CurSong = NowPlaying.List.FirstOrDefault(Function(x) x.SongID = CurrentSongID)
        Dim songindex As Integer = NowPlaying.List.IndexOf(CurSong)
        If songindex = 0 Then
            ShowAlert("Cannot Play Previous Song", "It's a first song of this playlist.")
        Else
            If MP.IsPlaying Then Await StopAsync()
            Await PlaySong(NowPlaying.List.Item(songindex - 1).SongURL)
        End If
    End Sub

    Private Async Sub BarSubItem9_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarSubItem9.ItemClick
        nowplayingCM.HidePopup()
        If nowplayinglist.SelectedItem Is Nothing Then Return
        Dim sItem As TangerineMedia = TryCast(nowplayinglist.SelectedValue, TangerineMedia)
        If MP.IsPlaying Then Await StopAsync()
        Await PlaySong(sItem.SongURL)
    End Sub

    Private Sub BarSubItem10_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarSubItem10.ItemClick
        nowplayingCM.HidePopup()
        If nowplayinglist.SelectedItem Is Nothing Then Return
        Dim sItem As TangerineMedia = TryCast(nowplayinglist.SelectedValue, TangerineMedia)
        If sItem IsNot Nothing Then
            Clipboard.SetText(sItem.SongName)
        End If
    End Sub

    Private Async Sub SongLenLbl_Click(sender As Object, e As EventArgs) Handles SongLenLbl.Click
        Await PlaySong("https://soundcloud.com/yua-10484219/in-the-middle-of-the-night")
    End Sub

    Private Async Sub BarSubItem11_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarSubItem11.ItemClick
        If InvokeRequired Then
            Invoke(New ItemClickEventHandler(AddressOf BarSubItem11_ItemClick), New Object() {sender, e})
            Return
        End If
        nowplayingCM.HidePopup()
        If nowplayinglist.SelectedItem Is Nothing Then Return
        Dim sItem As TangerineMedia = TryCast(nowplayinglist.SelectedValue, TangerineMedia)
        If MP.IsPlaying Then Await StopAsync()
        NowPlaying.RemoveSong(sItem.SongID)
        nowplayinglist.Items.Remove(nowplayinglist.SelectedItem)
        'ChangeUI
        songart.Image = Nothing
        SongLenLbl.Text = "00:00:00"
        SongDur.Text = "00:00:00"
        CurSong.Text = "..."
    End Sub

    Private Sub MainForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.S Then FlyoutPanel1.ShowPopup(False)
    End Sub

    Private Sub SimpleButton1_Click_1(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        If FlyoutPanel1.IsPopupOpen Then FlyoutPanel1.HidePopup(False)
    End Sub
End Class