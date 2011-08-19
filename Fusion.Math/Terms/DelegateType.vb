Public Class DelegateType

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
        'Dim a = GetType(Func(Of )).MakeGenericType(parameters.Select(Function(parameter) parameter.Type.SystemType).Concat({resultType.SystemType}).ToArray)
        _ResultType = resultType
        _Parameters = parameters
    End Sub

    Public Sub CheckIsAssignableFrom(other As DelegateType)
        Me.ResultType.CheckIsAssignableFrom(other.ResultType)
        
        For parameterIndex = 0 To Me.Parameters.Count - 1
            Dim parameter = Me.Parameters(parameterIndex)
            Dim otherParameter = other.Parameters(parameterIndex)

            otherParameter.Type.CheckIsAssignableFrom(parameter.Type)
        Next
    End Sub

End Class
