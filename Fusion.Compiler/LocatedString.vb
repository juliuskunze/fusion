Public Class LocatedString
    Private Shared ReadOnly _WhiteSpaceTrimCondition As Func(Of Char, Boolean) = Function(c) Char.IsWhiteSpace(c)

    Private ReadOnly _ContainingAnalyzedString As AnalyzedString
    Public ReadOnly Property ContainingAnalyzedString As AnalyzedString
        Get
            Return _ContainingAnalyzedString
        End Get
    End Property

    Private ReadOnly _String As String

    Private ReadOnly _Location As TextLocation
    Public ReadOnly Property Location As TextLocation
        Get
            Return _Location
        End Get
    End Property

    Public Sub New(containingAnalyzedString As AnalyzedString, location As TextLocation)
        _String = containingAnalyzedString.Text.Substring(location.StartIndex, location.Length)
        _ContainingAnalyzedString = containingAnalyzedString
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
            If index < 0 OrElse index >= Me.Length Then Throw New ArgumentOutOfRangeException("index")

            Return _ContainingAnalyzedString.Text(_Location.StartIndex + index)
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
        Return New LocatedString(_ContainingAnalyzedString, location)
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

    Public Function GetCharIsInBracketsArray(bracketTypes As IEnumerable(Of BracketType)) As Boolean()
        Dim _CharIsInBrackets(Me.Length - 1) As Boolean
        Dim bracketDepth = 0
        Dim openedBracketTypes = New Stack(Of BracketType)

        For charIndex = 0 To Me.Length - 1
            Dim character = Me.Chars(charIndex)
            Dim openingBracketType = bracketTypes.Where(Function(bracketType) bracketType.OpeningBracket = character).SingleOrDefault
            If openingBracketType IsNot Nothing Then
                openedBracketTypes.Push(openingBracketType)
                bracketDepth += 1
            End If

            If bracketDepth > 0 Then
                _CharIsInBrackets(charIndex) = True
            End If

            Dim closingBracketType = bracketTypes.Where(Function(bracketType) bracketType.ClosingBracket = character).SingleOrDefault
            If closingBracketType IsNot Nothing Then
                If Not openedBracketTypes.Any Then Throw New InvalidTermException(Me, message:="End of term expected.")
                If openedBracketTypes.Pop IsNot closingBracketType Then Throw New InvalidTermException(Me, message:="Brackets not matching.")
                bracketDepth -= 1
            End If
        Next

        If bracketDepth > 0 Then Throw New InvalidTermException(Me, message:="')' expected.")

        Return _CharIsInBrackets
    End Function

    Public Function TryGetIdentifierBeforeLastOpenedBracket(selection As TextLocation) As LocatedString
        If selection.Length = 0 Then Return Me.TryGetIdentifierBeforeLastOpenedBracket(selection.StartIndex)

        Dim startIdentifier = Me.TryGetIdentifierBeforeLastOpenedBracket(selection.StartIndex)
        Dim endIdentifier = Me.TryGetIdentifierBeforeLastOpenedBracket(selection.EndIndex)

        If startIdentifier <> endIdentifier Then Return Nothing

        Return startIdentifier
    End Function

    Public Function TryGetIdentifierBeforeLastOpenedBracket(pointer As Integer) As LocatedString
        Dim index = Me.GetLastUnclosedOpeningBracketIndexBefore(pointer, bracketType:=BracketType.Round)

        If Not index.HasValue Then Return Nothing

        Return Me.TryGetSurroundingIdentifier(pointer:=index.Value)
    End Function

    Private Function GetLastUnclosedOpeningBracketIndexBefore(pointer As Integer, bracketType As BracketType) As Nullable(Of Integer)
        Dim relativeBracketDepth = 0

        If pointer = 0 Then Return Nothing

        For index = pointer - 1 To 0 Step -1
            Select Case Me.Chars(index)
                Case bracketType.OpeningBracket
                    relativeBracketDepth -= 1
                Case bracketType.ClosingBracket
                    relativeBracketDepth += 1
            End Select

            If relativeBracketDepth < 0 Then Return index
        Next
    End Function

    Public Function TryGetSurroundingIdentifier(selection As TextLocation) As LocatedString
        Return Me.TryGetSurroundingIdentifier(selection.StartIndex)
    End Function

    Public Function TryGetSurroundingIdentifier(pointer As Integer) As LocatedString
        If pointer < 0 OrElse pointer > Me.Length Then Throw New ArgumentOutOfRangeException("pointer")

        Dim startIndex = 0
        Dim endIndex = Me.Length

        If Me.ToString = "" Then Return Me.Substring(startIndex:=pointer, length:=0)

        If pointer = 0 Then
            For i = 0 To Me.Length - 1
                If Not Me(i).IsIdentifierChar Then
                    endIndex = i
                    Exit For
                End If
            Next

            If Not Me(startIndex).IsIdentifierStartChar Then Return Me.Substring(startIndex:=pointer, length:=0)
        ElseIf pointer = Me.Length OrElse Not Me(pointer).IsIdentifierChar Then
            endIndex = pointer

            For i = pointer - 1 To 0 Step -1
                If Not Me(i).IsIdentifierChar Then
                    startIndex = i + 1
                    Exit For
                End If
            Next


        Else

            For i = pointer To 0 Step -1
                If Not Me(i).IsIdentifierChar Then
                    startIndex = i + 1
                    Exit For
                End If
            Next

            For i = pointer + 1 To Me.Length - 1
                If Not Me(i).IsIdentifierChar Then
                    endIndex = i
                    Exit For
                End If
            Next
        End If

        If startIndex < Me.Length - 1 AndAlso Not Me(startIndex).IsIdentifierStartChar Then Return Me.Substring(startIndex:=pointer, length:=0)

        Return Me.Substring(startIndex:=startIndex, length:=endIndex - startIndex)
    End Function
End Class
