Public Class RadianceSpectrumLorentzTransformationOptions
    Private ReadOnly _IgnoreGeometryEffect As Boolean
    Private ReadOnly _IgnoreDopplerEffect As Boolean
    Private ReadOnly _IgnoreSearchlightEffect As Boolean

    Public Sub New(Optional ignoreGeometryEffect As Boolean = False,
                   Optional ignoreDopplerEffect As Boolean = False,
                   Optional ignoreSearchlightEffect As Boolean = False)
        _IgnoreGeometryEffect = ignoreGeometryEffect
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

    Public ReadOnly Property IgnoreGeometryEffect As Boolean
        Get
            Return _IgnoreGeometryEffect
        End Get
    End Property
End Class
