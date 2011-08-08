Public Class FunctionType
    Inherits NamedType

    Private ReadOnly _ResultType As NamedType
    Public ReadOnly Property ResultType As NamedType
        Get
            Return _ResultType
        End Get
    End Property

    Private ReadOnly _Parameters As IEnumerable(Of NamedParameter)
    Public ReadOnly Property Parameters As IEnumerable(Of NamedParameter)
        Get
            Return _Parameters
        End Get
    End Property

    Public Sub New(resultType As NamedType, parameters As IEnumerable(Of NamedParameter))
        MyBase.New(Name:=Nothing, SystemType:=Nothing)
        'GetType(Func(Of )).MakeGenericType(parameters.Select(Function(parameter) parameter.Type.SystemType).Concat({resultType.SystemType}).ToArray)
        _ResultType = resultType
        _Parameters = parameters
    End Sub

End Class
