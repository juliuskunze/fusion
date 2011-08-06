Public Class QuadraticEquation
    Public Property QuadraticCoefficient As Double
    Public Property LinearCoefficient As Double
    Public Property AbsoluteCoefficient As Double

    Public Sub New(quadraticCoefficient As Double, linearCoefficient As Double, absoluteCoefficient As Double)
        Me.QuadraticCoefficient = quadraticCoefficient
        Me.LinearCoefficient = linearCoefficient
        Me.AbsoluteCoefficient = absoluteCoefficient
    End Sub

    Public Function Solve() As List(Of Double)
        ' x² + px + q = 0
        Dim p = LinearCoefficient / QuadraticCoefficient
        Dim q = AbsoluteCoefficient / QuadraticCoefficient

        Dim summand As Double = -p / 2
        Dim discriminant As Double = summand ^ 2 - q

        Dim solutions = New List(Of Double)
        If discriminant > 0 Then
            Dim root = Sqrt(discriminant)
            solutions.Add(summand + root)
            solutions.Add(summand - root)
        ElseIf discriminant < 0 Then
        ElseIf discriminant = 0 Then
            solutions.Add(summand)
        End If

        Return solutions
    End Function

End Class

