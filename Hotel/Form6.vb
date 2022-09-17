Imports System.IO
Public Class Form6
    Dim aksi As String
    Sub aksesteks(ByVal akses As Boolean)
        txt_username.Enabled = akses
        txt_password.Enabled = akses
        txt_password2.Enabled = akses
        txt_nama.Enabled = akses
        txt_email.Enabled = akses
        dtp_birth.Enabled = akses
        cb_job.Enabled = akses
        txt_alamat.Enabled = akses
    End Sub
    Sub clearData()
        txt_id.Text = ""
        txt_username.Text = ""
        txt_password.Text = ""
        txt_password2.Text = ""
        txt_nama.Text = ""
        txt_email.Text = ""
        cb_job.Text = ""
        txt_id_job.Text = ""
        txt_alamat.Text = ""
        PictureBox1.Image = Nothing
    End Sub
    Sub buttonAwal()
        btn_insert.Enabled = True
        btn_update.Enabled = True
        btn_delete.Enabled = True
        btn_browse.Enabled = False
        btn_save.Enabled = False
        btn_cancel.Enabled = False
    End Sub
    Sub tampilData()
        lv_employee.Items.Clear()
        konek_db()
        Dim tampil As New SqlClient.SqlCommand("SELECT * FROM v_employee", koneksi)
        Dim reader As SqlClient.SqlDataReader
        reader = tampil.ExecuteReader
        Dim i As Integer = 0
        Do While reader.Read
            lv_employee.Items.Add(reader!ID)
            lv_employee.Items(i).SubItems.Add(reader!Username)
            lv_employee.Items(i).SubItems.Add(reader!Name)
            lv_employee.Items(i).SubItems.Add(reader!Email)
            lv_employee.Items(i).SubItems.Add(reader!Address)
            lv_employee.Items(i).SubItems.Add(reader!DateofBirth)
            lv_employee.Items(i).SubItems.Add(reader!job_name)

            i = i + 1
        Loop
    End Sub
    Sub tampilJob()
        konek_db()
        Dim tampil As New SqlClient.SqlCommand("SELECT * FROM Job", koneksi)
        Dim reader As SqlClient.SqlDataReader
        reader = tampil.ExecuteReader

        Do While reader.Read
            cb_job.Items.Add(reader("Name"))
        Loop
    End Sub
    Private Sub btn_browse_Click(sender As Object, e As EventArgs) Handles btn_browse.Click

        OpenFileDialog1.Filter = "images|*.jpg;*.jpeg;*.png;*.gif;*.bmp"

        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            PictureBox1.Image = Image.FromFile(OpenFileDialog1.FileName)
        End If

    End Sub

    Private Sub btn_insert_Click(sender As Object, e As EventArgs) Handles btn_insert.Click
        aksi = "I"
        btn_insert.Enabled = False
        btn_update.Enabled = False
        btn_delete.Enabled = False
        btn_browse.Enabled = True
        btn_save.Enabled = True
        btn_cancel.Enabled = True
        aksesteks(True)
    End Sub

    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click
        aksi = "U"
        btn_insert.Enabled = False
        btn_update.Enabled = False
        btn_delete.Enabled = False
        btn_browse.Enabled = True
        btn_save.Enabled = True
        btn_cancel.Enabled = True
        aksesteks(True)
    End Sub

    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        aksesteks(False)
        tampilData()
        tampilJob()
    End Sub

    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click
        If txt_username.Text <> "" And txt_password.Text <> "" And txt_nama.Text <> "" And txt_email.Text <> "" And dtp_birth.Text <> "" And cb_job.Text <> "" And txt_alamat.Text <> "" Then
            If aksi = "I" Then
                Dim ms As New MemoryStream
                PictureBox1.Image.Save(ms, PictureBox1.Image.RawFormat)
                konek_db()
                Dim insert As New SqlClient.SqlCommand("INSERT INTO Employee VALUES('" & txt_username.Text & "','" & txt_password.Text & "','" & txt_nama.Text & "','" & txt_email.Text & "','" & txt_alamat.Text & "','" & dtp_birth.Text & "','" & txt_id_job.Text & "',@image)", koneksi)
                insert.Parameters.Add("@image", SqlDbType.Image).Value = ms.ToArray()

                Try
                    insert.ExecuteNonQuery()
                    MsgBox("Data berhasil dimasukkan!", vbInformation)
                Catch ex As Exception
                    MsgBox("Data gagal dimasukkan karena  " + ex.Message)
                End Try
            End If

            If aksi = "U" Then
                Dim ms As New MemoryStream
                PictureBox1.Image.Save(ms, PictureBox1.Image.RawFormat)
                konek_db()
                Dim update As New SqlClient.SqlCommand("UPDATE Employee set Username = '" & txt_username.Text & "',Password = '" & txt_password.Text & "',Name = '" & txt_nama.Text & "',Email = '" & txt_email.Text & "',Address = '" & txt_alamat.Text & "',DateofBirth = '" & dtp_birth.Text & "',JobID = '" & txt_id_job.Text & "',Photo = @image WHERE ID = '" & txt_id.Text & "'", koneksi)

                update.Parameters.Add("@image", SqlDbType.Image).Value = ms.ToArray()

                Try
                    update.ExecuteNonQuery()
                    MsgBox("Data berhasil diupdate")
                Catch ex As Exception
                    MsgBox("Data gagal diupdate karena  " + ex.Message)
                End Try
            End If
        Else
            MsgBox("Data harus diisi semua!", vbCritical)
        End If
        tampilData()
        buttonAwal()
        aksesteks(False)
        clearData()
    End Sub

    Private Sub lv_employee_MouseClick(sender As Object, e As MouseEventArgs) Handles lv_employee.MouseClick
        konek_db()
        Dim tampil As New SqlClient.SqlCommand("SELECT * FROM Employee WHERE ID = '" & lv_employee.SelectedItems(0).SubItems(0).Text & "'", koneksi)
        Dim reader As SqlClient.SqlDataReader
        reader = tampil.ExecuteReader

        Do While reader.Read
            txt_id.Text = reader!ID
            txt_username.Text = reader!Username
            txt_password.Text = reader!Password
            txt_password2.Text = reader!Password
            txt_nama.Text = reader!Name
            txt_email.Text = reader!Email
            txt_alamat.Text = reader!Address
            dtp_birth.Text = reader!DateofBirth
            cb_job.Text = reader!JobID
            Dim img() As Byte
            img = reader!Photo
            Dim ms As New MemoryStream(img)
            PictureBox1.Image = Image.FromStream(ms)
        Loop
    End Sub

    Private Sub cb_job_TextChanged(sender As Object, e As EventArgs) Handles cb_job.TextChanged
        konek_db()
        Dim tampil As New SqlClient.SqlCommand("SELECT * FROM Job WHERE Name = '" & cb_job.Text & "'", koneksi)
        Dim reader As SqlClient.SqlDataReader
        reader = tampil.ExecuteReader

        If cb_job.Text = "" Then
            txt_id_job.Text = ""
        End If

        Do While reader.Read
            If reader.RecordsAffected <> 0 Then
                txt_id_job.Text = reader!ID
            End If
        Loop
    End Sub

    Private Sub btn_cancel_Click(sender As Object, e As EventArgs) Handles btn_cancel.Click
        clearData()
        aksesteks(False)
        tampilData()
        buttonAwal()
    End Sub

    Private Sub btn_delete_Click(sender As Object, e As EventArgs) Handles btn_delete.Click
        Dim message As String
        message = MsgBox("Apakah anda yakin ingin menghapus data ini?", MsgBoxStyle.YesNo)
        If MsgBoxResult.Yes Then
            konek_db()
            Dim delete As New SqlClient.SqlCommand("DELETE FROM Employee WHERE ID = '" & txt_id.Text & "'", koneksi)

            Try
                delete.ExecuteNonQuery()
                MsgBox("Data berhasil dihapus!", vbInformation)
            Catch ex As Exception
                MsgBox("Data gagal dihapus karena  " + ex.Message)
            End Try
        End If
        aksesteks(False)
        buttonAwal()
        clearData()
        tampilData()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub txt_username_TextChanged(sender As Object, e As EventArgs) Handles txt_username.TextChanged

    End Sub

    Private Sub txt_id_TextChanged(sender As Object, e As EventArgs) Handles txt_id.TextChanged

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub cb_job_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_job.SelectedIndexChanged

    End Sub

    Private Sub txt_id_job_TextChanged(sender As Object, e As EventArgs) Handles txt_id_job.TextChanged

    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click

    End Sub
End Class