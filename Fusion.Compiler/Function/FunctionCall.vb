Public Class FunctionCall

    Private ReadOnly _FunctionName As LocatedString
    Public ReadOnly Property FunctionName As LocatedString
        Get
            Return _FunctionName
        End Get
    End Property

    Private ReadOnly _Arguments As IEnumerable(Of LocatedString)
    Public ReadOnly Property Arguments As IEnumerable(Of LocatedString)
        Get
            Return _Arguments
        End Get
    End Property

    Private ReadOnly _LocatedString As LocatedString
    Public ReadOnly Property LocatedString As LocatedString
        Get
            Return _LocatedString
        End Get
    End Property

    Public Sub New(locatedString As LocatedString)
        _LocatedString = locatedString.Trim

        _FunctionName = _LocatedString.GetStartingIdentifier
        _Arguments = CompilerTools.GetArguments(argumentsInBrackets:=_LocatedString.Substring(startIndex:=_FunctionName.Length, length:=_LocatedString.Length - _FunctionName.Length).Trim)
    End Sub

End Class
