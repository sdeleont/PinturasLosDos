Public Class form_existencias
    Public codigo_producto As String

    Private Sub form_existencias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'FerreteriaDataSet4.unidad' table. You can move, or remove it, as needed.
        Dim c As conexion = New conexion()
        Dim datos2 As DataTable = c.buscar_max("*", "unidad")

        For i As Integer = 0 To datos2.Rows.Count - 1
            cmd_medida.Items.Add(datos2.Rows(i).Item("nombre"))
        Next
        Try
            codigo_producto = VentanaPrincipal.DataGridView1.CurrentRow.Cells(0).Value
            Dim con As conexion = New conexion()
            Dim datos As DataTable = con.buscar("p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo'", "producto p ,tipo t, unidad u", "p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad and p.id_producto=" & codigo_producto)
            codigo.Text = datos.Rows(0).Item("Codigo")
            'cod_producto = datos.Rows(0).Item("Codigo")
            nombre.Text = datos.Rows(0).Item("Nombre")
            marca.Text = datos.Rows(0).Item("Marca")
            detalle.Text = datos.Rows(0).Item("Detalle")
            tipo.Text = datos.Rows(0).Item("Tipo")
        Catch ex As Exception
            Me.Close()
        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Try
            If MessageBox.Show("El ingreso de nuevas existencias se sumara a " & tipo.Text & ", ¿desea continuar?", "Aviso", MessageBoxButtons.YesNoCancel) = Windows.Forms.DialogResult.Yes Then
                Dim c As Double = Convert.ToDouble(cantidad.Text)
                Dim d As Double = Convert.ToDouble(numero_unidades.Text)
                Dim conversion As Double = 0
                Dim con As conexion = New conexion()
                Dim tasa As Double = Convert.ToDouble(con.buscar("tasa", "unidad", "nombre='" & cmd_medida.Text & "'").Rows(0).Item("tasa"))
                conversion = Convert.ToDouble(cantidad.Text) / tasa

                Dim existencia_actual As Double = Convert.ToDouble(con.buscar("existencias", "tipo", "nombre='" & tipo.Text & "' and id_sede=" & VentanaPrincipal.id_sede).Rows(0).Item("existencias"))
                'MessageBox.Show("es " & existencia_actual & " tipo es " & tipo.Text)
                existencia_actual = existencia_actual + (conversion * Convert.ToDouble(numero_unidades.Text))
                existencia_actual = Math.Round(Convert.ToDouble(existencia_actual), 2)
                con.insertar("update tipo set existencias=" & existencia_actual & " where nombre='" & tipo.Text & "' and id_sede=" & VentanaPrincipal.id_sede, "", False, 0)
                'MessageBox.Show("es " & existencia_actual & " tipo es " & tipo.Text)
                MsgBox("Nuevas existencias registradas")
                Me.Close()
            End If
        Catch ex As Exception
            MsgBox("Error en el ingreso de valores")
        End Try
        
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub cantidad_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cantidad.KeyPress
        If e.KeyChar = "." Then

        Else
            If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
                e.Handled = True
            End If
        End If
        If (Asc(e.KeyChar)) = 13 Then
            cmd_medida.Select()
        End If
    End Sub

    Private Sub cantidad_TextChanged(sender As Object, e As EventArgs) Handles cantidad.TextChanged

    End Sub

    Private Sub numero_unidades_KeyPress(sender As Object, e As KeyPressEventArgs) Handles numero_unidades.KeyPress
       If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
            e.Handled = True
        End If
        If (Asc(e.KeyChar)) = 13 Then
            Button1.Select()
        End If
    End Sub

    Private Sub numero_unidades_RightToLeftChanged(sender As Object, e As EventArgs) Handles numero_unidades.RightToLeftChanged

    End Sub

    Private Sub numero_unidades_TextChanged(sender As Object, e As EventArgs) Handles numero_unidades.TextChanged

    End Sub
End Class