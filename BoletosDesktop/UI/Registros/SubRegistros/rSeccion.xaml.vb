Imports System.Net.Http
Imports System.Text.Json

Public Class rSeccion
    Property RegistroPadre As rEventos
    Property Seccion As String
    Property Precio As Double

    Public Sub New(rE As rEventos)
        InitializeComponent()
        RegistroPadre = rE
        DataContext = Me

    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

    Private Sub AgregarClick(sender As Object, e As RoutedEventArgs)
        RegistroPadre.AgregarSeccion(Seccion, Precio)
        Close()
    End Sub
End Class
