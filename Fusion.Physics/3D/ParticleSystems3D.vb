Public Module ParticleSystems3D
    Public Function MoonEarthSystem() As ParticleSystem3D

        MoonEarthSystem = New ParticleSystem3D

        Dim earth = New SphereParticle3D(mass:=SunSystemConstants.EarthMass, _
                                     Location:=New Vector3D(0, 0, 0), _
                                     velocity:=New Vector3D(0, SunSystemConstants.MoonEarthMinimalVelocity * SunSystemConstants.MoonMass / SunSystemConstants.EarthMass, 0), _
                                     Color:=Color.Blue, _
                                     radius:=SunSystemConstants.EarthRadius)

        Dim moon = New SphereParticle3D(mass:=SunSystemConstants.MoonMass, _
                                           Location:=New Vector3D(SunSystemConstants.MoonEarthMaximalDistance, 0, 0), _
                                           velocity:=New Vector3D(0, -SunSystemConstants.MoonEarthMinimalVelocity, 0), _
                                           Color:=Color.GhostWhite, _
                                           radius:=SunSystemConstants.MoonRadius)

        MoonEarthSystem.AddNode(earth)
        MoonEarthSystem.AddNode(moon)
        MoonEarthSystem.AddEdge(New Force3D(New GravityField3D, New EndNodes(Of Particle3D)(earth, moon), Color.Gray))


        Return MoonEarthSystem
    End Function

    Public Function Dipole(ByVal mass As Double, ByVal positiveCharge As Double, ByVal negativeCharge As Double, ByVal distance As Double, ByVal radius As Double) As ParticleSystem3D
        Dipole = New ParticleSystem3D

        Dim plus = New SphereParticle3D(mass:=mass, _
                                     Location:=New Vector3D(distance / 2, 4, 1), _
                                     velocity:=New Vector3D, _
                                     Color:=Color.Red, _
                                     radius:=radius, _
                                     charge:=positiveCharge)

        Dim minus = New SphereParticle3D(mass:=mass, _
                                     Location:=New Vector3D(-distance / 2, 1, 10), _
                                     velocity:=New Vector3D, _
                                     Color:=Color.Blue, _
                                     radius:=radius, _
                                     charge:=negativeCharge)

        Dim minus2 = New SphereParticle3D(mass:=mass, _
                             Location:=New Vector3D(-distance / 3, 3, 20), _
                             velocity:=New Vector3D, _
                             Color:=Color.Blue, _
                             radius:=radius, _
                             charge:=2 * negativeCharge)

        Dim plus2 = New SphereParticle3D(mass:=mass, _
                     Location:=New Vector3D(distance / 3, -5, 4), _
                     velocity:=New Vector3D, _
                     Color:=Color.Red, _
                     radius:=radius, _
                     charge:=2 * positiveCharge)

        Dipole.AddNode(plus)
        Dipole.AddNode(minus)
        Dipole.AddNode(plus2)
        Dipole.AddNode(minus2)
        Dipole.AddEdge(New Force3D(New ElectricField3D, New EndNodes(Of Particle3D)(plus, minus), Color.Gray))
        Dipole.AddEdge(New Force3D(New ElectricField3D, New EndNodes(Of Particle3D)(plus, minus2), Color.Gray))
        Dipole.AddEdge(New Force3D(New ElectricField3D, New EndNodes(Of Particle3D)(minus, minus2), Color.Gray))
        Dipole.AddEdge(New Force3D(New ElectricField3D, New EndNodes(Of Particle3D)(plus2, minus), Color.Gray))
        Dipole.AddEdge(New Force3D(New ElectricField3D, New EndNodes(Of Particle3D)(plus2, minus2), Color.Gray))
        Dipole.AddEdge(New Force3D(New ElectricField3D, New EndNodes(Of Particle3D)(plus2, plus), Color.Gray))

        Return Dipole
    End Function
End Module
