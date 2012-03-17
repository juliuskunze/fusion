Class Application
    Private WithEvents _MainWindow As MainWindow

    Private ReadOnly _InitialDirectory As New DirectoryInfo(My.Computer.FileSystem.SpecialDirectories.Desktop)

    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        System.Threading.Thread.CurrentThread.CurrentCulture = New Globalization.CultureInfo("en-US")

        Dim startupFile = If(e.Args.Count = 1 AndAlso File.Exists(e.Args.Single), New FileInfo(e.Args.Single), Nothing)

        _MainWindow = New MainWindow(RelativisticRayTracerTermContextBuilder:=New RelativisticRayTracerTermContextBuilder,
                                     initialDirectory:=_InitialDirectory,
                                     startupFile:=startupFile)
        _MainWindow.Show()

        '!!!Dim oldDir = "B:\Julius-Ordner\Schule\BeLL\Shadow\Video\146-151"
        'Dim newDir = "B:\Julius-Ordner\Schule\BeLL\Shadow\Video\new2"

        'Directory.CreateDirectory(newDir)

        'Dim maxPicture = 5
        'Dim newMinPicture = 146
        'Dim filesToMove = From i In Enumerable.Range(0, maxPicture + 1)
        '                  Let fileToMove = IO.Path.Combine(oldDir, String.Format("picture{0}.jpg", CStr(i)))
        '                  Let newPath = IO.Path.Combine(newDir, String.Format("picture{0}.jpg", CStr(i + newMinPicture)))
        '                  Select fileToMove, newPath

        'For Each fileToMove In filesToMove
        '    File.Move(fileToMove.fileToMove, fileToMove.newPath)
        'Next

        '!!!Dim splicer = New VideoSplicer(Enumerable.Range(0, 241).Except({188}).Select(Function(i) String.Format("B:\Julius-Ordner\Schule\BeLL\Shadow\Video\new\picture{0}.jpg", CStr(i))),
        '                               "B:\Julius-Ordner\Schule\BeLL\Shadow\Video\new23.avi",
        '                               24)

        'splicer.Run()
    End Sub

    Private Sub _MainWindow_Unloaded(sender As Object, e As RoutedEventArgs) Handles _MainWindow.Unloaded
        Shutdown()
    End Sub
End Class
