using Microsoft.AspNetCore.Http;
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
        private readonly IConfiguration config;

        public BoletosController(IConfiguration _config)
        {
            this.config = _config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Boletos>>> GetBoletos()
        {
            using var connection = new MySqlConnection(config.GetConnectionString("RemoteConnection"));

            var boletos = await connection.QueryAsync<Boletos>("Select * from Boletos");

            return Ok(boletos);

        }
    }
}
