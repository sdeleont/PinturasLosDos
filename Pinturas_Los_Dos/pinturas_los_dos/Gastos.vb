Public Class Gastos

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim c As conexion = New conexion()
        c.llenar_grid("select g.id_gasto as 'Numero', u.nombre as 'Usuario', g.descripcion as 'Descripcion', g.monto as 'Monto (Q.)', g.fecha as 'Fecha' from gasto g, usuario u where g.id_usuario=u.id_usuario and g.fecha='" & calendario.Value.Date & "'", DataGridView1)
        Try
            Label3.Text = "Q. " & c.buscar("sum(g.monto) as 'Suma'", "gasto g, usuario u", "g.id_usuario=u.id_usuario and g.fecha='" & calendario.Value.Date & "'").Rows(0).Item("Suma")
        Catch ex As Exception
        End Try

    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Dim c As conexion = New conexion()
        Try
            Label5.Text = c.buscar("g.descripcion as 'Descripcion'", "gasto g, usuario u", "g.id_usuario=u.id_usuario and g.id_gasto=" & DataGridView1.CurrentRow.Cells(0).Value).Rows(0).Item("Descripcion")
        Catch ex As Exception

        End Try

    End Sub

    
    
    
    Private Sub Gastos_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class