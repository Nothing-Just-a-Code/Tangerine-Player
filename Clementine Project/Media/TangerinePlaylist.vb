Imports System.IO
Imports LibVLCSharp.Shared
Imports Newtonsoft.Json
Public Class TangerinePlaylist
    <JsonProperty("name")> Public Property PlaylistName As String
    <JsonProperty("playlist")> Public Property List As List(Of PlaylistItem)
    <JsonProperty("createdon")> Public Property CreatedOn As DateTime
    Public Sub AddSong(ByVal songurl As String, ByVal songname As String, ByVal arturl As String, ByVal id As Long)
        If SongExist(songurl) Then
            Return
        Else
            List.Add(New PlaylistItem() With {.SongName = songname, .SongURL = songurl, .ArtURL = arturl, .SongID = id})
        End If
    End Sub

    Public Function SongExist(ByVal songurl As String) As Boolean
        Return List.Any(Function(x) x.SongURL.Equals(songurl))
    End Function

    Public Function SongExist(ByVal songid As Long) As Boolean
        Return List.Any(Function(x) x.SongID.Equals(songid))
    End Function

    Public Sub RemoveSong(ByVal songurl As String)
        If SongExist(songurl) Then
            List.Remove(GetSong(songurl))
        End If
    End Sub

    Public Sub RemoveSong(ByVal songid As Long)
        If SongExist(songid) Then
            List.Remove(GetSong(songid))
        End If
    End Sub

    Public Function GetSong(ByVal songurl As String) As PlaylistItem
        Return List.FirstOrDefault(Function(x) x.SongName.Equals(songurl))
    End Function

    Public Function GetSong(ByVal songid As Long) As PlaylistItem
        Return List.FirstOrDefault(Function(x) x.SongID.Equals(songid))
    End Function
    Public Class PlaylistItem
        <JsonProperty("url")> Public Property SongURL As String
        <JsonProperty("songname")> Public Property SongName As String
        <JsonProperty("arturl")> Public Property ArtURL As String
        <JsonProperty("songid")> Public Property SongID As Long
    End Class
End Class
