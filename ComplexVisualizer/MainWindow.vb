Public Class MainWindow

    Private ReadOnly _CenterComplex As New Complex(0, 0)
    Private Const _Radius As Double = 100

    Private Function GetMap() As AffineMap2D
        Return AffineMap2D.Translation(-0.5 * New Vector2D(_PictureBox.Width, _PictureBox.Height)).
                   Before(AffineMap2D.Scaling(2 * _Radius / Min(_PictureBox.Width, _PictureBox.Height)).
                   Before(AffineMap2D.Translation(_CenterComplex.ToVector))).
                   Before(AffineMap2D.Reflection(0))
    End Function

    Private Sub _PictureBox_Click(sender As Object, e As EventArgs) Handles _PictureBox.Click
        Dim map = GetMap()
        Dim complexToColor = New SaturationStripedRainbowComplexToColor(pillowStrength:=1 / 5, lengthPartFactor:=6) ', argumentParts:=6)
        Dim complexFunction = Function(z As Complex) MainWindow.ComplexFunction(z)

        Dim bitmap = New Bitmap(_PictureBox.Width, _PictureBox.Height)

        For x = 0 To _PictureBox.Width - 1
            For y = 0 To _PictureBox.Height - 1
                Dim pictureVector = New Vector2D(x, y)
                Dim complex = New Complex(map.Apply(pictureVector))
                bitmap.SetPixel(x, y, GetColor(complexFunction, complex, complexToColor))
            Next
        Next

        _PictureBox.Image = bitmap
    End Sub

    Private Shared Function ComplexFunction(z As Complex) As Complex
        'Dim complexFunction = Function(z As Complex) (z ^ 18 - z) / (z - New Complex(1, 0))
        'Dim complexFunction = Function(z As Complex) (z.Inverse ^ 18 - z.Inverse) / (z.Inverse - New Complex(1, 0))
        'Dim complexFunction = Function(z As Complex) z * (z - New Complex(1, 1)) * (z - New Complex(-1, 1)) / (z - New Complex(-1, -1))
        'Dim complexFunction = Function(z As Complex) New Complex(1, 0) + z ^ 5 + z ^ 12 + z ^ 16
        'Return Function(z As Complex) Complex.Exp(z.Inverse)
        Return Enumerable.Range(0, 60).
                   Select(Function(index) z ^ index / New Complex(Factorial(index), 0)).
                   Aggregate(seed:=New Complex(0, 0), func:=Function(aggregate, newNumber) aggregate + newNumber)
    End Function

    Private Shared Function GetColor(complexFunction As Func(Of Complex, Complex), complex As Complex, complexToColor As IComplexToColor) As Color
        Try
            Dim complexResult = complexFunction(complex)
            Return complexToColor.GetColor(complexResult)
        Catch ex As ArithmeticException
            Return Color.Black
        End Try
    End Function

    Private Sub _PictureBox_MouseMove(sender As System.Object, e As Windows.Forms.MouseEventArgs) Handles _PictureBox.MouseMove
        Dim complex = New Complex(GetMap.Apply(New Vector2D(e.Location)))
        Dim transformedComplex = ComplexFunction(complex)

        _ComplexLabel.Text = ToString(complex) & " --> " & ToString(transformedComplex)
    End Sub

    Private Overloads Shared Function ToString(complex As Complex) As String
        Return complex.ToString("0.00e-0")
    End Function
End Class
