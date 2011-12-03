Public Class CompilerHelp

    Private ReadOnly _TermContext As TermContext
    Private ReadOnly _CurrentIdentifierIfDefined As LocatedString
    Private ReadOnly _Filter As String
    Private ReadOnly _InnermostCalledFunction As LocatedString

    Private Sub New()
    End Sub

    Public Sub New(termContext As TermContext,
                   currentIdentifierIfDefined As LocatedString,
                   innermostCalledFunction As LocatedString)
        If termContext Is Nothing Then Throw New ArgumentNullException("termContext")

        _TermContext = termContext
        _CurrentIdentifierIfDefined = currentIdentifierIfDefined
        _Filter = Me.GetFilter
        _InnermostCalledFunction = innermostCalledFunction
    End Sub

    Private Function GetFilter() As String
        If _CurrentIdentifierIfDefined Is Nothing Then Return ""

        Return _CurrentIdentifierIfDefined.ToString
    End Function

    Public ReadOnly Property InnermostCalledFunction As LocatedString
        Get
            Return _InnermostCalledFunction
        End Get
    End Property

    Public Function GetItems() As IEnumerable(Of CompilerHelpItem)
        Me.ThrowExceptionIfIsEmpty()

        If _CurrentIdentifierIfDefined Is Nothing Then Return Enumerable.Empty(Of CompilerHelpItem)()

        Dim constants = _TermContext.Constants.Select(Function(constant) New CompilerHelpItem(signature:=constant.Signature))
        Dim parameters = _TermContext.Parameters.Select(Function(parameter) New CompilerHelpItem(signature:=parameter.Signature))
        Dim functions = _TermContext.GroupedFunctionsAndFunctionParameters.Select(Function(functionGroup) New CompilerHelpItem(functionGroup:=functionGroup))
        Dim types = _TermContext.Types.Select(Function(type) CompilerHelpItem.FromType(type:=type, types:=_TermContext.Types))
        
        Dim all = constants.Concat(parameters).Concat(functions).Concat(types).Concat(Compiler.Keywords.HelpItems)

        Return all.Where(Function(item) Me.PassesFilter(item)).OrderBy(Function(item) item.Name)
    End Function

    Private Function PassesFilter(item As CompilerHelpItem) As Boolean
        Return Me.PassesFilter(item.Name)
    End Function

    Private Function PassesFilter(name As String) As Boolean
        For i = 0 To name.Length - _Filter.Length
            If CompilerTools.IdentifierEquals(name.Substring(i, length:=_Filter.Length), _Filter) Then Return True
        Next

        Return False
    End Function

    Private Sub ThrowExceptionIfIsEmpty()
        If Me.IsEmpty Then
            Throw New InvalidOperationException("CompilerHelp is empty.")
        End If
    End Sub

    Private Shared ReadOnly _Empty As New CompilerHelp
    Public Shared ReadOnly Property Empty As CompilerHelp
        Get
            Return _Empty
        End Get
    End Property

    Public ReadOnly Property IsEmpty As Boolean
        Get
            Return _TermContext Is Nothing
        End Get
    End Property

    Public Function TryGetInnermostOpenFunctionHelp() As CompilerHelpItem
        Me.ThrowExceptionIfIsEmpty()

        If _InnermostCalledFunction Is Nothing Then Return Nothing

        Dim functionGroup = (From x In _TermContext.GroupedFunctionsAndFunctionParameters Where CompilerTools.IdentifierEquals(x.Key, _InnermostCalledFunction.ToString)).SingleOrDefault

        If functionGroup Is Nothing Then Return Nothing

        Return New CompilerHelpItem(functionGroup:=functionGroup)
    End Function


End Class