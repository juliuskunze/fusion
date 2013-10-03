Imports Fusion.Compiler
Imports Fusion.Math

Class CalculatorWindow
    Private Sub RespectOrderCheckBox_Click(sender As System.Object, e As RoutedEventArgs) Handles _RespectOrderCheckBox.Click
        SetCalculationFormulaLabelText()
    End Sub

    Private Sub RespectDuplicationCheckBox_Click(sender As System.Object, e As RoutedEventArgs) Handles _RespectDuplicationCheckBox.Click
        SetCalculationFormulaLabelText()
    End Sub

    Private Sub SetCalculationFormulaLabelText()
        _CalculationFormulaLabel.Content = GetCalculationFormulaLabelText()
    End Sub

    Private Function GetCalculationFormulaLabelText() As String
        If _RespectDuplicationCheckBox.IsChecked Then
            If _RespectOrderCheckBox.IsChecked Then
                Return "n ^ k"
            Else
                Return "(n choose k) * k!"
            End If
        Else
            If _RespectOrderCheckBox.IsChecked Then
                Return "(n - 1 + k) choose k"
            Else
                Return "n choose k"
            End If
        End If
    End Function

    Private Sub CalculatePossibilityCountButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _CalculatePossibilityCountButton.Click
        Try
            Dim result = ChoosingPossibilityCount(total:=CLng(_TotalCountTextBox.Text),
                                          chosen:=CLng(_ChosenCountTextBox.Text),
                                          respectOrder:=_RespectOrderCheckBox.IsChecked.Value,
                                          respectDuplication:=_RespectDuplicationCheckBox.IsChecked.Value)

            _ResultTextBox.Text = CStr(result)
        Catch ex As OverflowException
            _ResultTextBox.Text = "Overflow."
        Catch ex As InvalidCastException
            _ResultTextBox.Text = "Wrong input."
        End Try
    End Sub

    Private Sub Window_Loaded(sender As System.Object, e As RoutedEventArgs) Handles MyBase.Loaded
        Me.SetCalculationFormulaLabelText()
    End Sub

    Private Sub CalculatePrimesButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _CalculatePrimesButton.Click
        _PrimesResultTextBox.Clear()
        _PrimesCountLabel.Content = "Anzahl"
        Try
            Dim primes = Math.Primes(upperBound:=CInt(_UpperBoundTextBox.Text), lowerBound:=CInt(_LowerBoundTextBox.Text))

            Dim primesStringBuilder = New Text.StringBuilder
            For Each prime In primes
                primesStringBuilder.Append(CStr(prime) & ", ")
            Next

            _PrimesResultTextBox.Text = primesStringBuilder.ToString.TrimEnd({" "c, ","c})

            _PrimesCountLabel.Content = primes.Count
        Catch ex As InvalidCastException
            _PrimesCountLabel.Content = "Falsche Eingabe."
        Catch ex As OverflowException
            _PrimesCountLabel.Content = "Überlauf."
        End Try
    End Sub

    Private Sub _CalculateButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles _CalculateButton.Click
        Dim compiler = New Compiler(Of Double)(LocatedString:=_CalculationInputTextBox.Text.ToLocated, baseContext:=TermContext.Default, TypeDictionary:=TypeDictionary.Default)
        Try
            _CalculationResultTextBox.Text = compiler.Compile.Result.ToString
        Catch ex As CompilerException
            _CalculationResultTextBox.Text = ex.Message
        End Try

    End Sub
End Class
