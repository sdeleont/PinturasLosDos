Public Class form_gastos

    Private Sub form_medidas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'FerreteriaDataSet3.Medida' table. You can move, or remove it, as needed.


    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim q As Double = Convert.ToDouble(TextBox2.Text)
            If Not TextBox1.Text.Trim = "" And Not TextBox2.Text.Trim = "" And VentanaPrincipal.CajaIniciada() = True Then
                Dim c As conexion = New conexion()
                c.insertar("insert into gasto values ('" & TextBox1.Text.Trim & "'," & TextBox2.Text & ",'" & Date.Today & "'," & VentanaPrincipal.codigo_usuario_activo & "," & VentanaPrincipal.id_sede & ")", "Gasto registrado", True, 0)
                TextBox1.Text = ""
                TextBox2.Text = ""
                TextBox1.Select()
            End If
        Catch ex As Exception
            MsgBox("Gasto no valido")
        End Try
        
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = "." Then

        Else
            If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub
End Class