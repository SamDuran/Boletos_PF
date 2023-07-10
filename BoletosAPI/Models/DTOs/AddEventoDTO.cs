namespace BoletosAPI.Models.DTOs
{
    public class AddEventoDTO
    {
        public string NombreEvento { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int CategoriaId { get; set; }
        public int UbicacionId { get; set; }
        public Byte[]? Foto { get; set; }
        public int Boletos { get; set; }

        public string FechaEvento { get; set; } = string.Empty;
        public List<AddSeccionDTO> Secciones { get; set; } = new List<AddSeccionDTO>();
    }
}
