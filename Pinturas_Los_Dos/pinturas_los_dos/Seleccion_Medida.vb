Public Class Seleccion_Medida
    Public cod_producto As String

    Private Sub Seleccion_Medida_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim con As conexion = New conexion()
        con.llenar_grid("select m.descripcion as 'Medida (Galon/Otros)', p.precio_venta as 'Precio (Q.)'  from Presentacion p, Medida m where p.id_medida =m.id_medida and  p.id_producto =" & VentanaPrincipal.codigo_producto, DataGridView1)
        Dim datos As DataTable = con.buscar("p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo',p.existencias as 'Existencias' ,u.nombre as 'Medida'", "producto p ,tipo t, unidad u", "p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad and p.id_producto=" & VentanaPrincipal.codigo_producto)
        codigo.Text = datos.Rows(0).Item("Codigo")
        cod_producto = datos.Rows(0).Item("Codigo")
        nombre.Text = datos.Rows(0).Item("Nombre")
        marca.Text = datos.Rows(0).Item("Marca")
        detalle.Text = datos.Rows(0).Item("Detalle")
        tipo.Text = datos.Rows(0).Item("Tipo")
        existencia.Text = datos.Rows(0).Item("Existencias")
        VentanaPrincipal.Enabled = False
        If Convert.ToDouble(existencia.Text) < 10 Then
            existencia.ForeColor = Color.Red
        Else
            existencia.ForeColor = Color.Green
        End If
        existencia.Visible = False
        Label6.Visible = False

    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        VentanaPrincipal.ListBox1.Items.Add("Q." & DataGridView1.CurrentRow.Cells(1).Value & "          " & DataGridView1.CurrentRow.Cells(0).Value & " " & nombre.Text & " Marca " & marca.Text)
        VentanaPrincipal.total = VentanaPrincipal.total + Convert.ToDouble(DataGridView1.CurrentRow.Cells(1).Value)
        VentanaPrincipal.lbltotal.Text = "Q.  " & VentanaPrincipal.total
        VentanaPrincipal.valtotal()
        Dim c As conexion = New conexion()
        Dim codigo_medida As String = c.buscar("id_medida", "Medida", "descripcion='" & DataGridView1.CurrentRow.Cells(0).Value & "'").Rows(0).Item("id_medida")
        VentanaPrincipal.lista_codigos_presentacion.Add(c.buscar("id_presentacion", "presentacion", "id_producto=" & cod_producto & " and id_medida=" & codigo_medida).Rows(0).Item("id_presentacion"))
        VentanaPrincipal.Enabled = True
        Me.Close()
    End Sub

    Private Sub DataGridView1_KeyDown(sender As Object, e As KeyEventArgs) Handles DataGridView1.KeyDown
        If (e.KeyCode = Keys.Enter) Then
            e.Handled = True
        End If
    End Sub

    Private Sub DataGridView1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DataGridView1.KeyPress
        Dim v1 As String = ""
        Dim v0 As String = ""


        v1 = DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(1).Value
        v0 = DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(0).Value
        
        If (Asc(e.KeyChar)) = 13 Then
            VentanaPrincipal.ListBox1.Items.Add("Q." & v1 & "          " & v0 & " " & nombre.Text & " Marca " & marca.Text)
            VentanaPrincipal.total = VentanaPrincipal.total + Convert.ToDouble(v1)
            VentanaPrincipal.lbltotal.Text = "Q.  " & VentanaPrincipal.total
            VentanaPrincipal.valtotal()
            Dim c As conexion = New conexion()
            ''estaba DataGridView1.CurrentRow.Cells(0).Value
            Dim codigo_medida As String = c.buscar("id_medida", "Medida", "descripcion='" & v0 & "'").Rows(0).Item("id_medida")
            VentanaPrincipal.lista_codigos_presentacion.Add(c.buscar("id_presentacion", "presentacion", "id_producto=" & cod_producto & " and id_medida=" & codigo_medida).Rows(0).Item("id_presentacion"))
            VentanaPrincipal.Enabled = True
            Me.Close()
        End If
        If (Asc(e.KeyChar)) = 27 Then
            VentanaPrincipal.Enabled = True
            Me.Close()
        End If

    End Sub

    
    
End Class