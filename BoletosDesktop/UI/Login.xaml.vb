Imports System.ComponentModel
Imports System.Text.Json
Imports System.Net.Http

Public Class Login
    Property ViewModel As New LoginViewModel()
    Public Sub New()
        InitializeComponent()
        DataContext = ViewModel

        If Debugger.IsAttached Then
            ViewModel.LoginData.User = "SamDuran"
            ViewModel.LoginData.Clave = "Samuel19"
            Logear()
        End If

    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Logear()
    End Sub

    Private Async Sub Logear()
        Using httpClient As New HttpClient()
            Dim apiUrl As String = $"{Application.API_URL}api/Usuario?userName={ViewModel.LoginData.User}&userClave={ViewModel.LoginData.Clave}"

            Dim response As HttpResponseMessage = Await httpClient.GetAsync(apiUrl)
            If response.IsSuccessStatusCode Then

                Dim responseContent As String = Await response.Content.ReadAsStringAsync()

                Dim userModel As Usuarios = JsonSerializer.Deserialize(Of Usuarios)(responseContent)

                Dim main = New MainWindow(userModel)
                main.Show()
                Close()
            Else
                MessageBox.Show("No se pudo logear")
            End If
        End Using
    End Sub
    Private Sub CerrarBTN_Click(sender As Object, e As RoutedEventArgs)
        End
    End Sub

    Private Sub PasswordBox_Loaded(sender As Object, e As RoutedEventArgs)
        passwordBox.DataContext = Me.ViewModel.LoginData
    End Sub
    Private Sub PasswordBox_PasswordChanged(sender As Object, e As RoutedEventArgs)
        Me.ViewModel.LoginData.Clave = passwordBox.Password
    End Sub
    Private Sub Minimizar(sender As Object, e As RoutedEventArgs)
        Me.WindowState = WindowState.Minimized
    End Sub

    Private Sub EnterPressed(sender As Object, e As KeyEventArgs) Handles passwordBox.KeyDown

        If e.Key.Equals(Key.Enter) Then
            If passwordBox.Password.Length < 1 Then
                passwordBox.Focus()
                Return
            End If
            Logear()
        End If
    End Sub
End Class


Public Class LoginViewModel
    Implements INotifyPropertyChanged

    Private _loginData As New LoginDataViewModel()

    Public Property LoginData As LoginDataViewModel
        Get
            Return _loginData
        End Get
        Set
            _loginData = Value
            OnPropertyChanged("LoginData")
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Protected Overridable Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub
End Class

Public Class LoginDataViewModel
    Implements INotifyPropertyChanged

    Private _user As String
    Private _clave As String

    Public Property User As String
        Get
            Return _user
        End Get
        Set
            _user = Value
            OnPropertyChanged("User")
        End Set
    End Property

    Public Property Clave As String
        Get
            Return _clave
        End Get
        Set
            _clave = Value
            OnPropertyChanged("Clave")
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Protected Overridable Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub
End Class
