Public Module ParticleSystems2D
    Public Function MoonEarthSystem() As ParticleSystem2D

        MoonEarthSystem = New ParticleSystem2D

        Dim earth = New SphereParticle2D(mass:=SunSystemConstants.EarthMass, _
                                     Location:=New Vector2D(0, 0), _
                                     velocity:=New Vector2D(0, SunSystemConstants.MoonEarthMinimalVelocity * SunSystemConstants.MoonMass / SunSystemConstants.EarthMass), _
                                     Color:=Color.Blue, _
                                     radius:=SunSystemConstants.EarthRadius)

        Dim moon = New SphereParticle2D(mass:=SunSystemConstants.MoonMass, _
                                           Location:=New Vector2D(SunSystemConstants.MoonEarthMaximalDistance, 0), _
                                           velocity:=New Vector2D(0, -SunSystemConstants.MoonEarthMinimalVelocity), _
                                           Color:=Color.GhostWhite, _
                                           radius:=SunSystemConstants.MoonRadius)

        MoonEarthSystem.AddNode(earth)
        MoonEarthSystem.AddNode(moon)
        MoonEarthSystem.AddEdge(New Force2D(New FieldForceGenerator2D(New Gravity2D), New EndNodes(Of Particle2D)(earth, moon), Color.Gray))


        Return MoonEarthSystem
    End Function

    Public Function Dipole(mass As Double, positiveCharge As Double, negativeCharge As Double, distance As Double, radius As Double) As ParticleSystem2D
        Dipole = New ParticleSystem2D

        Dim plus = New SphereParticle2D(mass:=mass, _
                                     Location:=New Vector2D(distance / 2, 0), _
                                     velocity:=New Vector2D, _
                                     Color:=Color.Red, _
                                     radius:=radius, _
                                     charge:=positiveCharge)

        Dim minus = New SphereParticle2D(mass:=mass, _
                                     Location:=New Vector2D(-distance / 2, 0), _
                                     velocity:=New Vector2D, _
                                     Color:=Color.Blue, _
                                     radius:=radius, _
                                     charge:=negativeCharge)

        Dim minus2 = New SphereParticle2D(mass:=mass, _
                             Location:=New Vector2D(-distance / 3, 0), _
                             velocity:=New Vector2D, _
                             Color:=Color.Blue, _
                             radius:=radius, _
                             charge:=5 * negativeCharge)

        Dim plus2 = New SphereParticle2D(mass:=mass, _
                     Location:=New Vector2D(distance / 3, 0), _
                     velocity:=New Vector2D, _
                     Color:=Color.Red, _
                     radius:=radius, _
                     charge:=5 * positiveCharge)

        Dipole.AddNode(plus)
        Dipole.AddNode(minus)
        Dipole.AddNode(plus2)
        Dipole.AddNode(minus2)
        Dipole.AddEdge(New Force2D(New FieldForceGenerator2D(New Electric2D), New EndNodes(Of Particle2D)(plus, minus), Color.Gray))
        Dipole.AddEdge(New Force2D(New FieldForceGenerator2D(New Electric2D), New EndNodes(Of Particle2D)(plus, minus2), Color.Gray))
        Dipole.AddEdge(New Force2D(New FieldForceGenerator2D(New Electric2D), New EndNodes(Of Particle2D)(minus, minus2), Color.Gray))

        Return Dipole
    End Function
End Module
