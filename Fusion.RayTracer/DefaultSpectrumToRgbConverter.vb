Public NotInheritable Class DefaultSpectrumToRgbConverter

    Private Shared ReadOnly _DefaultConverter As SpectrumToRgbConverter

    Shared Sub New()
        _DefaultConverter = New SpectrumToRgbConverter(testStepCount:=150)
    End Sub

    Public Shared ReadOnly Property [Get] As SpectrumToRgbConverter
        Get
            Return _DefaultConverter
        End Get
    End Property

End Class
