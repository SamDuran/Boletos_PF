using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using BoletosAPI.Models;
using Dapper;
using BoletosAPI.Models.DTOs;
using BoletosAPI.Utilities;

namespace BoletosAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        #region Props & Constructor
        private readonly string connectionString;

        public EventosController(IConfiguration _config)
        {
            connectionString = _config.GetConnectionString("RemoteConnection");
        }
        #endregion

        #region EVENTS API CALLS

        #region GETS
        /* GET LIST */
        /// <summary>
        /// This method return the top (15 * pages) of the available events with date higher than today
        /// </summary>
        /// <param name="pages">The numbers of pages that you want to charge.</param>
        /// <returns>All non-ocurring events as a list</returns>
        [HttpGet]
        public async Task<ActionResult<List<Eventos>>> GetEventos(int pages = 1)
        {
            using var connection = new MySqlConnection(connectionString);
            var sql = @$"SELECT
                            eventoid, e.boletosDisponibles, e.fechaEvento, nombreevento, e.descripcion,
                            e.userId, us.userId, us.usernombre, us.useremail,
                            e.categoriaId, ce.categoriaId, ce.categoria,
                            e.ubicacionId, u.ubicacionId, u.ubicacion, IFNULL(u.latitud,0) AS 'latitud', IFNULL(u.longitud,0) AS 'longitud', IFNULL(u.especificaciones,'') AS 'especificaciones'
                        FROM Eventos AS E
                        LEFT JOIN BPF_Usuarios AS Us ON Us.userid = e.userid                         
                        INNER JOIN categoriaeventos AS ce ON ce.categoriaId = E.categoriaId
                        INNER JOIN Ubicacion AS u ON u.ubicacionId = e.ubicacionId
                        WHERE FechaEvento > Current_Date()
                        LIMIT {15 * pages}; ";

            var eventos = await connection.QueryAsync<Eventos, Usuarios, CategoriaEventos, Ubicaciones, Eventos>(sql, (evento, user, categoria, ubicacion) =>
                {
                    evento.Creador = user;
                    evento.UserId = user.UserId;
                    evento.CategoriaEventos = categoria;
                    evento.CategoriaId = categoria.CategoriaId;
                    evento.Ubicacion = ubicacion;
                    evento.UbicacionId = ubicacion.UbicacionId;
                    return evento;
                },
                splitOn: "userId, categoriaId, ubicacionId"
            );
            string sqlSeccion;
            IEnumerable<Secciones> secciones;
            foreach( var evento in eventos )
            {
                sqlSeccion = $"SELECT * FROM dSecciones WHERE EventoId = {evento.EventoId} ORDER BY SeccionId";
                secciones = await connection.QueryAsync<Secciones>(sqlSeccion);
                evento.Secciones.AddRange( secciones );
            }
            string sqlBoletos;
            IEnumerable<Boletos> boletos;
            foreach (var evento in eventos)
            {
                sqlBoletos = $"SELECT * FROM boletos WHERE EventoId = {evento.EventoId}";
                boletos = await connection.QueryAsync<Boletos>(sqlBoletos);
                evento.Boletos.AddRange(boletos);
            }
            return Ok(eventos);
        }

        /* GET ONE EVENT BY ID */
        /// <summary>
        /// This method find a specific event searched by his EventoId
        /// </summary>
        /// <param name="id">The specific EventoId which matches with the event that you want to found</param>
        /// <returns>An event which its EventoId match with the parameter</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Eventos>> GetEventoById(int id)
        {
            using var connection = new MySqlConnection(connectionString);
            var sql = @$"SELECT 
                            eventoid, e.boletosDisponibles, e.fechaEvento, nombreevento, e.descripcion,
                            e.userId, us.userId, us.usernombre, us.useremail,
                            e.categoriaId, ce.categoriaId, ce.categoria,
                            e.ubicacionId, u.ubicacionId, u.ubicacion, IFNULL(u.latitud,0) AS 'latitud', IFNULL(u.longitud,0) AS 'longitud', IFNULL(u.especificaciones,'') AS 'especificaciones'
                        FROM Eventos AS E
                        LEFT JOIN BPF_Usuarios AS Us ON Us.userid = e.userid                         
                        INNER JOIN categoriaeventos AS ce ON ce.categoriaId = E.categoriaId
                        INNER JOIN Ubicacion AS u ON u.ubicacionId = e.ubicacionId
                        WHERE eventoId = {id}
                        LIMIT 1 ; ";

            var eventos = await connection.QueryAsync<Eventos, Usuarios, CategoriaEventos, Ubicaciones, Eventos>(sql, (e, u, c, ub) =>
            {
                e.Creador = u;
                e.UserId = u.UserId;
                e.CategoriaEventos = c;
                e.CategoriaId = c.CategoriaId;
                e.Ubicacion = ub;
                e.UbicacionId = ub.UbicacionId;
                return e;
            },
                splitOn: "userId, categoriaId, ubicacionId"
            );

            string sqlSeccion;
            IEnumerable<Secciones> secciones;
            foreach (var evento in eventos)
            {
                sqlSeccion = $"SELECT * FROM dSecciones WHERE EventoId = {evento.EventoId} ORDER BY SeccionId";
                secciones = await connection.QueryAsync<Secciones>(sqlSeccion);
                evento.Secciones.AddRange(secciones);
            }
            string sqlBoletos;
            IEnumerable<Boletos> boletos;
            foreach (var evento in eventos)
            {
                sqlBoletos = $"SELECT * FROM boletos WHERE EventoId = {evento.EventoId}";
                boletos = await connection.QueryAsync<Boletos>(sqlBoletos);
                evento.Boletos.AddRange(boletos);
            }
            return Ok(eventos);
        }

        /* GET LIST BY CATEGORIES */
        /// <summary>
        /// This method return all the available events with date higher than today and with the categoryId like the given parameter
        /// </summary>
        /// <param name="categoryId">The specific categoryId that matches the category in which the events you want to find are located</param>
        /// <param name="pages">The numbers of pages that you want to charge.</param>
        /// <returns>All non-ocurring events, as a list, that their categoryId matches with the given parameter.</returns>
        [HttpGet("byCategory/{categoryId}")]
        public async Task<ActionResult<List<Eventos>>> GetEventosByCategory(int categoryId, int pages = 1)
        {
            using var connection = new MySqlConnection(connectionString);
            var sql = @$"SELECT 
                            eventoid, e.boletosDisponibles, e.fechaEvento, nombreevento, e.descripcion,
                            e.userId, us.userId, us.usernombre, us.useremail,
                            e.categoriaId, ce.categoriaId, ce.categoria,
                            e.ubicacionId, u.ubicacionId, u.ubicacion, IFNULL(u.latitud,0) AS 'latitud', IFNULL(u.longitud,0) AS 'longitud', IFNULL(u.especificaciones,'') AS 'especificaciones'
                        FROM Eventos AS E
                        LEFT JOIN BPF_Usuarios AS Us ON Us.userid = e.userid                         
                        INNER JOIN categoriaeventos AS ce ON ce.categoriaId = E.categoriaId
                        INNER JOIN Ubicacion AS u ON u.ubicacionId = e.ubicacionId
                        WHERE FechaEvento > Current_Date() AND e.categoriaId = {categoryId}
                        LIMIT {15 * pages};";

            var eventos = await connection.QueryAsync<Eventos, Usuarios, CategoriaEventos, Ubicaciones, Eventos>(sql, (evento, user, categoria, ubicacion) =>
            {
                evento.Creador = user;
                evento.UserId = user.UserId;
                evento.CategoriaEventos = categoria;
                evento.CategoriaId = categoria.CategoriaId;
                evento.Ubicacion = ubicacion;
                evento.UbicacionId = ubicacion.UbicacionId;
                return evento;
            },
                splitOn: "userId, categoriaId, ubicacionId"
            );

            string sqlSeccion;
            IEnumerable<Secciones> secciones;
            foreach (var evento in eventos)
            {
                sqlSeccion = $"SELECT * FROM dSecciones WHERE EventoId = {evento.EventoId} ORDER BY SeccionId";
                secciones = await connection.QueryAsync<Secciones>(sqlSeccion);
                evento.Secciones.AddRange(secciones);
            }
            string sqlBoletos;
            IEnumerable<Boletos> boletos;
            foreach (var evento in eventos)
            {
                sqlBoletos = $"SELECT * FROM boletos WHERE EventoId = {evento.EventoId}";
                boletos = await connection.QueryAsync<Boletos>(sqlBoletos);
                evento.Boletos.AddRange(boletos);
            }
            return Ok(eventos);
        }

        /* GET LIST BY USERID */
        /// <summary>
        /// This method return all the events performed for the user that match with the given UserId
        /// </summary>
        /// <param name="UserId">The specific categoryId that matches the category in which the events you want to find are located</param>
        /// <param name="pages">The numbers of pages that you want to charge.</param>
        /// <returns>All non-ocurring events, as a list, that their categoryId matches with the given parameter.</returns>
        [HttpGet("byUserId/{UserId}")]
        public async Task<ActionResult<List<Eventos>>> GetEventosByUserId(int UserId, int pages = 1)
        {
            using var connection = new MySqlConnection(connectionString);
            var sql = @$"SELECT 
                            eventoid, e.boletosDisponibles, e.fechaEvento, nombreevento, e.descripcion,
                            e.userId, us.userId, us.usernombre, us.useremail,
                            e.categoriaId, ce.categoriaId, ce.categoria,
                            e.ubicacionId, u.ubicacionId, u.ubicacion, IFNULL(u.latitud,0) AS 'latitud', IFNULL(u.longitud,0) AS 'longitud', IFNULL(u.especificaciones,'') AS 'especificaciones'
                        FROM Eventos AS E
                        LEFT JOIN BPF_Usuarios AS Us ON Us.userid = e.userid                         
                        INNER JOIN categoriaeventos AS ce ON ce.categoriaId = E.categoriaId
                        INNER JOIN Ubicacion AS u ON u.ubicacionId = e.ubicacionId
                        WHERE FechaEvento > Current_Date() AND e.userId = {UserId}
                        LIMIT {15 * pages};";

            var eventos = await connection.QueryAsync<Eventos, Usuarios, CategoriaEventos, Ubicaciones, Eventos>(sql, (evento, user, categoria, ubicacion) =>
            {
                evento.Creador = user;
                evento.UserId = user.UserId;
                evento.CategoriaEventos = categoria;
                evento.CategoriaId = categoria.CategoriaId;
                evento.Ubicacion = ubicacion;
                evento.UbicacionId = ubicacion.UbicacionId;
                return evento;
            },
                splitOn: "userId, categoriaId, ubicacionId"
            );

            string sqlSeccion;
            IEnumerable<Secciones> secciones;
            foreach (var evento in eventos)
            {
                sqlSeccion = $"SELECT * FROM dSecciones WHERE EventoId = {evento.EventoId} ORDER BY SeccionId";
                secciones = await connection.QueryAsync<Secciones>(sqlSeccion);
                evento.Secciones.AddRange(secciones);
            }
            string sqlBoletos;
            IEnumerable<Boletos> boletos;
            foreach (var evento in eventos)
            {
                sqlBoletos = $"SELECT * FROM boletos WHERE EventoId = {evento.EventoId}";
                boletos = await connection.QueryAsync<Boletos>(sqlBoletos);
                evento.Boletos.AddRange(boletos);
            }
            return Ok(eventos);
        }

        #endregion

        #region UPDATES & CREATES

        [HttpPost]
        public async Task<ActionResult<Eventos>> AddEvento(AddEventoDTO e)
        {
            using var connection = new MySqlConnection(connectionString);

            var d = e.fechaEvento;

            var fecha = $"{d.Year}-{d.Month}-{d.Day}";

            var sql = $@"INSERT INTO Eventos (NombreEvento, Descripcion, userId, CategoriaId, UbicacionId, FechaEvento, BoletosDisponibles)
                        VALUES ({e.NombreEvento.ToSqlString()},{e.Descripcion.ToSqlString()},{e.UserId},{e.CategoriaId},{e.ubicacionId}, {fecha.ToSqlString()},{e.Boletos})";
            
            var eventosGuardados = await connection.ExecuteAsync(sql);

            var lastInsertedId = connection.QuerySingle<int>("SELECT LAST_INSERT_ID()");
            var lastDetailId = 1;
            var sumatoria = 0;
            foreach (var detalle in e.Secciones)
            {
                
                var detallesQuery = "INSERT INTO dSecciones (eventoId, Precio, seccion) " +
                                    "VALUES (@eventoId, @Precio, @seccion)";
                sumatoria += await connection.ExecuteAsync(detallesQuery, new
                {
                    eventoId = lastInsertedId,
                    Precio = detalle.Precio,
                    seccion = detalle.SeccionNombre
                });
                lastDetailId = connection.QuerySingle<int>("SELECT LAST_INSERT_ID()");
                var boletoQuery = $"INSERT INTO Boletos (SeccionId, EventoId) VALUES ({lastDetailId},{lastInsertedId})";
                await connection.ExecuteAsync(boletoQuery);
            }
            return Ok(eventosGuardados + sumatoria == e.Secciones.Count+1);
        }

        #endregion

        #endregion
    }
}
