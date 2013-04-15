Class Application
    Private WithEvents _MainWindow As CalculatorWindow

    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        System.Threading.Thread.CurrentThread.CurrentCulture = New Globalization.CultureInfo("en-US")

        _MainWindow = New CalculatorWindow
        _MainWindow.Show()
    End Sub

    Private Sub _MainWindow_Unloaded(sender As Object, e As RoutedEventArgs) Handles _MainWindow.Unloaded
        Shutdown()
    End Sub
End Class
