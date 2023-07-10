Imports Microsoft.Maps.MapControl.WPF
Imports System.Text.Json
Imports System.Net.Http
Imports Microsoft.Win32
Imports System.Text
Imports System.IO

Public Class rEventos
    Property ImagePath As String
    Property Evento As Eventos
    Property UserId As Integer
    Property Categorias As List(Of CategoriaEventos)

    Public Sub New(Usuario As Integer)
        ObtenerCategorias()
        InitializeComponent()
        UserId = Usuario
        Evento = New Eventos()
        Evento.FechaEvento = DateTime.Now
        Evento.FechaEvento.AddMonths(1)
        DataContext = Evento
        SeccionesDataGrid.ItemsSource = Evento.Secciones

        Dim center As New Location(19.216697040872106, -70.088899223594836)
        map.Center = center
        map.ZoomLevel = 9

        AddHandler map.MouseDoubleClick, AddressOf Map_MouseDoubleClick
    End Sub
    Private Async Sub ObtenerCategorias()
        Using httpClient As New HttpClient()
            Dim apiUrl As String = $"{Application.API_URL}api/Categorias"

            Dim response As HttpResponseMessage = Await httpClient.GetAsync(apiUrl)
            If response.IsSuccessStatusCode Then

                Dim responseContent As String = Await response.Content.ReadAsStringAsync()

                Categorias = JsonSerializer.Deserialize(Of List(Of CategoriaEventos))(responseContent)

                CategoriaComboBox.ItemsSource = Categorias
                CategoriaComboBox.SelectedValuePath = "CategoriaId"
                CategoriaComboBox.DisplayMemberPath = "NombreCategoria"
                CategoriaComboBox.SelectedIndex = 0
            Else
                MessageBox.Show("No se pudo obtener la lista de categorias")
            End If
        End Using
    End Sub

    Public Sub AgregarSeccion(seccionNombre As String, precio As Double)
        Evento.Secciones.Add(New Secciones(seccion:=seccionNombre, precio:=precio))
    End Sub

    Private Sub Grid_Drop(sender As Object, e As DragEventArgs)
        ' Verificar si los datos arrastrados son de tipo archivo
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
            ' Tomar el primer archivo (puedes modificar el código para manejar múltiples archivos)
            Dim filePath As String = files(0)

            ' Verificar si el archivo es una imagen
            If IsImageFile(filePath) Then
                ImagePath = filePath
                ProcessImage(filePath)
            End If
        End If
    End Sub

    Private Sub Grid_MouseDown(sender As Object, e As MouseButtonEventArgs)
        ' Abrir el OpenFileDialog para seleccionar una imagen
        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Filter = "Archivos de imagen|*.png"
        If openFileDialog.ShowDialog() = True Then
            Dim filePath As String = openFileDialog.FileName
            ' Realizar la operación deseada con la imagen seleccionada
            ProcessImage(filePath)
        End If
    End Sub

    Private Function IsImageFile(filePath As String) As Boolean
        ' Verificar la extensión del archivo para determinar si es una imagen
        Dim extension As String = Path.GetExtension(filePath).ToLower()
        Return extension = ".jpg" OrElse extension = ".jpeg" OrElse extension = ".png"
    End Function

    Private Sub ProcessImage(filePath As String)
        Dim bitmap As New BitmapImage()
        bitmap.BeginInit()
        bitmap.UriSource = New Uri(filePath, UriKind.Absolute)
        bitmap.EndInit()

        ImageViewer.Source = bitmap

        Dim encoder As New PngBitmapEncoder() ' O usa el codificador adecuado según el tipo de imagen que estés manejando
        encoder.Frames.Add(BitmapFrame.Create(bitmap))

        Using memoryStream As New MemoryStream()
            encoder.Save(memoryStream)
            Evento.Foto = memoryStream.ToArray()
        End Using
    End Sub

    Private Sub DataGridCell_PreviewMouseRightButtonDown(sender As Object, e As MouseButtonEventArgs)
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim registroSeccion As New rSeccion(Me)
        registroSeccion.ShowDialog()
    End Sub

    Private Sub Map_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        ' Obtener la ubicación de la posición del clic
        Dim mousePosition As Point = e.GetPosition(map)
        Dim location As Location = map.ViewportPointToLocation(mousePosition)

        ' Obtener la latitud y longitud de la ubicación seleccionada
        Dim latitude As Double = location.Latitude
        Dim longitude As Double = location.Longitude

        ' Realizar la operación deseada con la ubicación seleccionada (latitud y longitud)
        Dim pushpin As New Pushpin()
        pushpin.Location = location
        pushpin.ToolTip = "" + Evento.Ubicacion.Ubicacion
        pushpin.ToolTip = UbicacionTb.Text

        If map.Children.Count > 0 Then
            map.Children.Clear()
        End If
        map.Children.Add(pushpin)
        Evento.Ubicacion.Latitud = location.Latitude
        Evento.Ubicacion.Longitud = location.Longitude
    End Sub

    Private Async Sub OnSaveClick(sender As Object, e As RoutedEventArgs) Handles SaveBTN.Click

        Evento.CategoriaId = CategoriaComboBox.SelectedValue
        Evento.UserId = UserId


        Dim UbicacionDTO = New AddUbicacionDTO(Evento)

        Using httpClient As New HttpClient()

            Dim LocationcontenidoJson As New StringContent(JsonSerializer.Serialize(UbicacionDTO), Encoding.UTF8, "application/json")
            Dim locationResponse As HttpResponseMessage = Await httpClient.PostAsync($"{Application.API_URL}api/Ubicaciones", LocationcontenidoJson)
            If locationResponse.IsSuccessStatusCode Then
                Dim responseContent As String = Await locationResponse.Content.ReadAsStringAsync()
                Evento.UbicacionId = responseContent
                Evento.Ubicacion.UbicacionId = responseContent
            Else
                MessageBox.Show("No se pudo agregar la ubicacion")
            End If

            Dim eventoDTO = Evento.ToDTO()

            Dim contenidoJson As New StringContent(JsonSerializer.Serialize(eventoDTO), Encoding.UTF8, "application/json")
            Dim response As HttpResponseMessage = Await httpClient.PostAsync($"{Application.API_URL}api/Eventos", contenidoJson)
            If response.IsSuccessStatusCode Then
                MessageBox.Show("Se pudo postear el evento")

            Else
                MessageBox.Show("No se pudo postear el evento")
            End If
        End Using

    End Sub

    Private Sub OnCleanClick(sender As Object, e As RoutedEventArgs) Handles CleanBTN.Click
        Evento = New Eventos()
        DataContext = Evento
        ImageViewer.Source = Nothing
    End Sub
End Class
