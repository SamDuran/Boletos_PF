namespace BoletosAPI.Models.DTOs
{
    public class AddUbicacionDTO
    {
        public string Ubicacion { get; set; } = string.Empty;
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Especificaciones { get; set; }
    }
}
