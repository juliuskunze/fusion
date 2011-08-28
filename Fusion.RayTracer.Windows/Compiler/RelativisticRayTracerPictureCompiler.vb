Public Class RelativisticRayTracerPictureCompiler
    Inherits Compiler(Of RayTracerPicture(Of RadianceSpectrum))

    Public Sub New(richTextBox As RichTextBox, baseContext As TermContext, typeNamedTypeDictionary As TypeNamedTypeDictionary)
        MyBase.New(Text:=New TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text, baseContext:=baseContext, typeNamedTypeDictionary:=typeNamedTypeDictionary)
    End Sub

End Class
