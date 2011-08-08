''' <summary>
''' A term that contains no user defined constants, parameters and functions.
''' </summary>
''' <remarks></remarks>
Public Class ConstantTerm(Of TResult)
    Inherits Term

    Public Sub New(term As String)
        MyBase.New(term:=term, Type:=New NamedType(name:="", systemType:=GetType(TResult)), userContext:=New TermContext(constants:={}, parameters:={}, Functions:={}))
    End Sub

    Public Function GetResult() As TResult
        Return MyBase.GetDelegate(Of Func(Of TResult)).Invoke
    End Function

End Class
