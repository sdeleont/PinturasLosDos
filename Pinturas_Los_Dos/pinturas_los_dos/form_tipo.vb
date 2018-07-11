Imports System.Data.SqlClient
Public Class form_tipo
    Public id As String = ""
    Private Sub form_tipo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim con As conexion = New conexion()
        con.llenar_grid("select id_tipo as 'Codigo', nombre as 'Nombre',cantidadavisar as 'Cantidad Aviso'  from tipo where id_sede=" & VentanaPrincipal.id_sede, DataGridView1)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Not nombre_tipo.Text.Trim() = "" Then
            Dim con As conexion = New conexion()
            con.insertar("insert into Tipo values ('" & nombre_tipo.Text & "',0,0," & VentanaPrincipal.id_sede & ")", "Categoria registrada", True, 0)
            con.llenar_grid("select id_tipo as 'Codigo', nombre as 'Nombre',cantidadavisar as 'Cantidad Aviso'  from tipo where id_sede=" & VentanaPrincipal.id_sede, DataGridView1)
            nombre_tipo.Text = ""
            id = ""
        Else
            MessageBox.Show("No se ha especificado una categoria")
        End If
        
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Dim con As conexion = New conexion()
        Dim dato As DataTable = con.buscar("nombre, cantidadavisar", "tipo", "id_tipo=" & DataGridView1.CurrentRow.Cells(0).Value)
        TextBox1.Text = dato.Rows(0).Item("cantidadavisar")
        Label3.Text = dato.Rows(0).Item("nombre")
        id = DataGridView1.CurrentRow.Cells(0).Value
    End Sub

    Private Sub DataGridView1_DoubleClick(sender As Object, e As EventArgs) Handles DataGridView1.DoubleClick

    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Dim con As conexion = New conexion()
        Dim dato As DataTable = con.buscar("nombre, cantidadavisar", "tipo", "id_tipo=" & DataGridView1.CurrentRow.Cells(0).Value)
        TextBox1.Text = dato.Rows(0).Item("cantidadavisar")
        Label3.Text = dato.Rows(0).Item("nombre")
        id = DataGridView1.CurrentRow.Cells(0).Value
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Not Label3.Text = "-" Then
            Dim con As conexion = New conexion()
            con.insertar("update tipo set cantidadavisar=" & TextBox1.Text & " where id_tipo=" & id, "Cantidad Modificada", True, 1)
            TextBox1.Text = "0"
            Label3.Text = "-"
            id = ""
            con.llenar_grid("select id_tipo as 'Codigo', nombre as 'Nombre',cantidadavisar as 'Cantidad Aviso'  from tipo where id_sede=" & VentanaPrincipal.id_sede, DataGridView1)
        Else
            MessageBox.Show("Seleccione alguna categoria")
        End If
        
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = "." Then

        Else
            If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
                e.Handled = True
            End If
        End If
    End Sub
End Class
