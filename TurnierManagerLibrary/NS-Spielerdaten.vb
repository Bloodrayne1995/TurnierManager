Namespace Spielerdaten

    Public Class PlayerData

#Region "Internal"

        Private _ign As String = ""
        Private _fname As String = ""
        Private _vname As String = ""
        Private _nation As String = ""
        Private _spielerID As Integer = -1
        Private _neu As Boolean = False

        Private _verb As Datenbank.DatabaseInterface

#End Region

#Region "Properties"

        Public Property IngameName As String
            Get
                Return _ign
            End Get
            Set(value As String)
                _ign = value
            End Set
        End Property

        Public Property FamilienName As String
            Get
                Return _fname
            End Get
            Set(value As String)
                _fname = value
            End Set
        End Property

        Public Property Vorname As String
            Get
                Return _vname
            End Get
            Set(value As String)
                _vname = value
            End Set
        End Property

        Public Property Nation As String
            Get
                Return _nation
            End Get
            Set(value As String)
                _nation = value
            End Set
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
        Public Sub New(db As Datenbank.DatabaseInterface, id As Integer)
            Me.New(db)
            _neu = False
            ladeSpieler(id)
        End Sub

        ''' <summary>
        ''' Erstellt ein neuer Spieler
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(db As Datenbank.DatabaseInterface)
            _neu = True
            _verb = db
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

            Dim dt As DataTable = _verb.query("SELECT ign,nation,name,vName FROM Spieler WHERE spielerID=" & SpielerID)

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
          
            If _neu Then
                'Insert
                _verb.execute("INSERT INTO Spieler (ign,name,vName,nation) VALUES('" & _ign & "','" & _fname & "','" & _vname & "','" & _nation & "'")
                Dim dt As DataTable = _verb.query("SELECT  spielerID FROM Spieler ORDER BY SpielerID DESC LIMIT 1")

                If dt.Rows.Count > 0 Then
                    _spielerID = dt(0)(0)
                End If
                _neu = False
            Else
                'Update
                _verb.execute("UPDATE Spieler SET ign='" & _ign & "',name='" & _fname & "',vName='" & _vname & "', nation='" & _nation & "' WHERE spielerID=" & SpielerID)
            End If
            ladeSpieler(SpielerID)
        End Sub


        Public Overrides Function ToString() As String
            Return "ID=" & _spielerID & "|IGN=" & _ign & "|Name=" & _fname & "|Vorname=" & _vname & "|Nation=" & _nation
        End Function

#End Region

    End Class


    Public Class PlayerDBSchnittstelle

#Region "Internal"

        Private _db As Datenbank.DatabaseInterface

#End Region

#Region "Konstruktoren"

        Public Sub New(ByRef db As Datenbank.DatabaseInterface)
            _db = db
        End Sub

#End Region


#Region "Filtern"

        Public Function getAllPlayers() As List(Of PlayerData)
            Dim dt As DataTable = _db.query("SELECT spielerID FROM spieler")
            Dim ls As New List(Of PlayerData)

            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim c As New PlayerData(_db, Integer.Parse(dr(0)))
                    ls.Add(c)
                Next
            End If
            Return ls
        End Function


        Public Function getPlayerByIGNameLike(f As String) As List(Of PlayerData)
            Dim dt As DataTable = _db.query("SELECT spielerID FROM spieler WHERE ign Like '" & f & "%'")
            Dim ls As New List(Of PlayerData)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim c As New PlayerData(_db, Integer.Parse(dr(0)))
                    ls.Add(c)
                Next
            End If
            Return ls
        End Function

        Public Function getPlayerByNameLike(f As String) As List(Of PlayerData)
            Dim dt As DataTable = _db.query("SELECT spielerID FROM spieler WHERE `name` Like '" & f & "%'")
            Dim ls As New List(Of PlayerData)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim c As New PlayerData(_db, Integer.Parse(dr(0)))
                    ls.Add(c)
                Next
            End If
            Return ls
        End Function

        Public Function getPlayerByVorNameLike(f As String) As List(Of PlayerData)
            Dim dt As DataTable = _db.query("SELECT spielerID FROM spieler WHERE vName Like '" & f & "%'")
            Dim ls As New List(Of PlayerData)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim c As New PlayerData(_db, Integer.Parse(dr(0)))
                    ls.Add(c)
                Next
            End If
            Return ls
        End Function

        Public Function getPlayerByNationLike(f As String) As List(Of PlayerData)
            Dim dt As DataTable = _db.query("SELECT spielerID FROM spieler WHERE nation Like '" & f & "%'")
            Dim ls As New List(Of PlayerData)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim c As New PlayerData(_db, Integer.Parse(dr(0)))
                    ls.Add(c)
                Next
            End If
            Return ls
        End Function

        Public Function getPlayerByID(f As Integer) As List(Of PlayerData)
            Dim dt As DataTable = _db.query("SELECT spielerID FROM spieler WHERE spielerID=" & f)
            Dim ls As New List(Of PlayerData)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim c As New PlayerData(_db, Integer.Parse(dr(0)))
                    ls.Add(c)
                Next
            End If
            Return ls
        End Function



#End Region

    End Class



End Namespace