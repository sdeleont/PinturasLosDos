Public Class VerCaja

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        cargar()
    End Sub

    Private Sub VerCaja_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cargar()
    End Sub
    Private Sub cargar()
        Dim c As conexion = New conexion()
        Dim dt As DataTable = c.buscar("*", "caja", "fecha='" & DateTimePicker1.Value.Date & "'")
        If dt.Rows.Count > 0 Then
            Label2.Text = "Inicio en Efectivo: Q." & dt.Rows(0).Item("InicioEfectivo")
            Label3.Text = "Cierre en Efectivo: Q." & dt.Rows(0).Item("FinEfectivo")
            Label4.Text = "Nota*:" & dt.Rows(0).Item("Nota")
        End If
    End Sub
End Class