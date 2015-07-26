Imports RiotSharp
Imports RiotSharp.StaticDataEndpoint

Namespace ChampionDaten

    Public Class ChampionData

#Region "Internal"

        Private _db As Datenbank.DatabaseInterface
        Private _championID As Integer
        Private _name As String = ""
        Private _titel As String = ""
        Private _primary As String = ""
        Private _secondary As String = ""
        Private _key As String = ""

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

      

#End Region

#Region "Konstruktoren"

        Public Sub New(db As Datenbank.DatabaseInterface)
            _db = db
        End Sub

        Public Sub New(db As Datenbank.DatabaseInterface, id As Integer)
            Me.New(db)
            ladeChampion(id)
        End Sub

#End Region

#Region "Methoden"

        Public Sub ladeChampion(id As Integer)
            Dim dt As DataTable = _db.query("SELECT `name`,`titel`,`primary`,`secondary`,`key` FROM champion WHERE championID=" & id)

            _championID = id
            If dt.Rows.Count > 0 Then
                _name = dt.Rows(0)(0)
                _titel = dt.Rows(0)(1)
                _primary = dt.Rows(0)(2)
                _secondary = dt.Rows(0)(3)
                _key = dt.Rows(0)(4)
            End If

        End Sub


        Public Overrides Function ToString() As String
            Return "ID=" & _championID & "|Name=" & _name & "|Titel=" & _titel & "|Primary=" & _primary & "|Secodary=" & _secondary & "|Key=" & _key
        End Function
#End Region

    End Class


    ''' <summary>
    ''' Scnittsteller zur DB und RiotApi
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ChampionDBSchnittstelle

        Private _db As Datenbank.DatabaseInterface = Nothing
        Private _apiKey As String = ""

        Public Sub New(verb As Datenbank.DatabaseInterface, api As String)
            _db = verb
            _apiKey = api
        End Sub

        Public Sub ladeDB(Optional clearDB As Boolean = False)
            Console.WriteLine("Daten aus RIOTAPI")
            Dim client As StaticRiotApi = StaticRiotApi.GetInstance(_apiKey)
            Dim liste As ChampionListStatic = client.GetChampions(region:=Region.euw, language:=Language.de_DE, championData:=1)

            If clearDB Then
                Dim sqlD As String = "TRUNCATE TABLE champion"
                _db.execute(sqlD)
            End If

            For i As Integer = 0 To liste.Champions.Keys.Count - 1
                Dim k As String = liste.Champions.Keys(i)
                Dim x As ChampionStatic = liste.Champions(k)

                Dim t1 As String = ""
                Dim t2 As String = ""

                Try
                    t1 = x.Tags(0).ToString
                    t2 = x.Tags(1).ToString
                Catch ex As Exception

                End Try

                Dim sql As String = "INSERT INTO champion (`championID`,`name`,`titel`,`primary`,`secondary`,`key`) VALUES ("
                sql &= "" & x.Id & ","
                sql &= "" & Chr(34) & x.Name & Chr(34) & ","
                sql &= "'" & x.Title & "',"
                sql &= "'" & t1 & "',"
                sql &= "'" & t2 & "',"
                sql &= "'" & x.Key & "'"

                sql &= ")"

                'Ausführen
                _db.execute(sql)
            Next

        End Sub


        Public Function getDifferencesBetweenRiotAndDB() As Integer
            Dim r As Integer = getAnzahlChampionByRiot()
            Dim d As Integer = getAnzahlChampionsInDatabase()
            Return r - d
        End Function

        Public Function areChampionsInDB() As Boolean
            Return getAnzahlChampionsInDatabase() > 0
        End Function


    

        Public Function getAnzahlChampionsInDatabase() As Integer
            Dim dt As DataTable = _db.query("SELECT COUNT(*) FROM champion")
            Dim dbCount As Integer = 0
            If dt.Rows.Count > 0 Then
                dbCount = dt.Rows(0)(0)
            End If
            Return dbCount
        End Function

        Public Function getAnzahlChampionByRiot() As Integer
            Dim client As StaticRiotApi = StaticRiotApi.GetInstance(_apiKey)
            Dim liste As ChampionListStatic = client.GetChampions(region:=Region.euw, language:=Language.de_DE, championData:=1)
            Dim riotCount As Integer = liste.Keys.Count

            Return riotCount
        End Function


        Public Function getAllChampions() As List(Of ChampionData)
            Dim dt As DataTable = _db.query("SELECT championID FROM champion")
            Dim ls As New List(Of ChampionData)

            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim id As Integer = Integer.Parse(dr(0))
                    Dim c As New ChampionData(_db, id)
                    ls.Add(c)
                Next
            End If
            Return ls
        End Function

    End Class

End Namespace
