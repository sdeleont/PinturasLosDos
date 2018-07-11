Public Class Buscar_codigo

    Private Sub Buscar_codigo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim con As conexion = New conexion()
        con.llenar_grid("select p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle' from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad ", DataGridView1)
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim con As conexion = New conexion()
        con.llenar_grid("select p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle' from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad and p.nombre like '%" & TextBox1.Text & "%'", DataGridView1)

    End Sub
End Class