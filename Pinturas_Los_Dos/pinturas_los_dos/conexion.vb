Imports System.Data.SqlClient
Public Class conexion
    Public Sub llenar_grid(ByVal consulta As String, ByVal grid As DataGridView)
        Try
            Dim con As New SqlConnection(Form1.cadena_conexion)
            con.Open()
            Dim cmd As New SqlCommand(consulta, con)
            cmd.CommandType = CommandType.Text
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            grid.DataSource = dt
            grid.Refresh()
        Catch ex As Exception
                MsgBox("no existe conexion" + ex.ToString())
        End Try
    End Sub
    Public Sub insertar(ByVal consulta As String, ByVal mensaje As String, ByVal msg As Boolean, ByVal op As Integer)
        

        Try
            Dim con As New SqlConnection(Form1.cadena_conexion)
            con.Open()
            Dim cmd_in As New SqlCommand(consulta, con)
            cmd_in.ExecuteNonQuery()
            If msg = True Then
                MsgBox(mensaje, vbInformation)
            End If
            con.Close()
        Catch ex As Exception
            MessageBox.Show("Error 2 ," & consulta & " ------------nuevo Error--------------" & ex.Message)
        End Try
    End Sub
    Public Function buscar(ByVal campos, ByVal tabla, ByVal condicion) As DataTable
        Dim dt As New DataTable
        Try
            Dim con As New SqlConnection(Form1.cadena_conexion)
            con.Open()
            Dim cmd As New SqlCommand("select " & campos & " from " & tabla & " where " & condicion, con)
            cmd.CommandType = CommandType.Text
            Dim da As New SqlDataAdapter(cmd)

            da.Fill(dt)
            con.Close()
        Catch ex As Exception
            MessageBox.Show(condicion)
        End Try
        
        Return dt
    End Function
    Public Function buscar_max(ByVal campos, ByVal tabla) As DataTable
        Dim con As New SqlConnection(Form1.cadena_conexion)
        con.Open()
        Dim cmd As New SqlCommand("select " & campos & " from " & tabla, con)
        cmd.CommandType = CommandType.Text
        Dim da As New SqlDataAdapter(cmd)
        Dim dt As New DataTable
        da.Fill(dt)

        con.Close()
        Return dt
    End Function

    Public Function verificar_contra(ByVal usuario As String, ByVal contra As String) As Boolean
        Dim res As Boolean = False

        Dim con As New SqlConnection(Form1.cadena_conexion)
        con.Open()
        Dim cmd As New SqlCommand("select * from usuario where usuario = '" & usuario & "' and contraseña = '" & contra & "'", con)
        cmd.CommandType = CommandType.Text
        Dim da As New SqlDataAdapter(cmd)
        Dim dt As New DataTable
        da.Fill(dt)
        Try
            If Convert.ToString(dt.Rows(0).Item("usuario")) = usuario Or Convert.ToString(dt.Rows(0).Item("contraseña")) = contra Then
                res = True
            End If
        Catch ex As Exception
            res = False
        End Try
        con.Close()
        Return res
    End Function
End Class
