namespace BoletosAPI.Models
{
    public class Eventos
    {
        public int EventoId { get; set; }
        public int BoletosDisponibles { get; set; }
        public DateTime FechaEvento { get; set; } = DateTime.Now;
        public string NombreEvento { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public Byte[]? Foto { get; set; }
        public int UserId { get; set; }
        public Usuarios Creador { get;set; } = null!;
        public int CategoriaId { get; set; }
        public CategoriaEventos CategoriaEventos { get; set; } = null!;
        public int UbicacionId { get; set; }
        public Ubicaciones Ubicacion { get; set; } = null!;
        public List<Secciones> Secciones { get; set; } = new List<Secciones>();
        public List<Boletos> Boletos { get; set; } = new List<Boletos>();
    }
    public class CategoriaEventos
    {
        public int CategoriaId { get; set; }
        public string Categoria { get; set; } = string.Empty;
    }
}
