Imports System.IO
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Public Class form_ventas
    Public Overridable Property GroupName As String
    Public monto_total As Double = 0
    Public monto_cancelado As Double = 0
    Public monto_deuda As Double = 0
    Public ganancia As Double = 0
    Public listacampos As List(Of String) = New List(Of String)

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs)
        MsgBox("Activado")
        TextBox1.Text = DateTimePicker1.Value.Date
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)


    End Sub

    Private Sub form_ventas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Text = "Fecha"
        If Not VentanaPrincipal.administrador Then
            PictureBox4.Visible = False
            lblganancia.Visible = False
            Label4.Visible = False
        End If
        RadioButton1.Checked = True
        RadioButton6.Checked = True

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.Text = "Nombre Cliente" Then
            TextBox1.Visible = True
            Label2.Text = "Nombre Cliente"

            Label2.Visible = True
            Label3.Visible = False
            DateTimePicker1.Visible = False
            RadioButton1.Visible = False
            RadioButton2.Visible = False
            RadioButton3.Visible = False
        ElseIf ComboBox1.Text = "Nit Cliente" Then
            TextBox1.Visible = True
            Label2.Text = "Nit Cliente"

            Label2.Visible = True
            Label3.Visible = False
            DateTimePicker1.Visible = False
            RadioButton1.Visible = False
            RadioButton2.Visible = False
            RadioButton3.Visible = False
        ElseIf ComboBox1.Text = "Usuario" Then
            TextBox1.Visible = True
            Label2.Text = "Usuario"

            Label2.Visible = True
            Label3.Visible = False
            DateTimePicker1.Visible = False
            RadioButton1.Visible = False
            RadioButton2.Visible = False
            RadioButton3.Visible = False
        ElseIf ComboBox1.Text = "Vendedor" Then
            TextBox1.Visible = True
            Label2.Text = "Vendedor"

            Label2.Visible = True
            Label3.Visible = False
            DateTimePicker1.Visible = True
            RadioButton1.Visible = True
            RadioButton2.Visible = True
            RadioButton3.Visible = True
        ElseIf ComboBox1.Text = "Fecha" Then
            TextBox1.Visible = False

            Label2.Visible = False
            Label3.Visible = True
            DateTimePicker1.Visible = True
            RadioButton1.Visible = True
            RadioButton2.Visible = True
            RadioButton3.Visible = True
        ElseIf ComboBox1.Text = "Documento" Then
            TextBox1.Visible = True
            Label2.Text = "Documento"

            Label2.Visible = True
            Label3.Visible = False
            DateTimePicker1.Visible = False
            RadioButton1.Visible = False
            RadioButton2.Visible = False
            RadioButton3.Visible = False
        ElseIf ComboBox1.Text = "Cod. Venta" Then
            TextBox1.Visible = True
            Label2.Text = "Codigo"

            Label2.Visible = True
            Label3.Visible = False
            DateTimePicker1.Visible = False
            RadioButton1.Visible = False
            RadioButton2.Visible = False
            RadioButton3.Visible = False
        End If

    End Sub
    Public Sub Reporte(ByVal tam As Integer)



        Dim pgSize As New iTextSharp.text.Rectangle(800, 800)
        Dim documentoPDF As New Document(PageSize.LETTER, 36, 36, 36, 36)

        PdfWriter.GetInstance(documentoPDF,
            New FileStream("Reporte.pdf", FileMode.Create))
        documentoPDF.Open()
        Dim tabla As New PdfPTable(listacampos.Count())
        Dim t As Integer = tam
        documentoPDF.Add(New Paragraph("Reporte", FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
        ''la lista campos te dice que items son entonces
        Dim objeto As DatosReporte = GETDATOS()
        For i As Integer = 0 To listacampos.Count - 1
            Dim celda As New PdfPCell(New Phrase(GetItem(listacampos.ElementAt(i)), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
            celda.Border = 0
            tabla.AddCell(celda)
        Next

        Dim tope As Integer = objeto.datos.Rows.Count - 1
        For j As Integer = 0 To tope
            For i As Integer = 0 To listacampos.Count - 1
                Dim celda As New PdfPCell(New Phrase(objeto.datos.Rows(j).Item(GetItem(listacampos.ElementAt(i))), FontFactory.GetFont(FontFactory.TIMES, t, iTextSharp.text.Font.NORMAL)))
                celda.Border = 0
                tabla.AddCell(celda)
            Next
        Next
        documentoPDF.Add(tabla)
        documentoPDF.Close()
        If System.IO.File.Exists("Reporte.pdf") Then
            If MsgBox("Se ha creado un PDF" + _
                   "¿desea abrir el fichero PDF ?",
                   MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                'Abrimos el fichero PDF con la aplicación asociada
                System.Diagnostics.Process.Start("Reporte.pdf")
            End If
        End If
    End Sub
    Function GetItem(ByVal campo As String)
        If campo = "v.id_venta as 'Codigo'" Then
            Return "Codigo"
        ElseIf campo = "v.Documento as 'Documento'" Then
            Return "Documento"
        ElseIf campo = "u.nombre as 'Nombre Usuario'" Then
            Return "Nombre Usuario"
        ElseIf campo = "e.nombre as 'Vendedor'" Then
            Return "Vendedor"
        ElseIf campo = "v.fecha as 'Fecha'" Then
            Return "Fecha"
        ElseIf campo = "c.nombre as 'Nombre Cliente'" Then
            Return "Nombre Cliente"
        ElseIf campo = "c.nit as 'Nit'" Then
            Return "Nit"
        ElseIf campo = "c.telefono as 'Telefono'" Then
            Return "Telefono"
        ElseIf campo = "v.total_venta as 'Monto venta'" Then
            Return "Monto venta"
        ElseIf campo = "v.total_abonado as 'Monto Pagado'" Then
            Return "Monto Pagado"
        ElseIf campo = "v.total_ganancia as 'Monto Ganancia'" Then
            Return "Monto Ganancia"
        ElseIf campo = "v.fecha_credito as 'Fecha Credito'" Then
            Return "Fecha Credito"
        ElseIf campo = "v.aumento as '% Aumento'" Then
            Return "% Aumento"
        ElseIf campo = "v.descuento as '% Descuento'" Then
            Return "% Descuento"
        End If
        Return "NoExiste"
    End Function

    Public Function GETDATOS()
        Dim c As conexion = New conexion()
        Dim campos As String = listacampos.ElementAt(0)
        Dim tablas As String = "venta v,usuario u,cliente c, empleado e"
        Dim condicion As String = ""
        Dim datos As DataTable = New DataTable()
        If listacampos.Count > 1 Then
            For i As Integer = 1 To listacampos.Count - 1
                campos = campos & ", " & listacampos.ElementAt(i)
            Next
        End If

        If ComboBox1.Text = "Nombre Cliente" Then
            condicion = "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and c.nombre like '%" & TextBox1.Text & "%'"
            datos = c.buscar(campos, tablas, condicion)
            Try
                monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                monto_deuda = monto_total - monto_cancelado
                If VentanaPrincipal.administrador Then
                    ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                End If

            Catch ex As Exception

            End Try
        ElseIf ComboBox1.Text = "Nit Cliente" Then
            condicion = "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and c.nit like '%" & TextBox1.Text & "%'"
            datos = c.buscar(campos, tablas, condicion)
            Try
                monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                monto_deuda = monto_total - monto_cancelado
                If VentanaPrincipal.administrador Then
                    ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                End If

            Catch ex As Exception

            End Try
        ElseIf ComboBox1.Text = "Usuario" Then
            condicion = "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and u.nombre like '%" & TextBox1.Text & "%'"
            datos = c.buscar(campos, tablas, condicion)
            Try
                monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                monto_deuda = monto_total - monto_cancelado
                If VentanaPrincipal.administrador Then
                    ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                End If

            Catch ex As Exception

            End Try
        ElseIf ComboBox1.Text = "Vendedor" Then
            condicion = "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and e.nombre like '%" & TextBox1.Text & "%'"
            datos = c.buscar(campos, tablas, condicion)
            Try
                monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                monto_deuda = monto_total - monto_cancelado
                If VentanaPrincipal.administrador Then
                    ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                End If

            Catch ex As Exception

            End Try
        ElseIf ComboBox1.Text = "Documento" Then
            condicion = "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.Documento like '%" & TextBox1.Text & "%'"
            datos = c.buscar(campos, tablas, condicion)
            Try
                monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                monto_deuda = monto_total - monto_cancelado
                If VentanaPrincipal.administrador Then
                    ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                End If

            Catch ex As Exception

            End Try
        ElseIf ComboBox1.Text = "Cod. Venta" Then
            condicion = "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.id_venta = " & TextBox1.Text
            datos = c.buscar(campos, tablas, condicion)
            Try
                monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                monto_deuda = monto_total - monto_cancelado
                If VentanaPrincipal.administrador Then
                    ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                End If

            Catch ex As Exception

            End Try
        ElseIf ComboBox1.Text = "Fecha" Then
            If RadioButton1.Checked Then
                condicion = "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha ='" & DateTimePicker1.Value.Date & "'"
                Try
                    monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                    monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                    monto_deuda = monto_total - monto_cancelado
                    If VentanaPrincipal.administrador Then
                        ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                    End If

                Catch ex As Exception

                End Try


            ElseIf RadioButton2.Checked Then
                condicion = "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha like '__/" & DateTimePicker1.Value.ToString("MM") & "/" & DateTimePicker1.Value.ToString("yyyy") & "'"
                Try
                    monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                    monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                    monto_deuda = monto_total - monto_cancelado
                    If VentanaPrincipal.administrador Then
                        ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                    End If

                Catch ex As Exception

                End Try


            ElseIf RadioButton3.Checked Then
                condicion = "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha like '__/__/" & DateTimePicker1.Value.ToString("yyyy") & "'"
                Try
                    monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                    monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                    monto_deuda = monto_total - monto_cancelado
                    If VentanaPrincipal.administrador Then
                        ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", tablas, condicion).Rows(0).Item("Suma"))
                    End If

                Catch ex As Exception

                End Try
            End If
            datos = c.buscar(campos, tablas, condicion)
        End If
        Dim ret As DatosReporte = New DatosReporte()
        ret.datos = datos
        ret.total = monto_total
        ret.cancelado = monto_cancelado
        ret.deuda = monto_deuda
        ret.ganancia = ganancia

        Return ret
    End Function
    Public Sub busc()
        Dim c As conexion = New conexion()
        Dim consulta As String
        consulta = "select v.id_venta as 'Codigo',v.Documento as 'Documento', u.nombre as 'Nombre Usuario',e.nombre as 'Vendedor', v.fecha as 'Fecha', c.nombre as 'Nombre Cliente', c.nit as 'Nit',c.telefono as 'Telefono' , v.total_venta as 'Monto venta', "

        If VentanaPrincipal.administrador Then
            consulta = consulta & "v.total_abonado as 'Monto Pagado', v.total_ganancia as 'Monto Ganancia', v.fecha_credito as 'Fecha Credito', v.aumento as '% Aumento', v.descuento as '% Descuento' "
        Else
            consulta = consulta & "v.total_abonado as 'Monto Pagado', v.fecha_credito as 'Fecha Credito', v.aumento as '% Aumento', v.descuento as '% Descuento' "
        End If


        If ComboBox1.Text = "Nombre Cliente" Then
            consulta = consulta & "from venta v,usuario u,cliente c, empleado e where v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and c.nombre like '%" & TextBox1.Text & "%'"
            c.llenar_grid(consulta, DataGridView1)
            Try
                monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and c.nombre like '%" & TextBox1.Text & "%'").Rows(0).Item("Suma"))
                monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and c.nombre like '%" & TextBox1.Text & "%'").Rows(0).Item("Suma"))
                monto_deuda = monto_total - monto_cancelado
                If VentanaPrincipal.administrador Then
                    ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and c.nombre like '%" & TextBox1.Text & "%'").Rows(0).Item("Suma"))
                End If

            Catch ex As Exception

            End Try

        ElseIf ComboBox1.Text = "Nit Cliente" Then
            consulta = consulta & "from venta v,usuario u,cliente c, empleado e where v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and c.nit like '%" & TextBox1.Text & "%'"
            c.llenar_grid(consulta, DataGridView1)
            Try
                monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and c.nit like '%" & TextBox1.Text & "%'").Rows(0).Item("Suma"))
                monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and c.nit like '%" & TextBox1.Text & "%'").Rows(0).Item("Suma"))
                monto_deuda = monto_total - monto_cancelado
                If VentanaPrincipal.administrador Then
                    ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and c.nit like '%" & TextBox1.Text & "%'").Rows(0).Item("Suma"))
                End If

            Catch ex As Exception

            End Try
        ElseIf ComboBox1.Text = "Usuario" Then
            consulta = consulta & "from venta v,usuario u,cliente c, empleado e where v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and u.nombre like '%" & TextBox1.Text & "%'"
            c.llenar_grid(consulta, DataGridView1)
            Try
                monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and u.nombre like '%" & TextBox1.Text & "%'").Rows(0).Item("Suma"))
                monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and u.nombre like '%" & TextBox1.Text & "%'").Rows(0).Item("Suma"))
                monto_deuda = monto_total - monto_cancelado
                If VentanaPrincipal.administrador Then
                    ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and u.nombre like '%" & TextBox1.Text & "%'").Rows(0).Item("Suma"))
                End If

            Catch ex As Exception

            End Try
        ElseIf ComboBox1.Text = "Vendedor" Then
            'consulta = consulta & "from venta v,usuario u,cliente c, empleado e where v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and e.nombre like '%" & TextBox1.Text & "%'"
            'c.llenar_grid(consulta, DataGridView1)
            'Try
            '    monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and e.nombre like '%" & TextBox1.Text & "%'").Rows(0).Item("Suma"))
            '    monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and e.nombre like '%" & TextBox1.Text & "%'").Rows(0).Item("Suma"))
            '    monto_deuda = monto_total - monto_cancelado
            '    If VentanaPrincipal.administrador Then
            '        ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and e.nombre like '%" & TextBox1.Text & "%'").Rows(0).Item("Suma"))
            '    End If

            'Catch ex As Exception

            'End Try
            If RadioButton1.Checked Then
                consulta = consulta & "from venta v,usuario u,cliente c, empleado e where v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and e.nombre like '%" & TextBox1.Text & "%'" + " and v.fecha ='" & DateTimePicker1.Value.Date & "'"
                Try
                    monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and e.nombre like '%" & TextBox1.Text & "%'" + " and v.fecha ='" & DateTimePicker1.Value.Date & "'").Rows(0).Item("Suma"))
                    monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and e.nombre like '%" & TextBox1.Text & "%'" + " and v.fecha ='" & DateTimePicker1.Value.Date & "'").Rows(0).Item("Suma"))
                    monto_deuda = monto_total - monto_cancelado
                    If VentanaPrincipal.administrador Then
                        ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and e.nombre like '%" & TextBox1.Text & "%'" + " and v.fecha ='" & DateTimePicker1.Value.Date & "'").Rows(0).Item("Suma"))
                    End If

                Catch ex As Exception
                End Try


            ElseIf RadioButton2.Checked Then
                consulta = consulta & "from venta v,usuario u,cliente c, empleado e where v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and e.nombre like '%" & TextBox1.Text & "%'" + " and v.fecha like '__/" & DateTimePicker1.Value.ToString("MM") & "/" & DateTimePicker1.Value.ToString("yyyy") & "'"
                Try
                    ' MessageBox.Show("c.nit =v.nit and v.id_usuario = u.id_usuario and v.fecha like '__/" & DateTimePicker1.Value.ToString("MM") & "/" & DateTimePicker1.Value.ToString("yyyy") & "'")
                    monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and e.nombre like '%" & TextBox1.Text & "%'" + " and v.fecha like '__/" & DateTimePicker1.Value.ToString("MM") & "/" & DateTimePicker1.Value.ToString("yyyy") & "'").Rows(0).Item("Suma"))
                    monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and e.nombre like '%" & TextBox1.Text & "%'" + " and v.fecha like '__/" & DateTimePicker1.Value.ToString("MM") & "/" & DateTimePicker1.Value.ToString("yyyy") & "'").Rows(0).Item("Suma"))
                    monto_deuda = monto_total - monto_cancelado
                    If VentanaPrincipal.administrador Then
                        ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and e.nombre like '%" & TextBox1.Text & "%'" + " and v.fecha like '__/" & DateTimePicker1.Value.ToString("MM") & "/" & DateTimePicker1.Value.ToString("yyyy") & "'").Rows(0).Item("Suma"))
                    End If

                Catch ex As Exception

                End Try


            ElseIf RadioButton3.Checked Then
                consulta = consulta & "from venta v,usuario u,cliente c, empleado e where v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and e.nombre like '%" & TextBox1.Text & "%'" + " and v.fecha like '__/__/" & DateTimePicker1.Value.ToString("yyyy") & "'"
                Try
                    monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and e.nombre like '%" & TextBox1.Text & "%'" + " and v.fecha like '__/__/" & DateTimePicker1.Value.ToString("yyyy") & "'").Rows(0).Item("Suma"))
                    monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and e.nombre like '%" & TextBox1.Text & "%'" + " and v.fecha like '__/__/" & DateTimePicker1.Value.ToString("yyyy") & "'").Rows(0).Item("Suma"))
                    monto_deuda = monto_total - monto_cancelado
                    If VentanaPrincipal.administrador Then
                        ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and e.nombre like '%" & TextBox1.Text & "%'" + " and v.fecha like '__/__/" & DateTimePicker1.Value.ToString("yyyy") & "'").Rows(0).Item("Suma"))
                    End If

                Catch ex As Exception

                End Try


            End If
            c.llenar_grid(consulta, DataGridView1)
        ElseIf ComboBox1.Text = "Documento" Then
            consulta = consulta & "from venta v,usuario u,cliente c, empleado e where v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.Documento like '%" & TextBox1.Text & "%'"
            c.llenar_grid(consulta, DataGridView1)
            Try
                monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.Documento like '%" & TextBox1.Text & "%'").Rows(0).Item("Suma"))
                monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.Documento like '%" & TextBox1.Text & "%'").Rows(0).Item("Suma"))
                monto_deuda = monto_total - monto_cancelado
                If VentanaPrincipal.administrador Then
                    ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.Documento like '%" & TextBox1.Text & "%'").Rows(0).Item("Suma"))
                End If

            Catch ex As Exception

            End Try
        ElseIf ComboBox1.Text = "Cod. Venta" Then
            consulta = consulta & "from venta v,usuario u,cliente c, empleado e where v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.id_venta = " & TextBox1.Text
            c.llenar_grid(consulta, DataGridView1)
            Try
                monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.id_venta = " & TextBox1.Text).Rows(0).Item("Suma"))
                monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.id_venta = " & TextBox1.Text).Rows(0).Item("Suma"))
                monto_deuda = monto_total - monto_cancelado
                If VentanaPrincipal.administrador Then
                    ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.id_venta = " & TextBox1.Text).Rows(0).Item("Suma"))
                End If

            Catch ex As Exception

            End Try
        ElseIf ComboBox1.Text = "Fecha" Then
            If RadioButton1.Checked Then
                consulta = consulta & "from venta v,usuario u,cliente c, empleado e where v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha ='" & DateTimePicker1.Value.Date & "'"
                Try
                    monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha ='" & DateTimePicker1.Value.Date & "'").Rows(0).Item("Suma"))
                    monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha ='" & DateTimePicker1.Value.Date & "'").Rows(0).Item("Suma"))
                    monto_deuda = monto_total - monto_cancelado
                    If VentanaPrincipal.administrador Then
                        ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha ='" & DateTimePicker1.Value.Date & "'").Rows(0).Item("Suma"))
                    End If

                Catch ex As Exception
                End Try


            ElseIf RadioButton2.Checked Then
                consulta = consulta & "from venta v,usuario u,cliente c, empleado e where v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha like '__/" & DateTimePicker1.Value.ToString("MM") & "/" & DateTimePicker1.Value.ToString("yyyy") & "'"
                Try
                    ' MessageBox.Show("c.nit =v.nit and v.id_usuario = u.id_usuario and v.fecha like '__/" & DateTimePicker1.Value.ToString("MM") & "/" & DateTimePicker1.Value.ToString("yyyy") & "'")
                    monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha like '__/" & DateTimePicker1.Value.ToString("MM") & "/" & DateTimePicker1.Value.ToString("yyyy") & "'").Rows(0).Item("Suma"))
                    monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha like '__/" & DateTimePicker1.Value.ToString("MM") & "/" & DateTimePicker1.Value.ToString("yyyy") & "'").Rows(0).Item("Suma"))
                    monto_deuda = monto_total - monto_cancelado
                    If VentanaPrincipal.administrador Then
                        ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha like '__/" & DateTimePicker1.Value.ToString("MM") & "/" & DateTimePicker1.Value.ToString("yyyy") & "'").Rows(0).Item("Suma"))
                    End If

                Catch ex As Exception

                End Try


            ElseIf RadioButton3.Checked Then
                consulta = consulta & "from venta v,usuario u,cliente c, empleado e where v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha like '__/__/" & DateTimePicker1.Value.ToString("yyyy") & "'"
                Try
                    monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha like '__/__/" & DateTimePicker1.Value.ToString("yyyy") & "'").Rows(0).Item("Suma"))
                    monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha like '__/__/" & DateTimePicker1.Value.ToString("yyyy") & "'").Rows(0).Item("Suma"))
                    monto_deuda = monto_total - monto_cancelado
                    If VentanaPrincipal.administrador Then
                        ganancia = Convert.ToDouble(c.buscar("sum(v.total_ganancia ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha like '__/__/" & DateTimePicker1.Value.ToString("yyyy") & "'").Rows(0).Item("Suma"))
                    End If

                Catch ex As Exception

                End Try


            End If
            c.llenar_grid(consulta, DataGridView1)
        End If
        monto_total = Math.Round(Convert.ToDouble(monto_total), 2)
        monto_deuda = Math.Round(Convert.ToDouble(monto_deuda), 2)
        monto_cancelado = Math.Round(Convert.ToDouble(monto_cancelado), 2)
        ganancia = Math.Round(Convert.ToDouble(ganancia), 2)
        total.Text = "Q. " & monto_total
        deuda.Text = "Q. " & monto_deuda
        cancelado.Text = "Q. " & monto_cancelado
        If VentanaPrincipal.administrador Then
            lblganancia.Text = "Q. " & ganancia
            lblganancia.Visible = True
            PictureBox4.Visible = True
            Label4.Visible = True
        Else
            lblganancia.Visible = False
            PictureBox4.Visible = False
            Label4.Visible = False
        End If


        monto_total = 0.0
        monto_deuda = 0.0
        monto_cancelado = 0.0
        ganancia = 0.0
    End Sub
    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        busc()
    End Sub


    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Try
            form_detalle.codigo = DataGridView1.CurrentRow.Cells(0).Value
            form_detalle.Show()
            Me.Enabled = False
        Catch ex As Exception

        End Try

    End Sub



    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim c As conexion = New conexion()
        Try
            Dim datos As DataTable = c.buscar("*", "venta", "id_venta=" & DataGridView1.CurrentRow.Cells(0).Value)

            If Convert.ToDouble(datos.Rows(0).Item("total_venta")) > Convert.ToDouble(datos.Rows(0).Item("total_abonado")) Then
                Pago.Show()
                Me.Enabled = False
            Else
                MsgBox("Esta venta ya fue totalmente cancelada")
            End If

        Catch ex As Exception
            MsgBox("Seleccione una venta")
        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DataGridView1.KeyPress

    End Sub

    Private Sub DataGridView1_KeyUp(sender As Object, e As KeyEventArgs) Handles DataGridView1.KeyUp

        If e.KeyCode = Keys.F1 And VentanaPrincipal.administrador Then
            Try
                Dim cod As String = ""
                cod = DataGridView1.CurrentRow.Cells(0).Value
                If MessageBox.Show("¿Realmente desea anular esta venta?", "Advertencia", MessageBoxButtons.YesNoCancel) = Windows.Forms.DialogResult.Yes Then
                    Dim c As conexion = New conexion
                    Dim dato As DataTable = c.buscar("p.id_presentacion", "Presentacion p, detalle_venta d, venta v", "v.id_venta=d.id_venta and d.id_presentacion=p.id_presentacion and v.id_venta=" & cod)
                    If dato.Rows.Count > 0 Then
                        For t As Integer = 0 To dato.Rows.Count - 1
                            sumarExistencia(dato.Rows(0).Item("id_presentacion"))
                        Next
                    End If
                    c.insertar("delete from detalle_venta where id_venta=" + cod, "", False, 1)
                    c.insertar("delete from pago where id_venta=" & cod, "", False, 1)
                    c.insertar("delete from venta where id_venta=" + cod, "Registro de venta Eliminado", False, 1)

                End If
            Catch ex As Exception
                MessageBox.Show("Seleccione alguna venta")
            End Try
        ElseIf e.KeyCode = Keys.F2 Then
            Pago.ver = True
            Pago.Show()
            Me.Enabled = False
        End If
    End Sub
    Private Sub sumarExistencia(ByVal id As String)
        Dim c As conexion = New conexion()
        Dim id_tipo As String = c.buscar("pr.id_producto, t.id_tipo", "Presentacion p, Producto pr, tipo t", "p.id_producto =pr.id_producto and pr.id_tipo =t.id_tipo  and p.id_presentacion =" & id).Rows(0).Item("id_tipo")
        Dim medida_mas As String = c.buscar("m.cantidad , m.descripcion", "Presentacion p, Medida m", "p.id_medida =m.id_medida and p.id_presentacion =" & id).Rows(0).Item("cantidad")
        Dim nueva_cantidad As Double = Convert.ToDouble(c.buscar("existencias", "tipo", "id_tipo=" & id_tipo).Rows(0).Item("existencias")) + Convert.ToDouble(medida_mas)
        nueva_cantidad = Math.Round(Convert.ToDouble(nueva_cantidad), 2)
        c.insertar("update tipo set existencias =" & nueva_cantidad & " where id_tipo =" & id_tipo, "", False, 0)
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.Text = "Deudas Por Cliente" Then
            DateTimePicker2.Enabled = False
            RadioButton4.Enabled = False
            RadioButton5.Enabled = False
            RadioButton6.Enabled = False
            TextBox2.Enabled = True
            TextBox2.Text = ""
        ElseIf ComboBox2.Text = "Deudas Por Fecha" Then
            DateTimePicker2.Enabled = True
            RadioButton4.Enabled = True
            RadioButton5.Enabled = True
            RadioButton6.Enabled = True
            TextBox2.Enabled = False
            TextBox2.Text = ""
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim c As conexion = New conexion()
        Dim consulta As String
        consulta = "select v.id_venta as 'Codigo', v.Documento as 'Documento' , u.nombre as 'Nombre Usuario',e.nombre as 'Vendedor', v.fecha as 'Fecha', c.nombre as 'Nombre Cliente', c.nit as 'Nit',c.telefono as 'Telefono' , v.total_venta as 'Monto venta', "

        If VentanaPrincipal.administrador Then
            consulta = consulta & "v.total_abonado as 'Monto Pagado', v.total_ganancia as 'Monto Ganancia', v.fecha_credito as 'Fecha Credito', v.aumento as '% Aumento', v.descuento as '% Descuento' "
        Else
            consulta = consulta & "v.total_abonado as 'Monto Pagado', v.fecha_credito as 'Fecha Credito', v.aumento as '% Aumento', v.descuento as '% Descuento' "
        End If


        If ComboBox2.Text = "Deudas Por Cliente" Then
            consulta = consulta & "from venta v,usuario u,cliente c, empleado e where v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and c.nombre like '%" & TextBox2.Text & "%' and v.total_venta > v.total_abonado"
            c.llenar_grid(consulta, DataGridView1)
            'MessageBox.Show(consulta)
            Try
                monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and c.nombre like '%" & TextBox2.Text & "%' and v.total_venta > v.total_abonado").Rows(0).Item("Suma"))
                monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and c.nombre like '%" & TextBox2.Text & "%' and v.total_venta > v.total_abonado").Rows(0).Item("Suma"))
                monto_deuda = monto_total - monto_cancelado
            Catch ex As Exception

            End Try
        ElseIf ComboBox2.Text = "Deudas Por Fecha" Then
            If RadioButton6.Checked Then
                consulta = consulta & "from venta v,usuario u,cliente c, empleado e where v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha ='" & DateTimePicker2.Value.Date & "' and v.total_venta > v.total_abonado"
                c.llenar_grid(consulta, DataGridView1)
                Try
                    monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha ='" & DateTimePicker2.Value.Date & "' and v.total_venta > v.total_abonado").Rows(0).Item("Suma"))
                    monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha ='" & DateTimePicker2.Value.Date & "' and v.total_venta > v.total_abonado").Rows(0).Item("Suma"))
                    monto_deuda = monto_total - monto_cancelado
                Catch ex As Exception

                End Try
            ElseIf RadioButton5.Checked Then
                consulta = consulta & "from venta v,usuario u,cliente c, empleado e where v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha like '__/" & DateTimePicker2.Value.ToString("MM") & "/" & DateTimePicker2.Value.ToString("yyyy") & "' and v.total_venta > v.total_abonado"
                c.llenar_grid(consulta, DataGridView1)
                Try
                    monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha like '__/" & DateTimePicker2.Value.ToString("MM") & "/" & DateTimePicker2.Value.ToString("yyyy") & "' and v.total_venta > v.total_abonado").Rows(0).Item("Suma"))
                    monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha like '__/" & DateTimePicker2.Value.ToString("MM") & "/" & DateTimePicker2.Value.ToString("yyyy") & "' and v.total_venta > v.total_abonado").Rows(0).Item("Suma"))
                    monto_deuda = monto_total - monto_cancelado
                Catch ex As Exception

                End Try
            ElseIf RadioButton4.Checked Then
                consulta = consulta & "from venta v,usuario u,cliente c, empleado e where v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha like '__/__/" & DateTimePicker2.Value.ToString("yyyy") & "' and v.total_venta > v.total_abonado"
                c.llenar_grid(consulta, DataGridView1)
                Try
                    monto_total = Convert.ToDouble(c.buscar("sum(v.total_venta ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha like '__/__/" & DateTimePicker2.Value.ToString("yyyy") & "' and v.total_venta > v.total_abonado").Rows(0).Item("Suma"))
                    monto_cancelado = Convert.ToDouble(c.buscar("sum(v.total_abonado ) as 'Suma'", "venta v,usuario u,cliente c, empleado e", "v.id_empleado=e.id_empleado and c.id_cliente =v.id_cliente and v.id_usuario = u.id_usuario and v.fecha like '__/__/" & DateTimePicker2.Value.ToString("yyyy") & "' and v.total_venta > v.total_abonado").Rows(0).Item("Suma"))
                    monto_deuda = monto_total - monto_cancelado
                Catch ex As Exception

                End Try
            End If

        End If
        monto_deuda = Math.Round(Convert.ToDouble(monto_deuda), 2)
        lblvaltotal.Text = "Q. " & monto_deuda

        monto_total = 0.0
        monto_deuda = 0.0
        monto_cancelado = 0.0




    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            Dim c As conexion = New conexion()
            lbllimitecredito.Text = "Q. " & Convert.ToString(c.buscar("c.limite_credito as 'val'", "Cliente c, Venta v", "id_venta=" & DataGridView1.CurrentRow.Cells(0).Value & " and c.id_cliente=v.id_cliente").Rows(0).Item("val"))
        Catch ex As Exception

        End Try

    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If ComboBox1.Text = "Cod. Venta" Then

            If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
                e.Handled = True
            End If

        End If

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        OpReporte.Show()
        Me.Enabled = False
    End Sub
End Class