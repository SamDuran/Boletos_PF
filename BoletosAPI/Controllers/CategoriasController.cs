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
    public class CategoriasController : ControllerBase
    {
        #region Props & Constructor
        private readonly string conStr;

        public CategoriasController(IConfiguration _config)
        {
            conStr = _config.GetConnectionString("RemoteConnection");
        }
        #endregion

        [HttpGet]
        public async Task<ActionResult<List<CategoriaEventos>>> GetCategorias()
        {
            using var connection = new MySqlConnection(conStr);

            var sql = $@"SELECT * FROM categoriaeventos";

            var categorias = await connection.QueryAsync<CategoriaEventos>(sql);

            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaEventos>> GetCategoria(int id)
        {
            using var connection = new MySqlConnection(conStr);

            var sql = $@"SELECT * FROM categoriaeventos WHERE CategoriaId = {id}";

            var categorias = await connection.QueryAsync<CategoriaEventos>(sql);

            if(categorias.Count() == 0)
                return NotFound();

            return Ok(categorias);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaEventos>> AddCategoria(string c)
        {
            using var connection = new MySqlConnection(conStr);

            var sql = $@"INSERT INTO categoriaeventos (categoria) VALUES ({c.ToSqlString()})";

            var categorias = await connection.ExecuteAsync(sql);
;

            return Ok(categorias>0);
        }
    }
}
