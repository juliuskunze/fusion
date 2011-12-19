Imports System.Windows.Threading

Class Application

    Private WithEvents _MainWindow As MainWindow

    Private ReadOnly _InitialDirectory As New DirectoryInfo(My.Computer.FileSystem.SpecialDirectories.Desktop)

    Private Sub Application_Startup(sender As Object, e As System.Windows.StartupEventArgs) Handles Me.Startup
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

        _MainWindow = New MainWindow(RelativisticRayTracerTermContextBuilder:=New RelativisticRayTracerTermContextBuilder, initialDirectory:=_InitialDirectory)
        _MainWindow.Show()

        'Dim splicer = New VideoSplicer1(Enumerable.Range(0, 241).Select(Function(i) String.Format("B:\Julius-Ordner\Zeitlos\Programmierung\Fusion.Ry-Bilder\Examples\1 geometry\picture{0}.jpg", CStr(i))), "B:\Julius-Ordner\Zeitlos\Programmierung\Fusion.Ry-Bilder\Examples\1 geometryNORMAL.avi", 24)
        Dim splicer = New VideoSplicer(Enumerable.Range(0, 241).Select(Function(i) String.Format("B:\Julius-Ordner\Zeitlos\Programmierung\Fusion.Ry-Bilder\Examples\sun\picture{0}.jpg", CStr(i))), "B:\Julius-Ordner\Zeitlos\Programmierung\Fusion.Ry-Bilder\Examples\0 nothingAVILIB_jpg2.avi", 24)
        'splicer.Run()
    End Sub

    Private Sub _MainWindow_Unloaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles _MainWindow.Unloaded
        Me.Shutdown()
    End Sub

End Class
