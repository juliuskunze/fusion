Imports System.Runtime.CompilerServices

Public Module CompilerTools

    Private ReadOnly _ArgumentBracketTypes As IEnumerable(Of BracketType) = {BracketType.Round}
    Public ReadOnly Property ArgumentBracketTypes As IEnumerable(Of BracketType)
        Get
            Return _ArgumentBracketTypes
        End Get
    End Property

    Private ReadOnly _AllowedBracketTypes As IEnumerable(Of BracketType) = {BracketType.Round, BracketType.Inequality}
    Public ReadOnly Property AllowedBracketTypes As IEnumerable(Of BracketType)
        Get
            Return _AllowedBracketTypes
        End Get
    End Property


    Public Function GetArguments(ByVal argumentsInBrackets As String) As IEnumerable(Of String)
        If Not argumentsInBrackets.IsInBrackets(bracketTypes:=_ArgumentBracketTypes) Then Throw New ArgumentException("Invalid function call.")

        Dim argumentsText = argumentsInBrackets.Substring(1, argumentsInBrackets.Length - 2)
        Return SplitIfSeparatorIsNotInBrackets(argumentsText, separator:=","c, bracketTypes:=_AllowedBracketTypes)
    End Function

    <Extension()>
    Public Function SplitIfSeparatorIsNotInBrackets(s As String, separator As Char, bracketTypes As IEnumerable(Of BracketType)) As IEnumerable(Of String)
        Dim inBracketsArray = s.GetCharIsInBracketsArray(bracketTypes:=_AllowedBracketTypes)
        Dim arguments = New List(Of String)

        Dim lastSplitCharIndex = -1
        For splitCharIndex = 0 To s.Length
            If splitCharIndex = s.Length OrElse
               (s(splitCharIndex) = separator AndAlso Not inBracketsArray(splitCharIndex)) Then
                arguments.Add(s.Substring(startIndex:=lastSplitCharIndex + 1, length:=splitCharIndex - lastSplitCharIndex - 1))
                lastSplitCharIndex = splitCharIndex
            End If
        Next
        Return arguments
    End Function

    <Extension()>
    Public Function GetStartingValidVariableName(s As String) As String
        If s.Length = 0 Then Throw _InvalidNameException
        If Not s.First.IsValidVariableStartChar Then Throw _InvalidNameException

        Dim nameLength = 0
        Do While nameLength < s.Length - 1 AndAlso s(nameLength).IsValidVariableChar
            nameLength += 1
        Loop

        Dim name = s.Substring(0, length:=nameLength)

        If Not name.IsValidVariableName Then Throw _InvalidNameException

        Return name
    End Function

    <Extension()>
    Public Function WithoutBlanks(s As String) As String
        Return New String((s.Where(Function(c) Not Char.IsWhiteSpace(c))).ToArray)
    End Function

    <Extension()>
    Public Function IsValidVariableName(s As String) As Boolean
        If s = "" Then Return False

        Return s.First.IsValidVariableStartChar AndAlso s.All(Function(c) c.IsValidVariableChar)
    End Function

    <Extension()>
    Public Function IsValidVariableStartCharacter(s As String) As Boolean
        If s = "" Then Return False

        Return Char.IsLetter(s.First) AndAlso s.All(Function(c) Char.IsLetterOrDigit(c))
    End Function

    <Extension()>
    Public Function GetCharIsInBracketsArray(s As String) As Boolean()
        Return s.GetCharIsInBracketsArray(bracketTypes:={BracketType.Round})
    End Function

    <Extension()>
    Public Function GetCharIsInBracketsArray(s As String, bracketTypes As IEnumerable(Of BracketType)) As Boolean()
        Dim _CharIsInBrackets(s.Length - 1) As Boolean
        Dim bracketDepth = 0
        Dim openedBracketTypes = New Stack(Of BracketType)

        For charIndex = 0 To s.Length - 1
            Dim character = s.Chars(charIndex)
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
                If Not openedBracketTypes.Any Then Throw New InvalidTermException(s, message:="End of term expected.")
                If openedBracketTypes.Pop IsNot closingBracketType Then Throw New InvalidTermException(s, message:="Brackets not matching.")
                bracketDepth -= 1
            End If
        Next

        If bracketDepth > 0 Then Throw New InvalidTermException(s, message:="Missing ')'.")

        Return _CharIsInBrackets
    End Function

    <Extension()>
    Public Function IsInBrackets(s As String) As Boolean
        Return s.IsInBrackets(bracketTypes:={BracketType.Round})
    End Function

    <Extension()>
    Public Function IsInBrackets(s As String, bracketTypes As IEnumerable(Of BracketType)) As Boolean
        If s.Count < 2 Then Return False
        If Not bracketTypes.Select(Function(bracketType) bracketType.OpeningBracket).Contains(s.First) Then Return False
        If Not bracketTypes.Select(Function(bracketType) bracketType.ClosingBracket).Contains(s.Last) Then Return False

        Dim charIsInBracketsArray = s.GetCharIsInBracketsArray(bracketTypes:=bracketTypes)

        Return charIsInBracketsArray.All(Function(inBrackets) inBrackets)
    End Function

    Private ReadOnly _InvalidNameException As New ArgumentException("Invalid name.")

End Module
