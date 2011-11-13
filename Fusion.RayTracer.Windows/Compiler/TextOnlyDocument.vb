Public Class TextOnlyDocument

    Private Shared ReadOnly _LineBreak As String = Microsoft.VisualBasic.ControlChars.CrLf
    Private Shared ReadOnly _LineBreakLength As Integer = _LineBreak.Count

    Private ReadOnly _Text As String
    Public ReadOnly Property Text As String
        Get
            Return _Text
        End Get
    End Property

    Private ReadOnly _Document As FlowDocument

    Public Sub New(document As FlowDocument)
        _Document = document
        _Text = GetString(document)
    End Sub

    Private Shared Function GetString(document As FlowDocument) As String
        Return String.Join(_LineBreak, document.Blocks.Select(Function(block) GetString(block)))
    End Function

    Private Shared Function GetString(block As Block) As String
        If Not TypeOf block Is Paragraph Then Throw New InvalidOperationException("Only text expected.")

        Return String.Concat(CType(block, Paragraph).Inlines.Select(Function(inline) GetString(inline)))
    End Function

    Private Shared Function GetString(inline As Inline) As String
        Dim run = TryCast(inline, Run)
        If run IsNot Nothing Then Return run.Text

        Dim lineBreak = TryCast(inline, LineBreak)
        If lineBreak IsNot Nothing Then Return _LineBreak

        Throw New InvalidOperationException("Only text expected.")
    End Function

    Private Shared Function GetLength(block As Block) As Integer
        If Not TypeOf block Is Paragraph Then Throw New InvalidOperationException("Only text expected.")

        Return CType(block, Paragraph).Inlines.Select(Function(inline) GetLength(inline)).Sum

        Throw New InvalidOperationException("Only text expected.")
    End Function

    Private Shared Function GetLength(inline As Inline) As Integer
        Dim run = TryCast(inline, Run)
        If run IsNot Nothing Then Return run.Text.Count

        Dim lineBreak = TryCast(inline, LineBreak)
        If lineBreak IsNot Nothing Then Return _LineBreakLength

        Throw New InvalidOperationException("Only text expected.")
    End Function

    Public Function GetTextPointer(index As Integer) As TextPointer
        Return GetTextRange(startIndex:=index, length:=0).Start
    End Function

    Public Function GetTextRange(startIndex As Integer, length As Integer) As Documents.TextRange
        If startIndex < 0 Then Throw New ArgumentOutOfRangeException("startIndex")
        If length < 0 Then Throw New ArgumentOutOfRangeException("length")

        Dim endIndex = startIndex + length
        If endIndex > _Text.Count Then Throw New ArgumentOutOfRangeException("startIndex, length")

        Dim startPointer = GetPointer(startIndex)
        Dim endPointer = GetPointer(endIndex)
        
        If Not _Document.Blocks.Any OrElse Not _Document.Blocks.OfType(Of Paragraph).Any(Function(paragraph) paragraph.Inlines.Any) Then
            If startPointer Is Nothing Then startPointer = _Document.ContentStart
            If endPointer Is Nothing Then endPointer = _Document.ContentEnd
        End If

        Return New Documents.TextRange(startPointer, endPointer)
    End Function

    Private Function GetPointer(index As Integer) As TextPointer
        Dim paragraph As Paragraph
        Dim inline As Inline

        Dim elementStartIndex = 0
        For Each paragraph In _Document.Blocks.OfType(Of Paragraph)()
            For Each inline In paragraph.Inlines
                Dim inlineLength = GetLength(inline)

                If IsInRange(targetIndex:=index, elementStartIndex:=elementStartIndex, elementLength:=inlineLength) Then
                    Return inline.ContentStart.GetPositionAtOffset(index - elementStartIndex)
                End If

                elementStartIndex += inlineLength
            Next

            If IsInRange(
                elementStartIndex:=elementStartIndex,
                elementLength:=_LineBreakLength,
                targetIndex:=index) Then

                Return paragraph.ContentEnd
            End If

            elementStartIndex += _LineBreakLength
        Next

        Return Nothing
    End Function

    Private Shared Function IsInRange(targetIndex As Integer, elementStartIndex As Integer, elementLength As Integer) As Boolean
        Return elementStartIndex <= targetIndex AndAlso targetIndex < elementStartIndex + elementLength
    End Function

    Public Function GetIndex(textPointer As TextPointer) As Integer
        Dim parent = textPointer.Parent

        If TypeOf parent Is FlowDocument Then Return 0

        Dim position = 0

        If TypeOf parent Is Run Then
            Dim run = DirectCast(parent, Run)

            position += run.ContentStart.GetOffsetToPosition(textPointer)
            position += GetPositionInParent(run:=run)
            position += GetPositionInParent(block:=CType(run.Parent, Block))

        ElseIf TypeOf parent Is Block Then
            Dim block = DirectCast(parent, Block)

            position += GetPositionInParent(block)
        End If

        
        Return position
    End Function

    Private Function GetPositionInParent(ByVal run As Run) As Integer
        Dim position = 0

        Dim inline = run.PreviousInline
        Do While inline IsNot Nothing
            position += GetLength(inline)

            inline = inline.PreviousInline
        Loop
        Return position
    End Function

    Private Function GetPositionInParent(ByVal block As Block) As Integer
        Dim position = 0

        block = block.PreviousBlock
        Do While block IsNot Nothing
            position += GetLength(block) + _LineBreakLength

            block = block.PreviousBlock
        Loop
        Return position
    End Function

    Public Function GetTextRange(locatedString As LocatedString) As Documents.TextRange
        Return Me.GetTextRange(locatedString.Location.StartIndex, locatedString.Location.Length)
    End Function

    Public Function GetTextRange(location As TextLocation) As Documents.TextRange
        Return Me.GetTextRange(location.StartIndex, location.Length)
    End Function

    Public Shared Function GetDocumentFromText(text As String) As FlowDocument
        Dim document = New FlowDocument With {.PageWidth = 10000}

        For Each line In text.Split({_LineBreak}, StringSplitOptions.None)
            document.Blocks.Add(New Paragraph(New Run(line)))
        Next

        Return document
    End Function
End Class