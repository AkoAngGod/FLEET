Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.IO

Module GLOBALIZATION
    Public sqlconn As New SqlConnection
    Public cmd As New SqlCommand
    Public dr As SqlDataReader
    Public da As New SqlDataAdapter
    Public query As String
    Public str As String
    Public fname, userlevel, usrid As String

    Sub Connect()
        Try
            If sqlconn.State = ConnectionState.Open Then sqlconn.Close()
            sqlconn.ConnectionString = "Server=304-PC12\SQLEXPRESS;Database=BSIT;Trusted_Connection=True; MultipleActiveResultsets=True;"
            sqlconn.Open()
        Catch ex As Exception
            MsgBox("Error in connection please contact administrator")
        End Try
    End Sub
End Module
