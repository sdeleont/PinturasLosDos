Public Class FormCaja

    Private Sub FormCaja_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim dia As DateTime = DateTime.Today
        lblfecha.Text = dia.ToString("d")
        TextBox1.Focus()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim c As conexion = New conexion()
        Dim td As DataTable = c.buscar("*", "caja", "fecha= '" + lblfecha.Text + "' and id_sede=" & VentanaPrincipal.id_sede)
        If td.Rows.Count = 0 Then
            c.insertar("insert into caja values ('" + lblfecha.Text.Trim() + "'," + TextBox1.Text.Trim() + ",0,''," & VentanaPrincipal.id_sede & ");", "Caja Iniciada", True, 1)
            Me.Close()
        Else
            MessageBox.Show("Error, no se ha realizado el cierre de esta fecha o ya existe registro de esta fecha")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()

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