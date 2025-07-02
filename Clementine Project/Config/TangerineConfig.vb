Imports LibVLCSharp.Shared
Imports Newtonsoft.Json
Public Class TangerineConfig
    <JsonProperty("repeatmode")> Public Property RepeatMode As RepeatType = RepeatType.NoRepeat
    <JsonProperty("notifications")> Public Property ShowNotifications As Boolean = True
    <JsonProperty("volume")> Public Property Volume As Integer = 50
    <JsonProperty("channel")> Public Property ChannelOutput As AudioOutputChannel = AudioOutputChannel.Stereo
    <JsonProperty("networkcaching")> Public Property Caching As CachingType = CachingType.Medium
    <JsonProperty("equalizer")> Public Property Equalizer As Integer = 0
    <JsonProperty("equalizerstatus")> Public Property EnableEqualizer As Boolean = False

    Enum RepeatType As Integer
        NoRepeat
        [Single]
        AllSongs
    End Enum
    Enum CachingType As Integer
        Low = 0
        Medium = 1
        High = 2
    End Enum
End Class
