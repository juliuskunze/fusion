''' <summary>
''' A term that has no user defined parameters and functions.
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class IndependentTerm
    Inherits Term

    Public Sub New(ByVal term As String)
        MyBase.New(term:=term, doubleParameterNames:={})
    End Sub

    Public Function TryParse() As Double?
        Dim f = MyBase.TryGetDelegate(Of Func(Of Double))()
        If f Is Nothing Then Return Nothing

        Return f.Invoke
    End Function

    Public Function Parse() As Double
        Return MyBase.GetDelegate(Of Func(Of Double)).Invoke
    End Function

End Class
