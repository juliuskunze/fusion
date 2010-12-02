Public Module Geometry

    Public Function SphereVolume(ByVal radius As Double) As Double
        Return 4 / 3 * PI * radius ^ 3
    End Function

    Public Function SphereSurfaceArea(ByVal radius As Double) As Double
        Return 4 * PI * radius ^ 2
    End Function

    Public Function CircleArea(ByVal radius As Double) As Double
        Return PI * radius ^ 2
    End Function

    Public Function CircleCircumference(ByVal radius As Double) As Double
        Return 2 * PI * radius
    End Function

    Public Function SphereRadiusFromVolume(ByVal volume As Double) As Double
        Return (volume / (4 / 3 * PI)) ^ (1 / 3)
    End Function

End Module
