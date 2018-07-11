Public Class Pago
    Public ver As Boolean = False
    Private Sub Pago_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '5*6
        Try
            Label2.Text = "Cliente:   " & form_ventas.DataGridView1.CurrentRow.Cells(5).Value
            Dim deuda As Double = Convert.ToDouble(form_ventas.DataGridView1.CurrentRow.Cells(8).Value) - Convert.ToDouble(form_ventas.DataGridView1.CurrentRow.Cells(9).Value)
            deuda = Math.Round(Convert.ToDouble(deuda), 2)
            Label3.Text = "Deuda:    Q." & deuda
            Dim c As conexion = New conexion()
            c.llenar_grid("select fecha as 'Fecha', valor as 'Valor' from pago where id_venta=" & form_ventas.DataGridView1.CurrentRow.Cells(0).Value, DataGridView1)
            If ver = True Then
                TextBox1.Enabled = False
                Aceptar.Enabled = False
            End If
        Catch ex As Exception

        End Try
        
    End Sub

    
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        form_ventas.Enabled = True
        Me.Close()
    End Sub

    Private Sub Aceptar_Click(sender As Object, e As EventArgs) Handles Aceptar.Click
        Try
            Dim q As Double = Convert.ToDouble(TextBox1.Text)
            Dim c As conexion = New conexion()
            Dim datos As DataTable = c.buscar("*", "venta", "id_venta=" & form_ventas.DataGridView1.CurrentRow.Cells(0).Value)
            Dim nuevo_total As Double = Convert.ToDouble(datos.Rows(0).Item("total_abonado")) + Convert.ToDouble(TextBox1.Text)
            nuevo_total = Math.Round(Convert.ToDouble(nuevo_total), 2)
            If Not TextBox1.Text.Trim = "" And VentanaPrincipal.CajaIniciada() = True Then
                If nuevo_total < Math.Round(Convert.ToDouble(Convert.ToDouble(datos.Rows(0).Item("total_venta"))), 2) Or nuevo_total = Math.Round(Convert.ToDouble(Convert.ToDouble(datos.Rows(0).Item("total_venta"))), 2) Then
                    c.insertar("update venta set total_abonado=" & nuevo_total & " where id_venta=" & form_ventas.DataGridView1.CurrentRow.Cells(0).Value, "", False, 0)
                    c.insertar("insert into pago values (" & Convert.ToDouble(TextBox1.Text) & ",'" & Date.Today & "'," & form_ventas.DataGridView1.CurrentRow.Cells(0).Value & ")", "", False, 0)
                    If nuevo_total = Math.Round(Convert.ToDouble(Convert.ToDouble(datos.Rows(0).Item("total_venta"))), 2) Then
                        Dim ganancia As Double = 0
                        Dim detalles As DataTable = c.buscar("d.id_presentacion,m.descripcion ,p.precio_compra ,p.precio_venta ", "detalle_venta d, Presentacion p, Medida m", "d.id_presentacion =p.id_presentacion and p.id_medida =m.id_medida and d.id_venta =" & form_ventas.DataGridView1.CurrentRow.Cells(0).Value)
                        For i As Integer = 0 To detalles.Rows.Count - 1
                            ganancia = ganancia + Convert.ToDouble((Convert.ToDouble(detalles.Rows(i).Item("precio_venta")) - Convert.ToDouble(detalles.Rows(i).Item("precio_compra"))))
                            ganancia = Math.Round(Convert.ToDouble(ganancia), 2)
                            '  MessageBox.Show("Ganancia es " & ganancia)
                        Next
                        c.insertar("update venta set total_ganancia=" & ganancia & " where id_venta=" & form_ventas.DataGridView1.CurrentRow.Cells(0).Value, "", False, 0)
                    End If
                    MsgBox("Pago realizado con exito")
                    form_ventas.Enabled = True
                    form_ventas.busc()
                    Me.Close()
                End If
            Else
                MessageBox.Show("Asegurese de llenar datos, e iniciar Caja")
            End If

        Catch ex As Exception
            MsgBox("Ingrese monto valido")
        End Try
        

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