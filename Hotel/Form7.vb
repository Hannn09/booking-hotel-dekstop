Public Class Form7
    Dim aksi As String
    Sub clearData()
        txt_nama.Text = ""
        txt_price.Text = ""
        txt_fee.Text = ""
    End Sub
    Sub buttonAwal()
        btn_insert.Enabled = True
        btn_update.Enabled = True
        btn_delete.Enabled = True
        btn_save.Enabled = False
        btn_cancel.Enabled = False
    End Sub
    Sub aksesteks(ByVal akses As Boolean)
        txt_nama.Enabled = akses
        txt_price.Enabled = akses
        txt_fee.Enabled = akses
    End Sub
    Sub tampilData()
        lv_items.Items.Clear()
        konek_db()
        Dim tampil As New SqlClient.SqlCommand("SELECT * FROM Item", koneksi)
        Dim reader As SqlClient.SqlDataReader
        reader = tampil.ExecuteReader
        Dim i As Integer = 0

        Do While reader.Read
            lv_items.Items.Add(reader!ID)
            lv_items.Items(i).SubItems.Add(reader!Name)
            lv_items.Items(i).SubItems.Add(reader!RequestPrice)
            lv_items.Items(i).SubItems.Add(reader!CompensationFee)

            i = i + 1
        Loop
    End Sub
    Private Sub lv_items_MouseClick(sender As Object, e As MouseEventArgs) Handles lv_items.MouseClick
        txt_id.Text = lv_items.SelectedItems(0).SubItems(0).Text
        txt_nama.Text = lv_items.SelectedItems(0).SubItems(1).Text
        txt_price.Text = lv_items.SelectedItems(0).SubItems(2).Text
        txt_fee.Text = lv_items.SelectedItems(0).SubItems(3).Text
    End Sub

    Private Sub Form7_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

    Private Sub btn_cancel_Click(sender As Object, e As EventArgs) Handles btn_cancel.Click
        buttonAwal()
        aksesteks(False)
        clearData()
    End Sub

    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click
        If txt_nama.Text <> "" And txt_price.Text <> "" And txt_fee.Text <> "" Then
            If aksi = "I" Then
                konek_db()
                Dim insert As New SqlClient.SqlCommand("INSERT INTO Item VALUES('" & txt_nama.Text & "','" & txt_price.Text & "','" & txt_fee.Text & "')", koneksi)

                Try
                    insert.ExecuteNonQuery()
                    MsgBox("Data berhasil dimasukkan!", vbInformation)
                Catch ex As Exception
                    MsgBox("Data gagal dimasukkan karena  " + ex.Message)
                End Try
            End If
            If aksi = "U" Then
                konek_db()
                Dim update As New SqlClient.SqlCommand("UPDATE Item set Name = '" & txt_nama.Text & "',RequestPrice = '" & txt_price.Text & "',CompensationFee = '" & txt_fee.Text & "' WHERE ID = '" & txt_id.Text & "'", koneksi)

                Try
                    update.ExecuteNonQuery()
                    MsgBox("Data berhasil diupdate!", vbInformation)
                Catch ex As Exception
                    MsgBox("Data gagal diupdate karena  " + ex.Message)
                End Try
            End If
        Else
            MsgBox("Data harus diisi semua!", vbCritical)
        End If
        aksesteks(False)
        buttonAwal()
        clearData()
        tampilData()
    End Sub

    Private Sub btn_delete_Click(sender As Object, e As EventArgs) Handles btn_delete.Click
        Dim message As String
        message = MsgBox("Apakah anda yakin ingin menghapus data ini?", MsgBoxStyle.YesNo)
        If MsgBoxResult.Yes Then
            konek_db()
            Dim delete As New SqlClient.SqlCommand("DELETE FROM Item WHERE ID = '" & txt_id.Text & "'", koneksi)

            Try
                delete.ExecuteNonQuery()
                MsgBox("Data berhasil dihapus!", vbInformation)
            Catch ex As Exception
                MsgBox("Data gagal dihapus karena  " + ex.Message)
            End Try
        End If
        clearData()
        aksesteks(False)
        tampilData()
        buttonAwal()
    End Sub
End Class