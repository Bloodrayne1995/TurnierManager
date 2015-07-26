Imports System.Data.SQLite

Namespace Databases

    Public Class SQLLiteDB


        Private connection As New Data.SQLite.SQLiteConnection()

        ''' <summary>
        ''' Öffnet eine neue Datenbank-Verbindung zur SQLLite Datenbank her
        ''' </summary>
        ''' <param name="conn"></param>
        ''' <remarks></remarks>
        Public Sub open(conn As String)
            connection.ConnectionString = conn
            Try
                connection.Open()
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function query(sql As String) As DataTable
            If Not connection.State = ConnectionState.Open Then
                Throw New Exception("Keine DB-Verbindung geöffnet")
            Else
                Dim cmd As SQLite.SQLiteCommand = connection.CreateCommand
                cmd.CommandText = sql

                Dim r As SQLite.SQLiteDataReader = cmd.ExecuteReader()
                Dim dt As New DataTable
                dt.Load(r)
                Return dt
            End If
        End Function

        Public Function execute(sql As String) As Integer
            If Not connection.State = ConnectionState.Open Then
                Throw New Exception("Keine DB-Verbindung geöffnet")
            Else
                Dim cmd As SQLite.SQLiteCommand = connection.CreateCommand
                cmd.CommandText = sql

                Return cmd.ExecuteNonQuery()
            End If
        End Function

    End Class


End Namespace

