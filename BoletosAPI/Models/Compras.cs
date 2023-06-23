namespace BoletosAPI.Models
{
    public class Compras
    {
        public int CompraId { get; set; }
        public double Total { get; set; }
        public DateTime FechaCompra { get; set; } = DateTime.Now;
        public string UserEmail { get; set; } = string.Empty;
        public List<dCompras> dCompras { get; set; } = new List<dCompras>();
    }

    public class dCompras
    {
        public int Id { get; set; }
        public double Precio { get; set; }
        public int Cantidad { get; set; }
        public int CompraId { get; set; }
        public int BoletoId { get; set; }
    }
}
