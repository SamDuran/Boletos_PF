using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using BoletosAPI.Models.DTOs;
using BoletosAPI.Utilities;
using BoletosAPI.Models;
using Dapper;

namespace BoletosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeccionesController : ControllerBase
    {
        #region Props & Constructor
        private readonly string conStr;

        public SeccionesController(IConfiguration _config)
        {
            conStr = _config.GetConnectionString("RemoteConnection");
        }
        #endregion

        
        [HttpGet("byEventoId/{id}")]
        public async Task<ActionResult<List<Secciones>>> GetSecciones(int id)
        {
            using var connection = new MySqlConnection(conStr);

            var sql = $"SELECT * FROM dSecciones WHERE EventoId = {id} ORDER BY SeccionId";

            var secciones = await connection.QueryAsync<List<Secciones>>(sql);

            return Ok(secciones);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Secciones>> GetSeccion(int id)
        {
            using var connection = new MySqlConnection(conStr);

            var sql = $"SELECT * FROM dSecciones WHERE SeccionId = {id} LIMIT 1;";

            var secciones = await connection.QueryAsync<Secciones?>(sql);

            if(secciones.Count() == 0)
                return NotFound();

            return Ok(secciones);
        }


       
       
    }
}
