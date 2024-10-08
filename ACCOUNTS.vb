Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class ACCOUNTS
    Private Sub Accounts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect()
        readdata()
        rowheight()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "JPG Files(*.jpg)|*.jpg"
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            PictureBox1.Image = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Sub rowheight()
        For i = 0 To DataGridView1.Rows.Count - 1
            DataGridView1.Rows(i).Height = 50
        Next
    End Sub

    Sub clear()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
        PictureBox1.Image = Nothing
    End Sub
    Sub readdata()
        DataGridView1.Rows.Clear()
        str = "Select * from Accounts"
        cmd = New SqlClient.SqlCommand(str, sqlconn)
        dr = cmd.ExecuteReader
        While dr.Read
            DataGridView1.Rows.Add(dr("UserID"), dr("UserName"), dr("Password"), dr("Fname"), dr("Mname"), dr("Lname"), dr("UserLevel"), dr("UserImage"), "Edit", "Delete")

        End While
        dr.Close()
        cmd.Dispose()
    End Sub


    Sub savedata()
        Dim ms As New MemoryStream
        PictureBox1.Image.Save(ms, PictureBox1.Image.RawFormat)

        query = "Insert into Accounts (UserID,UserName,Password,Fname,Mname,Lname,
                UserLevel,UserImage,User_stamp,Date_stamp) values (@UserID,@UserName,
                @Password,@Fname,@Mname,@Lname,@UserLevel,@UserImage,@User_stamp,@Date_stamp)"
        cmd = New SqlClient.SqlCommand(query, sqlconn)
        With cmd.Parameters
            .AddWithValue("@UserID", TextBox1.Text)
            .AddWithValue("@UserName", TextBox2.Text)
            .AddWithValue("@Password", TextBox3.Text)
            .AddWithValue("@Fname", TextBox5.Text)
            .AddWithValue("@Mname", TextBox6.Text)
            .AddWithValue("@UserImage", ms.ToArray())
            .AddWithValue("@User_stamp", userlevel)
            .AddWithValue("@Date_stamp", CDate(Date.Now.ToString("MM/dd/yyyy")))

        End With
        cmd.ExecuteNonQuery()
        cmd.Dispose()
    End Sub
    Sub updatedata()                    ' subprocedure for updating data
        Dim ms As New MemoryStream
        PictureBox1.Image.Save(ms, PictureBox1.Image.RawFormat)

        query = "Update Accounts set UserName = @UserName,Password = @Password,Fname = @Fname,Mname = @Mname,
                Lname = @Lname, UserLevel = @UserLevel, UserImage = @UserImage where UserID = @UserID"
        ' update each column (username, password, fname, mname, lanme, userlevel, userimage) by userid
        'not include na yung datestamp saka userstamp iupdate kasi update lang naman to
        cmd = New SqlClient.SqlCommand(query, sqlconn)
        With cmd.Parameters
            .AddWithValue("@UserID", TextBox1.Text)
            .AddWithValue("@UserName", TextBox2.Text)
            .AddWithValue("@Password", TextBox3.Text)
            .AddWithValue("@Fname", TextBox5.Text)
            .AddWithValue("@Mname", TextBox6.Text)
            .AddWithValue("@Lname", TextBox7.Text)
            .AddWithValue("@UserImage", ms.ToArray())
        End With
        cmd.ExecuteReader()
        cmd.Dispose()

    End Sub

    Sub deletedata(ByVal uid As String) '  ito yung temporary variables name ko siya sa uid

        query = "delete from Accounts where UserID = @UserID" 'query based sa ID number na dedelete natin
        cmd = New SqlClient.SqlCommand(query, sqlconn)
        With cmd.Parameters
            .AddWithValue("@UserID", uid.ToString)
        End With
        'close connections
        cmd.ExecuteReader()
        cmd.Dispose()
    End Sub
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If e.ColumnIndex = 8 Then
            Dim i As Integer = DataGridView1.CurrentRow.Index

            TextBox1.Text = DataGridView1.Item(0, i).Value
            TextBox2.Text = DataGridView1.Item(1, i).Value
            TextBox5.Text = DataGridView1.Item(3, i).Value
            TextBox6.Text = DataGridView1.Item(4, i).Value
            TextBox7.Text = DataGridView1.Item(5, i).Value
            viewimage()

        ElseIf e.ColumnIndex = 9 Then
            'lagay tayo ng confrimation ng delete

            Dim confirm = MsgBox("Do you want to delete this record?", MsgBoxStyle.YesNo)
            'confirmation message answerable by yes or no
            If confirm = MsgBoxResult.Yes Then
                'if yes then dito natin lagay yung delete

                Dim i As Integer = DataGridView1.CurrentRow.Index 'kkuha ng current row natin na iddelete
                deletedata(DataGridView1.Item(0, i).Value)            'magkakaroon yan ng error kasi hinahanap niya yung temproray variables na uid
                ' assign natin sa temp variables na first row first column which is si userid
                readdata()
                rowheight()
                'wala ng clear kasi hinde naman tayo gumamit ng textboxes
            End If

        End If
    End Sub

    Sub viewimage()
        Dim img() As Byte                                           'image container as byte
        str = "Select * from User_Accounts where ID = '" & TextBox1.Text & "'" 'query based on UserID
        cmd = New SqlClient.SqlCommand(str, sqlconn)
        dr = cmd.ExecuteReader
        While dr.Read
            img = dr("UserImage")                           'read image as byte
            Dim ms As New MemoryStream(img)                 ' convert image into memory stream
            PictureBox1.Image = Image.FromStream(ms)        ' placing image to picturebox using memory stream
        End While
        dr.Close()
        cmd.Dispose()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim existingID As Boolean = False
        Dim existingname As Boolean = False


        str = "Select * from User_Accounts"
        cmd = New SqlClient.SqlCommand(str, sqlconn)
        dr = cmd.ExecuteReader
        While dr.Read
            If dr("UserID").ToString.Equals(TextBox1.Text) Then
                existingID = True
            ElseIf dr("Fname").ToString.Equals(TextBox5.Text) And dr("Mname").ToString.Equals(TextBox6.Text) And dr("Lname").ToString.Equals(TextBox7.Text) Then
                existingname = True
            End If
        End While
        dr.Close()
        cmd.Dispose()


        If existingID = True Then
            MsgBox("ID already exist", MsgBoxStyle.Critical)
        ElseIf existingname = True Then
            MsgBox("Name already exist", MsgBoxStyle.Critical)
        ElseIf TextBox2.Text = "" Then
            MsgBox("invalid username", MsgBoxStyle.Critical)
        ElseIf TextBox3.Text = "" Then
            MsgBox("invalid password", MsgBoxStyle.Critical)
        ElseIf TextBox4.Text = "" Then
            MsgBox("invalid confirm password", MsgBoxStyle.Critical)
        ElseIf TextBox3.Text <> TextBox4.Text Then
            MsgBox("failed to confirm password", MsgBoxStyle.Critical)
        ElseIf TextBox5.Text = "" Then
            MsgBox("invalid firstname", MsgBoxStyle.Critical)
        ElseIf TextBox6.Text = "" Then
            MsgBox("invalid middlename", MsgBoxStyle.Critical)
        ElseIf TextBox7.Text = "" Then
            MsgBox("invalid lastname", MsgBoxStyle.Critical)
        ElseIf PictureBox1.Image Is Nothing Then
            MsgBox("invalid user image", MsgBoxStyle.Critical)
        Else
            savedata()
            readdata()
            rowheight()
            clear()
        End If
    End Sub
End Class
