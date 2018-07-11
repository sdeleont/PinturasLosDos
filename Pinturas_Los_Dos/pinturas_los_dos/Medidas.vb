Imports System.Data.SqlClient
Public Class Medidas
    Private id_medidas As Integer
    Private precio_venta As Double
    Private precio_compra As Double
    'no es necesario el id producto por que es el mismo para todos

    Public Sub New(ByVal precio_v As Double, ByVal precio_c As Double)
        Me.precio_venta = precio_v
        Me.precio_compra = precio_c
    End Sub
    Public Sub set_id_medida(ByVal id_medida As Integer)
        Me.id_medidas = id_medida
    End Sub
    Public Function get_p_compra() As Double
        Return precio_compra
    End Function
    Public Function get_p_venta() As Double
        Return precio_venta
    End Function
    Public Function verificar_id_medida(ByVal descripcion As String) As Integer
        Dim id As Integer
        Try
            Dim con As New SqlConnection(Form1.cadena_conexion)
            con.Open()
            Dim cmd As New SqlCommand("select id_medida from Medida where descripcion='" & descripcion & "'", con)
            cmd.CommandType = CommandType.Text
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            id = dt.Rows(0).Item("id_medida")
            con.Close()
        Catch ex As Exception
            MsgBox("Error no existe conexión " & ex.Message)
        End Try
        Return id
    End Function
    Public Function get_id()
        Return Me.id_medidas
    End Function
    Public Sub insertar_presentación(ByVal id_producto As Integer, ByVal id_medida As Integer, ByVal id_p_compra As Integer, ByVal id_p_venta As Integer)
       
    End Sub

End Class
