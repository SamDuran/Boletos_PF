Imports System.Text.Json.Serialization

Public Class Usuarios
    <JsonPropertyName("userId")>
    Property UserId As Integer
    <JsonPropertyName("userNombre")>
    Property UserNombre As String
    <JsonPropertyName("userEmail")>
    Property UserEmail As String
    <JsonPropertyName("userClave")>
    Property UserClave As String
End Class
