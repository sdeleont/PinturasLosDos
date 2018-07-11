Imports System.Data.SqlClient
Public Class producto
    Private nombre_tipo As String
    Private nombre As String
    Private detalle As String
    Private marca As String
    Private existencias As Integer
    Private unidad As String
    Public Sub New(ByVal nombre_tipo As String, ByVal nombre As String, ByVal detalle As String, ByVal marca As String, ByVal existencias As Integer, ByVal unidad As String)
        Me.nombre_tipo = nombre_tipo
        Me.nombre = nombre
        Me.detalle = detalle
        Me.marca = marca
        Me.existencias = existencias
        Me.unidad = unidad
    End Sub
    Public Function insertar() As Integer
        Dim id As Integer = 0
        Try

            Dim con As New SqlConnection(Form1.cadena_conexion)
            con.Open()
            Dim cmd As New SqlCommand("select id_tipo from tipo where nombre = '" & Me.nombre_tipo & "'", con)
            cmd.CommandType = CommandType.Text
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Dim id_tipo As Integer = CType(dt.Rows(0).Item("id_tipo"), Integer)
            Dim c As conexion = New conexion()
            Dim id_unidad As String = c.buscar("*", "unidad", "nombre ='" & unidad & "'").Rows(0).Item("id_unidad")
            Dim cmd_in As New SqlCommand("insert into producto values ('" & nombre & "','" & detalle & "','" & marca & "'," & existencias & "," & id_tipo & "," & id_unidad & ")", con)
            cmd_in.ExecuteNonQuery()
            con.Close()
            con.Open()
            '------------------------------------
            Dim cmd2 As New SqlCommand("select MAX (id_producto) as val from producto ", con)
            cmd2.CommandType = CommandType.Text
            Dim da2 As New SqlDataAdapter(cmd2)
            Dim dt2 As New DataTable
            da2.Fill(dt2)
            Dim id2 As Integer = CType(dt2.Rows(0).Item("val"), Integer)

            con.Close()
            id = id2
        Catch ex As Exception
            MsgBox("Error no existe conexión" & ex.Message)
        End Try
        Return id
    End Function
End Class
