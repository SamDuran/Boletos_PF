Public Class Secciones
    Property SeccionId As Integer
    Property EventoId As Integer
    Property Seccion As String
    Property Precio As Double

    Public Sub New()

    End Sub
    Public Sub New(seccion As String, precio As Double)
        Me.Seccion = seccion
        Me.Precio = precio
    End Sub
End Class
