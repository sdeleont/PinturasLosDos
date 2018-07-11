Imports System.Data.SqlClient
Public Class medida

    Public Sub llenar_combo(ByVal combo As ComboBox, ByVal campo As String, ByVal tabla As String)
        Try
            Dim con As New SqlConnection(Form1.cadena_conexion)
            con.Open()
            Dim cmd As New SqlCommand("select " & campo & " from " & tabla, con)
            cmd.CommandType = CommandType.Text
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            For i = 0 To dt.Rows.Count - 1
                combo.Items.Add(dt.Rows(i).Item(campo))
            Next
            con.Close()
        Catch ex As Exception
            MsgBox("Error no existe conexión")
        End Try
    End Sub
    Public Function verificar_existencia(ByVal campo As String, ByVal tabla As String) As Boolean
        Dim respuesta As Boolean = True
        Try
            Dim con As New SqlConnection(Form1.cadena_conexion)
            con.Open()
            Dim cmd As New SqlCommand("select " & campo & " from " & tabla, con)
            cmd.CommandType = CommandType.Text
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count = 0 Then
                respuesta = False
            End If
            con.Close()
        Catch ex As Exception
            MsgBox("Error no existe conexión")
            respuesta = False
        End Try
        Return respuesta

    End Function


End Class
