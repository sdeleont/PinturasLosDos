Public Class Galon_disponible

    Private Sub Galon_disponible_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim c As conexion = New conexion()
        c.llenar_grid("select id_tipo as 'Codigo', nombre as 'Tipo de Producto', existencias as 'Total (Gal./Unidades)' from Tipo ", DataGridView1)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class