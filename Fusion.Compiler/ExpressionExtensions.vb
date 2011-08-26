Imports System.Runtime.CompilerServices

Friend Module ExpressionExtensions

    <Extension()>
    Friend Function WithNamedType(expression As Expression, namedType As NamedType) As ExpressionWithNamedType
        Return New ExpressionWithNamedType(expression, namedType)
    End Function

End Module
