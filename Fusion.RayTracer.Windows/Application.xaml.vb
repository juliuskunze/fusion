Imports System.Windows.Threading

Class Application

    Private WithEvents _MainWindow As MainWindow

    Private ReadOnly _InitialDirectory As New DirectoryInfo(My.Computer.FileSystem.SpecialDirectories.Desktop)

    Private Sub Application_Startup(sender As Object, e As System.Windows.StartupEventArgs) Handles Me.Startup
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

        _MainWindow = New MainWindow(RelativisticRayTracerTermContextBuilder:=New RelativisticRayTracerTermContextBuilder, initialDirectory:=_InitialDirectory)

        _MainWindow.Show()
    End Sub

    Private Sub _MainWindow_Unloaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles _MainWindow.Unloaded
        Me.Shutdown()
    End Sub

End Class
