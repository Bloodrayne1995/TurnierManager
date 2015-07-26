Public Class PlayerData

#Region "Internal"

    Private _ign As String = ""
    Private _fname As String = ""
    Private _vname As String = ""
    Private _nation As String = ""
    Private _spielerID As Integer = -1
    Private _neu As Boolean = False


#End Region

#Region "Properties"

    Public ReadOnly Property IngameName As String
        Get
            Return _ign
        End Get
    End Property

    Public ReadOnly Property FamilienName As String
        Get
            Return _fname
        End Get
    End Property

    Public ReadOnly Property Vorname As String
        Get
            Return _vname
        End Get
    End Property

    Public ReadOnly Property Nation As String
        Get
            Return _nation
        End Get
    End Property

    Public ReadOnly Property SpielerID As Integer
        Get
            Return _spielerID
        End Get
    End Property


#End Region

#Region "Konstruktoren"

    ''' <summary>
    ''' Erstellt ein SPieler und lädt seine Daten aus der Datenbank
    ''' </summary>
    ''' <param name="id"></param>
    ''' <remarks></remarks>
    Public Sub New(id As Integer)
        ladeSpieler(id)
        _neu = False
    End Sub

    ''' <summary>
    ''' Erstellt ein neuer Spieler
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _neu = True
    End Sub

#End Region

#Region "Methoden"

    ''' <summary>
    ''' Lädt den Spieler aus der Datenbank
    ''' </summary>
    ''' <param name="id"></param>
    ''' <remarks></remarks>
    Public Sub ladeSpieler(ByVal id As Integer)
        _spielerID = id

        Dim cDB As New DBVerbindung.Databases.SQLLiteDB()
        cDB.open(My.Settings.dbPfad)

        Dim dt As DataTable = cDB.query("SELECT ign,nation,name,vName FROM Spieler WHERE spielerID=" & SpielerID)

        If dt.Rows.Count > 0 Then
            _ign = dt(0)(0)
            _nation = dt(0)(1)
            _fname = dt(0)(2)
            _vname = dt(0)(3)
        End If

    End Sub

    ''' <summary>
    ''' Speichert den Spieler in der Datenbank
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub saveSpieler()
        Dim cDB As New DBVerbindung.Databases.SQLLiteDB()
        cDB.open(My.Settings.dbPfad)

        If _neu Then
            'Insert
            cDB.execute("INSERT INTO Spieler (ign,name,vName,nation) VALUES('" & _ign & "','" & _fname & "','" & _vname & "','" & _nation & "'")
            Dim dt As DataTable = cDB.query("SELECT  spielerID FROM SPieler ORDER BY SpielerID DESC LIMIT 1")

            If dt.Rows.Count > 0 Then
                _spielerID = dt(0)(0)
            End If
            _neu = False
        Else
            'Update
            cDB.execute("UPDATE Spieler SET ign='" & _ign & "',name='" & _fname & "',vName='" & _vname & "', nation='" & _nation & "' WHERE spielerID=" & SpielerID)
        End If
        ladeSpieler(SpielerID)
    End Sub

#End Region




End Class
