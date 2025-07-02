Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports DevExpress.XtraEditors
Imports LibVLCSharp.Shared
Imports Newtonsoft.Json
Module Utilities
    Public TConfig As TangerineConfig
    Public MainPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tangerine Player")
    Public LogsPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tangerine Player", "Logs")
    Public PlaylistPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tangerine Player", "Playlists")
    Public DataPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tangerine Player", "Data")
    Public AllPlaylists As New List(Of TangerinePlaylist)()
    Public EqL As Equalizer
    Public Function IsEmpty(ByVal text As String) As Boolean
        Return String.IsNullOrEmpty(text) OrElse String.IsNullOrWhiteSpace(text)
    End Function

    Public Function GetLibVlcEngineDirectory() As String
        Dim DirPath As String = ""
        If Environment.Is64BitOperatingSystem AndAlso Environment.Is64BitProcess Then
            DirPath = Path.Combine(Application.StartupPath, "libvlc", "win-x64")
        Else
            DirPath = Path.Combine(Application.StartupPath, "libvlc", "win-x86")
        End If
        If Directory.Exists(DirPath) Then
            Return DirPath
        Else
            MessageBox.Show("LibVLC Engine directory not found. Please re-install Clementine to fix this issue.", "LibVLC Engine Not Found", buttons:=MessageBoxButtons.OK, MessageBoxIcon.Error)
            Process.GetCurrentProcess().Kill()
        End If
    End Function

    Public Async Function SavePlaylistAsync(ByVal playlist As TangerinePlaylist) As Task
        Await Task.Run(Sub()
                           If playlist.PlaylistName.Equals("Now Playing", StringComparison.OrdinalIgnoreCase) Then
                               Return
                           Else
                               File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tangerine Player", "Playlists", playlist.PlaylistName & ".tpl"), JsonConvert.SerializeObject(playlist, Formatting.Indented))
                           End If
                       End Sub)
    End Function

    Public Async Function RenamePlaylist(ByVal newname As String, ByVal pl As TangerinePlaylist) As Task
        Try
            If File.Exists(Path.Combine(PlaylistPath, $"{pl.PlaylistName}.tpl")) Then
                My.Computer.FileSystem.RenameFile(Path.Combine(PlaylistPath, $"{pl.PlaylistName}.tpl"), $"{newname}.tpl")
                pl.PlaylistName = newname
                Await SavePlaylistAsync(pl)
            Else
                pl.PlaylistName = newname
                Await SavePlaylistAsync(pl)
            End If
        Catch ex As Exception
            MBox("Error", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Public Async Function LoadImageFromURL(ByVal url As String) As Task(Of Image)
        Try
            Using http As New Net.Http.HttpClient()
                Dim httpM = Await http.GetAsync(url)
                httpM.EnsureSuccessStatusCode()
                Dim stream = Await httpM.Content.ReadAsStreamAsync()
                Return Image.FromStream(stream)
            End Using
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Sub InitializeDirectories()
        If Not Directory.Exists(MainPath) Then Directory.CreateDirectory(MainPath)
        If Not Directory.Exists(LogsPath) Then Directory.CreateDirectory(LogsPath)
        If Not Directory.Exists(PlaylistPath) Then Directory.CreateDirectory(PlaylistPath)
        If Not Directory.Exists(DataPath) Then Directory.CreateDirectory(DataPath)
        ReadConfig()
    End Sub

#Region "CONFIG"
    Public Async Sub ReadConfig()
        If Not File.Exists(Path.Combine(MainPath, "Tangerine.config")) Then Await SaveConfig()
        TConfig = JsonConvert.DeserializeObject(Of TangerineConfig)(File.ReadAllText(Path.Combine(MainPath, "Tangerine.config")))
    End Sub

    Public Async Function SaveConfig() As Task
        Await Task.Run(Sub()
                           If TConfig Is Nothing Then TConfig = New TangerineConfig()
                           For attempt = 1 To 5
                               Try
                                   File.WriteAllText(Path.Combine(MainPath, "Tangerine.config"), JsonConvert.SerializeObject(TConfig, Formatting.Indented), System.Text.Encoding.UTF8)
                                   Return ' Success
                               Catch ex As IOException When attempt < 5
                                   Threading.Thread.Sleep(500)
                               Catch ex As UnauthorizedAccessException When attempt < 5
                                   Threading.Thread.Sleep(500)
                               Catch ex As Exception
                                   MBox("Error While Writing Config File", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
                               End Try
                           Next
                       End Sub)
    End Function
#End Region

    Public Function TimeSpanToWords(ts As TimeSpan) As String
        Dim sb As New StringBuilder()
        If ts.Days > 0 Then sb.Append($"{ts.Days} day{If(ts.Days > 1, "s", "")}, ")
        If ts.Hours > 0 Then sb.Append($"{ts.Hours} hour{If(ts.Hours > 1, "s", "")}, ")
        If ts.Minutes > 0 Then sb.Append($"{ts.Minutes} minute{If(ts.Minutes > 1, "s", "")}, ")
        If ts.Seconds > 0 Then sb.Append($"{ts.Seconds} second{If(ts.Seconds > 1, "s", "")}, ")

        If sb.Length = 0 Then
            Return "0 seconds"
        Else
            ' Remove the trailing comma and space
            sb.Length -= 2
            Return sb.ToString()
        End If
    End Function


    Public Function ResizeImage(sourceImage As Image, width As Integer, height As Integer) As Image
        Dim destRect As New Rectangle(0, 0, width, height)
        Dim destImage As New Bitmap(width, height)

        destImage.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution)

        Using graph = Graphics.FromImage(destImage)
            graph.CompositingMode = CompositingMode.SourceCopy
            graph.CompositingQuality = CompositingQuality.HighQuality
            graph.InterpolationMode = InterpolationMode.HighQualityBicubic
            graph.SmoothingMode = SmoothingMode.HighQuality
            graph.PixelOffsetMode = PixelOffsetMode.HighQuality

            Using wrapMode = New ImageAttributes()
                wrapMode.SetWrapMode(Drawing2D.WrapMode.TileFlipXY)
                graph.DrawImage(sourceImage, destRect, 0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel, wrapMode)
            End Using
        End Using

        Return destImage
    End Function

    Public Function MBox(title As String, message As String, button As MessageBoxButtons, icon As MessageBoxIcon, Optional owner As IWin32Window = Nothing) As DialogResult
        Return XtraMessageBox.Show(owner:=owner, text:=message, caption:=title, buttons:=button, icon:=icon)
    End Function

    Public Function GetCurrentVersion() As String
        Return $"{My.Application.Info.Version.Major}.{My.Application.Info.Version.Minor}.{My.Application.Info.Version.Build}"
    End Function

    Public Async Function IsNewVersionAvailable() As Task(Of Boolean)
        Try
            Dim CurVer = My.Application.Info.Version
            Using http As New Net.Http.HttpClient()

                http.DefaultRequestHeaders.CacheControl = New Headers.CacheControlHeaderValue() With {.NoCache = True, .NoStore = True, .MustRevalidate = True}
                http.DefaultRequestHeaders.Pragma.Add(New Headers.NameValueHeaderValue("no-cache"))

                Dim httpM = Await http.GetAsync("https://raw.githubusercontent.com/Nothing-Just-a-Code/Tangerine-Player/refs/heads/main/version.json")
                httpM.EnsureSuccessStatusCode()
                Dim jsn As String = Await httpM.Content.ReadAsStringAsync()
                Dim J As Linq.JObject = Linq.JObject.Parse(jsn)
                Dim NewVer As New Version(J("version").ToString())
                If NewVer > CurVer Then
                    Return True
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            TLogger.WriteLog(ex)
            MBox("Error While Checking for Update", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function


End Module
