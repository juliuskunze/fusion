''' <summary>
''' A term that contains no user defined constants, parameters and functions.
''' </summary>
''' <remarks></remarks>
Public Class IndependentTerm(Of TResult As Structure)
    Inherits Term

    Public Sub New(term As String)
        MyBase.New(term:=term, userContext:=New TermContext(constants:={}, parameters:={}, Functions:={}))
    End Sub

    Public Function TryGetResult() As TResult?
        Dim f = MyBase.TryGetDelegate(Of Func(Of TResult))()
        If f Is Nothing Then Return Nothing

        Return f.Invoke
    End Function

    Public Function GetResult() As TResult
        Return MyBase.GetDelegate(Of Func(Of TResult)).Invoke
    End Function

End Class
