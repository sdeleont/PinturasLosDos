Imports System.Data.SqlClient
Imports Pinturas_Los_Dos.Medidas
Public Class form_productos
    Public p As Boolean = False ''para precio compra
    Public p1 As Boolean = False ''para precio compra
    Public p2 As Boolean = False   ' para venta
    Public p3 As Boolean = False   ' para venta
    Public index_combo As Integer 'valor del combo medidas
    Public lista_medidas As List(Of Medidas) 'lista utilizada para el listbox para posteriormente almacenarla
    Public precio_compra As Double
    'esto es de modificar
    Public codigo_producto As String = ""

    Private Sub form_productos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim c As conexion = New conexion()
        Dim datos1 As DataTable = c.buscar("*", "tipo", "id_sede=" & VentanaPrincipal.id_sede)
        Dim datos2 As DataTable = c.buscar_max("*", "unidad")
        Dim datos3 As DataTable = c.buscar_max("*", "medida")
        For i As Integer = 0 To datos1.Rows.Count - 1
            combo_tipo.Items.Add(datos1.Rows(i).Item("nombre"))
        Next
        For i As Integer = 0 To datos2.Rows.Count - 1
            combo_medida.Items.Add(datos2.Rows(i).Item("nombre"))
        Next
        For i As Integer = 0 To datos3.Rows.Count - 1
            combo_med2.Items.Add(datos3.Rows(i).Item("descripcion"))
        Next

        'TODO: This line of code loads data into the 'FerreteriaDataSet2.Tipo' table. You can move, or remove it, as needed.


        'TODO: This line of code loads data into the 'FerreteriaDataSet1.Medida' table. You can move, or remove it, as needed.

        'TODO: This line of code loads data into the 'FerreteriaDataSet.unidad' table. You can move, or remove it, as needed.



        lista_medidas = New List(Of Medidas)
        If Not VentanaPrincipal.administrador Then
            TextBox7.Enabled = False
            Button4.Enabled = False
            Button5.Enabled = False
            Button6.Enabled = False
            DataGridView1.Enabled = False
            nombre.Enabled = False
            marca.Enabled = False
            detalle.Enabled = False
            precio.Enabled = False
            compra.Enabled = False
        End If

    End Sub

    Private Sub TextBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If e.KeyChar = "." Then

        Else
            If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
                e.Handled = True
            End If
        End If
        If (Asc(e.KeyChar)) = 13 Then
            combo_medida.Select()
        End If
    End Sub

    Private Sub TextBox6_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox6.KeyPress
        If e.KeyChar = "." Then

        Else
            If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
                e.Handled = True
            End If
        End If
        If (Asc(e.KeyChar)) = 13 Then
            If Not TextBox6.Text.Trim = "" Then
                ListBox1.Items.Add(combo_med2.Text & "       Q." & TextBox6.Text)
                Dim n_medida As New Medidas(Convert.ToDouble(TextBox6.Text), precio_compra) 'instancion nuevo objeto medida
                n_medida.set_id_medida(n_medida.verificar_id_medida(combo_med2.Text))
                lista_medidas.Add(n_medida)
                combo_med2.Refresh()
                combo_med2.Select()
                TextBox6.Text = ""
                lblcompra.Text = "Q. "
            End If
            
        End If
    End Sub
    Function get_precio_compra() As Double
        Dim precio As Double = 0 'lo que retorno
        Dim conversion As Double = 0
        'combo_medida
        'txtcantidad
        Dim c As conexion = New conexion()
        Dim tasa As Double = Convert.ToDouble(c.buscar("tasa", "unidad", "nombre='" & combo_medida.Text & "'").Rows(0).Item("tasa"))
        conversion = Convert.ToDouble(txtcantidad.Text) / tasa


        Dim medida_galon As Double = Convert.ToDouble(c.buscar("cantidad", "Medida", "descripcion ='" & combo_med2.Text & "'").Rows(0).Item("cantidad"))

        precio = (medida_galon * Convert.ToDouble(TextBox3.Text)) / conversion
        MsgBox("Retorna metodo " & precio)
        Return precio
    End Function

    Private Sub TextBox5_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox5.KeyPress
        If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
            e.Handled = True
        End If
        If (Asc(e.KeyChar)) = 13 Then
            combo_med2.Select()
        End If
    End Sub

    

    Private Sub combo_med2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles combo_med2.KeyPress
        If (Asc(e.KeyChar)) = 13 Then
            index_combo = combo_med2.SelectedIndex 'por si lo agregan se lo vuela
            TextBox6.Select()
            precio_compra = get_precio_compra()
            lblcompra.Text = "Q.  " & precio_compra
        End If
    End Sub

    Private Sub combo_tipo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles combo_tipo.KeyPress
        If (Asc(e.KeyChar)) = 13 Then
            txtnombre.Select()
        End If
    End Sub

    Private Sub combo_tipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles combo_tipo.SelectedIndexChanged

    End Sub

    Private Sub txtnombre_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtnombre.KeyPress
        If (Asc(e.KeyChar)) = 13 Then
            txtmarca.Select()
        End If
    End Sub

    Private Sub txtmarca_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtmarca.KeyPress
        If (Asc(e.KeyChar)) = 13 Then
            TextBox3.Select()
        End If
    End Sub
    Private Sub combo_med1_KeyPress(sender As Object, e As KeyPressEventArgs)
        If (Asc(e.KeyChar)) = 13 Then
            txtdetalle.Select()
        End If
    End Sub

    Private Sub txtdetalle_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtdetalle.KeyPress
        If (Asc(e.KeyChar)) = 13 Then
            TextBox5.Select()
        End If
    End Sub

    

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Para agregar un nuevo producto en su forma de compra solo se va usar para determinar los precios de compra de cada medida
        'MsgBox("Prueba")
        If verificar() Then
            Dim nuevo As New producto(combo_tipo.Text, txtnombre.Text, txtdetalle.Text, txtmarca.Text, Convert.ToInt32(TextBox5.Text), combo_medida.Text)
            Dim id As Integer = nuevo.insertar()
            '   MsgBox("el id es : " & id)
            '  MsgBox("Prueba")

            Dim con As conexion = New conexion()

            For Each c As Medidas In lista_medidas
                'MessageBox.Show("el id es : " & id)
                'MsgBox("insert into presentacion values (" & id & "," & Convert.ToInt32(c.get_id) & "," & Double.Parse(c.get_p_venta) & "," & Double.Parse(c.get_p_compra) & ")")
                Dim cad As String = "INSERT into presentacion values (" & Double.Parse(c.get_p_venta) & "," & Double.Parse(c.get_p_compra) & "," & id & "," & Integer.Parse(c.get_id) & ")"
                'MessageBox.Show(cad)

                con.insertar(cad, "Medida con exito", False, 1)
                'con.insertar("insert into presentacion values (" & id & "," & Convert.ToInt32(c.get_id) & "," & Double.Parse(c.get_p_venta) & "," & Double.Parse(c.get_p_compra) & ")", "Medida con exito", True)
            Next

            'este procedimiento me sirve para actualizar las existencias del tipo correspondiente
            Dim conversion As Double = 0

            Dim tasa As Double = Convert.ToDouble(con.buscar("tasa", "unidad", "nombre='" & combo_medida.Text & "'").Rows(0).Item("tasa"))
            conversion = Convert.ToDouble(txtcantidad.Text) / tasa

            Dim existencia_actual As Double = Convert.ToDouble(con.buscar("existencias", "tipo", "nombre='" & combo_tipo.Text & "' and id_sede=" & VentanaPrincipal.id_sede).Rows(0).Item("existencias"))

            existencia_actual = existencia_actual + (conversion * Convert.ToDouble(TextBox5.Text))
            con.insertar("update tipo set existencias=" & existencia_actual & " where nombre='" & combo_tipo.Text & "' and id_sede=" & VentanaPrincipal.id_sede, "", False, 0)
            'termnina el proceso de actualizacion
            MsgBox("Producto insertado", vbInformation)
            p = False
            p3 = False
            p1 = False
            lista_medidas.Clear()
            txtnombre.Text = ""
            txtcantidad.Text = ""
            txtdetalle.Text = ""
            txtmarca.Text = ""
            TextBox3.Text = ""
            TextBox5.Text = ""
            ListBox1.Items.Clear()
            combo_tipo.Select()
            VentanaPrincipal.DataGridView1.Refresh()

        End If
        
    End Sub

    Function verificar() As Boolean
        Dim res As Boolean = False
        If Not txtnombre.Text.Trim() = "" And Not txtcantidad.Text.Trim() = "" And Not txtdetalle.Text.Trim() = "" And Not txtmarca.Text.Trim() = "" And Not TextBox3.Text.Trim() = "" And Not TextBox5.Text.Trim() = "" Then
            If Not ListBox1.Items.Count = 0 Then
                res = True
            Else
                MsgBox("Debe agregar por lo menos una presentacion del producto")
            End If
        Else
            MsgBox("Debe llenar los campos solicitados")
        End If

        Return res
    End Function
   


    Private Sub txt_medida_KeyPress(sender As Object, e As KeyPressEventArgs)
        If (Asc(e.KeyChar)) = 13 Then
            txtdetalle.Select()
        End If
    End Sub

    

    


    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    

  

    Private Sub Button7_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    

    Private Sub combo_medida_KeyPress(sender As Object, e As KeyPressEventArgs) Handles combo_medida.KeyPress
        If (Asc(e.KeyChar)) = 13 Then
            txtcantidad.Select()
        End If

    End Sub

   

    Private Sub combo_medida_SelectedIndexChanged(sender As Object, e As EventArgs) Handles combo_medida.SelectedIndexChanged

    End Sub

   

    Private Sub txtcantidad_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtcantidad.KeyPress
        If e.KeyChar = "." Then

        Else
            If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
                e.Handled = True
            End If
        End If
        If (Asc(e.KeyChar)) = 13 Then
            txtdetalle.Select()
        End If
    End Sub

    
   
    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged

    End Sub

    
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'If Not TextBox7.Text.Trim().Equals("") Then
        buscar()
        'End If

    End Sub

    Private Sub TextBox7_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox7.KeyPress
        If (Asc(e.KeyChar)) = 13 Then
            '    If Not TextBox7.Text.Trim().Equals("") Then
            buscar()
            'End If
        End If
    End Sub

    Sub buscar()
        
        Dim con As conexion = New conexion()
        Try
            con.llenar_grid("select m.descripcion as 'Medida (Galon/Otros)', p.precio_venta as 'Precio (Q.)'  from Presentacion p, Medida m where p.id_medida =m.id_medida and  p.id_producto =" & TextBox7.Text, DataGridView1)
            Dim datos As DataTable = con.buscar("*", "producto", " id_producto=" & TextBox7.Text)
            nombre.Text = datos.Rows(0).Item("nombre")
            detalle.Text = datos.Rows(0).Item("detalle")
            marca.Text = datos.Rows(0).Item("marca")
            codigo_producto = TextBox7.Text
        Catch ex As Exception
            MsgBox("Codigo no encontrado")
        End Try



    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Label13.Text = DataGridView1.CurrentRow.Cells(0).Value()
        precio.Text = DataGridView1.CurrentRow.Cells(1).Value()
        Dim c As conexion = New conexion()
        Dim codigo_medida As String = c.buscar("id_medida", "Medida", "descripcion='" & DataGridView1.CurrentRow.Cells(0).Value() & "'").Rows(0).Item("id_medida")
        compra.Text = c.buscar("precio_compra", "presentacion", "id_producto=" & codigo_producto & " and id_medida=" & codigo_medida).Rows(0).Item("precio_compra")
    End Sub

    
    
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim c As conexion = New conexion()
        Try
            c.insertar("update producto set nombre='" & nombre.Text & "', detalle='" & detalle.Text & "', marca='" & marca.Text & "'  where id_producto=" & codigo_producto, "Modificación exitosa", False, 0)
            If Not Label13.Text = "----" Then

                Dim codigo_medida As String = c.buscar("id_medida", "Medida", "descripcion='" & Label13.Text & "'").Rows(0).Item("id_medida")
                c.insertar("update Presentacion set precio_venta=" & precio.Text & ", precio_compra=" & compra.Text & "  where id_producto=" & codigo_producto & " and id_medida=" & codigo_medida, "Modificación exitosa", True, 0)
                Label13.Text = "----"
                precio.Text = ""
                compra.Text = ""
            End If
        Catch ex As Exception
            MsgBox("Error al insertar Producto")
        End Try

    End Sub

    
    
    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged

    End Sub

    Private Sub txtcantidad_TextChanged(sender As Object, e As EventArgs) Handles txtcantidad.TextChanged

    End Sub

    Private Sub TextBox6_Leave(sender As Object, e As EventArgs) Handles TextBox6.Leave

    End Sub

    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles TextBox6.TextChanged

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        p = False
        p3 = False
        p1 = False
        lista_medidas.Clear()
        txtnombre.Text = ""
        txtcantidad.Text = ""
        txtdetalle.Text = ""
        txtmarca.Text = ""
        TextBox3.Text = ""
        TextBox5.Text = ""
        ListBox1.Items.Clear()
        combo_tipo.Select()
        VentanaPrincipal.DataGridView1.Refresh()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button6_Click_1(sender As Object, e As EventArgs) Handles Button6.Click
        Me.Close()
    End Sub

    Private Sub combo_med2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles combo_med2.SelectedIndexChanged

    End Sub
End Class