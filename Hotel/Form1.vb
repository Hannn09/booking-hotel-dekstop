Public Class frmLogin
    Sub clearData()
        txt_username.Text = ""
        txt_pw.Text = ""
    End Sub

    Private Sub btn_login_Click(sender As Object, e As EventArgs) Handles btn_login.Click
        konek_db()
        Dim tampil As New SqlClient.SqlCommand("SELECT * FROM Employee WHERE Username = '" & txt_username.Text & "' AND Password = '" & txt_pw.Text & "'", koneksi)
        Dim reader As SqlClient.SqlDataReader
        reader = tampil.ExecuteReader()
        reader.Read()

        Try
            Dim role As String
            role = reader!JobID

            If role = "1" Then
                MsgBox("Login sukses !", MsgBoxStyle.Information)
                frmMain.Show()
                Me.Hide()
                clearData()
            End If

            If role = "2" Then
                MsgBox("Login sukses !", MsgBoxStyle.Information)
                frmAdmin.Show()
                Me.Hide()
                clearData()
            End If
        Catch ex As Exception
            MsgBox("Username atau Password salah !", MsgBoxStyle.Critical)
        End Try
        clearData()
    End Sub
End Class
