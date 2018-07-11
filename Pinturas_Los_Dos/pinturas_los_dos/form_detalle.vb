Public Class form_detalle
    Public codigo As String = ""
    Private Sub form_detalle_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        buscar()
        DataGridView1.Select()
    End Sub
    Public Sub buscar()
        Dim c As conexion = New conexion()
        Dim consulta As String = ""
        If VentanaPrincipal.administrador Then
            consulta = "select d.id_presentacion as 'Codigo P.', pr.nombre as 'Nombre' , pr.marca as 'Marca' , pr.detalle as 'Detalle' ,m.descripcion as 'Medida (G/otro)' ,p.precio_compra as 'Precio de Compra', p.precio_venta as 'Precio de Venta' "
        Else
            consulta = "select d.id_presentacion as 'Codigo P.', pr.nombre as 'Nombre' , pr.marca as 'Marca' , pr.detalle as 'Detalle' ,m.descripcion as 'Medida (G/otro)', p.precio_venta as 'Precio de Venta' "
        End If

        consulta = consulta & "from detalle_venta d, Presentacion p, Producto pr, Medida m "
        consulta = consulta & "where d.id_presentacion =p.id_presentacion and p.id_producto =pr.id_producto and p.id_medida =m.id_medida and d.id_venta =" & codigo
        c.llenar_grid(consulta, DataGridView1)

    End Sub

    Private Sub DataGridView1_KeyDown(sender As Object, e As KeyEventArgs) Handles DataGridView1.KeyDown
        If (e.KeyCode = Keys.Enter) Then
            e.Handled = True
        End If
    End Sub

    

    Private Sub DataGridView1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DataGridView1.KeyPress
        If (Asc(e.KeyChar)) = 27 Then
            form_ventas.Enabled = True
            Me.Close()
        End If
    End Sub
End Class