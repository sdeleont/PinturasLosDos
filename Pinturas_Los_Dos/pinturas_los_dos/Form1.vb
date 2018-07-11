Imports System
Imports System.IO
Imports System.Collections
Imports System.Net.Mail
Public Class Form1
    'Public cadena_conexion As String = "Data Source=AUTOPINTURA-PC\SQLEXPRESS;Initial Catalog=ferreteria;Integrated Security=True"
    Public cadena_conexion As String ' = "Data Source=SERGIO\SQLEXPRESS;Initial Catalog=ferr;Integrated Security=True"
    'Public cadena_conexion As String = "Data Source=JON-PC\SQLEXPRESS;Initial Catalog=ferreteria;Integrated Security=True"
    'Public cadena_conexion As String = "Data Source=SERGIO\SQL2012;Initial Catalog=ferreteria;Integrated Security=True"
    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub



    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If (Asc(e.KeyChar)) = 13 Then
            Dim con As conexion = New conexion()
            If con.verificar_contra(TextBox1.Text.Trim(), TextBox2.Text.Trim()) Then
                Me.Visible = False
                VentanaPrincipal.codigo_usuario_activo = con.buscar("id_usuario", "usuario", "usuario='" & TextBox1.Text & "' and contraseña='" & TextBox2.Text & "'").Rows(0).Item("id_usuario")
                VentanaPrincipal.nombre_usuario = con.buscar("nombre", "usuario", "usuario='" & TextBox1.Text & "' and contraseña='" & TextBox2.Text & "'").Rows(0).Item("nombre")
                Dim dt As New DataTable
                dt = con.buscar("s.nombre, s.id_sede", "Usuario u, sede s", "usuario='" & TextBox1.Text & "' and contraseña='" & TextBox2.Text & "' and u.id_sede=s.id_sede")
                VentanaPrincipal.id_sede = dt.Rows(0).Item("id_sede")
                VentanaPrincipal.nomSede = dt.Rows(0).Item("nombre")
                If con.buscar("administrador", "usuario", "usuario='" & TextBox1.Text & "' and contraseña='" & TextBox2.Text & "'").Rows(0).Item("administrador") = "1" Then
                    VentanaPrincipal.administrador = True
                End If
                TextBox1.Text = ""
                TextBox2.Text = ""
                TextBox1.Select()
                VentanaPrincipal.Show()

            Else
                Label3.Text = "DATOS DE USUARIO NO VALIDOS"
            End If
        End If
    End Sub


    Sub sendMail()
        Dim correo As New MailMessage
        Dim smtp As New SmtpClient()

        correo.From = New MailAddress("sergio3giovanni@gmail.com", "Sergio", System.Text.Encoding.UTF8)
        correo.To.Add("sergio.giovanni360@hotmail.com")
        correo.To.Add("lenguajes.laboratorio@gmail.com")
        correo.SubjectEncoding = System.Text.Encoding.UTF8
        correo.Subject = "Asunto de tu mensaje"
        correo.Body = "Este es el primer intento de mensaje"
        correo.BodyEncoding = System.Text.Encoding.UTF8
        correo.IsBodyHtml = False
        correo.Priority = MailPriority.Normal


        smtp.Port = 587
        smtp.Host = "smtp.gmail.com"
        smtp.Credentials = New System.Net.NetworkCredential("sergio3giovanni@gmail.com", "abcdef12345")
        smtp.EnableSsl = True
        smtp.Send(correo)

    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label3.Text = ""
        'Dim sLine As String
        Try
            'sendMail()
            'Dim fecha As String = rnd.Next(10, 28) & "/0" & rnd.Next(1, 9) & "/" & rnd.Next(2017, 2019)
            'Dim objReader As New StreamWriter("registros.txt")
            'Dim rnd As New Random()
            'For i As Integer = 5004 To 10000


            '    Dim presentacion As String = rnd.Next(1, 5003)
            '    Dim cad1 As String = "insert into detalle_venta values (" & i & "," & rnd.Next(1, 5003) & ",100,50);"
            '    Dim cad2 As String = "insert into detalle_venta values (" & i & "," & rnd.Next(1, 5003) & ",150,100);"
            '    Dim cad3 As String = "insert into detalle_venta values (" & i & "," & rnd.Next(1, 5003) & ",1000,640);"

            '    objReader.WriteLine(cad1)
            '    objReader.WriteLine(cad2)
            '    objReader.WriteLine(cad3)

            'Next
            'objReader.Close()
            cadena_conexion = "Server=tcp:serverbdpinturas.database.windows.net,1433;Initial Catalog=AutoPinturasBDA;Persist Security Info=False;User ID=PinturasBD;Password=Sergio19*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
        Catch ex As Exception
            MessageBox.Show("Error en Path" & ex.ToString())
        End Try
        
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If (Asc(e.KeyChar)) = 13 Then
            TextBox2.Select()
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub
End Class
