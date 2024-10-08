Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.IO

Public Class LOGIN
    Dim attmpts As Integer = 0
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = "" And TextBox2.Text = "" Then
            MsgBox("Invalid Credentials", MsgBoxStyle.Critical)
        ElseIf TextBox2.Text = "" Then
            MsgBox("Invalid password", MsgBoxStyle.Critical)
        ElseIf TextBox1.Text = "" Then
            MsgBox("Invalid username", MsgBoxStyle.Critical)
        Else
            Dim resultat As Integer
            Str = "Select * from Accounts"
            cmd = New SqlClient.SqlCommand(Str, sqlconn)
            dr = cmd.ExecuteReader
            While dr.Read
                If dr("UserName").ToString.Equals(TextBox1.Text) And dr("Password").ToString.Equals(TextBox2.Text) Then
                    resultat = 1
                    fname = dr("Fname") + " " + dr("Lname")
                    userlevel = dr("UserLevel")
                    usrid = dr("UserID")
                End If
            End While
            cmd.Dispose()
            dr.Close()

            If resultat = 1 Then
                MsgBox("Welcome " + fname.ToString + " you are now logged in")
                DASHBOARD.Label1.Text = fname
                DASHBOARD.Label2.Text = userlevel
                viewimage()
                DASHBOARD.Show()
                Me.Hide()
            Else
                MsgBox("Wrong username and password", MsgBoxStyle.Critical)
                TextBox1.Clear()
                TextBox2.Clear()
                attmpts += 1
                If attmpts >= 3 Then
                    MsgBox("Reached the maximum login attempts, system will shutdown", MsgBoxStyle.Critical, "System Message")
                    'Me.Close()
                    Label4.Text = 30
                    Label4.Visible = True
                    Label3.Visible = True
                    Timer1.Start()
                    Me.Enabled = False
                End If
            End If
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            TextBox2.PasswordChar = ""
        Else
            TextBox2.PasswordChar = "*"
        End If
    End Sub

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect()
    End Sub

    Sub Viewimage()
        Dim img() As Byte                                           'image container as byte
        str = "Select * from Accounts where UserID = '" & usrid.ToString & "'" 'query based on UserID
        cmd = New SqlClient.SqlCommand(str, sqlconn)
        dr = cmd.ExecuteReader
        While dr.Read
            img = dr("UserImage")                           'read image as byte
            Dim ms As New MemoryStream(img)                 ' convert image into memory stream
            DASHBOARD.PictureBox1.Image = Image.FromStream(ms)        ' placing image to picturebox using memory stream
            'call natin si dashboard kasi dun nakalagay yung pciture box na lalagyan
        End While
        dr.Close()
        cmd.Dispose()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Label4.Text = 0 Then
            Label3.Visible = False
            Label4.Visible = False
            Me.Enabled = True
            Timer1.Stop()
        Else
            Label4.Text = Val(Label4.Text) - 1
        End If
    End Sub
End Class