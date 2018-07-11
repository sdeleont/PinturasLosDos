Imports System.IO
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Net.Mail
Imports System.Threading

Public Class VentanaPrincipal
    Public codigo_producto As Integer
    Public total As Double
    Public totalNetoNeto As Double
    Public cliente_nuevo As Boolean = True
    Public codigo_usuario_activo As String 'codigo de usuario activo
    Public administrador As Boolean = False
    Public nombre_usuario As String
    Public lista_codigos_presentacion As List(Of String) = New List(Of String)
    Public lista_vendedores As List(Of String) = New List(Of String)
    Public lista_ubicaciones As List(Of String) = New List(Of String)
    Public p As Boolean = False

    Public desc As Double = 0
    Public aument As Double = 0
    Public Vdesc As Double = 0
    Public Vaument As Double = 0
    Public fechaCredito As String = "---"

    Public id_cliente As String
    Dim t As Thread
    Public notificacioness As List(Of Notificacion) = New List(Of Notificacion)
    Public cad As String = Form1.cadena_conexion

    Public id_sede As String
    Public nomSede As String

    Private Sub ProcesodeFondo() 'este seria la funcion que maneja el subproceso


        Do While True
            'aqui tengo que validar
            ' si hay existencias que ya llegaron a su limite de aviso

            notificacioness.Clear()
            Dim c As conexion = New conexion()

            Form1.cadena_conexion = cad
            Dim dt As DataTable = c.buscar("*", "tipo", "existencias<= CantidadAvisar and existencias > 0 ")
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim nueva As Notificacion = New Notificacion()
                    nueva.texto = "La cantidad de existencias de " & dt.Rows(i).Item("nombre") & " es de " & Math.Round(Convert.ToDouble(dt.Rows(i).Item("existencias")), 2) & " el limite es " & dt.Rows(i).Item("cantidadavisar")
                    nueva.tipo = "Existencias"
                    notificacioness.Add(nueva)
                Next
            End If
            ' si hay existencias que ya llegaron a su final
            Dim dt2 As DataTable = c.buscar("*", "tipo", "existencias<= 0 ")
            If dt2.Rows.Count > 0 Then
                For i As Integer = 0 To dt2.Rows.Count - 1
                    Dim nueva As Notificacion = New Notificacion()
                    nueva.texto = "La cantidad de existencias de " & dt2.Rows(i).Item("nombre") & " llego a 0 "
                    nueva.tipo = "Existencias en 0"
                    notificacioness.Add(nueva)
                Next
            End If
            ' si alguien se le vence el credito hoy 
            dt = c.buscar("v.id_venta,v.fecha,v.total_venta, v.total_abonado,c.nombre 'Cliente', c.Telefono, v.Descuento,v.fecha_credito, v.Aumento", "venta v, Cliente c", "fecha_credito <> '' and fecha_credito <> '---' and total_abonado < total_venta and v.id_cliente=c.id_cliente")
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim fechacargo As Date = CDate(dt.Rows(i).Item("fecha_credito"))
                    If Date.Today = fechacargo Then
                        Dim nueva As Notificacion = New Notificacion()
                        'se vence hoy el credito
                        Dim dtn As DataTable = c.buscar("v.total_venta-v.total_abonado as 'Deuda' ", "venta v ", " v.id_venta=" & dt.Rows(i).Item("id_venta"))
                        nueva.texto = "Hoy se le vence el credito a " + dt.Rows(i).Item("Cliente") + " por venta No. " & dt.Rows(i).Item("id_venta") & " la deuda de esta venta es de Q." & dtn.Rows(0).Item("Deuda") & ", fecha de venta es " & dt.Rows(0).Item("fecha")
                        nueva.tipo = "Vence fecha credito"
                        notificacioness.Add(nueva)
                    ElseIf Date.Today > fechacargo Then
                        Dim nueva As Notificacion = New Notificacion()
                        'ya se vencio su credito
                        Dim dtn As DataTable = c.buscar("v.total_venta-v.total_abonado as 'Deuda' ", "venta v ", " v.id_venta=" & dt.Rows(i).Item("id_venta"))
                        nueva.texto = "Ya se vencio la fecha credito (" & fechacargo.Date & ") de " + dt.Rows(i).Item("Cliente") + " por venta No. " & dt.Rows(i).Item("id_venta") & " la deuda de esta venta es de Q." & dtn.Rows(0).Item("Deuda") & ", fecha de venta es " & dt.Rows(0).Item("fecha")
                        nueva.tipo = "Vence fecha credito"
                        notificacioness.Add(nueva)
                    End If
                Next

            End If
            ' si alguien ya se le va vencer el credito
            Dim dtdeudas As DataTable = c.buscar("t.id_cliente,t.nombre,t.limite_credito,t.deuda", "(select c.id_cliente,c.nombre,c.limite_credito, sum(v.total_venta-v.total_abonado) as 'Deuda' from Venta v, Cliente c where v.id_cliente=c.id_cliente and limite_credito > 0 group by c.id_cliente,c.nombre,c.limite_credito)as t,Cliente as c", "t.id_cliente=c.id_cliente and t.deuda+50 >= c.limite_credito and t.deuda>0")
            If dtdeudas.Rows.Count > 0 Then
                For i As Integer = 0 To dtdeudas.Rows.Count - 1
                    Dim nueva As Notificacion = New Notificacion()
                    nueva.texto = "Cliente No. " & dtdeudas.Rows(i).Item("id_cliente") & ", " & dtdeudas.Rows(i).Item("nombre") & " tiene limite de credito de Q." & dtdeudas.Rows(i).Item("limite_credito") & " y su deuda es de Q." & dtdeudas.Rows(i).Item("deuda")
                    nueva.tipo = "Credito por Cliente"
                    notificacioness.Add(nueva)
                Next
                End If
            'Thread.Sleep(600000) 'tiempo de espera del hilo(una pequeña pausa)

            If notificacioness.Count > 0 Then
                hacer(btnotificacion, "1")
            Else
                hacer(btnotificacion, "2")
            End If
            Thread.Sleep(7200000) 'tiempo de espera del hilo(una pequeña pausa) 5 minutos para revalidar
        Loop

    End Sub
    Private Delegate Sub AppendTextBoxDelegate(ByVal TB As Button, ByVal txt As String)
    Private Sub hacer(ByVal TB As Button, ByVal txt As String)
        If TB.InvokeRequired Then
            TB.Invoke(New AppendTextBoxDelegate(AddressOf hacer), New Object() {TB, txt})
        Else
            If txt = "1" Then
                TB.Text = "Notificaciones (" & notificacioness.Count & ")"
                Me.btnotificacion.BackColor = Color.Red
            Else
                TB.Text = "Notificaciones (0)"
                Me.btnotificacion.BackColor = Color.Green
            End If
            
        End If

    End Sub
    Private Sub ProductosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProductosToolStripMenuItem.Click

    End Sub
    Sub nueva_venta()
        Label1.Visible = True
        Label2.Visible = True
        Label3.Visible = True
        Label4.Visible = True
        Label5.Visible = True
        Label6.Visible = True
        Label7.Visible = True
        lbltotal.Visible = True
        'Panel1.Visible = True
        DataGridView1.Visible = True
        ListBox1.Visible = True
        Button1.Visible = True
        Button2.Visible = True
        ComboBox1.Visible = True
        nit.Visible = True
        nombres.Visible = True
        apellidos.Visible = True
        TextBox4.Visible = True
        Button3.Visible = True
        Button4.Visible = True
        Button5.Visible = True
        Button7.Visible = True
        Label11.Visible = True
        Label12.Visible = True
        Label13.Visible = True
        Label14.Visible = True
        cmbUbicacion.Visible = True
        Label17.Visible = True
        combovendedor.Visible = True
        telefono.Visible = True
        correo.Visible = True
        limitecredito.Visible = True
        GroupBox1.Visible = True
        Button6.Visible = True
        doc.Select()
        combovendedor.Text = "Ninguno"
        doc.Visible = True
        Label16.Visible = True
        new_venta()
    End Sub
    Sub cancelar_venta()
        Label1.Visible = False
        Label2.Visible = False
        Label3.Visible = False
        Label4.Visible = False
        Label5.Visible = False
        Label6.Visible = False
        Label7.Visible = False
        lbltotal.Visible = False
        ' Panel1.Visible = False
        DataGridView1.Visible = False
        ListBox1.Visible = False
        Button1.Visible = False
        Button2.Visible = False
        ComboBox1.Visible = False
        nit.Visible = False
        nombres.Visible = False
        apellidos.Visible = False
        TextBox4.Visible = False
        Button3.Visible = False
        Button4.Visible = False
        Button5.Visible = False
        Button7.Visible = False
        Label11.Visible = False
        Label12.Visible = False
        Label13.Visible = False
        telefono.Visible = False
        correo.Visible = False
        limitecredito.Visible = False
        Label14.Visible = False
        combovendedor.Visible = False
        GroupBox1.Visible = False
        Button6.Visible = False
        doc.Visible = False
        Label16.Visible = False
        cmbUbicacion.Visible = False
        Label17.Visible = False
    End Sub
    Private Sub VentanaPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.btnotificacion.Text = "Notificaciones (0)"
        Me.btnotificacion.BackColor = Color.Green
        t = New Thread(AddressOf Me.ProcesodeFondo) 'creamos el hilo con nombre "t"
        t.Start()
        combovendedor.Text = "Ninguno"
        Dim con As conexion = New conexion()
        Dim datos1 As DataTable = con.buscar_max("*", "empleado where id_sede=" + id_sede)
        For i As Integer = 0 To datos1.Rows.Count - 1
            combovendedor.Items.Add(datos1.Rows(i).Item("Nombre"))
            lista_vendedores.Add(datos1.Rows(i).Item("id_empleado"))
        Next
        Dim dataUbicaciones As DataTable = con.buscar("u.id_ubicacion, u.nombre", "ubicacion u, sede s", "s.id_sede=u.id_sede and u.id_sede=" + id_sede)
        For i As Integer = 0 To dataUbicaciones.Rows.Count - 1
            cmbUbicacion.Items.Add(dataUbicaciones.Rows(i).Item("nombre"))
            lista_ubicaciones.Add(dataUbicaciones.Rows(i).Item("id_ubicacion"))
            cmbUbicacion.Text = dataUbicaciones.Rows(i).Item("nombre")
        Next
        '' me vole ",p.existencias as 'Existencias'"
        ComboBox1.Text = "Nombre"
        con.llenar_grid("select top 15 p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo', u.nombre as 'Medida'from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad and p.id_ubicacion=" + lista_ubicaciones.ElementAt(cmbUbicacion.SelectedIndex), DataGridView1)
        DataGridView1.RowHeadersWidth = 20
        'DataGridView1.Font = FontFactory.GetFont("Arial ", 8, FontStyle.Regular)
        cancelar_venta()
        lblusuario.Text = "Bienvenido(a) " & nombre_usuario
        lblsede.Text = "Sede: " + nomSede
        If Not administrador Then
            UsuariosToolStripMenuItem.Visible = False
            EmpleadosToolStripMenuItem.Visible = False
        End If

    End Sub

    Private Sub nit_KeyPress(sender As Object, e As KeyPressEventArgs)
        If (Asc(e.KeyChar)) = 13 Then
            Dim c As conexion = New conexion()
            Try
                If Trim(nit.Text.ToLower()) = "c/f" Then
                    nombres.Text = "Consumidor Final"
                    apellidos.Text = "-"
                    nombres.Focus()
                Else
                    'si hubo nit entonces lo que hago es que busco el nit si existe relleno datos sino no
                    Dim datos As DataTable = c.buscar("*", "cliente", "nit='" & nit.Text & "' and id_sede=" + id_sede)
                    If datos.Rows.Count > 0 Then 'si hay nuevos
                        nombres.Text = Convert.ToString(datos.Rows(0).Item("nombre"))
                        apellidos.Text = Convert.ToString(datos.Rows(0).Item("apellido"))
                        telefono.Text = Convert.ToString(datos.Rows(0).Item("telefono"))
                        correo.Text = Convert.ToString(datos.Rows(0).Item("correo"))
                        limitecredito.Text = Convert.ToString(datos.Rows(0).Item("limite_credito"))
                        cliente_nuevo = False
                        id_cliente = Convert.ToString(datos.Rows(0).Item("id_cliente"))
                        nombres.Focus()
                    Else 'no hay nuevos
                        nombres.Focus()
                    End If
                End If

            Catch ex As Exception
                cliente_nuevo = True
                'nombres.Select()
            End Try
        End If
    End Sub



    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        codigo_producto = Convert.ToInt32(DataGridView1.CurrentRow.Cells(0).Value)
        Seleccion_Medida.Show()
    End Sub

    Private Sub DataGridView1_KeyDown(sender As Object, e As KeyEventArgs)
        If (e.KeyCode = Keys.Enter) Then
            e.Handled = True
        End If
    End Sub



    Private Sub DataGridView1_KeyPress(sender As Object, e As KeyPressEventArgs)

        If (Asc(e.KeyChar)) = 13 Then
            codigo_producto = Convert.ToInt32(DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(0).Value)
            Seleccion_Medida.Show()
        End If
    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Try
            Dim c As conexion = New conexion()
            total = total - Convert.ToDouble(c.buscar("precio_venta", "presentacion", "id_presentacion=" & lista_codigos_presentacion.ElementAt(ListBox1.SelectedIndex)).Rows(0).Item("precio_venta"))
            lbltotal.Text = "Q.  " & total
            valtotal()
            lista_codigos_presentacion.RemoveAt(ListBox1.SelectedIndex)
            ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)

        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        probar()
        ListBox1.Items.Clear()
        lista_codigos_presentacion.Clear()
        total = 0
        lbltotal.Text = "Q.  0.00"
        valtotal()
    End Sub

    Private Sub TextBox4_KeyPress(sender As Object, e As KeyPressEventArgs)
        If (Asc(e.KeyChar)) = 13 Then
            Try
                codigo_producto = Convert.ToInt32(DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(0).Value)
                Seleccion_Medida.Show()
            Catch ex As Exception
            End Try


        End If
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs)
        'Dim con As conexion = New conexion()
        ' ''le quite esta parte ",p.existencias as 'Existencias' "
        'If ComboBox1.Text = "Codigo" Then
        '    con.llenar_grid("select p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo',u.nombre as 'Medida'from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad and p.id_producto like '%" & TextBox4.Text & "%'", DataGridView1)
        'Else
        '    con.llenar_grid("select p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo',u.nombre as 'Medida'from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad and p." & ComboBox1.Text & " like '%" & TextBox4.Text & "%'", DataGridView1)
        'End If
    End Sub

    Private Sub nombres_KeyPress(sender As Object, e As KeyPressEventArgs)
        If (Asc(e.KeyChar)) = 13 Then
            apellidos.Select()
        End If
    End Sub



    Private Sub apellidos_KeyPress(sender As Object, e As KeyPressEventArgs)
        If (Asc(e.KeyChar)) = 13 Then
            telefono.Select()
        End If
    End Sub


    Private Sub NuevaOrdenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NuevaOrdenToolStripMenuItem.Click
        nueva_venta()
        new_venta()
        nit.Enabled = True
    End Sub

    Private Sub CancelarVentaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CancelarVentaToolStripMenuItem.Click
        cancelar_venta()
        new_venta()
    End Sub

    Private Sub NuevoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NuevoToolStripMenuItem.Click
        form_productos_nuevo.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Dim con As conexion = New conexion()
        ''me vole ,p.existencias as 'Existencias'
        'con.llenar_grid("select p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo', u.nombre as 'Medida'from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad ", DataGridView1)
        DataGridView1.RowHeadersWidth = 20
        If ComboBox1.Text = "Codigo" Then
            con.llenar_grid("select top 15 p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo',u.nombre as 'Medida'from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad and p.id_producto like '%" & TextBox4.Text & "%' and p.id_ubicacion=" + lista_ubicaciones.ElementAt(cmbUbicacion.SelectedIndex), DataGridView1)
        Else
            con.llenar_grid("select top 15 p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo',u.nombre as 'Medida'from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad and p." & ComboBox1.Text & " like '%" & TextBox4.Text & "%' and p.id_ubicacion=" + lista_ubicaciones.ElementAt(cmbUbicacion.SelectedIndex), DataGridView1)
        End If
        'DataGridView1.Font = New Font("Arial ", 8, FontStyle.Regular)
    End Sub


    Private Sub DfgToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub UsuariosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UsuariosToolStripMenuItem.Click
        form_usuarios.Show()
    End Sub

    Private Sub VentasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VentasToolStripMenuItem.Click
        form_ventas.Show()
    End Sub

    Private Sub NuevaCategoriaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NuevaCategoriaToolStripMenuItem.Click
        form_tipo.Show()
    End Sub

    Private Sub AyudaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AyudaToolStripMenuItem.Click
        form_gastos.Show()

    End Sub
    Public Sub valtotal()
        Dim ret As String = ""
        Dim valor As Double = total - Vdesc + Vaument
        ret = "Q. " & valor
        totalNetoNeto = valor
        lbltotalneto.Text = ret
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs)
        new_venta()
        'Dim message As New MailMessage
        'Dim smtp As New SmtpClient
        'message.From = New MailAddress("sergio.giovanni360@hotmail.com")

        '        message.To.Add("sergiovem2016@outlook.com")

        'message.Body = "Esto es un a prueba de correo"

        'message.Subject = "TITULO DEL CORREO"

        '        message.Priority = MailPriority.Normal

        '  smtp.EnableSsl = True

        '        smtp.Port = "587"

        ' smtp.Host = "smtp.live.com"

        'smtp.Credentials = New Net.NetworkCredential("sergio.giovanni360@hotmail.com", "mariwana")

        'smtp.Send(message)

    End Sub
    Sub new_venta()
        lbltotal.Text = "Q.  0.00"
        valtotal()
        nit.Enabled = True
        ListBox1.Items.Clear()
        doc.Text = ""
        nit.Text = ""
        nombres.Text = ""
        apellidos.Text = ""
        TextBox4.Text = ""
        total = 0
        pago.Text = ""
        cliente_nuevo = True
        lista_codigos_presentacion.Clear()
        doc.Select()
        p = False
        desc = 0
        Vdesc = 0
        aument = 0
        Vaument = 0
        porcentaje.Text = "Q. 0.00"
        Label19.Text = "%"
        lblfechacredito.Text = "---"
        fechaCredito = "---"
        id_cliente = ""
        telefono.Text = ""
        correo.Text = ""
        limitecredito.Text = "0"
        combovendedor.Text = "Ninguno"
    End Sub
    Sub probar()
        Dim pgSize As New iTextSharp.text.Rectangle(800, 800)
        Dim documentoPDF As New Document(PageSize.LETTER, 36, 36, 36, 36)

        PdfWriter.GetInstance(documentoPDF,
            New FileStream("prueba.pdf", FileMode.Create))
        documentoPDF.Open()

        Dim t As Integer = 8
        documentoPDF.Add(New Paragraph("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim Tab1 As New PdfPTable(3)
        Dim T1 As New PdfPCell(New Phrase("primeraaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        T1.Border = 0
        Dim T2 As New PdfPCell(New Phrase("segundaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        T2.Border = 0
        Dim T3 As New PdfPCell(New Phrase("terceraawesdsdaasdasdasdasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        T3.Border = 0
        Dim Tn As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Tn.Border = 0
        Tab1.AddCell(T1)
        Tab1.AddCell(T2)
        Tab1.AddCell(T3)

        documentoPDF.Add(Tab1)
        documentoPDF.Close()
        If System.IO.File.Exists("prueba.pdf") Then

            'Abrimos el fichero PDF con la aplicación asociada
            System.Diagnostics.Process.Start("prueba.pdf")
        End If
    End Sub
    Sub GenerarPDF(ByVal nit As String, ByVal nombre As String, ByVal apellidos As String, ByVal id_venta As String, ByVal direccion As String, ByVal telefono As String, ByVal vendedor As String, ByVal condicion As String, ByVal tnetoser As String)
        Dim pgSize As New iTextSharp.text.Rectangle(800, 800)
        Dim documentoPDF As New Document(PageSize.LETTER, 36, 36, 36, 36)

        PdfWriter.GetInstance(documentoPDF,
            New FileStream("Venta" + id_venta + ".pdf", FileMode.Create))
        documentoPDF.Open()


        Dim t As Integer = 9
        documentoPDF.Add(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        documentoPDF.Add(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        documentoPDF.Add(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        documentoPDF.Add(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        documentoPDF.Add(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        'documentoPDF.Add(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        'documentoPDF.Add(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        '-----------------------------------------------------------------------------------------------------------------
        documentoPDF.Add(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim Tab1 As New PdfPTable(7)
        Dim T1nada As New PdfPCell(New Phrase("", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim T1nom As New PdfPCell(New Phrase("   " & nombre & " " & apellidos, FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim T1nit As New PdfPCell(New Phrase(nit, FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim T1hora As New PdfPCell(New Phrase(Now.ToString("HH:mm"), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim T1fecha As New PdfPCell(New Phrase(Date.Now.Date.ToString("dd/MM/yyyy"), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))

        T1nada.Border = 0
        T1nom.Border = 0
        T1nit.Border = 0
        T1hora.Border = 0
        T1fecha.Border = 0
        T1nom.Colspan = 3
        Tab1.AddCell(T1nada)
        Tab1.AddCell(T1nom)
        Tab1.AddCell(T1nit)
        Tab1.AddCell(T1hora)
        Tab1.AddCell(T1fecha)
        documentoPDF.Add(Tab1)
        'documentoPDF.Add(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        '--------------------------------------------------------------------------------------------------------------------
        Dim Tab2 As New PdfPTable(7)
        Dim T1dir As New PdfPCell(New Phrase("            " & direccion, FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim T1tel As New PdfPCell(New Phrase(telefono, FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim T1vend As New PdfPCell(New Phrase(vendedor, FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim T1cond As New PdfPCell(New Phrase(condicion, FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        T1dir.Colspan = 4
        T1dir.Border = 0
        T1tel.Border = 0
        T1vend.Border = 0
        T1cond.Border = 0
        Tab2.AddCell(T1dir)
        Tab2.AddCell(T1tel)
        Tab2.AddCell(T1vend)
        Tab2.AddCell(T1cond)
        documentoPDF.Add(Tab2)

        documentoPDF.Add(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        'documentoPDF.Add(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))

        Dim Table As New PdfPTable(7)

        Dim c As conexion = New conexion()
        Dim listaTemp As List(Of ListTemp) = New List(Of ListTemp)
        For Each el As String In lista_codigos_presentacion
            Dim encon As Boolean = False

            For Each Icon As ListTemp In listaTemp

                If Icon.GetCod() = el And encon = False Then
                    Icon.SumCant()
                    encon = True
                End If
            Next
            If encon = False Then
                Dim nu As ListTemp = New ListTemp()
                nu.setCodigo(el)
                listaTemp.Add(nu)
            End If
        Next
        Dim totalVenta As Double = 0
        Dim conteo = 0

        For Each elemento As ListTemp In listaTemp 'para cada presentacion de productos
            conteo = conteo + 1
            Dim datos As DataTable = c.buscar("p.id_presentacion, m.descripcion, Pr.nombre, Pr.marca , p.precio_venta", "Presentacion p, Producto PR, Medida m", "p.id_producto=PR.id_producto and p.id_medida=m.id_medida and p.id_presentacion=" & elemento.GetCod())
            'Dim COD As New PdfPCell(New Phrase(Convert.ToString(datos.Rows(0).Item("id_presentacion"))))
            Dim CA As New PdfPCell(New Phrase("      " & Convert.ToString(elemento.GetCantidad()), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
            Dim NOM As New PdfPCell(New Phrase("     " & Convert.ToString(datos.Rows(0).Item("descripcion")) & " " & Convert.ToString(datos.Rows(0).Item("nombre")) & " " & Convert.ToString(datos.Rows(0).Item("marca")), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
            Dim PU As New PdfPCell(New Phrase("  " & Convert.ToString(datos.Rows(0).Item("precio_venta")), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
            Dim mul As Double = elemento.GetCantidad() * Convert.ToDouble(datos.Rows(0).Item("precio_venta"))
            Dim TOTT As New PdfPCell(New Phrase(Convert.ToString(mul), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))



            NOM.Border = 0
            NOM.Colspan = 4
            PU.Border = 0
            CA.Border = 0
            TOTT.Border = 0
            Table.AddCell(CA)
            Table.AddCell(NOM)
            Table.AddCell(PU)
            Table.AddCell(TOTT)
            totalVenta = totalVenta + mul
        Next
        While conteo < 11
            conteo = conteo + 1
            Dim n1 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
            Dim n2 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
            Dim nx As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
            Dim n3 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
            Dim n4 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
            n2.Colspan = 4
            n1.Border = 0
            n2.Border = 0
            n3.Border = 0
            n4.Border = 0
            nx.Border = 0
            Table.AddCell(n1)
            Table.AddCell(n2)
            'Table.AddCell(nx)
            Table.AddCell(n3)
            Table.AddCell(n4)
        End While



        Dim p1 As New PdfPCell(New Phrase(Convert.ToString(""), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim p2 As New PdfPCell(New Phrase(Convert.ToString(""), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim p3 As New PdfPCell(New Phrase(Convert.ToString(""), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim qq As String = Convert.ToString(Math.Round(totalVenta, 2))
        p1.Border = 0
        p2.Colspan = 4
        p2.Border = 0
        p3.Border = 0
        Dim tottuu As New PdfPCell(New Phrase(Convert.ToString(tnetoser), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        tottuu.Border = 0
        Table.AddCell(p1)
        Table.AddCell(p2)
        Table.AddCell(p3)
        Table.AddCell(tottuu)

        documentoPDF.Add(Table)
        documentoPDF.Close()
        If System.IO.File.Exists("Venta" + id_venta + ".pdf") Then
            If MsgBox("Se ha creado un PDF" + _
                   "¿desea abrir el fichero PDF ?",
                   MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                'Abrimos el fichero PDF con la aplicación asociada
                System.Diagnostics.Process.Start("Venta" + id_venta + ".pdf")
            End If
        End If
    End Sub
    Sub GenerarPDFCARPAINT(ByVal nit As String, ByVal nombre As String, ByVal apellidos As String, ByVal id_venta As String, ByVal direccion As String, ByVal telefono As String, ByVal vendedor As String, ByVal condicion As String, ByVal documento As String, ByVal totalneto As String)
        Dim sLine As String = ""
        Dim sLine2 As String = ""
        Dim t As Integer = 8
        Try
            Dim objReader As New StreamReader("info.txt")
            Do
                sLine2 = objReader.ReadLine()
                If Not sLine2 Is Nothing Then
                    sLine = sLine & vbNewLine & sLine2
                End If
            Loop Until sLine2 Is Nothing

        Catch ex As Exception
            MessageBox.Show("Error en Path" & ex.ToString())
        End Try
        Dim pgSize As New iTextSharp.text.Rectangle(800, 800)
        Dim documentoPDF As New Document(PageSize.LETTER, 36, 36, 36, 36)

        PdfWriter.GetInstance(documentoPDF,
            New FileStream("Venta" + id_venta + ".pdf", FileMode.Create))
        documentoPDF.Open()
        documentoPDF.Add(New Paragraph("Documento " & documento, FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        documentoPDF.Add(New Paragraph(sLine, FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        documentoPDF.Add(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        '---------------------------------------------------Empieza la Tabla----------------------------------------------------------
        Dim Table As New PdfPTable(7)

        Dim c As conexion = New conexion()
        Dim listaTemp As List(Of ListTemp) = New List(Of ListTemp)
        For Each el As String In lista_codigos_presentacion
            Dim encon As Boolean = False

            For Each Icon As ListTemp In listaTemp

                If Icon.GetCod() = el And encon = False Then
                    Icon.SumCant()
                    encon = True
                End If
            Next
            If encon = False Then
                Dim nu As ListTemp = New ListTemp()
                nu.setCodigo(el)
                listaTemp.Add(nu)
            End If
        Next
        Dim totalVenta As Double = 0
        Dim conteo = 0
        ''-----------------------------------------------Encabezados
        Dim CA2 As New PdfPCell(New Phrase("Cantidad", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim NOM2 As New PdfPCell(New Phrase("Descripcion", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim PU2 As New PdfPCell(New Phrase("P/U Q.", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))

        Dim TOTT2 As New PdfPCell(New Phrase("Total Q.", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))



        NOM2.Border = 2
        NOM2.Colspan = 4
        PU2.Border = 2
        CA2.Border = 2
        TOTT2.Border = 2
        Table.AddCell(CA2)
        Table.AddCell(NOM2)
        Table.AddCell(PU2)
        Table.AddCell(TOTT2)
        ''------------------------------------------------Emcabezados
        For Each elemento As ListTemp In listaTemp 'para cada presentacion de productos
            conteo = conteo + 1
            Dim datos As DataTable = c.buscar("p.id_presentacion, m.descripcion, Pr.nombre, Pr.marca , p.precio_venta", "Presentacion p, Producto PR, Medida m", "p.id_producto=PR.id_producto and p.id_medida=m.id_medida and p.id_presentacion=" & elemento.GetCod())
            'Dim COD As New PdfPCell(New Phrase(Convert.ToString(datos.Rows(0).Item("id_presentacion"))))
            Dim CA As New PdfPCell(New Phrase("                " & Convert.ToString(elemento.GetCantidad()), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
            Dim NOM As New PdfPCell(New Phrase("     " & Convert.ToString(datos.Rows(0).Item("descripcion")) & " " & Convert.ToString(datos.Rows(0).Item("nombre")) & " " & Convert.ToString(datos.Rows(0).Item("marca")), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
            Dim PU As New PdfPCell(New Phrase("  " & Convert.ToString(datos.Rows(0).Item("precio_venta")), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
            Dim mul As Double = elemento.GetCantidad() * Convert.ToDouble(datos.Rows(0).Item("precio_venta"))
            Dim TOTT As New PdfPCell(New Phrase(Convert.ToString(mul), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))



            NOM.Border = 0
            NOM.Colspan = 4
            PU.Border = 0
            CA.Border = 0
            TOTT.Border = 0
            Table.AddCell(CA)
            Table.AddCell(NOM)
            Table.AddCell(PU)
            Table.AddCell(TOTT)
            totalVenta = totalVenta + mul
        Next
        'While conteo < 13
        '    conteo = conteo + 1
        '    Dim n1 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        '    Dim n2 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        '    Dim nx As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        '    Dim n3 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        '    Dim n4 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        '    n2.Colspan = 4
        '    n1.Border = 0
        '    n2.Border = 0
        '    n3.Border = 0
        '    n4.Border = 0
        '    nx.Border = 0
        '    Table.AddCell(n1)
        '    Table.AddCell(n2)
        '    'Table.AddCell(nx)
        '    Table.AddCell(n3)
        '    Table.AddCell(n4)
        'End While



        Dim p1 As New PdfPCell(New Phrase(Convert.ToString(""), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim p2 As New PdfPCell(New Phrase(Convert.ToString(""), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim p3 As New PdfPCell(New Phrase(Convert.ToString(""), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim qq As String = Convert.ToString(Math.Round(totalVenta, 2))
        p1.Border = 0
        p2.Colspan = 4
        p2.Border = 0
        p3.Border = 0
        Dim tottuu As New PdfPCell(New Phrase("Subtotal" & " " & Convert.ToString(qq), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        tottuu.Border = 0
        Table.AddCell(p1)
        Table.AddCell(p2)
        Table.AddCell(p3)
        Table.AddCell(tottuu)

        Dim p11 As New PdfPCell(New Phrase(Convert.ToString(""), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim p21 As New PdfPCell(New Phrase(Convert.ToString(""), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        Dim p31 As New PdfPCell(New Phrase(Convert.ToString(""), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))

        p11.Border = 0
        p21.Colspan = 4
        p21.Border = 0
        p31.Border = 0
        Dim tottuu1 As New PdfPCell(New Phrase("Total" & " " & Convert.ToString(totalneto), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        tottuu1.Border = 0
        Table.AddCell(p11)
        Table.AddCell(p21)
        Table.AddCell(p31)
        Table.AddCell(tottuu1)

        documentoPDF.Add(Table)
        ''------------------------------------------------------Termina la tabla-----------------------------------------------------------
        documentoPDF.Close()
        If System.IO.File.Exists("Venta" + id_venta + ".pdf") Then
            If MsgBox("Se ha creado un PDF" + _
                   "¿desea abrir el fichero PDF ?",
                   MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                'Abrimos el fichero PDF con la aplicación asociada
                System.Diagnostics.Process.Start("Venta" + id_venta + ".pdf")
            End If
        End If
    End Sub
    Function numitems()
        Dim cantidad As Integer = 0
        Dim lista_temp As List(Of Integer) = New List(Of Integer)
        For i As Integer = 0 To lista_codigos_presentacion.Count - 1
            Dim ban As Boolean = False
            For a As Integer = i + 1 To lista_codigos_presentacion.Count - 1
                If lista_codigos_presentacion.ElementAt(i) = lista_codigos_presentacion.ElementAt(a) Then
                    ban = True
                End If
            Next
            If (ban = False) Then
                lista_temp.Add(lista_codigos_presentacion.ElementAt(i))
            End If
        Next
        Return lista_temp.Count
    End Function
    Private Sub Button4_Click(sender As Object, e As EventArgs)
        Dim verificar As Boolean = False
        Try
            Dim p As Double = Convert.ToDouble(Pago.Text)
            verificar = True
        Catch ex As Exception
            verificar = False
        End Try
        'aqui van a ver multiples procesos
        '(1) Verificar que la venta se pueda hacer 
        
            If verificar And CajaIniciada() = True Then
                If Not nit.Text.Trim = "" And Not nombres.Text.Trim = "" And Not apellidos.Text.Trim = "" And Not Pago.Text.Trim = "" Then
                    If Not ListBox1.Items.Count = 0 Then
                        If Not Convert.ToDouble(Pago.Text.Trim) > total Then
                            If combovendedor.Text.Trim() <> "" Then

                                Dim c As conexion = New conexion()
                                ''esto solo es para poner el total con aumentos y todo
                                Dim tottal As String = ""
                                If desc = 0 And aument = 0 And Vdesc = 0 And Vaument = 0 Then
                                    tottal = total
                                Else
                                    tottal = totalNetoNeto
                                End If
                                ''este procedimiento se valida el limite de credito del cliente si sobrepaso o no
                                ''se le pregunta al usuario si desea continuar con bandera banderapasa
                                Dim banderapasa As Boolean = False

                                If limitecredito.Text > 0 Then
                                    If cliente_nuevo Then
                                        If Convert.ToDouble(tottal) > Convert.ToDouble(limitecredito.Text) Then
                                            If MessageBox.Show("Valor sobrepasa limite de credito ¿desea continuar?", "Advertencia", MessageBoxButtons.YesNoCancel) = Windows.Forms.DialogResult.Yes Then
                                                banderapasa = True
                                            End If
                                        Else
                                            banderapasa = True
                                        End If
                                    Else
                                        Dim dt As DataTable = c.buscar("SUM(total_venta-total_abonado) as Deuda", "venta", "id_cliente=" & id_cliente & " and total_abonado < total_venta")
                                        If IsDBNull(dt.Rows(0).Item("Deuda")) Then
                                            banderapasa = True
                                        Else
                                            Dim va As Double = Convert.ToDouble(dt.Rows(0).Item("Deuda")) + tottal - Convert.ToDouble(Pago.Text.Trim)
                                            If (va) > (Convert.ToDouble(limitecredito.Text)) Then
                                                Dim cad As String = "Con esta venta el cliente sobrepasa su limite de credito. "
                                                cad = cad & vbNewLine
                                                cad = cad & "Deuda:   Q." & dt.Rows(0).Item("Deuda")
                                                cad = cad & vbNewLine
                                                cad = cad & "Total:    Q." & va
                                                cad = cad & vbNewLine
                                                cad = cad & "Credito: Q. " & limitecredito.Text.Trim
                                                cad = cad & vbNewLine
                                                cad = cad & "¿Desea continuar de todas formas?"
                                                If MessageBox.Show(cad, "Advertencia", MessageBoxButtons.YesNoCancel) = Windows.Forms.DialogResult.Yes Then
                                                    banderapasa = True
                                                End If
                                            ElseIf (va) = (Convert.ToDouble(limitecredito.Text)) Then
                                                Dim cad As String = "Con esta venta el cliente llego a su limite de credito."
                                                cad = cad & vbNewLine
                                                cad = cad & "Deuda:   Q." & dt.Rows(0).Item("Deuda")
                                                cad = cad & vbNewLine
                                                cad = cad & "Total:    Q." & va
                                                cad = cad & vbNewLine
                                                cad = cad & "Credito: Q. " & limitecredito.Text.Trim
                                                cad = cad & vbNewLine
                                                cad = cad & "¿Desea continuar de todas formas?"
                                                If MessageBox.Show(cad, "Advertencia", MessageBoxButtons.YesNoCancel) = Windows.Forms.DialogResult.Yes Then
                                                    banderapasa = True
                                                End If
                                            Else
                                                banderapasa = True
                                            End If
                                        End If
                                    End If
                                Else
                                    banderapasa = True
                                End If
                                ''se continua si si paso la bandera
                                If banderapasa = True Then
                                    '(2) Registrar la venta almacenando y usuario y cliente responsable 'asignar fechas

                                    If cliente_nuevo Then 'esto es para el cliente nuevo
                                        If (nit.Text.ToLower().Trim.Equals("c/f")) Then
                                        c.insertar("insert into cliente values ('c/f','" & nombres.Text.Trim & "','" & apellidos.Text.Trim & "','" & telefono.Text.Trim & "','" & correo.Text.Trim & "'," & limitecredito.Text.Trim & "," & id_sede & ")", "", False, 0)
                                        Else 'la diferencia es que aqui va su nit
                                        c.insertar("insert into cliente values ('" & nit.Text.Trim & "','" & nombres.Text.Trim & "','" & apellidos.Text.Trim & "','" & telefono.Text.Trim & "','" & correo.Text.Trim & "'," & limitecredito.Text.Trim & "," & id_sede & ")", "", False, 0)
                                        End If
                                        id_cliente = c.buscar_max("MAX(id_cliente) as Codigo", "cliente").Rows(0).Item("Codigo")
                                    Else
                                        c.insertar("update cliente set nombre='" & nombres.Text.Trim & "' , apellido='" & apellidos.Text.Trim & "', nit='" & nit.Text.Trim & "', telefono='" & telefono.Text.Trim & "', correo='" & correo.Text.Trim & "', limite_credito=" & limitecredito.Text.Trim & " where id_cliente=" & id_cliente, "", False, 0)
                                    End If



                                    ''de aqui para abajo esta bien
                                c.insertar("insert into venta values ('" & Date.Today & "'," & Math.Round(Convert.ToDouble(tottal), 2) & "," & pago.Text.Trim & ",0," & codigo_usuario_activo & "," & id_cliente & "," & desc & "," & lista_vendedores.ElementAt(combovendedor.SelectedIndex) & ",'" & fechaCredito & "'," & aument & "," & doc.Text.Trim() & "')", "", False, 0)

                                    '(3) Registrar detalle de venta
                                    Dim ganancia As Double = 0
                                    Dim id_venta As String = c.buscar_max("MAX(id_venta) as Codigo", "venta").Rows(0).Item("Codigo")

                                    If Convert.ToDouble(Pago.Text.Trim) > 0 Then
                                        'registro el pago inicial
                                        c.insertar("insert into pago values (" & Convert.ToDouble(Pago.Text.Trim) & ",'" & Date.Today & "'," & id_venta & ")", "", False, 0)
                                    End If

                                    For Each elemento As String In lista_codigos_presentacion 'para cada presentacion de productos
                                        Dim datos As DataTable = c.buscar("precio_venta, precio_compra", "presentacion", "id_presentacion=" & elemento)
                                        ganancia = ganancia + Convert.ToDouble((Convert.ToDouble(datos.Rows(0).Item("precio_venta")) - Convert.ToDouble(datos.Rows(0).Item("precio_compra"))))
                                    c.insertar("insert into detalle_venta values (" & id_venta & "," & elemento & "," & Convert.ToDouble(datos.Rows(0).Item("precio_venta")) & "," & Convert.ToDouble(datos.Rows(0).Item("precio_compra")) & ")", "", False, 0)
                                    'procedimiento para debitar del tipo
                                        quitar_existencia(elemento)
                                    Next

                                    If totalNetoNeto = Pago.Text.Trim Then
                                        c.insertar("update venta set total_ganancia=" & ganancia & " where id_venta=" & id_venta, "", False, 0)
                                    End If
                                    MsgBox("Venta registrada con exito")
                                    ''Codigo condicion
                                    Dim cond As String = ""
                                    If fechaCredito <> "---" Then
                                        Dim f1 As Date = CDate(fechaCredito)
                                        Dim f2 As Date = CDate(Date.Today)
                                        'MessageBox.Show(f2)
                                        Dim kk As String = Convert.ToString(DateDiff(DateInterval.Day, f2, f1))
                                        cond = kk & " dias"
                                    End If
                                    If desc > 0 Then
                                        cond = cond & " Desc " & desc & "%"
                                    End If
                                    If aument > 0 Then
                                        cond = cond & " Aum " & aument & "%"
                                    End If
                                    ''
                                    'GenerarPDFCARPAINT(nit.Text.Trim, nombres.Text.Trim, apellidos.Text.Trim, id_venta, correo.Text.Trim, telefono.Text.Trim, combovendedor.Text.Trim, cond, doc.Text, tottal)
                                    GenerarPDF(nit.Text.Trim, nombres.Text.Trim, apellidos.Text.Trim, id_venta, correo.Text.Trim, telefono.Text.Trim, combovendedor.Text.Trim, cond, tottal)
                                    new_venta()
                                    '(4) Generar documentos como factura o recibo de venta
                                End If

                            Else
                                MessageBox.Show("Seleccione alguna opcion en Vendedor")
                            End If
                        Else
                            MsgBox("Pago excede cantidad")
                        End If
                    Else
                        MsgBox("No se han especificado productos")
                    End If
                Else
                    MsgBox("Debe rellenar los datos necesarios")

                End If

            Else
                MsgBox("Ingrese una cantidad valida en el pago, e inicie Caja antes de cualquier Operacion")
            End If

    End Sub
    Sub quitar_existencia(ByVal id As String)
        Dim c As conexion = New conexion()
        Dim id_tipo As String = c.buscar("pr.id_producto, t.id_tipo", "Presentacion p, Producto pr, tipo t", "p.id_producto =pr.id_producto and pr.id_tipo =t.id_tipo  and p.id_presentacion =" & id).Rows(0).Item("id_tipo")
        Dim medida_menos As String = c.buscar("m.cantidad , m.descripcion", "Presentacion p, Medida m", "p.id_medida =m.id_medida and p.id_presentacion =" & id).Rows(0).Item("cantidad")
        Dim nueva_cantidad As Double = Convert.ToDouble(c.buscar("existencias", "tipo", "id_tipo=" & id_tipo).Rows(0).Item("existencias")) - Convert.ToDouble(medida_menos)
        nueva_cantidad = Math.Round(Convert.ToDouble(nueva_cantidad), 2)
        c.insertar("update tipo set existencias =" & nueva_cantidad & " where id_tipo =" & id_tipo, "", False, 0)
    End Sub


    Private Sub CerrarSesiónToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CerrarSesiónToolStripMenuItem.Click
        If MsgBox("Esta seguro que desea cerrar sesión", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            administrador = False
            Form1.Show()
            Buscar_codigo.Close()
            Clientes.Close()
            EmpleadoN.Close()
            form_existencias.Close()
            form_detalle.Close()
            form_inventario.Close()
            form_gastos.Close()
            Form_principal.Close()
            form_productos.Close()
            form_productos_nuevo.Close()
            form_tipo.Close()
            form_usuarios.Close()
            form_ventas.Close()
            FormCaja.Close()
            FormCajaCierre.Close()
            FormEmpleados.Close()
            Galon_disponible.Close()
            Gastos.Close()
            GenCliente.Close()
            Notificaciones.Close()
            OpcionesVenta.Close()
            Seleccion_Medida.Close()
            VerCaja.Close()
            t.Abort()
            Me.Close()
        End If
    End Sub

    Private Sub GastosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GastosToolStripMenuItem.Click
        Gastos.Show()
    End Sub

    Private Sub NuevasExistenciasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NuevasExistenciasToolStripMenuItem.Click

        Try
            Dim prueba As String = DataGridView1.CurrentRow.Cells(0).Value
            form_existencias.Show()
        Catch ex As Exception
            MsgBox("No se encuentra ningun producto seleccionado")
        End Try
    End Sub

    Private Sub GalonesEnTiendaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GalonesEnTiendaToolStripMenuItem.Click
        Galon_disponible.Show()
    End Sub


    Private Sub AcercaDeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AcercaDeToolStripMenuItem.Click
        Dim c As String = "Sistema Realizado Por:" & vbCrLf
        c = c & "    Sergio Giovanni de León Torón" & vbCrLf
        c = c & vbCrLf
        c = c & "Correo Electronico: " & vbCrLf
        c = c & "    sergio3giovanni@gmail.com" & vbCrLf
        c = c & vbCrLf
        c = c & "Nombre del Sistema: " & vbCrLf
        c = c & "    AutoPVem 4.0"

        MsgBox(c, vbInformation)

    End Sub

    Private Sub pago_KeyPress(sender As Object, e As KeyPressEventArgs)
        If e.KeyChar = "." Then

        Else
            If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
                e.Handled = True
            End If
        End If
        If (Asc(e.KeyChar)) = 13 Then
            Button4.Select()
            Button4.Refresh()
        End If
    End Sub

    Private Sub pago_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub IniciarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles IniciarToolStripMenuItem.Click
        FormCaja.Show()
    End Sub

    Private Sub CierreToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CierreToolStripMenuItem.Click
        FormCajaCierre.Show()
    End Sub

    Private Sub EmpleadosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmpleadosToolStripMenuItem.Click
        FormEmpleados.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)
        OpcionesVenta.Vcliente = nombres.Text & " " & apellidos.Text
        OpcionesVenta.Vmonto = Convert.ToString(total)
        OpcionesVenta.Show()
    End Sub

    Private Sub nit_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs)
        GenCliente.Show()
    End Sub

    Private Sub DataGridView1_KeyUp(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.F1 Then
            Try
                Dim cod As String = ""
                cod = DataGridView1.CurrentRow.Cells(0).Value
                If administrador = True Then
                    If MessageBox.Show("¿Realmente desea eliminar este Producto? Se eliminara todo registro de venta de este mismo", "Advertencia", MessageBoxButtons.YesNoCancel) = Windows.Forms.DialogResult.Yes Then
                        Dim c As conexion = New conexion
                        'c.insertar("delete from detalle_venta where id_venta=" + cod, "", False, 1)
                        'c.insertar("delete from venta where id_venta=" + cod, "Registro de venta Eliminado", False, 1)
                    End If
                End If

            Catch ex As Exception

            End Try

        End If
    End Sub
    Public Function CajaIniciada()
        Dim respuesta As Boolean = False
        Dim c As conexion = New conexion()
        Dim datos As DataTable = c.buscar("*", "caja", "fecha='" & Date.Today & "' and id_sede=" & id_sede)
        If datos.Rows.Count > 0 Then
            respuesta = True
        End If
        Return respuesta
    End Function
    Private Sub DeudasToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnotificacion_Click(sender As Object, e As EventArgs) Handles btnotificacion.Click
        notificaciones.show()
    End Sub

    Private Sub OtrosDiasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OtrosDiasToolStripMenuItem.Click
        VerCaja.Show()
    End Sub

    Private Sub CambiarCreditoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CambiarCreditoToolStripMenuItem.Click
        Clientes.Show()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

  
    Private Sub nit_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyData = Keys.Tab Then
            MessageBox.Show("Tab")
        End If
    End Sub

    Private Sub correo_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub telefono_KeyPress(sender As Object, e As KeyPressEventArgs)
        If (Asc(e.KeyChar)) = 13 Then
            correo.Select()
        End If
        If e.KeyChar = "-" Or e.KeyChar = " " Then

        Else
            If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub correo_KeyPress(sender As Object, e As KeyPressEventArgs)
        If (Asc(e.KeyChar)) = 13 Then
            limitecredito.Select()
        End If
    End Sub

    Private Sub limitecredito_KeyPress(sender As Object, e As KeyPressEventArgs)
        If (Asc(e.KeyChar)) = 13 Then
            TextBox4.Select()
        End If
        If e.KeyChar = "." Then

        Else
            If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub nit_KeyPress_1(sender As Object, e As KeyPressEventArgs) Handles nit.KeyPress
        If (Asc(e.KeyChar)) = 13 Then
            Dim c As conexion = New conexion()
            Try
                If Trim(nit.Text.ToLower()) = "c/f" Then
                    nombres.Text = "Consumidor Final"
                    apellidos.Text = "-"
                    nombres.Focus()
                Else
                    'si hubo nit entonces lo que hago es que busco el nit si existe relleno datos sino no
                    Dim datos As DataTable = c.buscar("*", "cliente", "nit='" & nit.Text & "'")
                    If datos.Rows.Count > 0 Then 'si hay nuevos
                        nombres.Text = Convert.ToString(datos.Rows(0).Item("nombre"))
                        apellidos.Text = Convert.ToString(datos.Rows(0).Item("apellido"))
                        telefono.Text = Convert.ToString(datos.Rows(0).Item("telefono"))
                        correo.Text = Convert.ToString(datos.Rows(0).Item("correo"))
                        limitecredito.Text = Convert.ToString(datos.Rows(0).Item("limite_credito"))
                        cliente_nuevo = False
                        id_cliente = Convert.ToString(datos.Rows(0).Item("id_cliente"))
                        nombres.Focus()
                    Else 'no hay nuevos
                        nombres.Focus()
                    End If
                End If

            Catch ex As Exception
                cliente_nuevo = True
                'nombres.Select()
            End Try
        End If
    End Sub

    Private Sub TextBox4_TextChanged_1(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        'Dim con As conexion = New conexion()
        ' ''le quite esta parte ",p.existencias as 'Existencias' "
        'If ComboBox1.Text = "Codigo" Then
        '    con.llenar_grid("select p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo',u.nombre as 'Medida'from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad and p.id_producto like '%" & TextBox4.Text & "%'", DataGridView1)
        'Else
        '    con.llenar_grid("select p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo',u.nombre as 'Medida'from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad and p." & ComboBox1.Text & " like '%" & TextBox4.Text & "%'", DataGridView1)
        'End If
    End Sub

    Private Sub DataGridView1_KeyDown_1(sender As Object, e As KeyEventArgs) Handles DataGridView1.KeyDown
        If (e.KeyCode = Keys.Enter) Then
            e.Handled = True
        End If
    End Sub

    Private Sub DataGridView1_KeyPress_1(sender As Object, e As KeyPressEventArgs) Handles DataGridView1.KeyPress

        If (Asc(e.KeyChar)) = 13 Then
            codigo_producto = Convert.ToInt32(DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(0).Value)
            Seleccion_Medida.Show()
        End If
    End Sub

    Private Sub DataGridView1_KeyUp_1(sender As Object, e As KeyEventArgs) Handles DataGridView1.KeyUp
       
    End Sub

    Private Sub DataGridView1_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick

    End Sub

    Private Sub DataGridView1_CellDoubleClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        codigo_producto = Convert.ToInt32(DataGridView1.CurrentRow.Cells(0).Value)
        Seleccion_Medida.Show()
    End Sub

    Private Sub Button7_Click_1(sender As Object, e As EventArgs) Handles Button7.Click
        GenCliente.Show()
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        'Dim con As conexion = New conexion()
        ''me vole ,p.existencias as 'Existencias'
        'con.llenar_grid("select p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo', u.nombre as 'Medida'from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad ", DataGridView1)
        'DataGridView1.RowHeadersWidth = 20
        'DataGridView1.Font = New Font("Arial ", 8, FontStyle.Regular)
        Dim con As conexion = New conexion()
        ' ''le quite esta parte ",p.existencias as 'Existencias' "
        If ComboBox1.Text = "Codigo" Then
            con.llenar_grid("select top 15 p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo', u.nombre as 'Medida'from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad and p.id_producto like '%" & TextBox4.Text & "%' and p.id_ubicacion=" + lista_ubicaciones.ElementAt(cmbUbicacion.SelectedIndex), DataGridView1)
            'con.llenar_grid("select p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo',u.nombre as 'Medida'from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad and p.id_producto like '%" & TextBox4.Text & "%'", DataGridView1)
        Else
            con.llenar_grid("select top 15 p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo', u.nombre as 'Medida'from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad and p." & ComboBox1.Text & " like '%" & TextBox4.Text & "%' and p.id_ubicacion=" + lista_ubicaciones.ElementAt(cmbUbicacion.SelectedIndex), DataGridView1)
            'con.llenar_grid("select p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo',u.nombre as 'Medida'from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad and p." & ComboBox1.Text & " like '%" & TextBox4.Text & "%'", DataGridView1)
        End If
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim c As conexion = New conexion()
            total = Math.Round(Convert.ToDouble(total - Convert.ToDouble(c.buscar("precio_venta", "presentacion", "id_presentacion=" & lista_codigos_presentacion.ElementAt(ListBox1.SelectedIndex)).Rows(0).Item("precio_venta"))), 2)
            lbltotal.Text = "Q.  " & total
            valtotal()
            lista_codigos_presentacion.RemoveAt(ListBox1.SelectedIndex)
            ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        'probar()
        ListBox1.Items.Clear()
        lista_codigos_presentacion.Clear()
        total = 0
        lbltotal.Text = "Q.  0.00"
        valtotal()
    End Sub

    Private Sub Button6_Click_1(sender As Object, e As EventArgs) Handles Button6.Click
        OpcionesVenta.Vcliente = nombres.Text & " " & apellidos.Text
        OpcionesVenta.Vmonto = Convert.ToString(total)
        OpcionesVenta.Show()
    End Sub

    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        Dim verificar As Boolean = False
        Try
            Dim p As Double = Convert.ToDouble(pago.Text)
            verificar = True
        Catch ex As Exception
            verificar = False
        End Try
        'aqui van a ver multiples procesos
        '(1) Verificar que la venta se pueda hacer 
        'If numitems() > 8 Then
        '    MessageBox.Show("Venta no puede superar 8 productos diferentes")
        'Else
        If verificar And CajaIniciada() = True Then
            If Not nit.Text.Trim = "" And Not nombres.Text.Trim = "" And Not apellidos.Text.Trim = "" And Not pago.Text.Trim = "" Then
                If Not ListBox1.Items.Count = 0 Then
                    Dim tottal As String = ""
                    If desc = 0 And aument = 0 And Vdesc = 0 And Vaument = 0 Then
                        tottal = total
                    Else
                        tottal = totalNetoNeto
                    End If
                    If Not Math.Round(Convert.ToDouble(Convert.ToDouble(pago.Text.Trim)), 2) > Math.Round(Convert.ToDouble(tottal), 2) Then
                        If combovendedor.Text.Trim() <> "" Then

                            Dim c As conexion = New conexion()
                            ''esto solo es para poner el total con aumentos y todo

                            ''este procedimiento se valida el limite de credito del cliente si sobrepaso o no
                            ''se le pregunta al usuario si desea continuar con bandera banderapasa
                            Dim banderapasa As Boolean = False

                            If limitecredito.Text > 0 Then
                                If cliente_nuevo Then
                                    If Math.Round(Convert.ToDouble(tottal), 2) > Math.Round(Convert.ToDouble(limitecredito.Text), 2) Then
                                        If MessageBox.Show("Valor sobrepasa limite de credito ¿desea continuar?", "Advertencia", MessageBoxButtons.YesNoCancel) = Windows.Forms.DialogResult.Yes Then
                                            banderapasa = True
                                        End If
                                    Else
                                        banderapasa = True
                                    End If
                                Else
                                    Dim dt As DataTable = c.buscar("SUM(total_venta-total_abonado) as Deuda", "venta", "id_cliente=" & id_cliente & " and total_abonado < total_venta")
                                    If IsDBNull(dt.Rows(0).Item("Deuda")) Then
                                        banderapasa = True
                                    Else
                                        Dim va As Double = Convert.ToDouble(dt.Rows(0).Item("Deuda")) + tottal - Convert.ToDouble(pago.Text.Trim)
                                        If Math.Round(Convert.ToDouble(va), 2) > (Math.Round(Convert.ToDouble(Convert.ToDouble(limitecredito.Text)), 2)) Then
                                            Dim cad As String = "Con esta venta el cliente sobrepasa su limite de credito. "
                                            cad = cad & vbNewLine
                                            cad = cad & "Deuda:   Q." & Math.Round(Convert.ToDouble(dt.Rows(0).Item("Deuda")), 2)
                                            cad = cad & vbNewLine
                                            cad = cad & "Total:    Q." & Math.Round(Convert.ToDouble(va), 2)
                                            cad = cad & vbNewLine
                                            cad = cad & "Credito: Q. " & limitecredito.Text.Trim
                                            cad = cad & vbNewLine
                                            cad = cad & "¿Desea continuar de todas formas?"
                                            If MessageBox.Show(cad, "Advertencia", MessageBoxButtons.YesNoCancel) = Windows.Forms.DialogResult.Yes Then
                                                banderapasa = True
                                            End If
                                        ElseIf Math.Round(Convert.ToDouble((va)), 2) = Math.Round((Convert.ToDouble(limitecredito.Text)), 2) Then
                                            Dim cad As String = "Con esta venta el cliente llego a su limite de credito."
                                            cad = cad & vbNewLine
                                            cad = cad & "Deuda:   Q." & Math.Round(Convert.ToDouble(dt.Rows(0).Item("Deuda")), 2)
                                            cad = cad & vbNewLine
                                            cad = cad & "Total:    Q." & Math.Round(Convert.ToDouble(va), 2)
                                            cad = cad & vbNewLine
                                            cad = cad & "Credito: Q. " & limitecredito.Text.Trim
                                            cad = cad & vbNewLine
                                            cad = cad & "¿Desea continuar de todas formas?"
                                            If MessageBox.Show(cad, "Advertencia", MessageBoxButtons.YesNoCancel) = Windows.Forms.DialogResult.Yes Then
                                                banderapasa = True
                                            End If
                                        Else
                                            banderapasa = True
                                        End If
                                    End If
                                End If
                            Else
                                banderapasa = True
                            End If
                            ''se continua si si paso la bandera
                            If Not MessageBox.Show("Esta venta se atribuira a [" + combovendedor.Text + "] ¿es correcto?", "Advertencia de Vendedor", MessageBoxButtons.YesNoCancel) = Windows.Forms.DialogResult.Yes Then
                                banderapasa = False
                            End If
                            If banderapasa = True Then
                                '(2) Registrar la venta almacenando y usuario y cliente responsable 'asignar fechas

                                If cliente_nuevo Then 'esto es para el cliente nuevo
                                    If (nit.Text.ToLower().Trim.Equals("c/f")) Then
                                        c.insertar("insert into cliente values ('c/f','" & nombres.Text.Trim & "','" & apellidos.Text.Trim & "','" & telefono.Text.Trim & "','" & correo.Text.Trim & "'," & limitecredito.Text.Trim & "," & id_sede & ")", "", False, 0)
                                    Else 'la diferencia es que aqui va su nit
                                        c.insertar("insert into cliente values ('" & nit.Text.Trim & "','" & nombres.Text.Trim & "','" & apellidos.Text.Trim & "','" & telefono.Text.Trim & "','" & correo.Text.Trim & "'," & limitecredito.Text.Trim & "," & id_sede & ")", "", False, 0)
                                    End If
                                    id_cliente = c.buscar_max("MAX(id_cliente) as Codigo", "cliente").Rows(0).Item("Codigo")
                                Else
                                    c.insertar("update cliente set nombre='" & nombres.Text.Trim & "' , apellido='" & apellidos.Text.Trim & "', nit='" & nit.Text.Trim & "', telefono='" & telefono.Text.Trim & "', correo='" & correo.Text.Trim & "', limite_credito=" & limitecredito.Text.Trim & " where id_cliente=" & id_cliente, "", False, 0)
                                End If



                                ''de aqui para abajo esta bien
                                c.insertar("insert into venta values ('" & Date.Today & "'," & tottal & "," & pago.Text.Trim & ",0," & codigo_usuario_activo & "," & id_cliente & "," & desc & "," & lista_vendedores.ElementAt(combovendedor.SelectedIndex) & ",'" & fechaCredito & "'," & aument & ",'" & doc.Text.Trim() & "')", "", False, 0)

                                '(3) Registrar detalle de venta
                                Dim ganancia As Double = 0
                                Dim id_venta As String = c.buscar_max("MAX(id_venta) as Codigo", "venta").Rows(0).Item("Codigo")

                                If Convert.ToDouble(pago.Text.Trim) > 0 Then
                                    'registro el pago inicial
                                    c.insertar("insert into pago values (" & Convert.ToDouble(pago.Text.Trim) & ",'" & Date.Today & "'," & id_venta & ")", "", False, 0)
                                End If

                                For Each elemento As String In lista_codigos_presentacion 'para cada presentacion de productos

                                    Dim datos As DataTable = c.buscar("precio_venta, precio_compra", "presentacion", "id_presentacion=" & elemento)
                                    ganancia = ganancia + Convert.ToDouble((Convert.ToDouble(datos.Rows(0).Item("precio_venta")) - Convert.ToDouble(datos.Rows(0).Item("precio_compra"))))
                                    'procedimiento para debitar del tipo
                                    ganancia = Math.Round(Convert.ToDouble(ganancia), 2)
                                    c.insertar("insert into detalle_venta values (" & id_venta & "," & elemento & "," & Convert.ToDouble(datos.Rows(0).Item("precio_venta")) & "," & Convert.ToDouble(datos.Rows(0).Item("precio_compra")) & ")", "", False, 0)
                                    quitar_existencia(elemento)
                                Next
                                totalNetoNeto = Math.Round(Convert.ToDouble(totalNetoNeto), 2)
                                If totalNetoNeto = pago.Text.Trim Then
                                    c.insertar("update venta set total_ganancia=" & ganancia & " where id_venta=" & id_venta, "", False, 0)
                                End If
                                MsgBox("Venta registrada con exito")
                                ''Codigo condicion
                                Dim cond As String = ""
                                If fechaCredito <> "---" Then
                                    Dim f1 As Date = CDate(fechaCredito)
                                    Dim f2 As Date = CDate(Date.Today)
                                    MessageBox.Show(f2)
                                    Dim kk As String = Convert.ToString(DateDiff(DateInterval.Day, f2, f1))
                                    cond = kk & " dias"
                                End If
                                If desc > 0 Then
                                    cond = cond & " Desc " & desc & "%"
                                End If
                                If aument > 0 Then
                                    cond = cond & " Aum " & aument & "%"
                                End If
                                ''
                                'GenerarPDFCARPAINT(nit.Text.Trim, nombres.Text.Trim, apellidos.Text.Trim, id_venta, correo.Text.Trim, telefono.Text.Trim, combovendedor.Text.Trim, cond, doc.Text.Trim(), Math.Round(Convert.ToDouble(tottal), 2))
                                GenerarPDF(nit.Text.Trim, nombres.Text.Trim, apellidos.Text.Trim, id_venta, correo.Text.Trim, telefono.Text.Trim, combovendedor.Text.Trim, cond, tottal)
                                new_venta()
                                '(4) Generar documentos como factura o recibo de venta
                            End If

                        Else
                            MessageBox.Show("Seleccione alguna opcion en Vendedor")
                        End If
                    Else
                        MsgBox("Pago excede cantidad")
                    End If
                Else
                    MsgBox("No se han especificado productos")
                End If
            Else
                MsgBox("Debe rellenar los datos necesarios")

            End If

        Else
            MsgBox("Ingrese una cantidad valida en el pago, e inicie Caja antes de cualquier Operacion")
        End If

    End Sub

    Private Sub Button5_Click_1(sender As Object, e As EventArgs) Handles Button5.Click
        new_venta()
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbUbicacion.SelectedIndexChanged
        Dim con As conexion = New conexion()
        con.llenar_grid("select top 15 p.id_producto as 'Codigo',p.nombre as 'Nombre',p.marca as 'Marca',p.detalle as 'Detalle',t.nombre as 'Tipo', u.nombre as 'Medida'from producto p ,tipo t, unidad u where p.id_tipo =t.id_tipo and p.id_unidad =u.id_unidad and p.id_ubicacion=" + lista_ubicaciones.ElementAt(cmbUbicacion.SelectedIndex), DataGridView1)
        DataGridView1.RowHeadersWidth = 20
    End Sub
End Class