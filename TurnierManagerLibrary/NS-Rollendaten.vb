Namespace RollenDaten

    ''' <summary>
    ''' Eine Rolle im Team
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Rolle

#Region "Internal"

        Private _rolleID As Integer = 0
        Private _bezeichnung As String = ""
        Private _position As Integer = 0
        Private _db As Datenbank.DatabaseInterface

#End Region

#Region "Properties"

        ''' <summary>
        ''' Gibt die interne RollenID zurück
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RolleID As Integer
            Get
                Return _rolleID
            End Get
        End Property

        ''' <summary>
        ''' Gibt die Ordnungs-Position zurück
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Position As Integer
            Get
                Return _position
            End Get
            Set(value As Integer)
                _position = value
            End Set
        End Property

        ''' <summary>
        ''' Gibt die Bezeichnung der Rolle zurück
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Bezeichnung As String
            Get
                Return _bezeichnung
            End Get
            Set(value As String)
                _bezeichnung = value
            End Set
        End Property

#End Region

#Region "Konstruktoren"

        Public Sub New(ByRef db As Datenbank.DatabaseInterface)
            _db = db
        End Sub


        Public Sub New(ByRef db As Datenbank.DatabaseInterface, id As Integer)
            Me.New(db)
            ladeRolle(id)
        End Sub

#End Region

#Region "Methoden"

        Public Sub ladeRolle(id As Integer)
            Dim dt As DataTable = _db.query("SELECT `position`,`bezeichnung` FROM rolle WHERE rolleID=" & id)
            _rolleID = id
            If dt.Rows.Count > 0 Then
                _bezeichnung = dt(0)(1)
                _position = dt(0)(0)
            End If
        End Sub


        Public Sub speichereRolle()
            Dim sql As String = "UPDATE rolle SET `position`=" & _position & ",`bezeichnung`='" & Bezeichnung & "' WHERE rolleID=" & RolleID
        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Verbindungs-Klasse zur Rollen-Datenbank
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RolleDBSchnittstelle

#Region "Internal"

        Private _db As Datenbank.DatabaseInterface

#End Region

#Region "Konstruktoren"

        Public Sub New(ByRef db As Datenbank.DatabaseInterface)
            _db = db
        End Sub

#End Region

#Region "Filtern"

        ''' <summary>
        ''' Liefert alle verfügbaren Rollen
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllRoles() As List(Of Rolle)
            Dim dt As DataTable = _db.query("SELECT rolleID FROM rolle")
            Dim ls As New List(Of Rolle)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim r As New Rolle(_db, Integer.Parse(dr(0)))
                    ls.Add(r)
                Next
            End If
            Return ls
        End Function

        ''' <summary>
        ''' Liefert eine Rolle anhand ihrer ID zurück
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getRoleByID(id As Integer) As Rolle
            Dim dt As DataTable = _db.query("SELECT rolleID FROM rolle WHERE rolleID=" & id)
            Dim r As Rolle = New Rolle(_db)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    r = New Rolle(_db, Integer.Parse(dr(0)))
                Next
            End If
            Return r
        End Function

        ''' <summary>
        ''' Liefert alle Rollen auf die die Bezeichnung zutrifft
        ''' </summary>
        ''' <param name="str"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getRolesByDescLike(str As String) As List(Of Rolle)
            Dim dt As DataTable = _db.query("SELECT rolleID FROM rolle WHERE bezeichnung Like '" & str & "%'")
            Dim ls As New List(Of Rolle)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim r As New Rolle(_db, Integer.Parse(dr(0)))
                    ls.Add(r)
                Next
            End If
            Return ls
        End Function

        ''' <summary>
        ''' Liefert alle Rollen, die auf einer bestimmte Ordnungs-Position zutreffen
        ''' </summary>
        ''' <param name="str"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getRolesByPosition(str As Integer) As List(Of Rolle)
            Dim dt As DataTable = _db.query("SELECT rolleID FROM rolle WHERE position=" & str)
            Dim ls As New List(Of Rolle)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim r As New Rolle(_db, Integer.Parse(dr(0)))
                    ls.Add(r)
                Next
            End If
            Return ls
        End Function

#End Region


    End Class

End Namespace
