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
        _ResultType = resultType
        _Parameters = parameters
        _SystemType = Me.GetSystemType()
    End Sub

    Public Sub CheckIsAssignableFrom(other As DelegateType)
        Me.ResultType.CheckIsAssignableFrom(other.ResultType)

        For parameterIndex = 0 To Me.Parameters.Count - 1
            Dim parameter = Me.Parameters(parameterIndex)
            Dim otherParameter = other.Parameters(parameterIndex)

            otherParameter.Type.CheckIsAssignableFrom(parameter.Type)
        Next
    End Sub

    Public Function IsAssignableFrom(other As DelegateType) As Boolean
        If Not Me.ResultType.IsAssignableFrom(other.ResultType) Then Return False

        For parameterIndex = 0 To Me.Parameters.Count - 1
            Dim parameter = Me.Parameters(parameterIndex)
            Dim otherParameter = other.Parameters(parameterIndex)

            If Not otherParameter.Type.IsAssignableFrom(parameter.Type) Then Return False
        Next

        Return True
    End Function

    Private ReadOnly _SystemType As Type
    Public ReadOnly Property SystemType As Type
        Get
            Return _SystemType
        End Get
    End Property

    Private Function GetSystemType() As Type
        Dim resultType = Me.ResultType.SystemType
        Dim parameterTypes = Me.Parameters.Select(Function(parameter) parameter.Type.SystemType)
        Return GetDelegateFunctionType(parameterTypes, resultType)
    End Function
    
    Private Shared Function GetDelegateFunctionType(ByVal parameterTypes As IEnumerable(Of Type), ByVal resultType As Type) As Type
        Return GetDelegateFunctionType(parameterCount:=parameterTypes.Count).MakeGenericType(parameterTypes.Concat({resultType}).ToArray)
    End Function

    Private Shared Function GetDelegateFunctionType(parameterCount As Integer) As Type
        Select Case parameterCount
            Case 0
                Return GetType(Func(Of ))
            Case 1
                Return GetType(Func(Of ,))
            Case 2
                Return GetType(Func(Of ,,))
            Case 3
                Return GetType(Func(Of ,,,))
            Case 4
                Return GetType(Func(Of ,,,,))
            Case 5
                Return GetType(Func(Of ,,,,,))
            Case 6
                Return GetType(Func(Of ,,,,,,))
            Case 7
                Return GetType(Func(Of ,,,,,,,))
            Case 8
                Return GetType(Func(Of ,,,,,,,,))
            Case 9
                Return GetType(Func(Of ,,,,,,,,,))
            Case 10
                Return GetType(Func(Of ,,,,,,,,,,))
            Case 11
                Return GetType(Func(Of ,,,,,,,,,,,))
            Case 12
                Return GetType(Func(Of ,,,,,,,,,,,,))
            Case 13
                Return GetType(Func(Of ,,,,,,,,,,,,,))
            Case 14
                Return GetType(Func(Of ,,,,,,,,,,,,,,))
            Case 15
                Return GetType(Func(Of ,,,,,,,,,,,,,,,))
            Case 16
                Return GetType(Func(Of ,,,,,,,,,,,,,,,,))
            Case Else
                Throw New ArgumentException("Function can not have more than 16 parameters.")
        End Select
    End Function

End Class
