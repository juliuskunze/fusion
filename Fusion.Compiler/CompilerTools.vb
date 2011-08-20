﻿Imports System.Runtime.CompilerServices

Public Module CompilerTools

    Private ReadOnly _ParameterBracketTypes As IEnumerable(Of BracketType) = {BracketType.Round}
    Public ReadOnly Property ParameterBracketTypes As IEnumerable(Of BracketType)
        Get
            Return _ParameterBracketTypes
        End Get
    End Property

    Private ReadOnly _ArgumentBracketTypes As IEnumerable(Of BracketType) = {BracketType.Curly}
    Public ReadOnly Property ArgumentBracketTypes As IEnumerable(Of BracketType)
        Get
            Return _ArgumentBracketTypes
        End Get
    End Property

    Private ReadOnly _AllowedBracketTypes As IEnumerable(Of BracketType) = {BracketType.Round, BracketType.Curly, BracketType.Inequality}
    Public ReadOnly Property AllowedBracketTypes As IEnumerable(Of BracketType)
        Get
            Return _AllowedBracketTypes
        End Get
    End Property

    Public Function GetParameters(ByVal parametersInBrackets As String) As IEnumerable(Of String)
        Return GetArgumentsOrParameters(parametersInBrackets, bracketTypes:=_ParameterBracketTypes)
    End Function

    Public Function GetArguments(ByVal argumentsInBrackets As String) As IEnumerable(Of String)
        Return GetArgumentsOrParameters(argumentsInBrackets, bracketTypes:=_ArgumentBracketTypes)
    End Function

    Public Function GetTypeArguments(ByVal typeArgumentsInBrackets As String) As IEnumerable(Of String)
        Return GetArgumentsOrParameters(typeArgumentsInBrackets, bracketTypes:={_TypeArgumentBracketTypes})
    End Function

    Public Function GetArgumentsOrParameters(ByVal inBrackets As String, bracketTypes As IEnumerable(Of BracketType)) As IEnumerable(Of String)
        If Not inBrackets.IsInBrackets(bracketTypes:=bracketTypes) Then Throw New ArgumentException("Invalid argument enumeration: '" & inBrackets & "'.")

        Dim argumentsText = inBrackets.Substring(1, inBrackets.Length - 2)
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
        Do While nameLength < s.Length AndAlso s(nameLength).IsValidVariableChar
            nameLength += 1
        Loop

        Dim name = s.Substring(0, length:=nameLength)

        If Not name.IsValidVariableName Then Throw _InvalidNameException

        Return name
    End Function

    Private ReadOnly _TypeArgumentBracketTypes As BracketType = BracketType.Inequality

    <Extension()>
    Public Function GetStartingType(s As String, types As NamedTypes, Optional ByRef out_rest As String = Nothing) As NamedType
        Dim typeNameWithoutParameters = s.GetStartingValidVariableName()
        out_rest = s.Substring(startIndex:=typeNameWithoutParameters.Count).Trim

        If out_rest.First <> _TypeArgumentBracketTypes.OpeningBracket Then Return types.Parse(typeNameWithoutParameters)

        Dim charIsInBracketsArray = out_rest.GetCharIsInBracketsArray({_TypeArgumentBracketTypes})

        Dim charsInBracketsCount = charIsInBracketsArray.Count
        For index = 0 To charIsInBracketsArray.Count - 1
            If Not charIsInBracketsArray(index) Then
                charsInBracketsCount = index
                Exit For
            End If
        Next

        Dim argumentStrings = CompilerTools.GetTypeArguments(typeArgumentsInBrackets:=out_rest.Substring(0, charsInBracketsCount))
        out_rest = out_rest.Substring(startIndex:=charsInBracketsCount)

        Return types.Parse(typeNameWithoutParameters).MakeGenericType(argumentStrings.Select(Function(argumentString) types.Parse(argumentString)))
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

        If bracketDepth > 0 Then Throw New InvalidTermException(s, message:="')' expected.")

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

    Public Function GetStartingTypedAndNamedVariable(text As String, types As NamedTypes, Optional ByRef out_rest As String = Nothing) As TypeAndName
        Dim trim = text.TrimStart

        Dim rest As String = Nothing
        Dim type = CompilerTools.GetStartingType(trim, types:=types, out_rest:=rest)

        Dim rest2 = rest.TrimStart
        Dim name = CompilerTools.GetStartingValidVariableName(rest2)

        out_rest = rest.Substring(startIndex:=name.Length)

        Return New TypeAndName(name:=name, type:=type)
    End Function

End Module
