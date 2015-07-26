Imports TurnierManagerLibrary.ChampionDaten
Imports TurnierManagerLibrary.Datenbank
Imports TurnierManagerLibrary.RollenDaten
Imports TurnierManagerLibrary.TeamDaten
Imports TurnierManagerLibrary.Spielerdaten

Public Class Form1
    Dim db As New MySQLDB()

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim erg As Boolean = db.openDB(My.Settings.dbHostOrFile, My.Settings.dbUser, My.Settings.dbPass, My.Settings.dbName)
        If erg = True Then

            Dim t As New TeamDBSchnittstelle(db)


            ListBox1.Items.Clear()
            Dim ls As List(Of Team) = t.getAllTeams
            For Each x As Team In ls
                ListBox1.Items.Add(x)
            Next

        End If



    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ListBox1.SelectedIndex >= 0 Then
            Dim t As Team = DirectCast(ListBox1.Items(ListBox1.SelectedIndex), Team)

            ListBox2.Items.Clear()
            For Each x As TeamSpielerRolle In t.getAlleSpieler
                ListBox2.Items.Add(x)
            Next
        End If
    End Sub
End Class
