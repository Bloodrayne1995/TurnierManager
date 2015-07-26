Public Class ChampionData

#Region "Internal"

    Private _championID As Integer
    Private _name As String = ""
    Private _titel As String = ""
    Private _primary As String = ""
    Private _secondary As String = ""
    Private _icon() As Byte
    Private _banner() As Byte

#End Region


#Region "Property"

    Public ReadOnly Property Name As String
        Get
            Return _name
        End Get
    End Property

    Public ReadOnly Property Titel As String
        Get
            Return _titel
        End Get
    End Property

    Public ReadOnly Property Primary As String
        Get
            Return _primary
        End Get
    End Property

    Public ReadOnly Property Secondary As String
        Get
            Return _secondary
        End Get
    End Property

    Public Property Icon As Byte()
        Get
            Return _icon
        End Get
        Set(value As Byte())
            _icon = value
        End Set
    End Property

    Public Property Banner As Byte()
        Get
            Return _banner
        End Get
        Set(value As Byte())
            _banner = value
        End Set
    End Property

#End Region

End Class
