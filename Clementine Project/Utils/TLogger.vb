Imports System.IO
Imports System.Text
Public Class TLogger
    Public Shared LogFile As String
    Private Shared SWriter As StreamWriter
    Public Shared Sub InitLogger()
        LogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tangerine Player", "Logs", $"Log_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.log")
        SWriter = New StreamWriter(LogFile, True, System.Text.Encoding.UTF8) With {.AutoFlush = True}
        SWriter.WriteLine($"X-----------------TANGERINE PLAYER SESSION STARTED-----------------X")
    End Sub

    Public Shared Sub WriteLog(ByVal message As String)
        SWriter.WriteLine($"[{DateTime.Now}] Error: {message}")
    End Sub

    Public Shared Sub WriteLog(ByVal ex As Exception)
        Dim stack As String = ""
        Dim source As String = ""
        If Not IsEmpty(ex.StackTrace) Then stack = ex.StackTrace
        If Not IsEmpty(ex.Source) Then source = ex.Source
        SWriter.WriteLine($"[{DateTime.Now.ToString()}] Error: {ex.Message}  |  Stack: {stack}  |  Source: {source}")
    End Sub
End Class
