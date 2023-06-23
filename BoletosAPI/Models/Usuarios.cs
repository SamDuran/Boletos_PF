namespace BoletosAPI.Models
{
    public class Usuarios
    {
        public int UserId { get; set; }
        public string UserNombre { get; set; } = String.Empty;
        public string UserEmail { get; set; } = String.Empty;
        public string UserClave { get; set; } = String.Empty;
    }
}
