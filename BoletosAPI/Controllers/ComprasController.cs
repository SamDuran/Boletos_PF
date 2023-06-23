using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using BoletosAPI.Models.DTOs;
using BoletosAPI.Utilities;
using BoletosAPI.Models;
using Dapper;

namespace BoletosAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        #region PROPS & CONSTRUCTORS
        private readonly string connectionString;

        public ComprasController(IConfiguration _config)
        {
            connectionString = _config.GetConnectionString("RemoteConnection");
        }

        #endregion

        #region GETS

        [HttpGet()]
        public async Task<ActionResult<Compras>> GetMyPurchases(string Email)
        {
            var connection = new MySqlConnection(connectionString);

            var query = $@"SELECT * FROM Compras WHERE userEmail = {Email.ToSqlString()}";

            var purchases = await connection.QueryAsync<Compras>(query);

            foreach (var compras in purchases)
            {
                if (compras != null)
                {
                    var detallesQuery = "SELECT * FROM dCompras WHERE CompraId = @CompraId";
                    var detalles = connection.Query<dCompras>(detallesQuery, new { CompraId = compras.CompraId });

                    compras.dCompras.AddRange(detalles);
                }
            }

            return Ok(purchases);
        }

        #endregion

        #region CREATE & UPDATES

        [HttpPost("/BuyBoletos")]
        public async Task<ActionResult<Compras>> BuyBoletos(NewCompraDTO compra)
        {
            var connection = new MySqlConnection(connectionString);
            var cantidadComprada = compra.Details.Sum(d => d.Cantidad);
            var sumatoria = compra.Details.Sum(d => d.Precio * d.Cantidad);
            int dia, mes, anio;
            dia = DateTime.Now.Day;
            mes = DateTime.Now.Month;
            anio = DateTime.Now.Year;
            var sqlDate = $"{anio}-{mes}-{dia}";
            var query = $@"INSERT INTO Compras (Total,FechaCompra,UserEmail)
                            VALUES ({sumatoria},{sqlDate.ToSqlString()},{compra.UserEmail.ToSqlString()})";

            var comprasGuardadas = await connection.ExecuteAsync(query);

            var lastInsertedId = connection.QuerySingle<int>("SELECT LAST_INSERT_ID()");
            sumatoria = 0;
            foreach (var detalle in compra.Details)
            {
                var detallesQuery = "INSERT INTO dCompras (CompraId, Precio, Cantidad, BoletoId) " +
                                    "VALUES (@CompraId, @Precio, @Cantidad, @BoletoId)";
                sumatoria += await connection.ExecuteAsync(detallesQuery, new
                {
                    CompraId = lastInsertedId,
                    Precio = detalle.Precio,
                    Cantidad = detalle.Cantidad,
                    BoletoId = detalle.BoletoId
                });
            }
            var readEvento = $@"SELECT e.* FROM Eventos as e
                                INNER JOIN Boletos as b on b.eventoId = e.eventoId
                                WHERE b.boletoId = {compra.Details.First(e => true).BoletoId}";
            var eventos = await connection.QueryAsync<Eventos>(readEvento); 
            var updateQuery = $@"UPDATE Eventos 
                                SET BoletosDisponibles = {eventos.First().BoletosDisponibles - cantidadComprada} 
                                WHERE EventoId = {eventos.First().EventoId} LIMIT 1;";
            var eventoActualizado = await connection.ExecuteAsync(updateQuery);
            return Ok(comprasGuardadas  + sumatoria + eventoActualizado == compra.Details.Count+2);
            
        }


        #endregion
    }
}
