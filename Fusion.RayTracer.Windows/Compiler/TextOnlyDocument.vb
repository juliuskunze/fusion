Public Class TextOnlyDocument

    Private Shared ReadOnly _LineBreak As String = Microsoft.VisualBasic.ControlChars.NewLine
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
        _Text = DocumentToString(document)
    End Sub

    Private Shared Function DocumentToString(document As FlowDocument) As String
        Return String.Join(_LineBreak, document.Blocks.Select(Function(block) BlockToString(block)))
    End Function

    Private Shared Function BlockToString(block As Block) As String
        If Not TypeOf block Is Paragraph Then Throw New InvalidOperationException("Only text expected.")

        Return String.Concat(CType(block, Paragraph).Inlines.Select(Function(inline) InlineToString(inline)))
    End Function

    Private Shared Function InlineToString(inline As Inline) As String
        Dim run = TryCast(inline, Run)
        If run IsNot Nothing Then Return run.Text

        Dim lineBreak = TryCast(inline, Run)
        If lineBreak IsNot Nothing Then Return _LineBreak

        Throw New InvalidOperationException("Only text expected.")
    End Function

    Public Function GetTextRange(startIndex As Integer, length As Integer) As TextRange
        If startIndex < 0 Then Throw New ArgumentOutOfRangeException("startIndex")
        If length < 0 Then Throw New ArgumentOutOfRangeException("length")
        If startIndex + length > _Text.Count Then Throw New ArgumentOutOfRangeException("startIndex, length")

        Dim endIndex = startIndex + length

        Dim index = 0
        Dim startPointer As TextPointer = Nothing
        Dim endPointer As TextPointer = Nothing
        For Each paragraph In _Document.Blocks.OfType(Of Paragraph)()
            For Each inline In paragraph.Inlines
                Dim inlineLength = GetInlineLength(inline)

                If startPointer Is Nothing AndAlso index <= startIndex AndAlso startIndex <= index + inlineLength Then
                    startPointer = inline.ContentStart.GetPositionAtOffset(startIndex - index)
                End If

                If endPointer Is Nothing AndAlso index <= endIndex AndAlso endIndex <= index + inlineLength Then
                    endPointer = inline.ContentStart.GetPositionAtOffset(endIndex - index)
                End If

                index += inlineLength
            Next

            index += _LineBreakLength
        Next

        If Not _Document.Blocks.Any OrElse Not _Document.Blocks.OfType(Of Paragraph).Any(Function(paragraph) paragraph.Inlines.Any) Then
            If startPointer Is Nothing Then startPointer = _Document.ContentStart
            If endPointer Is Nothing Then endPointer = _Document.ContentEnd
        End If

        Return New TextRange(startPointer, endPointer)
    End Function

    Private Shared Function GetInlineLength(inline As Inline) As Integer
        Dim run = TryCast(inline, Run)
        If run IsNot Nothing Then Return run.Text.Count

        Dim lineBreak = TryCast(inline, LineBreak)
        If lineBreak IsNot Nothing Then Return _LineBreakLength

        Throw New InvalidOperationException("Only text expected.")
    End Function

End Class
