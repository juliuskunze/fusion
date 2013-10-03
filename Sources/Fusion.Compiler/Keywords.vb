Public NotInheritable Class Keywords
    Private Sub New()
    End Sub

    Public Const Cases = "Cases"
    Public Const [Else] = "Else"
    Public Const [Return] = "Return"
    Public Const FunctionType = "FunctionType"

    Private Shared ReadOnly _HelpItems As IEnumerable(Of CompilerHelpItem) =
                                {
                                    CompilerHelpItem.FromKeyword(Keywords.Return,
                                                                 "Syntax: 'Return <expression>'" & Microsoft.VisualBasic.vbCrLf &
                                                                 "Returns the specified value as the total result of the program."),
                                                                 CompilerHelpItem.FromKeyword(Keywords.Cases,
                                                                                              "Syntax: 'Cases([<condition1> : <value1> [, <condition2> : <value2> [, ...]], ]else : <elseValue>)'" & Microsoft.VisualBasic.vbCrLf &
                                                                                              "Supports case distinction. Returns the assigned specified value of the first specified condition that is true." & Microsoft.VisualBasic.vbCrLf &
                                                                                              "Example: 'cases(x>0 : 1, x<0 : -1, else : 0)' is 1 if x>0, -1 if x<0 and 0 if x=0."),
                                                                 CompilerHelpItem.FromKeyword(Keywords.FunctionType,
                                                                                              "Supports function type definition. " & Microsoft.VisualBasic.vbCrLf &
                                                                                              "Syntax: 'FunctionType <type> <name>(<parameters>)'" & Microsoft.VisualBasic.vbCrLf &
                                                                                              "Example: 'FunctionType Real SimpleFunction(Real a); Real Square(Real a) = a^2; Real ApplyTwice(SimpleFunction f, Real a) = f(f(a)); Real result = ApplyTwice(square, 3)' (Result would be (3^2)^2=81.)")
                                }

    Public Shared ReadOnly Property HelpItems As IEnumerable(Of CompilerHelpItem)
        Get
            Return _HelpItems
        End Get
    End Property
End Class
