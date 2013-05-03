Public Class LorentzTransformationAtSightRayOptions
    Private ReadOnly _IgnoreAberrationEffect As Boolean
    Private ReadOnly _IgnoreDopplerEffect As Boolean
    Private ReadOnly _IgnoreSearchlightEffect As Boolean

    Public Sub New(Optional ignoreAberrationEffect As Boolean = False,
                   Optional ignoreDopplerEffect As Boolean = False,
                   Optional ignoreSearchlightEffect As Boolean = False)
        _IgnoreAberrationEffect = ignoreAberrationEffect
        _IgnoreDopplerEffect = ignoreDopplerEffect
        _IgnoreSearchlightEffect = ignoreSearchlightEffect
    End Sub

    Public ReadOnly Property IgnoreSearchlightEffect As Boolean
        Get
            Return _IgnoreSearchlightEffect
        End Get
    End Property

    Public ReadOnly Property IgnoreDopplerEffect As Boolean
        Get
            Return _IgnoreDopplerEffect
        End Get
    End Property

    Public ReadOnly Property IgnoreAberrationEffect As Boolean
        Get
            Return _IgnoreAberrationEffect
        End Get
    End Property
End Class
