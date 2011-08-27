Public Class RelativisticRayTracerPictureCompiler
    Inherits Compiler(Of RayTracerPicture(Of RadianceSpectrum))

    Public Sub New(textBox As TextBox, baseContext As TermContext, typeNamedTypeDictionary As TypeNamedTypeDictionary)
        MyBase.New(Text:=textBox.Text, baseContext:=baseContext, typeNamedTypeDictionary:=typeNamedTypeDictionary)
    End Sub

End Class
