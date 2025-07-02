Imports LibVLCSharp.Shared

Public Class TangerineMedia
    Public Property SongName As String
    Public Property SongArtURL As String
    Public Property SongURL As String
    Public Property SongID As Long
    Public Property SongStreamURL As String

    Sub New(ByVal soundcloudurl As String, ByVal songname As String, ByVal arturl As String, ByVal id As Long)
        Me.SongName = songname
        Me.SongArtURL = arturl
        Me.SongURL = soundcloudurl
        Me.SongID = id
    End Sub
    Public Overrides Function ToString() As String
        Return Me.SongName
    End Function
End Class
