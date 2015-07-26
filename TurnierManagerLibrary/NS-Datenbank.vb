
Namespace Datenbank

    Public MustInherit Class DatabaseInterface

        Protected _connection As System.Data.Common.DbConnection

        Public MustOverride Function openDB(host As String, user As String, pw As String, dbName As String) As Boolean

        Public Function query(sql As String) As DataTable
            Dim dt As New DataTable
            Try
                If _connection.State = ConnectionState.Open Then
                    Dim cmd As Data.Common.DbCommand = _connection.CreateCommand
                    cmd.CommandText = sql
                    dt.Load(cmd.ExecuteReader)
                End If

            Catch ex As Exception
                Throw ex
            End Try

            Return dt
        End Function

        Public Function execute(sql As String) As Integer
            Dim erg As Integer = -1

            Try
                If _connection.State = ConnectionState.Open Then
                    Dim cmd As Data.Common.DbCommand = _connection.CreateCommand
                    cmd.CommandText = sql
                    erg = cmd.ExecuteNonQuery
                End If
                Return erg
            Catch ex As Exception
                Throw ex
                Return erg
            End Try
        End Function

        Public MustOverride Function executeWithBlob(sql As String, blobPara As String, blobData As Byte()) As Integer

    End Class

    Public Class MySQLDB
        Inherits DatabaseInterface


        Public Overrides Function openDB(host As String, user As String, pw As String, dbName As String) As Boolean
            _connection = New MySql.Data.MySqlClient.MySqlConnection("Server=" & host & ";Database=" & dbName & ";Uid=" & user & ";Pwd=" & pw & ";")
            Try
                _connection.Open()
                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Overrides Function executeWithBlob(sql As String, blobPara As String, blobData() As Byte) As Integer
            Dim erg As Integer = -1
            Dim db As MySql.Data.MySqlClient.MySqlConnection = _connection
            Try
                If _connection.State = ConnectionState.Open Then
                    Dim cmd As MySql.Data.MySqlClient.MySqlCommand = db.CreateCommand
                    cmd.CommandText = sql
                    cmd.Parameters.Add(blobPara, MySql.Data.MySqlClient.MySqlDbType.Binary).Value = blobData
                    erg = cmd.ExecuteNonQuery
                End If
                Return erg
            Catch ex As Exception
                Throw ex
                Return erg
            End Try
        End Function
    End Class

    Public Class SqliteDB
        Inherits DatabaseInterface


        ''' <summary>
        ''' Öffnet eine Verdibung zur Datenbank
        ''' </summary>
        ''' <param name="host">Datei-Name</param>
        ''' <param name="user">Nicht verwendet</param>
        ''' <param name="pw">Passwort zur Datenbank</param>
        ''' <param name="dbName">Nicht verwendet</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function openDB(host As String, user As String, pw As String, dbName As String) As Boolean
            _connection = New SQLite.SQLiteConnection()

            Dim str As String = "Data Source=" & host & ";Version=3;"

            If Not pw = "" Then
                str &= "Password=" & pw & ";"
            End If

            _connection.ConnectionString = str

            Try
                _connection.Open()
                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Overrides Function executeWithBlob(sql As String, blobPara As String, blobData() As Byte) As Integer
            Return 0
        End Function
    End Class


End Namespace
