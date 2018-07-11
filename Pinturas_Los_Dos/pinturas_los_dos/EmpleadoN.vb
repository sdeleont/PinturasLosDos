Public Class EmpleadoN
    Public codigo As String = ""

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        If Trim(dpi.Text) = "" Or Trim(nombre.Text) = "" Or Trim(telefono.Text) = "" Then
            MessageBox.Show("El DPI, nombre y telefono no pueden ser campos vacios")
        Else
            Dim c As conexion = New conexion
            Dim salarioo As String = "0"
            Dim est As String = estado.Text
            If Trim(salario.Text) <> "" Then
                salarioo = salario.Text
            End If
            If est = "Inactivo" Then
                est = "0"
            ElseIf est = "Activo" Then
                est = "1"
            End If
            c.insertar("insert into empleado values('" + nombre.Text + "','" + dpi.Text + "','" + fechainicio.Value.Date + "','" + fechafinal.Value.Date + "'," + salarioo + ",'" + telefono.Text + "','" + correo.Text + "','" + est + "')", "Empleado Registrado", True, 1)
            limpiar()
        End If

    End Sub
    Sub limpiar()
        nombre.Text = ""
        dpi.Text = ""
        salario.Text = ""
        telefono.Text = ""
        correo.Text = ""
        comision.Text = ""
    End Sub

    Private Sub EmpleadoN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If codigo <> "" Then
            Dim c As conexion = New conexion
            Dim a As DataTable = c.buscar("*", "empleado", "id_empleado=" + codigo)
            dpi.Text = a.Rows(0).Item("Dpi")
            nombre.Text = a.Rows(0).Item("Nombre")
            fechainicio.Text = a.Rows(0).Item("FechaInicio")
            fechafinal.Text = a.Rows(0).Item("FechaFin")
            salario.Text = a.Rows(0).Item("Salario")
            telefono.Text = a.Rows(0).Item("telefono")
            correo.Text = a.Rows(0).Item("correo")
            comision.Text = a.Rows(0).Item("Comision")
            If a.Rows(0).Item("Activo") = "1" Then
                estado.Text = "Activo"
            ElseIf a.Rows(0).Item("Activo") = "0" Then
                estado.Text = "Inactivo"
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        If Trim(dpi.Text) = "" Or Trim(nombre.Text) = "" Or Trim(telefono.Text) = "" Then
            MessageBox.Show("El DPI, nombre y telefono no pueden ser campos vacios")
        Else
            Dim c As conexion = New conexion
            Dim salarioo As String = "0"
            Dim est As String = estado.Text
            If Trim(salario.Text) <> "" Then
                salarioo = salario.Text
            End If
            If est = "Inactivo" Then
                est = "0"
            ElseIf est = "Activo" Then
                est = "1"
            End If
            c.insertar("update empleado set Nombre='" + nombre.Text + "', Dpi='" + dpi.Text + "', FechaInicio='" + fechainicio.Value.Date + "', FechaFin='" + fechafinal.Value.Date + "', Salario=" + salarioo + ", telefono='" + telefono.Text + "',correo= '" + correo.Text + "', Activo='" + est + "' where id_empleado= " + codigo + ";", "Empleado Modificado", True, 1)
            limpiar()
            Me.Close()
        End If
    End Sub

    Private Sub dpi_TextChanged(sender As Object, e As EventArgs) Handles dpi.TextChanged
        
    End Sub

    Private Sub dpi_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dpi.KeyPress
      
            If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
                e.Handled = True
            End If
    End Sub

    Private Sub salario_KeyPress(sender As Object, e As KeyPressEventArgs) Handles salario.KeyPress
        If e.KeyChar = "." Then

        Else
            If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If Trim(dpi.Text) = "" Or Trim(nombre.Text) = "" Or Trim(telefono.Text) = "" Then
            MessageBox.Show("El DPI, nombre y telefono no pueden ser campos vacios")
        Else
            Dim c As conexion = New conexion
            Dim salarioo As String = "0"
            Dim est As String = estado.Text
            If Trim(salario.Text) <> "" Then
                salarioo = salario.Text
            End If
            If est = "Inactivo" Then
                est = "0"
            ElseIf est = "Activo" Then
                est = "1"
            End If
            c.insertar("insert into empleado values('" + nombre.Text + "','" + dpi.Text + "','" + fechainicio.Value.Date + "','" + fechafinal.Value.Date + "'," + salarioo + ",'" + telefono.Text + "','" + correo.Text + "','" + est + "','" + comision.Text + "'," & VentanaPrincipal.id_sede & ")", "Empleado Registrado", True, 1)
            limpiar()
            FormEmpleados.cargar()
        End If


    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click

        If Trim(dpi.Text) = "" Or Trim(nombre.Text) = "" Or Trim(telefono.Text) = "" Then
            MessageBox.Show("El DPI, nombre y telefono no pueden ser campos vacios")
        Else
            Dim c As conexion = New conexion
            Dim salarioo As String = "0"
            Dim est As String = estado.Text
            If Trim(salario.Text) <> "" Then
                salarioo = salario.Text
            End If
            If est = "Inactivo" Then
                est = "0"
            ElseIf est = "Activo" Then
                est = "1"
            End If
            c.insertar("update empleado set Nombre='" + nombre.Text + "', Dpi='" + dpi.Text + "', FechaInicio='" + fechainicio.Value.Date + "', FechaFin='" + fechafinal.Value.Date + "', Salario=" + salarioo + ", telefono='" + telefono.Text + "',correo= '" + correo.Text + "', Activo='" + est + "', Comision='" + comision.Text.Trim() + "' where id_empleado= " + codigo + ";", "Empleado Modificado", True, 1)
            limpiar()
            FormEmpleados.cargar()
            Me.Close()
        End If
    End Sub

    Private Sub salario_TextChanged(sender As Object, e As EventArgs) Handles salario.TextChanged

    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles comision.KeyPress
        If e.KeyChar = "." Then

        Else
            If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub total_Click(sender As Object, e As EventArgs) Handles total.Click
        Dim fecha As String = DateTimePicker1.Value.Date
        Dim multiplicador As Double = Convert.ToDouble(comision.Text) / 100
        Dim c As conexion = New conexion()
        If RadioButton1.Checked = True Then
            Dim dt As DataTable = c.buscar("SUM(total_venta)*" & multiplicador & " as 'Total' ", "venta", "id_empleado =" & codigo & " and fecha = '" & DateTimePicker1.Value.Date & "'")
            If Not IsDBNull(dt.Rows(0).Item("Total")) Then
                lbltotal.Text = "Q. " & Math.Round(Convert.ToDouble(dt.Rows(0).Item("Total")), 2)
            Else
                lbltotal.Text = "Q. 0.00"
            End If
        ElseIf RadioButton2.Checked = True Then
            Dim dt As DataTable = c.buscar("SUM(total_venta)*" & multiplicador & " as 'Total' ", "venta", "id_empleado =" & codigo & " and fecha like '__/" & DateTimePicker1.Value.ToString("MM") & "/" & DateTimePicker1.Value.ToString("yyyy") & "'")
            If Not IsDBNull(dt.Rows(0).Item("Total")) Then
                lbltotal.Text = "Q. " & Math.Round(Convert.ToDouble(dt.Rows(0).Item("Total")), 2)
            Else
                lbltotal.Text = "Q. 0.00"
            End If
        ElseIf RadioButton3.Checked = True Then
            Dim dt As DataTable = c.buscar("SUM(total_venta)*" & multiplicador & " as 'Total' ", "venta", "id_empleado =" & codigo & " and fecha like '__/__/" & DateTimePicker1.Value.ToString("yyyy") & "'")
            If Not IsDBNull(dt.Rows(0).Item("Total")) Then
                lbltotal.Text = "Q. " & Math.Round(Convert.ToDouble(dt.Rows(0).Item("Total")), 2)
            Else
                lbltotal.Text = "Q. 0.00"
            End If
        End If

    End Sub

    Private Sub comision_TextChanged(sender As Object, e As EventArgs) Handles comision.TextChanged

    End Sub
End Class