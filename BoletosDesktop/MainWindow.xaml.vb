Class MainWindow

#Region "Props"
    Property Usuario As Usuarios
    Property Registro As rEventos
    Property Consulta As cEventos
#End Region

    Public Sub New(user As Usuarios)

        InitializeComponent()
        Me.Usuario = user
        Title += " - " + Usuario.UserNombre
        BienvenidoLabel.Content += " " + Usuario.UserNombre

    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Registro = New rEventos(Usuario.UserId)
        Registro.Show()
    End Sub
    Private Sub SeeEvents_Click(sender As Object, e As RoutedEventArgs) Handles SeeEvents.Click
        Consulta = New cEventos()
        Consulta.Show()
    End Sub

    Private Sub OnMainClose(sender As Object, e As ComponentModel.CancelEventArgs)
        End
    End Sub
End Class
