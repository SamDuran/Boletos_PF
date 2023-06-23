namespace BoletosAPI.Models.DTOs
{
    public class NewCompraDTO
    {
        public string UserEmail { get; set; } = string.Empty;
        public List<NewCompraDTODetails> Details { get; set; } = new List<NewCompraDTODetails>();
    }
    public class  NewCompraDTODetails
    {
        public double Precio { get; set; }
        public int Cantidad { get; set; }
        public int BoletoId { get; set; }
    }
}
