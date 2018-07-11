Imports System.Data.SqlClient
Public Class form_usuarios
    Public usuario As String = ""

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If (Asc(e.KeyChar)) = 13 Then
            Dim c As conexion = New conexion()
            Try
                usuario = c.buscar("id_usuario", "usuario", "nombre='" & TextBox1.Text & "' and id_sede=" & VentanaPrincipal.id_sede).Rows(0).Item("id_usuario")
                Button2.Select()
            Catch ex As Exception
                TextBox2.Select()
            End Try

        End If
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If (Asc(e.KeyChar)) = 13 Then
            TextBox3.Select()
        End If
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        If Not TextBox1.Text.Trim = "" And Not TextBox2.Text.Trim = "" And Not TextBox3.Text.Trim = "" Then
            Dim c As conexion = New conexion()
            If CheckBox1.Checked = True Then
                c.insertar("insert into usuario values ('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "',1," & VentanaPrincipal.id_sede & ")", "Usuario registrado con exito", True, 0)
            Else
                c.insertar("insert into usuario values ('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "',0," & VentanaPrincipal.id_sede & ")", "Usuario registrado con exito", True, 0)
            End If
            TextBox1.Text = ""
            TextBox2.Text = ""
            TextBox3.Text = ""
            CheckBox1.Checked = False
        Else
            MsgBox("LLene los campos necesarios")
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Not usuario.Trim = "" Then
            Dim c As conexion = New conexion()
            c.insertar("delete from usuario where id_usuario =" & usuario, "", False, 0)
            MsgBox("Usuario eliminado")
            TextBox1.Text = ""
            TextBox1.Select()
        End If
    End Sub
End Class