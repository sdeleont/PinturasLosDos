Public Class GenCliente
    Public c As conexion = New conexion()
    Private Sub GenCliente_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        c.llenar_grid("select id_cliente as 'Codigo', nit,Nombre,Apellido,Telefono,correo as 'Direccion',limite_credito as 'Limite Credito' from cliente", DataGridView1)
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        c.llenar_grid("select id_cliente as 'Codigo', nit,Nombre,Apellido,Telefono,correo as 'Direccion',limite_credito as 'Limite Credito' from cliente where nombre like '%" + TextBox1.Text + "%'", DataGridView1)
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If (Asc(e.KeyChar)) = 13 Then
            Dim datos As DataTable = c.buscar("*", "cliente", "id_cliente=" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(0).Value)
            If datos.Rows.Count > 0 Then
                VentanaPrincipal.nit.Text = Convert.ToString(datos.Rows(0).Item("nit"))
                VentanaPrincipal.nombres.Text = Convert.ToString(datos.Rows(0).Item("nombre"))
                VentanaPrincipal.apellidos.Text = Convert.ToString(datos.Rows(0).Item("apellido"))
                VentanaPrincipal.telefono.Text = Convert.ToString(datos.Rows(0).Item("telefono"))
                VentanaPrincipal.correo.Text = Convert.ToString(datos.Rows(0).Item("correo"))
                VentanaPrincipal.limitecredito.Text = Convert.ToString(datos.Rows(0).Item("limite_credito"))
                VentanaPrincipal.id_cliente = Convert.ToString(datos.Rows(0).Item("id_cliente"))
                VentanaPrincipal.cliente_nuevo = False
                Me.Close()
            End If
        End If
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Dim datos As DataTable = c.buscar("*", "cliente", "id_cliente=" & DataGridView1.CurrentRow.Cells(0).Value)
        If datos.Rows.Count > 0 Then
            VentanaPrincipal.nit.Text = Convert.ToString(datos.Rows(0).Item("nit"))
            VentanaPrincipal.nombres.Text = Convert.ToString(datos.Rows(0).Item("nombre"))
            VentanaPrincipal.apellidos.Text = Convert.ToString(datos.Rows(0).Item("apellido"))
            VentanaPrincipal.telefono.Text = Convert.ToString(datos.Rows(0).Item("telefono"))
            VentanaPrincipal.correo.Text = Convert.ToString(datos.Rows(0).Item("correo"))
            VentanaPrincipal.limitecredito.Text = Convert.ToString(datos.Rows(0).Item("limite_credito"))
            VentanaPrincipal.id_cliente = Convert.ToString(datos.Rows(0).Item("id_cliente"))
            VentanaPrincipal.cliente_nuevo = False
            Me.Close()
        End If
    End Sub

    Private Sub DataGridView1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DataGridView1.KeyPress
        If (Asc(e.KeyChar)) = 13 Then
            Dim datos As DataTable = c.buscar("*", "cliente", "id_cliente=" & DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(0).Value)
            If datos.Rows.Count > 0 Then
                VentanaPrincipal.nit.Text = Convert.ToString(datos.Rows(0).Item("nit"))
                VentanaPrincipal.nombres.Text = Convert.ToString(datos.Rows(0).Item("nombre"))
                VentanaPrincipal.apellidos.Text = Convert.ToString(datos.Rows(0).Item("apellido"))
                VentanaPrincipal.telefono.Text = Convert.ToString(datos.Rows(0).Item("telefono"))
                VentanaPrincipal.correo.Text = Convert.ToString(datos.Rows(0).Item("correo"))
                VentanaPrincipal.limitecredito.Text = Convert.ToString(datos.Rows(0).Item("limite_credito"))
                VentanaPrincipal.id_cliente = Convert.ToString(datos.Rows(0).Item("id_cliente"))
                VentanaPrincipal.cliente_nuevo = False
                Me.Close()
            End If
        End If
    End Sub

    Private Sub DataGridView1_KeyDown(sender As Object, e As KeyEventArgs) Handles DataGridView1.KeyDown
        If (e.KeyCode = Keys.Enter) Then
            e.Handled = True
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class