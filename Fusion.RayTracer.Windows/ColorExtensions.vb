Imports System.Runtime.CompilerServices

Public Module ColorExtensions

    <Extension()>
    Public Function ToMediaColor(ByVal systemColor As System.Drawing.Color) As System.Windows.Media.Color
        Return System.Windows.Media.Color.FromArgb(a:=systemColor.A,
                                                   r:=systemColor.R,
                                                   g:=systemColor.G,
                                                   b:=systemColor.B)
    End Function

    <Extension()>
    Public Function ToMediaBrush(ByVal systemColor As System.Drawing.Color) As System.Windows.Media.Brush
        Return New System.Windows.Media.SolidColorBrush(systemColor.ToMediaColor)
    End Function

End Module
