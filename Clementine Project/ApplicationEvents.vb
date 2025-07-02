Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

        Protected Overrides Function OnStartup(eventArgs As StartupEventArgs) As Boolean
            AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf OnAppDomainException
            AddHandler System.Windows.Forms.Application.ThreadException, AddressOf OnThreadException
            AddHandler TaskScheduler.UnobservedTaskException, AddressOf OnUnobservedTaskException
            Utilities.InitializeDirectories()
            TLogger.InitLogger()
            Return MyBase.OnStartup(eventArgs)
        End Function

        Private Sub OnAppDomainException(sender As Object, e As System.UnhandledExceptionEventArgs)
            TLogger.WriteLog($"Error in Background thread: {DirectCast(e.ExceptionObject, Exception) }")
        End Sub

        Private Sub OnThreadException(sender As Object, e As Threading.ThreadExceptionEventArgs)
            TLogger.WriteLog($"Error in UI Thread: {e.Exception}")
        End Sub

        Private Sub OnUnobservedTaskException(sender As Object, e As UnobservedTaskExceptionEventArgs)
            TLogger.WriteLog($"Error in Task: {e.Exception}")
            e.SetObserved()
        End Sub
    End Class
End Namespace
