Imports DevExpress.XtraBars.Docking2010
Imports LibVLCSharp.Shared

Public Class Settings
    Private CachingWarningShowed As Boolean = False
    Private IsReady As Boolean = False

    ''' <summary>
    ''' Sets the audio delay on a MediaPlayer in milliseconds.
    ''' </summary>
    ''' <param name="delayMs">The delay in milliseconds. Can be negative or Positive.</param>
    Public Sub SetAudioDelay(delayMs As Integer)
        Dim player = CType(Me.Owner, MainForm).MP
        If player Is Nothing Then
            Return
        End If

        ' Convert milliseconds to microseconds (1 ms = 1000 μs)
        If player.SetAudioDelay(CLng(delayMs) * 1000) Then
            Return
        Else
            MBox("Error", "Error while setting audio delay. Please try again or check logs for more information.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Async Sub audiodelaytime_ValueChanged(sender As Object, e As EventArgs) Handles audiodelaytime.ValueChanged
        audiodelaytime.Enabled = False
        Await Task.Delay(1000)
        SetAudioDelay(CInt(audiodelaytime.Value))
        audiodelaytime.Enabled = True
    End Sub

    Private Async Sub Settings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
        Await ReadSettings()
        IsReady = True
    End Sub

    Private Async Function ReadSettings() As Task
        Await Task.Run(Async Sub()
                           'audio delay
                           If audiodelaytime IsNot Nothing Then audiodelaytime.Value = CInt(CType(Me.Owner, MainForm).MP.AudioDelay \ 1000)

                           'channel output
                           Select Case CType(Me.Owner, MainForm).MP.Channel
                               Case AudioOutputChannel.Dolbys
                                   If channelOut IsNot Nothing Then channelOut.SelectedIndex = 4
                               Case AudioOutputChannel.Error
                                   MBox("Invalid Channel Output", "There is an error in channel output. Please check logs for more information.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                               Case AudioOutputChannel.Left
                                   If channelOut IsNot Nothing Then channelOut.SelectedIndex = 2
                               Case AudioOutputChannel.Right
                                   If channelOut IsNot Nothing Then channelOut.SelectedIndex = 3
                               Case AudioOutputChannel.RStereo
                                   If channelOut IsNot Nothing Then channelOut.SelectedIndex = 1
                               Case AudioOutputChannel.Stereo
                                   If channelOut IsNot Nothing Then channelOut.SelectedIndex = 0
                           End Select


                           'network caching
                           If networkcache IsNot Nothing Then networkcache.SelectedIndex = TConfig.Caching

                           'Equalizer
                           If EqPre IsNot Nothing Then GetEQ()
                           Await Task.Delay(300)
                           If eqsts IsNot Nothing Then eqsts.Checked = TConfig.EnableEqualizer
                           PanelControl1.Enabled = TConfig.EnableEqualizer
                       End Sub)
    End Function

    Private Async Sub channelOut_SelectedIndexChanged(sender As Object, e As EventArgs) Handles channelOut.SelectedIndexChanged
        If IsReady = True Then
            Dim frm As MainForm = CType(Me.Owner, MainForm)
            Select Case channelOut.SelectedIndex
                Case 0
                    channelOut.Enabled = False
                    If frm.MP.SetChannel(AudioOutputChannel.Stereo) Then
                        TConfig.ChannelOutput = AudioOutputChannel.Stereo
                        Await SaveConfig()
                    Else
                        MBox("Error", "There is an error while changing output channel.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                    channelOut.Enabled = True
                    Exit Select

                Case 1
                    channelOut.Enabled = False
                    If frm.MP.SetChannel(AudioOutputChannel.RStereo) Then
                        TConfig.ChannelOutput = AudioOutputChannel.RStereo
                        Await SaveConfig()
                    Else
                        MBox("Error", "There is an error while changing output channel.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                    channelOut.Enabled = True
                    Exit Select

                Case 2
                    channelOut.Enabled = False
                    If frm.MP.SetChannel(AudioOutputChannel.Left) Then
                        TConfig.ChannelOutput = AudioOutputChannel.Left
                        Await SaveConfig()
                    Else
                        MBox("Error", "There is an error while changing output channel.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                    channelOut.Enabled = True
                    Exit Select

                Case 3
                    channelOut.Enabled = False
                    If frm.MP.SetChannel(AudioOutputChannel.Right) Then
                        TConfig.ChannelOutput = AudioOutputChannel.Right
                        Await SaveConfig()
                    Else
                        MBox("Error", "There is an error while changing output channel.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                    channelOut.Enabled = True
                    Exit Select

                Case 4
                    channelOut.Enabled = False
                    If frm.MP.SetChannel(AudioOutputChannel.Dolbys) Then
                        TConfig.ChannelOutput = AudioOutputChannel.Dolbys
                        Await SaveConfig()
                    Else
                        MBox("Error", "There is an error while changing output channel.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                    channelOut.Enabled = True
                    Exit Select
            End Select
        End If
    End Sub

    Private Async Sub networkcache_SelectedIndexChanged(sender As Object, e As EventArgs) Handles networkcache.SelectedIndexChanged
        If IsReady Then
            Dim frm As MainForm = CType(Me.Owner, MainForm)
            If frm.MP Is Nothing Then Exit Sub
            Select Case networkcache.SelectedIndex
                Case 0
                    frm.MP.NetworkCaching = 700
                    TConfig.Caching = TangerineConfig.CachingType.Low
                    Await SaveConfig()
                Case 1
                    frm.MP.NetworkCaching = 1800
                    TConfig.Caching = TangerineConfig.CachingType.Medium
                    Await SaveConfig()
                Case 2
                    frm.MP.NetworkCaching = 3000
                    TConfig.Caching = TangerineConfig.CachingType.High
                    Await SaveConfig()
            End Select
            If frm.MP.IsPlaying AndAlso CachingWarningShowed = False Then
                MBox("Changing Cache", "It seems Tangerine is already playing media and New caching settings will take effect after the current song finishes.”, MessageBoxButtons.OK, MessageBoxIcon.Information)
                CachingWarningShowed = True
            End If
        End If
    End Sub

    Private Sub GetEQ()
        If InvokeRequired Then
            Invoke(New MethodInvoker(Sub() GetEQ()))
            Return
        End If
        If Not EqPre.Properties.Items.Count = 0 Then EqPre.Properties.Items.Clear()
        For i As Integer = 0 To EqL.PresetCount - 1
            If Not IsEmpty(EqL.PresetName(i)) Then EqPre.Properties.Items.Add(EqL.PresetName(i))
        Next
        If TConfig.EnableEqualizer Then EqPre.SelectedIndex = TConfig.Equalizer
    End Sub

    Private Async Sub EqPre_SelectedIndexChanged(sender As Object, e As EventArgs) Handles EqPre.SelectedIndexChanged
        If IsReady Then
            EqPre.Enabled = False
            If EqL IsNot Nothing Then EqL.Dispose()
            EqL = New Equalizer(EqPre.SelectedIndex)
            Dim result = Await SetEqualizer(EqL)
            If result = True Then
                TConfig.Equalizer = EqPre.SelectedIndex
                Await SaveConfig()
            Else
                MBox("Error", "There is an error while changing equalizer. Please check logs for more information.", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            EqPre.Enabled = True
        End If
    End Sub

    Private Async Function SetEqualizer(ByVal EQ As Equalizer) As Task(Of Boolean)
        Return Await Task.Run(Function()
                                  Return CType(Me.Owner, MainForm).MP?.SetEqualizer(EQ)
                              End Function)
    End Function

    Private Async Function DisableEqualizer() As Task(Of Boolean)
        Return Await Task.Run(Function()
                                  Return CType(Me.Owner, MainForm).MP?.UnsetEqualizer()
                              End Function)
    End Function

    Private Async Sub eqsts_CheckedChanged(sender As Object, e As EventArgs) Handles eqsts.CheckedChanged
        If InvokeRequired Then
            Invoke(New EventHandler(AddressOf eqsts_CheckedChanged), New Object() {sender, e})
            Return
        End If
        If IsReady AndAlso eqsts IsNot Nothing Then
            Select Case eqsts.CheckState
                Case CheckState.Checked
                    PanelControl1.Enabled = True
                    eqsts.Enabled = False
                    TConfig.EnableEqualizer = True
                    If EqL IsNot Nothing Then EqL.Dispose()
                    If EqPre.SelectedItem Is Nothing Or EqPre.SelectedIndex = -1 Then EqPre.SelectedIndex = TConfig.Equalizer
                    EqL = New Equalizer(EqPre.SelectedIndex)
                    Dim result = Await SetEqualizer(EqL)
                    If result = True Then
                        TConfig.Equalizer = EqPre.SelectedIndex
                        Await SaveConfig()
                        MsgBox("saved")
                    Else
                        MBox("Error", "There is an error while changing equalizer. Please check logs for more information.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                    EqPre.Enabled = True
                    eqsts.Enabled = True
                Case CheckState.Unchecked
                    PanelControl1.Enabled = False
                    eqsts.Enabled = False
                    Dim result = Await DisableEqualizer()
                    If result = True Then
                        TConfig.EnableEqualizer = False
                        Await SaveConfig()
                        MsgBox("saved")
                    Else
                        MBox("Error", "Cannot disable the equalizer. Please check logs for more information.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                    eqsts.Enabled = True
            End Select
        End If
    End Sub
End Class