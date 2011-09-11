Public Class IntelliSense

    Private ReadOnly _TermContext As TermContext
    Private ReadOnly _Filter As String

    Public ReadOnly Property IsEmpty As Boolean
        Get
            Return _TermContext Is Nothing
        End Get
    End Property

    Private Sub New()
    End Sub

    Public Sub New(termContext As TermContext, filter As String)
        If termContext Is Nothing Then Throw New ArgumentNullException("termContext")

        _TermContext = termContext
        _Filter = filter
    End Sub

    Public Function GetExpressionItems() As IEnumerable(Of IntelliSenseItem)
        If Me.IsEmpty Then Throw New InvalidOperationException("IntelliSense is empty.")

        Return _TermContext.Constants.Where(Function(constant) constant.Signature.Name.Contains(_Filter)).Select(Function(constant) New IntelliSenseItem(signature:=constant.Signature)).Concat(
               _TermContext.GroupedFunctionsAndDelegateParameters.Where(Function(group) group.Key.Contains(_Filter)).Select(Function(functionGroup) New IntelliSenseItem(functionGroup:=functionGroup))).OrderBy(Function(item) item.Name)
    End Function

    Public Function GetTypeItems() As IEnumerable(Of IntelliSenseItem)
        If Me.IsEmpty Then Throw New InvalidOperationException("IntelliSense is empty.")

        Return _TermContext.Types.Select(Function(type) New IntelliSenseItem(signature:=type))
    End Function

    Private Shared ReadOnly _Empty As New IntelliSense
    Public Shared ReadOnly Property Empty As IntelliSense
        Get
            Return _Empty
        End Get
    End Property

End Class