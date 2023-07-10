Public Class SeccionesDto
    Property SeccionNombre As String
    Property Precio As Double
    Public Sub New(name As String, precio As Double)
        Me.SeccionNombre = name
        Me.Precio = precio
    End Sub
End Class
