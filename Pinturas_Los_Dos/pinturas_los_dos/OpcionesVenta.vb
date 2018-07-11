Public Class OpcionesVenta
    Public Vcliente As String
    Public Vlimite As Double = 0
    Public Vmonto As Double = 0
    Public desc As Double = 0
    Public aument As Double = 0
    Public Vdesc As Double = 0
    Public Vaument As Double = 0
    Public fecha As String = "---"

    Public tNeto As Double
    Sub Actualizar()
        descuentoventa.Text = Convert.ToString(desc) + " %"
        aumentoventa.Text = Convert.ToString(aument) + " %"
        fechacredito.Text = fecha
        Dim val As Double
        val = Vmonto - Vdesc + Vaument
        val = Math.Round(Convert.ToDouble(val), 2)
        Totalneto.Text = Convert.ToString(val)
        tNeto = val
    End Sub
    Private Sub OpcionesVenta_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cliente.Text = Vcliente
        limite.Text = Convert.ToString(Vlimite)
        montoventa.Text = Convert.ToString(Vmonto)
        Totalneto.Text = Convert.ToString(Vmonto)

        tNeto = Vmonto
        VentanaPrincipal.Enabled = False
        desc = VentanaPrincipal.desc
        aument = VentanaPrincipal.aument
        Vdesc = VentanaPrincipal.Vdesc
        Vaument = VentanaPrincipal.Vaument
        fecha = VentanaPrincipal.fechaCredito
        Actualizar()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim valor As Double = Convert.ToDouble(porcentaje.Text) * Vmonto / 100 ''descuento
        valor = Math.Round(Convert.ToDouble(valor), 2)
        Vdesc = valor
        desc = porcentaje.Text
        porcentaje.Text = "0"
        Actualizar()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim valor As Double = Convert.ToDouble(porcentaje.Text) * Vmonto / 100 ''aumento
        valor = Math.Round(Convert.ToDouble(valor), 2)
        Vaument = valor
        aument = porcentaje.Text
        porcentaje.Text = "0"
        Actualizar()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        fecha = DateTimePicker1.Value.Date
        Actualizar()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Vdesc = 0
        desc = 0
        Actualizar()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Vaument = 0
        aument = 0
        Actualizar()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        fecha = "---"
        Actualizar()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If (desc > 0 And aument > 0) Then
            MessageBox.Show("No puede existir Aumento y Descuento al mismo tiempo")
        Else
            tNeto = Math.Round(Convert.ToDouble(tNeto), 2)
            VentanaPrincipal.totalNetoNeto = tNeto
            VentanaPrincipal.lbltotalneto.Text = "Q.  " + Convert.ToString(tNeto)

            VentanaPrincipal.lbldescaument.Text = "Desc/Aum"
            VentanaPrincipal.porcentaje.Text = "Q.  "
            VentanaPrincipal.Label19.Text = "%"
            VentanaPrincipal.lbldescaument.ForeColor = Color.Green
            VentanaPrincipal.Label19.ForeColor = Color.Green
            VentanaPrincipal.porcentaje.ForeColor = Color.Green
            VentanaPrincipal.lblfechacredito.Text = "---"
            VentanaPrincipal.fechaCredito = "---"
            Vdesc = Math.Round(Convert.ToDouble(Vdesc), 2)
            Vaument = Math.Round(Convert.ToDouble(Vaument), 2)
            If (desc > 0) Then
                VentanaPrincipal.lbldescaument.Text = "Descuento"
                VentanaPrincipal.porcentaje.Text = "Q.  " + Convert.ToString(Vdesc)
                VentanaPrincipal.Label19.Text = Convert.ToString(desc) + " %"
                VentanaPrincipal.lbldescaument.ForeColor = Color.Red
                VentanaPrincipal.Label19.ForeColor = Color.Red
                VentanaPrincipal.porcentaje.ForeColor = Color.Red
            ElseIf (aument > 0) Then
                VentanaPrincipal.lbldescaument.Text = "Aumento"
                VentanaPrincipal.porcentaje.Text = "Q.  " + Convert.ToString(Vaument)
                VentanaPrincipal.Label19.Text = Convert.ToString(aument) + " %"
                VentanaPrincipal.lbldescaument.ForeColor = Color.Green
                VentanaPrincipal.Label19.ForeColor = Color.Green
                VentanaPrincipal.porcentaje.ForeColor = Color.Green
            End If
            Vdesc = Math.Round(Convert.ToDouble(Vdesc), 2)
            Vaument = Math.Round(Convert.ToDouble(Vaument), 2)
            VentanaPrincipal.desc = desc
            VentanaPrincipal.Vdesc = Vdesc
            VentanaPrincipal.aument = aument
            VentanaPrincipal.Vaument = Vaument
            If fecha <> "---" Then
                VentanaPrincipal.lblfechacredito.Text = fecha
                VentanaPrincipal.fechaCredito = fecha
            End If
            VentanaPrincipal.Enabled = True
            Me.Close()
        End If
        


    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        VentanaPrincipal.Enabled = True
        Me.Close()
    End Sub

    Private Sub porcentaje_TextChanged(sender As Object, e As EventArgs) Handles porcentaje.TextChanged

    End Sub

    Private Sub porcentaje_KeyPress(sender As Object, e As KeyPressEventArgs) Handles porcentaje.KeyPress
        If e.KeyChar = "." Then

        Else
            If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
                e.Handled = True
            End If
        End If
    End Sub
End Class