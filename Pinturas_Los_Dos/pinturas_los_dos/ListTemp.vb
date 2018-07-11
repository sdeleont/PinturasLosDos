Public Class ListTemp
    Private codPresentacion As String
    Private cantidad As Integer = 0

    Public Sub setCodigo(ByVal cod As Integer)
        Me.codPresentacion = cod
        Me.cantidad = 1
    End Sub
    Public Sub SumCant()
        Me.cantidad = Me.cantidad + 1
    End Sub
    Public Function GetCod() As String
        Return Me.codPresentacion
    End Function
    Public Function GetCantidad() As Integer
        Return Me.cantidad
    End Function
End Class
