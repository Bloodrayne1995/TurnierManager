Namespace TeamDaten

    Public Class Team

#Region "Internal"

        Private _db As Datenbank.DatabaseInterface
        Private _neu As Boolean = False

        Private _teamID As Integer = 0
        Private _name As String = ""
        Private _symbol() As Byte = Nothing
        Private _kurzName As String = ""
        'TODO: Region implementieren
        Private _regionID As Integer = 0
        Private _spieler As New List(Of TeamSpielerRolle)

#End Region

#Region "Properties"

        Public ReadOnly Property TeamID As Integer
            Get
                Return _teamID
            End Get
        End Property

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
            End Set
        End Property

        Public Property Symbol As Byte()
            Get
                Return _symbol
            End Get
            Set(value As Byte())
                _symbol = value
            End Set
        End Property


        Public Property KurzName As String
            Get
                Return _kurzName
            End Get
            Set(value As String)
                _kurzName = value
            End Set
        End Property


        Public ReadOnly Property Spieler(rolle As Integer) As Spielerdaten.PlayerData
            Get
                For Each x As TeamSpielerRolle In _spieler
                    If x.RolleID = rolle Then
                        Return x.getSpieler
                    End If
                Next
                Return Nothing
            End Get
        End Property

#End Region

#Region "Kontruktoren"

        Public Sub New(ByRef db As Datenbank.DatabaseInterface)
            _db = db
            _neu = True
        End Sub

        Public Sub New(ByRef db As Datenbank.DatabaseInterface, id As Integer)
            Me.New(db)
            _neu = False
            ladeTeam(id)
        End Sub

#End Region

#Region "Methoden"

        Public Sub ladeTeam(id As Integer)
            Dim dt As DataTable = _db.query("SELECT * FROM team WHERE teamID=" & id)
            _teamID = id
            If dt.Rows.Count > 0 Then
                _name = dt.Rows(0)(1)
                _symbol = dt.Rows(0)(2)
                _kurzName = dt.Rows(0)(3)
                _regionID = dt.Rows(0)(4)
            End If

            dt = _db.query("SELECT teamSpielerID FROM teamspieler WHERE teamID=" & id)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim ts As New TeamSpielerRolle(_db, Integer.Parse(dr(0)))
                    _spieler.Add(ts)
                Next
            End If

        End Sub

        Public Sub speichern()
            If _neu Then
                Dim sql As String = "INSERT INTO team (`name`,kName,regionID) VALUES ("
                sql &= "'" & _name & "',"
                sql &= "'" & _kurzName & "',"
                sql &= "" & _regionID & ")"
                _db.execute(sql)

                Dim dt As DataTable = _db.query("SELECT teamID FROM team WHERE `name`='" & _name & "' AND kName='" & _kurzName & "' AND regionID=" & _regionID)
                If dt.Rows.Count > 0 Then
                    _teamID = Integer.Parse(dt(0)(0))
                End If

                sql = "UPDATE team SET symbol=@s WHERE teamID=" & _teamID
                _db.executeWithBlob(sql, "@s", Symbol)
            Else
                Dim sql As String = "UPDATE team SET `name`='" & _name & "' , kName='" & _kurzName & "', regionID=" & _regionID & ",symbol=@bild WHERE teamID=" & _teamID
                _db.executeWithBlob(sql, "@bild", _symbol)

                For Each x As TeamSpielerRolle In _spieler
                    x.speichere()
                Next
            End If
            ladeTeam(_teamID)
        End Sub

        Public Overrides Function ToString() As String
            Return "ID=" & _teamID & "|Name=" & _name
        End Function

#End Region

#Region "Spielerverwaltung"

        Public Sub addSpieler(spielerID As Integer, rolleID As Integer)
            Dim x As New TeamSpielerRolle(_db)
            x.RolleID = rolleID
            x.TeamID = _teamID
            x.SpielerID = spielerID
            _spieler.Add(x)
        End Sub

        Public Sub addSpieler(x As TeamSpielerRolle)
            _spieler.Add(x)
        End Sub

        Public Sub removeSpieler(spielerID As Integer)
            For i As Integer = 0 To _spieler.Count - 1
                If (_spieler(i).SpielerID = spielerID) Then
                    _spieler.RemoveAt(i)
                End If
            Next
        End Sub


        Public Function getAlleSpieler() As List(Of TeamSpielerRolle)
            Return _spieler
        End Function

#End Region

    End Class

    Public Class TeamSpielerRolle

#Region "Internal"

        Private _teamSpielerRolleID As Integer = 0
        Private _teamID As Integer = 0
        Private _spielerID As Integer = 0
        Private _rolleID As Integer

        Private _db As Datenbank.DatabaseInterface
        Private _neu As Boolean = False

#End Region

