Public Class OpReporte

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles todo.CheckedChanged
        If todo.Checked = True Then
            codigo.Checked = True
            documento.Checked = True
            usuario.Checked = True
            vendedor.Checked = True
            fechaventa.Checked = True
            nombre.Checked = True
            nit.Checked = True
            telefono.Checked = True
            total.Checked = True
            pagado.Checked = True
            ganancia.Checked = True
            credito.Checked = True
            aumento.Checked = True
            descuento.Checked = True
        Else
            codigo.Checked = False
            documento.Checked = False
            usuario.Checked = False
            vendedor.Checked = False
            fechaventa.Checked = False
            nombre.Checked = False
            nit.Checked = False
            telefono.Checked = False
            total.Checked = False
            pagado.Checked = False
            ganancia.Checked = False
            credito.Checked = False
            aumento.Checked = False
            descuento.Checked = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
       
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If codigo.Checked = True Then
            form_ventas.listacampos.Add("v.id_venta as 'Codigo'")
        End If
        If documento.Checked = True Then
            form_ventas.listacampos.Add("v.Documento as 'Documento'")
        End If
        If usuario.Checked = True Then
            form_ventas.listacampos.Add("u.nombre as 'Nombre Usuario'")
        End If
        If vendedor.Checked = True Then
            form_ventas.listacampos.Add("e.nombre as 'Vendedor'")
        End If
        If fechaventa.Checked = True Then
            form_ventas.listacampos.Add("v.fecha as 'Fecha'")
        End If
        If nombre.Checked = True Then
            form_ventas.listacampos.Add("c.nombre as 'Nombre Cliente'")
        End If
        If nit.Checked = True Then
            form_ventas.listacampos.Add("c.nit as 'Nit'")
        End If
        If telefono.Checked = True Then
            form_ventas.listacampos.Add("c.telefono as 'Telefono'")
        End If
        If total.Checked = True Then
            form_ventas.listacampos.Add("v.total_venta as 'Monto venta'")
        End If
        If pagado.Checked = True Then
            form_ventas.listacampos.Add("v.total_abonado as 'Monto Pagado'")
        End If
        If ganancia.Checked = True Then
            form_ventas.listacampos.Add("v.total_ganancia as 'Monto Ganancia'")
        End If
        If credito.Checked = True Then
            form_ventas.listacampos.Add("v.fecha_credito as 'Fecha Credito'")
        End If
        If aumento.Checked = True Then
            form_ventas.listacampos.Add("v.aumento as '% Aumento'")
        End If
        If descuento.Checked = True Then
            form_ventas.listacampos.Add("v.descuento as '% Descuento'")
        End If
        If TextBox1.Text.Trim() = "" Then
            TextBox1.Text = "10"
        End If
        form_ventas.Enabled = True
        form_ventas.Reporte(Convert.ToInt32(TextBox1.Text.Trim()))
        Me.Close()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        form_ventas.Enabled = True
        Me.Close()
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
            e.Handled = True
        End If
    End Sub

    Private Sub OpReporte_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class