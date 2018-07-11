Public Class Clientes
    Public c As conexion = New conexion()
    Public cod As String = "0"

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        c.llenar_grid("select id_cliente as 'Codigo', nit,Nombre,Apellido,Telefono,correo as 'Direccion',limite_credito as 'Limite Credito' from cliente where nombre like '%" + TextBox1.Text + "%' and id_sede=" & VentanaPrincipal.id_sede, DataGridView1)
    End Sub

    Private Sub Clientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        c.llenar_grid("select id_cliente as 'Codigo', nit,Nombre,Apellido,Telefono,correo as 'Direccion',limite_credito as 'Limite Credito' from cliente where id_sede=" & VentanaPrincipal.id_sede, DataGridView1)
    End Sub

    Private Sub DataGridView1_KeyDown(sender As Object, e As KeyEventArgs) Handles DataGridView1.KeyDown
        If (e.KeyCode = Keys.Enter) Then
            e.Handled = True
        End If
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        cod = DataGridView1.CurrentRow.Cells(0).Value
        txnit.Text = DataGridView1.CurrentRow.Cells(1).Value
        txnombre.Text = DataGridView1.CurrentRow.Cells(2).Value
        txapellido.Text = DataGridView1.CurrentRow.Cells(3).Value
        txtelefono.Text = DataGridView1.CurrentRow.Cells(4).Value
        txdireccion.Text = DataGridView1.CurrentRow.Cells(5).Value
        txcredito.Text = DataGridView1.CurrentRow.Cells(6).Value
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If txnombre.Text.Trim() = "" And txapellido.Text.Trim() = "" And txnit.Text.Trim() = "" Then
        Else
            Try
                c.insertar("update cliente set nombre='" & txnombre.Text.Trim & "' , apellido='" & txapellido.Text.Trim & "', nit='" & txnit.Text.Trim & "', telefono='" & txtelefono.Text.Trim & "', correo='" & txdireccion.Text.Trim & "', limite_credito=" & txcredito.Text.Trim & " where id_cliente=" & cod, "", False, 0)
                MessageBox.Show("Modificado")
                cod = "0"
                txnit.Text = ""
                txnombre.Text = ""
                txapellido.Text = ""
                txtelefono.Text = ""
                txdireccion.Text = ""
                txcredito.Text = ""
                c.llenar_grid("select id_cliente as 'Codigo', nit,Nombre,Apellido,Telefono,correo as 'Direccion',limite_credito as 'Limite Credito' from cliente where nombre like '%" + TextBox1.Text + "%' and id_sede=" & VentanaPrincipal.id_sede, DataGridView1)
            Catch ex As Exception
                MessageBox.Show("Seleccione un cliente")
            End Try
        End If
        
        
    End Sub

    Private Sub txtelefono_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtelefono.KeyPress
        If e.KeyChar = "-" Or e.KeyChar = " " Then

        Else
            If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub txcredito_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txcredito.KeyPress
        If e.KeyChar = "." Then

        Else
            If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
                e.Handled = True
            End If
        End If
    End Sub
End Class