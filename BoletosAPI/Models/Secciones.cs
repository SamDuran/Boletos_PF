namespace BoletosAPI.Models
{
    public class Secciones
    {
        public int SeccionId { get; set; }
        public int EventoId { get; set; }
        public string Seccion { get; set; } = String.Empty;
        public double Precio { get; set; }
    }
}
