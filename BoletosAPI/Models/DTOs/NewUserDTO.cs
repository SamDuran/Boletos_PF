namespace BoletosAPI.Models.DTOs
{
    public class NewUserDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserClave { get; set; } = string.Empty;
    }
}
