Imports System.IO
Public Class frmRoom
    Dim aksi As String
    Sub buttonAwal()
        btn_insert.Enabled = True
        btn_update.Enabled = True
        btn_delete.Enabled = True
        btn_browse.Enabled = True
        btn_save.Enabled = False
        btn_cancel.Enabled = False
    End Sub
    Sub clearData()
        txt_id.Text = ""
        txt_nama.Text = ""
        nmric_capacity.Text = ""
        txt_price.Text = ""
        PictureBox1.Image = Nothing
    End Sub
    Sub aksesteks(ByVal akses As Boolean)
        txt_nama.Enabled = akses
        nmric_capacity.Enabled = akses
        txt_price.Enabled = akses
        btn_browse.Enabled = akses
    End Sub
    Sub tampilData()
        lv_room.Items.Clear()
        konek_db()
        Dim tampil As New SqlClient.SqlCommand("SELECT * FROM RoomType", koneksi)
        Dim reader As SqlClient.SqlDataReader
        reader = tampil.ExecuteReader
        Dim i As Integer = 0

        Do While reader.Read
            lv_room.Items.Add(reader!ID)
            lv_room.Items(i).SubItems.Add(reader!Name)
            lv_room.Items(i).SubItems.Add(reader!Capacity)
            lv_room.Items(i).SubItems.Add(reader!RoomPrice)

            i = i + 1
        Loop
    End Sub
    Private Sub btn_browse_Click(sender As Object, e As EventArgs) Handles btn_browse.Click
        Dim OpenFileDialog1 As New OpenFileDialog
        OpenFileDialog1.Filter = "images|*.jpg;*.png;*.gif;*.bmp"

        If OpenFileDialog1.ShowDialog = DialogResult.OK Then

            PictureBox1.Image = Image.FromFile(OpenFileDialog1.FileName)

        End If
    End Sub
    Private Sub frmRoom_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tampilData()
        aksesteks(False)
    End Sub

    Private Sub btn_insert_Click(sender As Object, e As EventArgs) Handles btn_insert.Click
        aksi = "I"
        btn_insert.Enabled = False
        btn_update.Enabled = False
        btn_delete.Enabled = False
        btn_save.Enabled = True
        btn_cancel.Enabled = True
        aksesteks(True)
    End Sub

    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click
        aksi = "U"
        btn_insert.Enabled = False
        btn_update.Enabled = False
        btn_delete.Enabled = False
        btn_save.Enabled = True
        btn_cancel.Enabled = True
        aksesteks(True)
    End Sub

    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click
        If txt_nama.Text <> "" And nmric_capacity.Text <> "" And txt_price.Text <> "" Then
            If aksi = "I" Then
                Dim ms As New MemoryStream
                PictureBox1.Image.Save(ms, PictureBox1.Image.RawFormat)
                konek_db()
                Dim insert As New SqlClient.SqlCommand("INSERT INTO RoomType VALUES('" & txt_nama.Text & "','" & nmric_capacity.Text & "','" & txt_price.Text & "',@image)", koneksi)

                insert.Parameters.Add("@image", SqlDbType.Image).Value = ms.ToArray()


                Try
                    insert.ExecuteNonQuery()
                    MsgBox("Data berhasil disimpan!", vbInformation)
                Catch ex As Exception
                    MsgBox("Data gagal disimpan karena  " + ex.Message, vbCritical)
                End Try
            End If
            If aksi = "U" Then
                Dim ms As New MemoryStream
                PictureBox1.Image.Save(ms, PictureBox1.Image.RawFormat)
                konek_db()
                Dim update As New SqlClient.SqlCommand("UPDATE RoomType set Name = '" & txt_nama.Text & "',Capacity = '" & nmric_capacity.Text & "',RoomPrice = '" & txt_price.Text & "',Photo = @image WHERE ID = '" & txt_id.Text & "'", koneksi)

                update.Parameters.Add("@image", SqlDbType.Image).Value = ms.ToArray()

                Try
                    update.ExecuteNonQuery()
                    MsgBox("Data berhasil diupdate!", vbInformation)
                Catch ex As Exception
                    MsgBox("Data gagal diupdate karena  " + ex.Message, vbCritical)
                End Try
            End If
            clearData()
            tampilData()
            aksesteks(False)
            buttonAwal()
        Else
            MsgBox("Data tidak lengkap!", vbCritical)
        End If

    End Sub

    Private Sub btn_cancel_Click(sender As Object, e As EventArgs) Handles btn_cancel.Click
        buttonAwal()
        clearData()
        aksesteks(False)
    End Sub

    Private Sub lv_room_MouseClick(sender As Object, e As MouseEventArgs) Handles lv_room.MouseClick
        konek_db()
        Dim tampil As New SqlClient.SqlCommand("SELECT * FROM RoomType WHERE ID = '" & lv_room.SelectedItems(0).SubItems(0).Text & "'", koneksi)
        Dim reader As SqlClient.SqlDataReader
        reader = tampil.ExecuteReader
        Do While reader.Read
            txt_id.Text = reader!ID
            txt_nama.Text = reader!Name
            nmric_capacity.Text = reader!Capacity
            txt_price.Text = reader!RoomPrice
            Dim img() As Byte
            img = reader!Photo
            Dim ms As New MemoryStream(img)
            PictureBox1.Image = Image.FromStream(ms)
        Loop
    End Sub


    Private Sub btn_delete_Click(sender As Object, e As EventArgs) Handles btn_delete.Click
        konek_db()
        Dim delete As New SqlClient.SqlCommand("DELETE FROM RoomType WHERE ID = '" & txt_id.Text & "'", koneksi)

        Try
            delete.ExecuteNonQuery()
            MsgBox("Data berhasil dihapus", vbInformation)
        Catch ex As Exception
            MsgBox("Data gagal dihapus karena " + ex.Message)
        End Try

        buttonAwal()
        tampilData()
        aksesteks(False)
        clearData()
    End Sub
End Class