Public Class RelativisticRayTracerTermContext
    Inherits TermContext

    Private Shared ReadOnly _SpecialTypes As New NamedTypes({New NamedType("Plane", GetType(Plane))})
    Private Shared ReadOnly _NamedTypes As NamedTypes = NamedTypes.Default.Merge(_SpecialTypes)
    Private Shared ReadOnly _TypeDictionary As New TypeNamedTypeDictionary(_NamedTypes)
    Public Shared ReadOnly Property TypeDictionary As TypeNamedTypeDictionary
        Get
            Return _TypeDictionary
        End Get
    End Property

    Private Shared ReadOnly _Constants As IEnumerable(Of ConstantInstance) = {New ConstantInstance(Of Double)("c", SpeedOfLight, _TypeDictionary)}
    Private Shared ReadOnly _Functions As IEnumerable(Of FunctionInstance) = {New FunctionInstance(Of Func(Of Vector3D, Vector3D, Plane))("Plane", Function(location, normal) New Plane(location:=location, normal:=normal), _TypeDictionary)}

    Public Sub New()
        MyBase.New(Constants:=_Constants, Functions:=_Functions, Types:=_NamedTypes)
    End Sub

End Class
