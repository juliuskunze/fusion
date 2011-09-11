Public Class TextOnlyDocument

    Private Shared ReadOnly _LineBreak As Char = Microsoft.VisualBasic.ControlChars.Cr
    Private Shared ReadOnly _LineBreakLength As Integer = _LineBreak.ToString.Count

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

        Dim lineBreak = TryCast(inline, Run)
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

    Public Function GetTextRange(startIndex As Integer, length As Integer) As TextRange
        If startIndex < 0 Then Throw New ArgumentOutOfRangeException("startIndex")
        If length < 0 Then Throw New ArgumentOutOfRangeException("length")
        If startIndex + length > _Text.Count Then Throw New ArgumentOutOfRangeException("startIndex, length")

        Dim endIndex = startIndex + length

        Dim inlineStartIndex = 0
        Dim startPointer As TextPointer = Nothing
        Dim endPointer As TextPointer = Nothing
        For Each paragraph In _Document.Blocks.OfType(Of Paragraph)()
            For Each inline In paragraph.Inlines
                Dim inlineLength = GetLength(inline)

                SetTextPointerIfIsInRangeAndNothing(inline, inlineStartIndex, inlineLength, startPointer, startIndex)
                SetTextPointerIfIsInRangeAndNothing(inline, inlineStartIndex, inlineLength, endPointer, endIndex)

                inlineStartIndex += inlineLength
            Next

            inlineStartIndex += _LineBreakLength
        Next

        If Not _Document.Blocks.Any OrElse Not _Document.Blocks.OfType(Of Paragraph).Any(Function(paragraph) paragraph.Inlines.Any) Then
            If startPointer Is Nothing Then startPointer = _Document.ContentStart
            If endPointer Is Nothing Then endPointer = _Document.ContentEnd
        End If

        Return New TextRange(startPointer, endPointer)
    End Function

    Private Sub SetTextPointerIfIsInRangeAndNothing( inline As Inline,  inlineStartIndex As Integer,  inlineLength As Integer, ByRef textPointer As TextPointer,  targetIndex As Integer)
        If textPointer Is Nothing AndAlso inlineStartIndex <= targetIndex AndAlso targetIndex <= inlineStartIndex + inlineLength Then
            textPointer = inline.ContentStart.GetPositionAtOffset(targetIndex - inlineStartIndex)
        End If
    End Sub

    Public Function GetIndex(textPointer As TextPointer) As Integer
        Dim parent = textPointer.Parent

        If TypeOf parent Is FlowDocument Then Return 0

        Dim run = CType(parent, Run)

        Dim position = run.ContentStart.GetOffsetToPosition(textPointer)

        Dim inline = run.PreviousInline
        Do While inline IsNot Nothing
            position += GetLength(inline)

            inline = inline.PreviousInline
        Loop

        Dim block = CType(run.Parent, Block).PreviousBlock
        Do While block IsNot Nothing
            position += GetLength(block) + _LineBreakLength

            block = block.PreviousBlock
        Loop

        Return position
    End Function

    Public Function GetTextRange(locatedString As LocatedString) As TextRange
        Return Me.GetTextRange(locatedString.StartIndex, locatedString.Length)
    End Function

    Public Shared Function GetDocumentFromText(text As String) As FlowDocument
        Dim document = New FlowDocument

        For Each line In text.Split(_LineBreak)
            document.Blocks.Add(New Paragraph(New Run(line)))
        Next

        Return document
    End Function
End Class