Public Class FormEmpleados

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        EmpleadoN.Show()
        EmpleadoN.total.Enabled = False
        EmpleadoN.Button8.Enabled = False
    End Sub

    Private Sub FormEmpleados_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cargar()
    End Sub
    Public Sub cargar()
        Dim c As conexion = New conexion
        c.llenar_grid("select id_empleado as 'Codigo',Nombre,Dpi as 'Dpi',Fechainicio,Fechafin,Salario,Telefono,Correo,Activo as 'Estado', Comision as '% Comision' from empleado", DataGridView1)
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(i).Cells(8).Value = "1" Then
                DataGridView1.Rows(i).Cells(8).Value = "Activo"
            ElseIf DataGridView1.Rows(i).Cells(8).Value = "0" Then
                DataGridView1.Rows(i).Cells(8).Value = "InActivo"
            End If
        Next
    End Sub
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Dim codigo As String = DataGridView1.CurrentRow.Cells(0).Value
            EmpleadoN.codigo = codigo
            EmpleadoN.Show()
            EmpleadoN.Button9.Enabled = False
            EmpleadoN.total.Enabled = False
        Catch ex As Exception
            MsgBox("No se encuentra empleado seleccionado")
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Dim codigo As String = DataGridView1.CurrentRow.Cells(0).Value
            If MessageBox.Show("¿Realmente desea eliminar esta empleado?", "Advertencia", MessageBoxButtons.YesNoCancel) = Windows.Forms.DialogResult.Yes Then
                Dim c As conexion = New conexion
                c.insertar("delete from empleado where id_empleado=" + codigo, "Eliminado con exito", True, 1)
                cargar()
            End If

        Catch ex As Exception
            MsgBox("No se encuentra empleado seleccionado")
        End Try
    End Sub
    Public cc As conexion = New conexion()

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        cc.llenar_grid("select id_empleado as 'Codigo',Nombre,Dpi as 'Dpi',Fechainicio,Fechafin,Salario,Telefono,Correo,Activo as 'Estado' from empleado where Nombre like '%" + TextBox1.Text + "%'", DataGridView1)
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(i).Cells(8).Value = "1" Then
                DataGridView1.Rows(i).Cells(8).Value = "Activo"
            ElseIf DataGridView1.Rows(i).Cells(8).Value = "0" Then
                DataGridView1.Rows(i).Cells(8).Value = "InActivo"
            End If
        Next
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            Dim codigo As String = DataGridView1.CurrentRow.Cells(0).Value
            EmpleadoN.codigo = codigo
            EmpleadoN.Show()
            EmpleadoN.Button8.Enabled = False
            EmpleadoN.Button9.Enabled = False
            EmpleadoN.dpi.Enabled = False
            EmpleadoN.fechafinal.Enabled = False
            EmpleadoN.fechainicio.Enabled = False
            EmpleadoN.nombre.Enabled = False
            EmpleadoN.salario.Enabled = False
            EmpleadoN.telefono.Enabled = False
            EmpleadoN.correo.Enabled = False
            EmpleadoN.estado.Enabled = False
            EmpleadoN.comision.Enabled = False
        Catch ex As Exception
            MsgBox("No se encuentra empleado seleccionado")
        End Try
        

    End Sub
End Class