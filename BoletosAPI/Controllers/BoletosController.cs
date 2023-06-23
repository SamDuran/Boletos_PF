using Microsoft.AspNetCore.Mvc;
using BoletosAPI.Models;
using MySql.Data.MySqlClient;
using Dapper;

namespace BoletosAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class BoletosController : ControllerBase
    {

        #region PROPS & CONSTRUCTOR

        private readonly IConfiguration config;

        public BoletosController(IConfiguration _config)
        {
            this.config = _config;
        }

        #endregion

        #region GETs

        /* GET ALL MY TICKETS FROM A EVENT */
        /// <summary>
        /// This method returns the tickets of the user matching the given UserId and of a specific event matching the given EventoId.
        /// </summary>
        /// <param name="userId">UserId's user who bought the tickets</param>
        /// <param name="eventoId">Event's eventId that was for the ticket</param>
        /// <returns>Returns true if the creation was success and false if not.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Boletos>>> GetBoletos(int userId, int eventoId)
        {
            using var connection = new MySqlConnection(config.GetConnectionString("RemoteConnection"));

            var boletos = await connection.QueryAsync<Boletos>("SELECT * FROM Boletos");

            return Ok(boletos);
        }


        [HttpGet("id")]
        public async Task<ActionResult<Boletos>> GetBoleto(int id)
        {
            using var connection = new MySqlConnection(config.GetConnectionString("RemoteConnection"));

            var boleto = await connection.QueryFirstAsync<Boletos>($"SELECT * FROM Boletos WHERE BoletoId = {id}");

            return Ok(boleto);
        }
        #endregion



    }
}
