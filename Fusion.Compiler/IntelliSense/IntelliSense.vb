Public Class IntelliSense

    Private ReadOnly _TermContext As TermContext

    Public Sub New(termContext As TermContext)
        _TermContext = termContext
    End Sub

    Public Function GetExpressionItems() As IEnumerable(Of IntelliSenseItem)
        Return _TermContext.Constants.Select(Function(constant) New IntelliSenseItem(signature:=constant.Signature)).Concat(
               _TermContext.GroupedFunctionsAndDelegateParameters.Select(Function(functionGroup) New IntelliSenseItem(functionGroup:=functionGroup)))
    End Function

    Public Function GetTypeItems() As IEnumerable(Of IntelliSenseItem)
        Return _TermContext.Types.Select(Function(type) New IntelliSenseItem(signature:=type))
    End Function

End Class