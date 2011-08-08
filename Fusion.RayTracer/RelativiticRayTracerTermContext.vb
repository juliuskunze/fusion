Imports System.Linq.Expressions

Public NotInheritable Class RelativiticRayTracerTermContext
    Private Sub New()
    End Sub

    Private Shared ReadOnly _Sphere As New NamedType(name:="Sphere", systemType:=GetType(Sphere))

    Private Shared ReadOnly _Types As NamedTypes = New NamedTypes({_Sphere})

    Private Shared ReadOnly _SphereConstructor As New FunctionExpression(
        name:="Sphere",
        Type:=New FunctionType(
            resultType:=_Sphere,
            parameters:={New NamedParameter(name:="center", Type:=NamedType.Vector3D),
                         New NamedParameter(name:="radius", Type:=NamedType.Real)}),
        LambdaExpression:=CType(Function(center As Vector3D, radius As Double) New Sphere(center:=center, radius:=radius), Expression(Of Func(Of Vector3D, Double, Sphere))))

    Private Shared ReadOnly _Functions As IEnumerable(Of FunctionExpression) = {_SphereConstructor}

    Private Shared ReadOnly _Get As New TermContext(Constants:={},
                                                    parameters:={},
                                                    Functions:=_Functions,
                                                    types:=_Types)

    Public Shared Function [Get]() As TermContext
        Return _Get
    End Function


End Class
