Public Class LocatedString

    Private Shared ReadOnly _WhiteSpaceTrimCondition As Func(Of Char, Boolean) = Function(c) Char.IsWhiteSpace(c)

    Private ReadOnly _ContainingAnalizedString As AnalizedString

    Private ReadOnly _StartIndex As Integer
    Public ReadOnly Property StartIndex As Integer
        Get
            Return _StartIndex
        End Get
    End Property

    Private ReadOnly _Length As Integer
    Public ReadOnly Property Length As Integer
        Get
            Return _Length
        End Get
    End Property

    Private ReadOnly _String As String

    Public Sub New(containingAnalizedString As AnalizedString, startIndex As Integer, length As Integer)
        _String = containingAnalizedString.Text.Substring(startIndex, length)
        _ContainingAnalizedString = containingAnalizedString
        _StartIndex = startIndex
        _Length = length
    End Sub

    Private Function TrimStart(trimCondition As Func(Of Char, Boolean)) As LocatedString
        Dim stopIndex = _Length

        For i = 0 To _Length - 1
            If Not trimCondition(_String(i)) Then
                stopIndex = i
                Exit For
            End If
        Next

        Return Me.Substring(startIndex:=stopIndex, length:=_Length - stopIndex)
    End Function

    Public Function TrimStart(trimChars As IEnumerable(Of Char)) As LocatedString
        Return Me.TrimStart(Function(c) trimChars.Contains(c))
    End Function

    Public Function TrimStart() As LocatedString
        Return Me.TrimStart(_WhiteSpaceTrimCondition)
    End Function

    Private Function TrimEnd(trimCondition As Func(Of Char, Boolean)) As LocatedString
        Dim stopIndex = -1

        For i = _Length - 1 To 0 Step -1
            If Not trimCondition(_String(i)) Then
                stopIndex = i
                Exit For
            End If
        Next

        Return Me.Substring(startIndex:=0, length:=stopIndex + 1)
    End Function

    Public Function TrimEnd(trimChars As IEnumerable(Of Char)) As LocatedString
        Return Me.TrimEnd(Function(c) trimChars.Contains(c))
    End Function

    Public Function Trim(trimChars As IEnumerable(Of Char)) As LocatedString
        Return Me.TrimEnd(trimChars).TrimStart(trimChars)
    End Function

    Public Function TrimEnd() As LocatedString
        Return Me.TrimEnd(_WhiteSpaceTrimCondition)
    End Function

    Public Function Trim() As LocatedString
        Return Me.TrimEnd(_WhiteSpaceTrimCondition).TrimStart(_WhiteSpaceTrimCondition)
    End Function

    Public ReadOnly Property Chars(index As Integer) As Char
        Get
            Return _ContainingAnalizedString.Text(_StartIndex + index)
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return _String
    End Function

    Public Function Substring(startIndex As Integer) As LocatedString
        Return Me.Substring(startIndex:=startIndex, length:=_Length - startIndex)
    End Function

    Public Function Substring(startIndex As Integer, length As Integer) As LocatedString
        Return New LocatedString(_ContainingAnalizedString, startIndex:=_StartIndex + startIndex, length:=length)
    End Function

    Public Function Split(separatorChars As IEnumerable(Of Char)) As IEnumerable(Of LocatedString)
        Dim strings = New List(Of LocatedString)
        Dim lastSplitCharIndex = -1

        For index = 0 To _Length - 1
            If separatorChars.Contains(_String(index)) Then
                strings.Add(Me.Substring(startIndex:=lastSplitCharIndex + 1, length:=index - lastSplitCharIndex - 1))
                lastSplitCharIndex = index
            End If
        Next

        strings.Add(Me.Substring(startIndex:=lastSplitCharIndex + 1, length:=_Length - lastSplitCharIndex - 1))

        Return strings
    End Function

End Class
