Public Class BracketType
    Private ReadOnly _OpeningBracket As Char
    Public ReadOnly Property OpeningBracket As Char
        Get
            Return _OpeningBracket
        End Get
    End Property

    Private ReadOnly _ClosingBracket As Char
    Public ReadOnly Property ClosingBracket As Char
        Get
            Return _ClosingBracket
        End Get
    End Property

    Public Sub New(openingBracket As Char, closingBracket As Char)
        _OpeningBracket = openingBracket
        _ClosingBracket = closingBracket
    End Sub

    Public Function InBrackets(s As String) As String
        Return Me.OpeningBracket & s & Me.ClosingBracket
    End Function

    Private Shared ReadOnly _Round As New BracketType("("c, ")"c)
    Public Shared ReadOnly Property Round As BracketType
        Get
            Return _Round
        End Get
    End Property

    Private Shared ReadOnly _Square As New BracketType("["c, "]"c)
    Public Shared ReadOnly Property Square As BracketType
        Get
            Return _Square
        End Get
    End Property

    Private Shared ReadOnly _Curly As New BracketType("{"c, "}"c)
    Public Shared ReadOnly Property Curly As BracketType
        Get
            Return _Curly
        End Get
    End Property

    Private Shared ReadOnly _Inequality As New BracketType("<"c, ">"c)
    Public Shared ReadOnly Property Inequality As BracketType
        Get
            Return _Inequality
        End Get
    End Property
End Class
