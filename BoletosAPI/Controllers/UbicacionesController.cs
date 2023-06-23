using Microsoft.AspNetCore.Mvc;
using BoletosAPI.Models.DTOs;
using MySql.Data.MySqlClient;
using BoletosAPI.Utilities;
using BoletosAPI.Models;
using Dapper;


namespace BoletosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UbicacionesController : ControllerBase
    {

        #region Props & Constructor
        private readonly string conStr;

        public UbicacionesController(IConfiguration _config)
        {
            conStr = _config.GetConnectionString("RemoteConnection");
        }
        #endregion

        // GET: api/<UbicacionesController>
        [HttpGet]
        public async Task<ActionResult<List<Ubicaciones>>> GetUbicaciones()
        {
            using var connection = new MySqlConnection(conStr);

            var sql = "SELECT * FROM Ubicacion";

            var ubicaciones = await connection.QueryAsync<Ubicaciones>(sql);

            return Ok(ubicaciones);
        }

        // GET api/<UbicacionesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ubicaciones>> GetUbicacion(int id)
        {
            using var connection = new MySqlConnection(conStr);

            var sql = $"SELECT * FROM Ubicacion WHERE UbicacionId = {id} LIMIT 1;";

            var ubicaciones = await connection.QueryAsync<Ubicaciones>(sql);
            //,
            if (ubicaciones.Count() == 0)
                return NotFound();

            return Ok(ubicaciones);
        }

        // POST api/<UbicacionesController>
        [HttpPost]
        public async Task<ActionResult<Ubicaciones>> Post(AddUbicacionDTO u)
        {
            using var connection = new MySqlConnection(conStr);

            var sql = $@"INSERT INTO Ubicacion (Ubicacion, Latitud, Longitud, Especificaciones)
                        VALUES ({u.Ubicacion.ToSqlString()},{u.Latitud},{u.Longitud},{u.Especificaciones.ToSqlString()});";

            var ubicacion = await connection.ExecuteAsync(sql);

            return Ok(ubicacion > 0);
        }
    }
}
