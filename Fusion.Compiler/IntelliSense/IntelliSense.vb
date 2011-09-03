Public Class IntelliSense

    Private ReadOnly _TermContext As TermContext
    Private ReadOnly _Filter As String
    
    Public Sub New(termContext As TermContext, filter As String)
        _TermContext = termContext
        _Filter = filter
    End Sub

    Public Function GetExpressionItems() As IEnumerable(Of IntelliSenseItem)
        Return _TermContext.Constants.Where(Function(constant) constant.Signature.Name.Contains(_Filter)).Select(Function(constant) New IntelliSenseItem(signature:=constant.Signature)).Concat(
               _TermContext.GroupedFunctionsAndDelegateParameters.Where(Function(group) group.Key.Contains(_Filter)).Select(Function(functionGroup) New IntelliSenseItem(functionGroup:=functionGroup)))
    End Function

    Public Function GetTypeItems() As IEnumerable(Of IntelliSenseItem)
        Return _TermContext.Types.Select(Function(type) New IntelliSenseItem(signature:=type))
    End Function

End Class