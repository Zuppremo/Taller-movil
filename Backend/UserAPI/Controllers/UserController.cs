using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data.Common;
using UserAPI.Data;
using UserAPI.Models;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        // GET: api/<UserController>
        [HttpGet]
        public async Task<IEnumerable<User>> GetAllUsers([FromServices] MySqlDataSource db)
        {
            var repository = new UserRepository(db);
            var result = await repository.GetAllUsersAsync();
            return result;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<User?> Get([FromServices] MySqlDataSource db, int id)
        {
            var repository = new UserRepository(db);
            var result = await repository.FindOneAsync(id);
            return result;
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<User> Post([FromServices] MySqlDataSource db, [FromBody] User body)
        {
            var repository = new UserRepository(db);
            await repository.InsertAsync(body);
            return body;
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<User> Put(int id, [FromServices] MySqlDataSource db, [FromBody]User body)
        {
            var repository = new UserRepository(db);
            var result = await repository.FindOneAsync(id);
            if (result == null)
                return null;
            result.Name = body.Name;
            result.Email = body.Email;
            result.Password = body.Password;
            await repository.UpdateAsync(result);
            return result;
        }
        
        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<User> Delete([FromServices] MySqlDataSource db, int id)
        {
            var repository = new UserRepository(db);
            var result = await repository.FindOneAsync(id);
            if (result == null)
                return null;
            await repository.DeleteAsync(result);
            return result;
        }
        

        [HttpPost("Login")]
        public async Task<ResponseLogin> UserLogin([FromBody] RequestLogin requestLogin)
        {
            ResponseLogin userRequest = new ResponseLogin();
            await using var connection = new MySqlConnection("Server=localhost;Port=3306;User ID=root;Password=admin;Database=movildb");
            await connection.OpenAsync();
            string sqlEmailVerification = @"SELECT id_user FROM user WHERE user_email = @user_email;";
            MySqlCommand command = new MySqlCommand(sqlEmailVerification, connection);
            command.Parameters.AddWithValue("user_email", requestLogin.Email);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                userRequest.Id = reader.GetInt32(0);
            await connection.CloseAsync();
            await connection.OpenAsync();
            if (userRequest.Id > 0)
            {
                string sqlPasswordVerification = @"SELECT user_password FROM user WHERE id_user = @id;";
                MySqlCommand commandPassword = new MySqlCommand(sqlPasswordVerification, connection);
                commandPassword.Parameters.AddWithValue("id", userRequest.Id);
                await using var reader2 = await commandPassword.ExecuteReaderAsync();
                var temporaryPassword = string.Empty;
                while (await reader2.ReadAsync())
                    temporaryPassword = reader2.GetString(0);

                if (requestLogin.Password == temporaryPassword) 
                {
                    string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    userRequest.LoginToken = token;
                    userRequest.Status = "Ok";
                    userRequest.ResponseData = new ResponseData(200, "Sucess, correct password");
                    Console.WriteLine("It's correct");
                }
                else
                {
                    userRequest.Status = "Error";
                    userRequest.ResponseData = new ResponseData(500, "Incorrect Password");
                    Console.WriteLine("Wrong password");
                }
            }
            else
            {
                userRequest.Status = "Error";
                userRequest.ResponseData = new ResponseData(500, "Email not registered");
                Console.WriteLine("No existe ese email");
            }
            await connection.CloseAsync();
            return userRequest;
        }
    }
}
