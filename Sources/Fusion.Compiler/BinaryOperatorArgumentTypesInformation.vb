Public Class BinaryOperatorArgumentTypesInformation
    Private ReadOnly _Argument1TypeInformation As TypeInformation
    Public ReadOnly Property Argument1TypeInformation As TypeInformation
        Get
            Return _Argument1TypeInformation
        End Get
    End Property

    Private ReadOnly _Argument2TypeInformation As TypeInformation
    Public ReadOnly Property Argument2TypeInformation As TypeInformation
        Get
            Return _Argument2TypeInformation
        End Get
    End Property

    Public Sub New(argument1TypeInformation As TypeInformation, argument2TypeInformation As TypeInformation)
        _Argument1TypeInformation = argument1TypeInformation
        _Argument2TypeInformation = argument2TypeInformation
    End Sub

    Private Shared ReadOnly _Infer As New BinaryOperatorArgumentTypesInformation(TypeInformation.Infer, TypeInformation.Infer)
    Public Shared ReadOnly Property Infer As BinaryOperatorArgumentTypesInformation
        Get
            Return _Infer
        End Get
    End Property
End Class
