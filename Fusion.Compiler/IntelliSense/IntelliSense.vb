﻿Public Class IntelliSense

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

    Public Function GetItems() As IEnumerable(Of IntelliSenseItem)
        If Me.IsEmpty Then Me.ThrowIsEmptyException()

        Dim constants = _TermContext.Constants.Select(Function(constant) New IntelliSenseItem(signature:=constant.Signature))
        Dim parameters = _TermContext.Parameters.Select(Function(parameter) New IntelliSenseItem(signature:=parameter.Signature))
        Dim functions = _TermContext.GroupedFunctionsAndDelegateParameters.Select(Function(functionGroup) New IntelliSenseItem(functionGroup:=functionGroup))
        Dim types = _TermContext.Types.Select(Function(type) New IntelliSenseItem(signature:=type))

        Dim all = constants.Concat(parameters).Concat(functions).Concat(types)

        Return all.Where(Function(item) Me.PassesFilter(item)).OrderBy(Function(item) item.Name)
    End Function

    Private Function PassesFilter(item As IntelliSenseItem) As Boolean
        Return Me.PassesFilter(item.Name)
    End Function

    Private Function PassesFilter(name As String) As Boolean
        For i = 0 To name.Length - _Filter.Length
            If CompilerTools.IdentifierEquals(name.Substring(i, length:=_Filter.Length), _Filter) Then Return True
        Next

        Return False
    End Function

    Private Sub ThrowIsEmptyException()
        Throw New InvalidOperationException("IntelliSense is empty.")
    End Sub

    Private Shared ReadOnly _Empty As New IntelliSense
    Public Shared ReadOnly Property Empty As IntelliSense
        Get
            Return _Empty
        End Get
    End Property

End Class