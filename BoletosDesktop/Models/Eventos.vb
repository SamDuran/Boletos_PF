Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Text.Json.Serialization

Public Class Eventos
    Implements INotifyPropertyChanged
    Property EventoId As Integer
    Property BoletosDisponibles As Integer
    Property FechaEvento As DateTime
    Property NombreEvento As String
    Property Descripcion As String
    Property Foto As Byte()

    <JsonPropertyName("creador")>
    Property Creador As Usuarios = New Usuarios()
    Property UserId As Integer

    <JsonPropertyName("categoriaEventos")>
    Property Categoria As CategoriaEventos = New CategoriaEventos()
    Property CategoriaId As Integer

    <JsonPropertyName("ubicacion")>
    Property Ubicacion As Ubicaciones = New Ubicaciones()
    Property UbicacionId As Integer


    Private _secciones As New ObservableCollection(Of Secciones)()

    <JsonPropertyName("secciones")>
    Public Property Secciones As ObservableCollection(Of Secciones)
        Get
            Return _secciones
        End Get
        Set(value As ObservableCollection(Of Secciones))
            _secciones = value
            OnPropertyChanged("Secciones")
        End Set
    End Property

    <JsonPropertyName("boletos")>
    Property Boletos As List(Of Boletos) = New List(Of Boletos)


    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Protected Overridable Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Public Function ToDTO() As AddEventoDto
        Return New AddEventoDto(Me)
    End Function
End Class

Public Class CategoriaEventos
    <JsonPropertyName("categoriaId")>
    Property CategoriaId As Integer
    <JsonPropertyName("categoria")>
    Property NombreCategoria As String
End Class