<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class form_existencias
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.tipo = New System.Windows.Forms.Label()
        Me.detalle = New System.Windows.Forms.Label()
        Me.marca = New System.Windows.Forms.Label()
        Me.nombre = New System.Windows.Forms.Label()
        Me.codigo = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.cmd_medida = New System.Windows.Forms.ComboBox()
        Me.UnidadBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.FerreteriaDataSet4 = New Pinturas_Los_Dos.ferreteriaDataSet4()
        Me.cantidad = New System.Windows.Forms.TextBox()
        Me.numero_unidades = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.UnidadTableAdapter = New Pinturas_Los_Dos.ferreteriaDataSet4TableAdapters.unidadTableAdapter()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.UnidadBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FerreteriaDataSet4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(86, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Codigo Producto"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Nombre:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 93)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(43, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Detalle:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 69)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Marca:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 118)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(92, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Tipo de Producto:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.tipo)
        Me.GroupBox1.Controls.Add(Me.detalle)
        Me.GroupBox1.Controls.Add(Me.marca)
        Me.GroupBox1.Controls.Add(Me.nombre)
        Me.GroupBox1.Controls.Add(Me.codigo)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Location = New System.Drawing.Point(39, 35)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(230, 160)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Detalle Producto"
        '
        'tipo
        '
        Me.tipo.AutoSize = True
        Me.tipo.Location = New System.Drawing.Point(108, 118)
        Me.tipo.Name = "tipo"
        Me.tipo.Size = New System.Drawing.Size(86, 13)
        Me.tipo.TabIndex = 10
        Me.tipo.Text = "Codigo Producto"
        '
        'detalle
        '
        Me.detalle.AutoSize = True
        Me.detalle.Location = New System.Drawing.Point(108, 93)
        Me.detalle.Name = "detalle"
        Me.detalle.Size = New System.Drawing.Size(86, 13)
        Me.detalle.TabIndex = 10
        Me.detalle.Text = "Codigo Producto"
        '
        'marca
        '
        Me.marca.AutoSize = True
        Me.marca.Location = New System.Drawing.Point(108, 69)
        Me.marca.Name = "marca"
        Me.marca.Size = New System.Drawing.Size(86, 13)
        Me.marca.TabIndex = 10
        Me.marca.Text = "Codigo Producto"
        '
        'nombre
        '
        Me.nombre.AutoSize = True
        Me.nombre.Location = New System.Drawing.Point(108, 46)
        Me.nombre.Name = "nombre"
        Me.nombre.Size = New System.Drawing.Size(86, 13)
        Me.nombre.TabIndex = 10
        Me.nombre.Text = "Codigo Producto"
        '
        'codigo
        '
        Me.codigo.AutoSize = True
        Me.codigo.Location = New System.Drawing.Point(108, 25)
        Me.codigo.Name = "codigo"
        Me.codigo.Size = New System.Drawing.Size(86, 13)
        Me.codigo.TabIndex = 10
        Me.codigo.Text = "Codigo Producto"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 23)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(105, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Cantidad de Medida:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 60)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(45, 13)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Medida:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(276, 153)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(110, 13)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Numero de Unidades:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.cmd_medida)
        Me.GroupBox2.Controls.Add(Me.cantidad)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Location = New System.Drawing.Point(275, 44)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(231, 97)
        Me.GroupBox2.TabIndex = 9
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Detalle Medida"
        '
        'cmd_medida
        '
        Me.cmd_medida.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmd_medida.FormattingEnabled = True
        Me.cmd_medida.Location = New System.Drawing.Point(118, 57)
        Me.cmd_medida.Name = "cmd_medida"
        Me.cmd_medida.Size = New System.Drawing.Size(100, 21)
        Me.cmd_medida.TabIndex = 9
        '
        'UnidadBindingSource
        '
        Me.UnidadBindingSource.DataMember = "unidad"
        Me.UnidadBindingSource.DataSource = Me.FerreteriaDataSet4
        '
        'FerreteriaDataSet4
        '
        Me.FerreteriaDataSet4.DataSetName = "ferreteriaDataSet4"
        Me.FerreteriaDataSet4.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'cantidad
        '
        Me.cantidad.Location = New System.Drawing.Point(118, 23)
        Me.cantidad.Name = "cantidad"
        Me.cantidad.Size = New System.Drawing.Size(100, 20)
        Me.cantidad.TabIndex = 8
        '
        'numero_unidades
        '
        Me.numero_unidades.Location = New System.Drawing.Point(393, 150)
        Me.numero_unidades.Name = "numero_unidades"
        Me.numero_unidades.Size = New System.Drawing.Size(100, 20)
        Me.numero_unidades.TabIndex = 10
        '
        'Button1
        '
        Me.Button1.Image = Global.Pinturas_Los_Dos.My.Resources.Resources.save_as
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(537, 67)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(80, 39)
        Me.Button1.TabIndex = 14
        Me.Button1.Text = "         Guardar"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Image = Global.Pinturas_Los_Dos.My.Resources.Resources.cancel
        Me.Button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button3.Location = New System.Drawing.Point(537, 128)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(80, 34)
        Me.Button3.TabIndex = 16
        Me.Button3.Text = "Cancelar"
        Me.Button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button3.UseVisualStyleBackColor = True
        '
        'UnidadTableAdapter
        '
        Me.UnidadTableAdapter.ClearBeforeFill = True
        '
        'form_existencias
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(655, 215)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.numero_unidades)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "form_existencias"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Formulario Nuevas existencias"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.UnidadBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FerreteriaDataSet4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents tipo As System.Windows.Forms.Label
    Friend WithEvents detalle As System.Windows.Forms.Label
    Friend WithEvents marca As System.Windows.Forms.Label
    Friend WithEvents nombre As System.Windows.Forms.Label
    Friend WithEvents codigo As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cmd_medida As System.Windows.Forms.ComboBox
    Friend WithEvents cantidad As System.Windows.Forms.TextBox
    Friend WithEvents numero_unidades As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents FerreteriaDataSet4 As Pinturas_Los_Dos.ferreteriaDataSet4
    Friend WithEvents UnidadBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents UnidadTableAdapter As Pinturas_Los_Dos.ferreteriaDataSet4TableAdapters.unidadTableAdapter
End Class
