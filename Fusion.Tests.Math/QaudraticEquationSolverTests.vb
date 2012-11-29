Public Class QaudraticEquationSolverTests
    <Test()>
    Public Sub Solve()
        Dim quadraticEqaution = New QuadraticEquation(1, -4, 4)
        Dim solutions = quadraticEqaution.Solve
        Assert.That(solutions.Contains(2))
        Assert.That(solutions.Count = 1)
    End Sub

    <Test()>
    Public Sub Solve2()
        Dim quadraticEqaution = New QuadraticEquation(1, 0, -1)
        Dim solutions = quadraticEqaution.Solve
        Assert.That(solutions.Contains(1))
        Assert.That(solutions.Contains(-1))
        Assert.That(solutions.Count = 2)
    End Sub

    <Test()>
    Public Sub Solve3()
        Dim quadraticEqaution = New QuadraticEquation(1, -3, 2)
        Dim solutions = quadraticEqaution.Solve
        Assert.That(solutions.Contains(2))
        Assert.That(solutions.Contains(1))
        Assert.That(solutions.Count = 2)
    End Sub

    <Test()>
    Public Sub Solve4()
        Dim quadraticEqaution = New QuadraticEquation(1, 3, 2)
        Dim solutions = quadraticEqaution.Solve
        Assert.That(solutions.Contains(-1))
        Assert.That(solutions.Contains(-2))
        Assert.That(solutions.Count = 2)
    End Sub

    <Test()>
    Public Sub Solve5()
        Dim quadraticEqaution = New QuadraticEquation(1, 0, 1)
        Dim solutions = quadraticEqaution.Solve
        Assert.That(solutions.Count = 0)
    End Sub
End Class
