Public Class AddUbicacionDTO
    Property Ubicacion As String
    Property Latitud As Double
    Property Longitud As Double
    Property Especificaciones As String = ""

    Public Sub New(ev As Eventos)
        Me.Ubicacion = ev.Ubicacion.Ubicacion
        Me.Latitud = ev.Ubicacion.Latitud
        Me.Longitud = ev.Ubicacion.Longitud
        Me.Especificaciones = ""


    End Sub
End Class
