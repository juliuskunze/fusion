Imports System.Runtime.CompilerServices

Public Module RichTextBoxExtensions

    <Extension()>
    Public Function Text(richTextBox As RichTextBox) As String
        Return New TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text
    End Function

End Module