Public Class Notificaciones

    Private Sub Notificaciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim tipo As New DataColumn("Tipo")
        tipo.DataType = GetType(String)
        Dim detalles As New DataColumn("Detalles")
        detalles.DataType = GetType(String)


        Dim datos As DataTable = New DataTable("Notificaciones")
        datos.Columns.Add(tipo)
        datos.Columns.Add(detalles)

        For i As Integer = 0 To VentanaPrincipal.notificacioness.Count - 1
            datos.Rows.Add(VentanaPrincipal.notificacioness.ElementAt(i).tipo, VentanaPrincipal.notificacioness.ElementAt(i).texto)
        Next
        
        DataGridView1.DataSource = datos
        DataGridView1.Refresh()
        DataGridView1.Columns(0).Width = 150
        DataGridView1.Columns(1).Width = 800

    End Sub
End Class