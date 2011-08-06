Public Class RelativisticRayTracerDrawerCompiler

    Private ReadOnly _PictureSize As System.Drawing.Size
    Private ReadOnly _DescriptionText As String
    Private ReadOnly _RadiancePerWhite As Double

    Public Sub New(pictureSize As System.Drawing.Size, radiancePerWhite As Double, descriptionText As String)
        _PictureSize = pictureSize
        _DescriptionText = descriptionText
        _RadiancePerWhite = radiancePerWhite
    End Sub

    Public Function Compile() As RayTraceDrawer(Of RadianceSpectrum)
        Return New RayTracingExamples(_PictureSize).BlackBodyPlaneRelativistic(New RadianceSpectrumToColorConverter(testStepCount:=150, spectralRadiancePerWhite:=_RadiancePerWhite))
    End Function

    Public ReadOnly Property Errors As List(Of String)
        Get
            Return New List(Of String) From {"TestError"}
        End Get
    End Property

End Class