#Region "Properties"

        Public ReadOnly Property TeamSpielerRolleID As Integer
            Get
                Return _teamSpielerRolleID
            End Get
        End Property

        Public Property TeamID As Integer
            Get
                Return _teamID
            End Get
            Set(value As Integer)
                _teamID = value
            End Set
        End Property

        Public Property SpielerID As Integer
            Get
                Return _spielerID
            End Get
            Set(value As Integer)
                _spielerID = value
            End Set
        End Property

        Public Property RolleID As Integer
            Get
                Return _rolleID
            End Get
            Set(value As Integer)
                _rolleID = value
            End Set
        End Property

#End Region

#Region "Konstruktoren"

        Public Sub New(ByRef db As Datenbank.DatabaseInterface)
            _db = db
            _neu = True
        End Sub

        Public Sub New(ByRef db As Datenbank.DatabaseInterface, id As Integer)
            Me.New(db)
            lade(id)
        End Sub

#End Region

#Region "Methoden"

        Public Sub lade(id As Integer)
            Dim dt As DataTable = _db.query("SELECT teamID,spielerID,rolleID FROM teamspieler WHERE teamSpielerID=" & id)
            _teamSpielerRolleID = id
            If dt.Rows.Count > 0 Then
                _teamID = Integer.Parse(dt(0)(0))
                _spielerID = Integer.Parse(dt(0)(1))
                _rolleID = Integer.Parse(dt(0)(2))
            End If
        End Sub

        Public Sub speichere()
            Dim sql As String = ""
            If _neu Then
                sql = "INSERT INTO teamspieler (teamID,spielerID,rolleID) VALUES (" & _teamID & "," & _spielerID & "," & _rolleID & ")"

                _db.execute(sql)

                Dim dt As DataTable = _db.query("SELECT teamSpielerID FROM teamspieler WHERE teamID=" & _teamID & " AND spielerID=" & _spielerID & " AND rolleID=" & _rolleID)
                If dt.Rows.Count > 0 Then
                    _teamSpielerRolleID = Integer.Parse(dt(0)(0))
                End If
            Else
                sql = "UPDATE teamspieler SET teamID=" & _teamID & ", spielerID=" & _spielerID & ", rolleID=" & _rolleID & " WHERE teamSpielerID=" & _teamSpielerRolleID
                _db.execute(sql)
            End If
        End Sub

        Public Overrides Function ToString() As String
            Return "Rolle=" & getRolle.Bezeichnung & " | Spieler=" & getSpieler().Vorname & " '" & getSpieler.IngameName & "' " & getSpieler().FamilienName
        End Function

#End Region

#Region "Vereinfachter Zugriff"

        Public Function getSpieler() As Spielerdaten.PlayerData
            Dim p As New Spielerdaten.PlayerData(_db, _spielerID)
            Return p
        End Function

        Public Function getRolle() As RollenDaten.Rolle
            Dim r As New RollenDaten.Rolle(_db, _rolleID)
            Return r
        End Function

#End Region

    End Class

    Public Class TeamDBSchnittstelle

#Region "Internal"

        Private _db As Datenbank.DatabaseInterface

#End Region

#Region "Konstruktoren"

        Public Sub New(ByRef db As Datenbank.DatabaseInterface)
            _db = db
        End Sub

#End Region

#Region "Filtern"

        Public Function getAllTeams() As List(Of Team)
            Dim dt As DataTable = _db.query("SELECT teamID FROM team")
            Dim ls As New List(Of Team)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim t As Integer = Integer.Parse(dr(0))
                    Dim x As New Team(_db, t)
                    ls.Add(x)
                Next
            End If
            Return ls
        End Function


        Public Function getTeamsByNameLike(s As String) As List(Of Team)
            Dim dt As DataTable = _db.query("SELECT teamID FROM team WHERE `name` Like '" & s & "%'")
            Dim ls As New List(Of Team)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim t As Integer = Integer.Parse(dr(0))
                    Dim x As New Team(_db, t)
                    ls.Add(x)
                Next
            End If
            Return ls
        End Function

        Public Function getTeamsByKurzNameLike(s As String) As List(Of Team)
            Dim dt As DataTable = _db.query("SELECT teamID FROM team WHERE `kName` Like '" & s & "%'")
            Dim ls As New List(Of Team)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim t As Integer = Integer.Parse(dr(0))
                    Dim x As New Team(_db, t)
                    ls.Add(x)
                Next
            End If
            Return ls
        End Function

        Public Function getTeamsByRegion(s As Integer) As List(Of Team)
            Dim dt As DataTable = _db.query("SELECT teamID FROM team WHERE `regionID`=" & s & "")
            Dim ls As New List(Of Team)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim t As Integer = Integer.Parse(dr(0))
                    Dim x As New Team(_db, t)
                    ls.Add(x)
                Next
            End If
            Return ls
        End Function

        Public Function getTeamsByID(s As Integer) As List(Of Team)
            Dim dt As DataTable = _db.query("SELECT teamID FROM team WHERE `teamID`=" & s & "")
            Dim ls As New List(Of Team)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim t As Integer = Integer.Parse(dr(0))
                    Dim x As New Team(_db, t)
                    ls.Add(x)
                Next
            End If
            Return ls
        End Function

#End Region


    End Class


End Namespace