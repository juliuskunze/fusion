Imports System.Runtime.CompilerServices

Public Module TimeSpanExtensions

    <Extension()>
    Public Function Divide(timeSpan As TimeSpan, divisor As Double) As TimeSpan
        Return timeSpan.FromTicks(CLng(timeSpan.Ticks / divisor))
    End Function
End Module
