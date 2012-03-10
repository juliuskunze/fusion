﻿Class Application
    Private WithEvents _MainWindow As MainWindow

    Private ReadOnly _InitialDirectory As New DirectoryInfo(My.Computer.FileSystem.SpecialDirectories.Desktop)

    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        System.Threading.Thread.CurrentThread.CurrentCulture = New Globalization.CultureInfo("en-US")

        Dim startupFile = If(e.Args.Count = 1 AndAlso File.Exists(e.Args.Single), New FileInfo(e.Args.Single), Nothing)

        _MainWindow = New MainWindow(RelativisticRayTracerTermContextBuilder:=New RelativisticRayTracerTermContextBuilder,
                                     initialDirectory:=_InitialDirectory,
                                     startupFile:=startupFile)
        _MainWindow.Show()
    End Sub

    Private Sub _MainWindow_Unloaded(sender As Object, e As RoutedEventArgs) Handles _MainWindow.Unloaded
        Shutdown()
    End Sub
End Class
