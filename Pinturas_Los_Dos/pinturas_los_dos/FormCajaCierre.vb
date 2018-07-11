Imports System.Net.Mail
Public Class FormCajaCierre

    Private Sub FormCajaCierre_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim tipo As New DataColumn("Tipo")
        tipo.DataType = GetType(String)
        Dim detalles As New DataColumn("Detalles")
        detalles.DataType = GetType(String)


        Dim datos As DataTable = New DataTable("movimientos")
        datos.Columns.Add(tipo)
        datos.Columns.Add(detalles)

        Dim dia As DateTime = DateTime.Today
        lblfecha.Text = dia.ToString("d")
        Dim c As conexion = New conexion()
        Dim dt As DataTable = c.buscar("InicioEfectivo", "caja", "fecha='" + lblfecha.Text + "' and id_sede=" & VentanaPrincipal.id_sede)

        Dim inicio As Double = 0
        Dim pagos As Double = 0
        Dim gastos As Double = 0
        If dt.Rows.Count > 0 Then
            lblinicio.Text = dt.Rows(0).Item("InicioEfectivo")
            inicio = Convert.ToDouble(dt.Rows(0).Item("InicioEfectivo")) ''
            datos.Rows.Add("Apertura Caja", "Se inicio caja con Q." & inicio)
            Dim dt2 As DataTable = c.buscar("SUM(valor) as 'suma'", "pago", "fecha='" & lblfecha.Text & "'")
            If Not IsDBNull(dt2.Rows(0).Item("suma")) Then
                pagos = Convert.ToDouble(dt2.Rows(0).Item("suma"))
                pagos = Math.Round(Convert.ToDouble(pagos), 2)
                lblpagorecibido.Text = pagos
                dt2 = c.buscar("p.id_pago as 'No.Pago',p.fecha , p.valor, p.id_venta as 'No.'", "pago p, venta v, usuario u", "p.fecha='" & lblfecha.Text & "' and p.id_venta=v.id_venta and v.id_usuario=u.id_usuario and u.id_sede=" & VentanaPrincipal.id_sede)
                If dt2.Rows.Count > 0 Then
                    For u As Integer = 0 To dt2.Rows.Count - 1
                        datos.Rows.Add("Pago recibido", "Pago por Q." & dt2.Rows(u).Item("valor") & " Venta No. " & dt2.Rows(u).Item("No."))
                    Next
                End If
            End If

            Dim dt3 As DataTable = c.buscar("SUM(monto) as 'suma'", "Gasto", "fecha='" + lblfecha.Text + "' and id_sede=" & VentanaPrincipal.id_sede)
            If Not IsDBNull(dt3.Rows(0).Item("suma")) Then
                lblgastos.Text = dt3.Rows(0).Item("suma")
                gastos = Convert.ToDouble(dt3.Rows(0).Item("suma"))
                dt3 = c.buscar("descripcion,monto", "gasto", "fecha='" & lblfecha.Text & "' and id_sede=" & VentanaPrincipal.id_sede)
                If dt3.Rows.Count > 0 Then
                    For u As Integer = 0 To dt3.Rows.Count - 1
                        datos.Rows.Add("Gasto Realizado", dt3.Rows(u).Item("descripcion") & " Q." & dt3.Rows(u).Item("monto"))
                    Next
                End If
            End If
        End If
        Dim total As Double = inicio + pagos - gastos
        total = Math.Round(Convert.ToDouble(total), 2)
        lbltotal.Text = Convert.ToString(total)
        TextBox1.Focus()

        DataGridView1.DataSource = datos
        DataGridView1.Refresh()
        DataGridView1.Columns(0).Width = 150
        DataGridView1.Columns(1).Width = 500
    End Sub
    Sub sendMail(fromMail As String, name As String, toMail As String, body As String, subject As String, host As String, password As String)
        Try
            Dim correo As New MailMessage
            Dim smtp As New SmtpClient()

            correo.From = New MailAddress(fromMail, name, System.Text.Encoding.UTF8)
            correo.To.Add(toMail)
            correo.SubjectEncoding = System.Text.Encoding.UTF8
            correo.Subject = subject
            correo.Body = body
            correo.BodyEncoding = System.Text.Encoding.UTF8
            correo.IsBodyHtml = False
            correo.Priority = MailPriority.Normal

            smtp.Port = 587
            smtp.Host = host
            smtp.Credentials = New System.Net.NetworkCredential(fromMail, password)
            smtp.EnableSsl = True
            smtp.Send(correo)
        Catch ex As Exception
            MessageBox.Show("An Exception Ocurred: " & ex.ToString())
        End Try
        
    End Sub
    Sub sendMails(body As String, subject As String)
        Dim c As conexion = New conexion()
        Dim correos As DataTable = c.buscar("*", "Correo", "id_sede=" & VentanaPrincipal.id_sede)
        For i As Integer = 0 To correos.Rows.Count - 1
            If correos.Rows(i).Item("remitente") = "1" Then
                For a As Integer = 0 To correos.Rows.Count - 1
                    If correos.Rows(a).Item("remitente") = "0" Then
                        sendMail(correos.Rows(i).Item("correo"), correos.Rows(i).Item("nombre"), correos.Rows(a).Item("correo"), body, subject, correos.Rows(i).Item("dominio"), correos.Rows(i).Item("contraseña"))
                        MessageBox.Show("Enviado")
                    End If
                Next
            End If
        Next


    End Sub
    Sub send()
        Dim body As String = "Fecha: " & lblfecha.Text.Trim() & vbNewLine
        body = body & "Apertura:              Q." & lblinicio.Text & vbNewLine
        body = body & "Pagos Recibidos:    Q." & lblpagorecibido.Text & vbNewLine
        body = body & "Gastos efectuados: Q." & lblgastos.Text & vbNewLine
        body = body & "-----------------------------------" & vbNewLine
        body = body & "Total:                    Q." & lbltotal.Text & vbNewLine
        body = body & "-----------------------------------" & vbNewLine
        body = body & "Cierre:                  Q." & TextBox1.Text.Trim() & vbNewLine
        body = body & "Nota: " & TextBox2.Text.Trim() & vbNewLine
        Dim subject As String = "Cierre de caja " & lblfecha.Text.Trim()
        sendMails(body, subject)
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If Trim(TextBox1.Text) <> "" Then
            If TextBox1.Text <> lbltotal.Text Then
                If MessageBox.Show("El efectivo en caja no es el mismo registrado en el sistema, ¿Desea proceder de todas formas?", "Advertencia", MessageBoxButtons.YesNoCancel) = Windows.Forms.DialogResult.Yes Then
                    Dim c As conexion = New conexion()
                    c.insertar("update caja set FinEfectivo=" + TextBox1.Text.Trim() + ", Nota='" + TextBox2.Text.Trim() + "' where fecha='" + lblfecha.Text + "' and id_sede=" & VentanaPrincipal.id_sede, "Cierre Exitoso ", True, 1)
                    send()
                    Me.Close()
                End If
            Else
                Dim c As conexion = New conexion()
                c.insertar("update caja set FinEfectivo=" + TextBox1.Text + ", Nota='" + TextBox2.Text + "' where fecha='" + lblfecha.Text + "' and id_sede=" & VentanaPrincipal.id_sede, "Cierre Exitoso ", True, 1)
                send()
                Me.Close()
            End If
        Else
            MessageBox.Show("Debe colocar un valor de cierre de efectivo")
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