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
    public class UsuarioController : ControllerBase
    {
        #region PROPS & CONSTRUCTORS
        private readonly IConfiguration config;

        public UsuarioController(IConfiguration _config)
        {
            this.config = _config;
        }

        #endregion

        #region USERS API CALLS 
        #region GETS

        /* LOG IN */
        /// <summary>
        /// This method allow the user login 
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="userClave">User Password</param>
        /// <returns>Returns the an User if was succesfully logged, and null if not.</returns>
        [HttpGet]
        public async Task<ActionResult<Usuarios>> Login(string userName, string userClave)
        {
            using var connection = new MySqlConnection(config.GetConnectionString("RemoteConnection"));

            var query = @$"SELECT
                                *
                            FROM BPF_Usuarios
                            WHERE USerNombre = {userName.ToSqlString()} AND UserClave = {userClave.ToSqlString()}";

            var user = await connection.QueryAsync<Usuarios>(query);

            if(user.Count() == 0)
                return NotFound();

            return Ok(user);
        }


        /* UNIQUE EMAIL VERIFICATION */
        [HttpGet("/UserEmailExists")]
        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            using var connection = new MySqlConnection(config.GetConnectionString("RemoteConnection"));

            var query = @$"SELECT
                                COUNT(*)
                            FROM BPF_Usuarios
                            WHERE UserEmail LIKE {email.ToSqlLikeString()}";

            await connection.OpenAsync();
            var userCount = (long)await connection.ExecuteScalarAsync(query);

            return Ok(userCount > 0);
        }


        #endregion
        #region CREATES & UPDATES

        /* SIGN UP */
        /// <summary>
        /// This method register a new user
        /// </summary>
        /// <param name="newUser">The new user data</param>
        /// <returns>Returns true if the creation was success and false if not.</returns>
        [HttpPost]
        public async Task<ActionResult<Usuarios>> SingUp(NewUserDTO newUser)
        {
            using var connection = new MySqlConnection(config.GetConnectionString("RemoteConnection"));

            var query = @$"INSERT INTO  BPF_Usuarios 
                            (UserNombre,UserEmail,UserClave)
                            VALUES ({newUser.UserName.ToSqlString()},{newUser.UserEmail.ToSqlString()},{newUser.UserClave.ToSqlString()})";

            var user = await connection.ExecuteAsync(query);

            return Ok(user>0);
        }
        #endregion

        #endregion

    }
}
