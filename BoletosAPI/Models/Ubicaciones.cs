namespace BoletosAPI.Models
{
    public class Ubicaciones
    {
        public int UbicacionId { get; set; }
        public string Ubicacion { get; set; } = String.Empty;
        public double? Latitud { get; set; }
        public double? Longitud { get; set;}
        public string? Especificaciones { get; set; }
    }
}
