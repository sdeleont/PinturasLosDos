Public Class Form_principal

    Private Sub SplitContainer1_Panel1_Paint(sender As Object, e As PaintEventArgs) Handles SplitContainer1.Panel1.Paint
        'Data Source=SERGIO\SQL2012;Initial Catalog=ferreteria;Integrated Security=True
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        form_productos.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        form_tipo.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        form_gastos.Show()
    End Sub

    Private Sub Form_principal_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim con As conexion = New conexion()

        con.llenar_grid("select p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo',p.existencias as 'Existencias' ,u.nombre as 'Medida'from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad ", DataGridView1)
        DataGridView1.RowHeadersWidth = 20
        DataGridView1.Font = New Font("Arial ", 8, FontStyle.Regular)



    End Sub

    
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        form_inventario.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        form_ventas.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        form_usuarios.Show()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim con As conexion = New conexion()
        con.llenar_grid("select p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo',p.existencias as 'Existencias' ,u.nombre as 'Medida'from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad and p.nombre like '%" & TextBox1.Text & "%'", DataGridView1)
    End Sub


    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        MsgBox(DataGridView1.CurrentRow.Cells(0).Value)
    End Sub

    Private Sub SplitContainer1_Panel2_Paint(sender As Object, e As PaintEventArgs) Handles SplitContainer1.Panel2.Paint

    End Sub
End Class