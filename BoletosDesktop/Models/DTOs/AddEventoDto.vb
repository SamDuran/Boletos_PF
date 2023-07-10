Public Class AddEventoDto
    Property nombreEvento As String
    Property descripcion As String
    Property userId As Integer
    Property categoriaId As Integer
    Property ubicacionId As Integer
    Property foto As Byte()
    Property boletos As Integer
    Property fechaEvento As String
    Property secciones As List(Of SeccionesDto) = New List(Of SeccionesDto)
    Public Sub New(ev As Eventos)
        Me.nombreEvento = ev.NombreEvento
        Me.descripcion = ev.Descripcion
        Me.userId = ev.UserId
        Me.categoriaId = ev.CategoriaId
        Me.ubicacionId = ev.UbicacionId
        Me.foto = ev.Foto
        Me.boletos = ev.BoletosDisponibles
        Me.fechaEvento = ev.FechaEvento.ToString("yyyy-MM-dd")
        For Each seccion In ev.Secciones
            Me.secciones.Add(New SeccionesDto(seccion.Seccion, seccion.Precio))
        Next
    End Sub
End Class
