Imports System.IO
Public Class Form8
    Dim aksi As String
    Sub clearData()
        txt_name.Text = ""
        cb_type.Text = ""
        txt_id.Text = ""
        txt_id_type.Text = ""
        txt_price.Text = ""
        PictureBox1.Image = Nothing
    End Sub
    Sub buttonAwal()
        btn_insert.Enabled = True
        btn_update.Enabled = True
        btn_delete.Enabled = True
        btn_save.Enabled = False
        btn_cancel.Enabled = False
        btn_browse.Enabled = False
    End Sub
    Sub aksesteks(ByVal akses As Boolean)
        txt_name.Enabled = akses
        cb_type.Enabled = akses
        txt_price.Enabled = akses
    End Sub
    Sub tampilData()
        lv_food.Items.Clear()
        konek_db()
        Dim tampil As New SqlClient.SqlCommand("SELECT * FROM FoodsAndDrinks", koneksi)
        Dim reader As SqlClient.SqlDataReader
        reader = tampil.ExecuteReader
        Dim i As Integer = 0

        Do While reader.Read
            lv_food.Items.Add(reader!ID)
            lv_food.Items(i).SubItems.Add(reader!Name)
            lv_food.Items(i).SubItems.Add(reader!Type)
            lv_food.Items(i).SubItems.Add(reader!Price)

            i = i + 1
        Loop
    End Sub
    Private Sub btn_browse_Click(sender As Object, e As EventArgs) Handles btn_browse.Click
        open_photo.Filter = "images|*.jpg;*.png;*.jpeg;*.gmf;*.gif"

        If open_photo.ShowDialog = DialogResult.OK Then
            PictureBox1.Image = Image.FromFile(open_photo.FileName)
        End If
    End Sub

    Private Sub Form8_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tampilData()
        aksesteks(False)
        cb_type.Items.Clear()
        cb_type.Items.Add("Food")
        cb_type.Items.Add("Drink")
    End Sub

    Private Sub btn_insert_Click(sender As Object, e As EventArgs) Handles btn_insert.Click
        aksi = "I"
        btn_insert.Enabled = False
        btn_update.Enabled = False
        btn_delete.Enabled = False
        btn_save.Enabled = True
        btn_cancel.Enabled = True
        btn_browse.Enabled = True
        aksesteks(True)
    End Sub

    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click
        aksi = "U"
        btn_insert.Enabled = False
        btn_update.Enabled = False
        btn_delete.Enabled = False
        btn_save.Enabled = True
        btn_cancel.Enabled = True
        btn_browse.Enabled = True
        aksesteks(True)
    End Sub

    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click
        If txt_name.Text <> "" And cb_type.Text <> "" And txt_price.Text <> "" Then

            If aksi = "I" Then
                Dim ms As New MemoryStream
                PictureBox1.Image.Save(ms, PictureBox1.Image.RawFormat)
                konek_db()
                Dim insert As New SqlClient.SqlCommand("INSERT INTO FoodsAndDrinks VALUES('" & txt_name.Text & "','" & cb_type.Text & "','" & txt_price.Text & "',@image)", koneksi)
                insert.Parameters.Add("@image", SqlDbType.Image).Value = ms.ToArray()

                Try
                    insert.ExecuteNonQuery()
                    MsgBox("Data berhasil disimpan!", vbInformation)
                Catch ex As Exception
                    MsgBox("Data gagal disimpan karena  " + ex.Message)
                End Try
            End If

            If aksi = "U" Then
                Dim ms As New MemoryStream
                PictureBox1.Image.Save(ms, PictureBox1.Image.RawFormat)
                konek_db()
                Dim update As New SqlClient.SqlCommand("UPDATE FoodsAndDrinks set Name = '" & txt_name.Text & "',Type = '" & cb_type.Text & "',Price = '" & txt_price.Text & "',Photo = @image WHERE ID = '" & txt_id.Text & "'", koneksi)
                update.Parameters.Add("@image", SqlDbType.Image).Value = ms.ToArray()

                Try
                    update.ExecuteNonQuery()
                    MsgBox("Data berhasil diupdate", vbInformation)
                Catch ex As Exception
                    MsgBox("Data gagal diupdate karena  " + ex.Message)
                End Try
            End If
        Else
            MsgBox("Data harus diisi semua!", vbCritical)
        End If
        clearData()
        tampilData()
        aksesteks(False)
        buttonAwal()
    End Sub

    Private Sub lv_food_MouseClick(sender As Object, e As MouseEventArgs) Handles lv_food.MouseClick
        konek_db()
        Dim tampil As New SqlClient.SqlCommand("SELECT * FROM FoodsAndDrinks WHERE ID = '" & lv_food.SelectedItems(0).SubItems(0).Text & "'", koneksi)
        Dim reader As SqlClient.SqlDataReader
        reader = tampil.ExecuteReader

        Do While reader.Read
            txt_id.Text = reader!ID
            txt_name.Text = reader!Name
            cb_type.Text = reader!Type
            txt_price.Text = reader!Price
            Dim img() As Byte
            img = reader!Photo
            Dim ms As New MemoryStream(img)
            PictureBox1.Image = Image.FromStream(ms)
        Loop
    End Sub

    Private Sub btn_delete_Click(sender As Object, e As EventArgs) Handles btn_delete.Click
        konek_db()
        Dim message As String
        message = MsgBox("Apakah anda yakin ingin menghapus data ini?", MsgBoxStyle.YesNo)
        If DialogResult.Yes Then
            Dim delete As New SqlClient.SqlCommand("DELETE FROM FoodsAndDrinks WHERE ID = '" & txt_id.Text & "'", koneksi)

            Try
                delete.ExecuteNonQuery()
                MsgBox("Data berhasil dihapus", vbInformation)
            Catch ex As Exception
                MsgBox("Data gagal dihapus karena  " + ex.Message)
            End Try
        End If
        clearData()
        tampilData()
        aksesteks(False)
        buttonAwal()
    End Sub

    Private Sub cb_type_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_type.SelectedIndexChanged

    End Sub
End Class