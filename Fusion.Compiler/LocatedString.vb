Public Class LocatedString

    Private Shared ReadOnly _WhiteSpaceTrimCondition As Func(Of Char, Boolean) = Function(c) Char.IsWhiteSpace(c)

    Private ReadOnly _ContainingAnalizedString As AnalizedString
    Public ReadOnly Property ContainingAnalizedString As AnalizedString
        Get
            Return _ContainingAnalizedString
        End Get
    End Property

    Private ReadOnly _String As String

    Private ReadOnly _Location As TextLocation
    Public ReadOnly Property Location As TextLocation
        Get
            Return _Location
        End Get
    End Property

    Public Sub New(containingAnalizedString As AnalizedString, location As TextLocation)
        _String = containingAnalizedString.Text.Substring(location.StartIndex, location.Length)
        _ContainingAnalizedString = containingAnalizedString
        _Location = location
    End Sub

    Private Function TrimStart(trimCondition As Func(Of Char, Boolean)) As LocatedString
        Dim stopIndex = _Location.Length

        For i = 0 To _Location.Length - 1
            If Not trimCondition(_String(i)) Then
                stopIndex = i
                Exit For
            End If
        Next

        Return Me.Substring(startIndex:=stopIndex, length:=_Location.Length - stopIndex)
    End Function

    Public Function TrimStart(trimChars As IEnumerable(Of Char)) As LocatedString
        Return Me.TrimStart(Function(c) trimChars.Contains(c))
    End Function

    Public Function TrimStart() As LocatedString
        Return Me.TrimStart(_WhiteSpaceTrimCondition)
    End Function

    Private Function TrimEnd(trimCondition As Func(Of Char, Boolean)) As LocatedString
        Dim stopIndex = -1

        For i = _Location.Length - 1 To 0 Step -1
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

    Default Public ReadOnly Property Chars(index As Integer) As Char
        Get
            Return _ContainingAnalizedString.Text(_Location.StartIndex + index)
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return _String
    End Function

    Public Function Substring(startIndex As Integer) As LocatedString
        Return Me.Substring(_Location.SubLocation(startIndex:=startIndex))
    End Function

    Public Function Substring(startIndex As Integer, length As Integer) As LocatedString
        Return Me.Substring(_Location.SubLocation(startIndex:=startIndex, length:=length))
    End Function

    Private Function Substring(location As TextLocation) As LocatedString
        Return New LocatedString(_ContainingAnalizedString, location)
    End Function

    Public Function Split(separatorChars As IEnumerable(Of Char)) As IEnumerable(Of LocatedString)
        Dim strings = New List(Of LocatedString)
        Dim lastSplitCharIndex = -1

        For index = 0 To _Location.Length - 1
            If separatorChars.Contains(_String(index)) Then
                strings.Add(Me.Substring(startIndex:=lastSplitCharIndex + 1, length:=index - lastSplitCharIndex - 1))
                lastSplitCharIndex = index
            End If
        Next

        strings.Add(Me.Substring(startIndex:=lastSplitCharIndex + 1, length:=_Location.Length - lastSplitCharIndex - 1))

        Return strings
    End Function

    Public ReadOnly Property Length As Integer
        Get
            Return _Location.Length
        End Get
    End Property

    Public ReadOnly Property Any As Boolean
        Get
            Return _Location.Length > 0
        End Get
    End Property

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim other = TryCast(obj, LocatedString)
        If other Is Nothing Then Return False

        Return Me._String = other._String AndAlso Me.Location = other.Location
    End Function

    Public Shared Operator =(l1 As LocatedString, l2 As LocatedString) As Boolean
        Return l1.Equals(l2)
    End Operator

    Public Shared Operator <>(l1 As LocatedString, l2 As LocatedString) As Boolean
        Return Not l1 = l2
    End Operator

End Class
