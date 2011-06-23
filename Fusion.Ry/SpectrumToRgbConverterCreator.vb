Public Class SpectrumToRgbConverterFactory

    Private Shared ReadOnly _SpectrumToRgbConverter As SpectrumToRgbConverter

    Shared Sub New()
        Dim wavelengthRgbDictionary = New Dictionary(Of Double, RgbLight)
        Dim lines = IO.File.ReadLines(IO.Directory.GetCurrentDirectory & "\Data\CIE 1931 RGB tristimulus values.txt")
        Dim numbersCollection = From line In lines Select line.Split(" "c)
        For Each numbers In numbersCollection
            wavelengthRgbDictionary.Add(key:=CDbl(numbers(0)), value:=New RgbLight(red:=CDbl(numbers(1)), green:=CDbl(numbers(2)), blue:=CDbl(numbers(3))))
        Next

        _SpectrumToRgbConverter = New SpectrumToRgbConverter(wavelengthRgbDictionary:=wavelengthRgbDictionary, wavelengthStep:=5 * 10 ^ -9)
    End Sub

    Public Shared Function Create() As SpectrumToRgbConverter
        Return _SpectrumToRgbConverter
    End Function

End Class
