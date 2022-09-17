Imports System.Data.SqlClient
Module Module1
    Public koneksi As SqlConnection = Nothing
    Public Sub konek_db()
        Dim server As String
        server = "server = Hannn\SQLEXPRESS;database = Hotel;Integrated Security = true"
        koneksi = New SqlConnection(server)
        koneksi.Open()
    End Sub
End Module
