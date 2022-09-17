Public Class Form5
    Dim aksi As String
    Sub clearData()
        txt_number.Text = ""
        txt_id.Text = ""
        cb_room.Text = ""
        txt_id_room.Text = ""
        txt_floor.Text = ""
        txt_des.Text = ""
    End Sub
    Sub buttonAwal()
        btn_insert.Enabled = True
        btn_update.Enabled = True
        btn_delete.Enabled = True
        btn_save.Enabled = False
        btn_cancel.Enabled = False
    End Sub
    Sub aksesteks(ByVal akses As Boolean)
        txt_number.Enabled = akses
        cb_room.Enabled = akses
        txt_floor.Enabled = akses
        txt_des.Enabled = akses
    End Sub
    Sub tampilData()
        lv_room.Items.Clear()
        konek_db()
        Dim tampil As New SqlClient.SqlCommand("SELECT * FROM v_room", koneksi)
        Dim reader As SqlClient.SqlDataReader
        reader = tampil.ExecuteReader
        Dim i As Integer = 0

        Do While reader.Read
            lv_room.Items.Add(reader!ID)
            lv_room.Items(i).SubItems.Add(reader!room_name)
            lv_room.Items(i).SubItems.Add(reader!RoomNumber)
            lv_room.Items(i).SubItems.Add(reader!RoomFloor)
            lv_room.Items(i).SubItems.Add(reader!Description)
            i = i + 1
        Loop
    End Sub
    Sub tampilRoom()
        konek_db()
        Dim tampil As New SqlClient.SqlCommand("SELECT * FROM RoomType", koneksi)
        Dim reader As SqlClient.SqlDataReader
        reader = tampil.ExecuteReader

        Do While reader.Read
            cb_room.Items.Add(reader("Name"))
        Loop
    End Sub
    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tampilRoom()
        tampilData()
        aksesteks(False)
    End Sub

    Private Sub cb_room_TextChanged(sender As Object, e As EventArgs) Handles cb_room.TextChanged
        konek_db()
        Dim tampil As New SqlClient.SqlCommand("SELECT * FROM RoomType WHERE Name = '" & cb_room.Text & "'", koneksi)
        Dim reader As SqlClient.SqlDataReader
        reader = tampil.ExecuteReader

        If cb_room.Text = "" Then
            txt_id_room.Text = ""
        End If

        Do While reader.Read
            If reader.RecordsAffected <> 0 Then
                txt_id_room.Text = reader!ID
            End If
        Loop
    End Sub

    Private Sub lv_room_MouseClick(sender As Object, e As MouseEventArgs) Handles lv_room.MouseClick
        txt_id.Text = lv_room.SelectedItems(0).SubItems(0).Text
        cb_room.Text = lv_room.SelectedItems(0).SubItems(1).Text
        txt_number.Text = lv_room.SelectedItems(0).SubItems(2).Text
        txt_floor.Text = lv_room.SelectedItems(0).SubItems(3).Text
        txt_des.Text = lv_room.SelectedItems(0).SubItems(4).Text
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
        If txt_number.Text <> "" And cb_room.Text <> "" And txt_floor.Text <> "" And txt_des.Text <> "" Then
            If aksi = "I" Then
                konek_db()
                Dim insert As New SqlClient.SqlCommand("INSERT INTO Room VALUES('" & txt_id_room.Text & "','" & txt_number.Text & "','" & txt_floor.Text & "','" & txt_des.Text & "')", koneksi)

                Try
                    insert.ExecuteNonQuery()
                    MsgBox("Data berhasil disimpan!", vbInformation)
                Catch ex As Exception
                    MsgBox("Data gagal disimpan karena " + ex.Message)
                End Try
            End If
            If aksi = "U" Then
                konek_db()
                Dim update As New SqlClient.SqlCommand("UPDATE Room set RoomTypeID = '" & txt_id_room.Text & "',RoomNumber = '" & txt_number.Text & "',RoomFloor = '" & txt_floor.Text & "',Description = '" & txt_des.Text & "' WHERE ID = '" & txt_id.Text & "'", koneksi)

                Try
                    update.ExecuteNonQuery()
                    MsgBox("Data berhasil diupdate!", vbInformation)
                Catch ex As Exception
                    MsgBox("Data gagal diupdate karena  " + ex.Message)
                End Try
            End If
            buttonAwal()
            clearData()
            tampilData()
            aksesteks(False)
        Else
            MsgBox("Data harus diisi semua!", vbCritical)
        End If
    End Sub

    Private Sub btn_cancel_Click(sender As Object, e As EventArgs) Handles btn_cancel.Click
        clearData()
        tampilData()
        aksesteks(False)
        buttonAwal()
    End Sub

    Private Sub btn_delete_Click(sender As Object, e As EventArgs) Handles btn_delete.Click
        Dim message As String
        message = MsgBox("Apakah anda yakin ingin mengahpus ini?", MsgBoxStyle.YesNo)
        konek_db()
        If MsgBoxResult.Yes Then
            Dim delete As New SqlClient.SqlCommand("DELETE FROM Room WHERE ID = '" & txt_id.Text & "'", koneksi)

            Try
                delete.ExecuteNonQuery()
                MsgBox("Data gagal dihapus!", vbInformation)
            Catch ex As Exception
                MsgBox("Data gagal dihapus karena  " + ex.Message)
            End Try
        End If
        clearData()
        buttonAwal()
        aksesteks(False)
        tampilData()
    End Sub

    Private Sub cb_room_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_room.SelectedIndexChanged

    End Sub
End Class