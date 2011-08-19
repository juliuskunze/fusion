Imports System.Linq.Expressions

Public NotInheritable Class RelativiticRayTracerTermContext
    Inherits TermContext

    Public Sub New()
        MyBase.New(Constants:={},
                   Functions:=_Functions,
                   Types:=_Types)
    End Sub

    Private Shared ReadOnly _Sphere As New NamedType(name:="Sphere", systemType:=GetType(Sphere))

    Private Shared ReadOnly _Types As NamedTypes = New NamedTypes({_Sphere})

    Private Shared ReadOnly _SphereConstructor As New FunctionInstance(
        name:="Sphere",
        Type:=New DelegateType(
            resultType:=_Sphere,
            parameters:={New NamedParameter(name:="center", Type:=NamedType.Vector3D),
                         New NamedParameter(name:="radius", Type:=NamedType.Real)}),
        LambdaExpression:=CType(Function(center As Vector3D, radius As Double) New Sphere(center:=center, radius:=radius), Expression(Of Func(Of Vector3D, Double, Sphere))))

    Private Shared ReadOnly _Functions As IEnumerable(Of FunctionInstance) = {_SphereConstructor}

End Class
