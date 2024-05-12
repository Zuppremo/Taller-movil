using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
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
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            List<User> users = new List<User>();
            using var connection = new MySqlConnection("Server=localhost;Port=3306;User ID=root;Password=admin;Database=movildb");
            await connection.OpenAsync();
            using var command = new MySqlCommand("SELECT * FROM user;", connection);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                User user = new User();
                user.Id = reader.GetInt32(0);
                user.Name = reader.GetString(1);
                user.Email = reader.GetString(2);
                user.Password = reader.GetString(3);
                users.Add(user);
            }
            await connection.CloseAsync();
            return users;
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
        public async Task<User> Post(string name, string email, string password)
        {
            await using var connection = new MySqlConnection("Server=localhost;Port=3306;User ID=root;Password=admin;Database=movildb");
            await connection.OpenAsync();
            string sql = @"INSERT INTO user (user_name, user_email, user_password) VALUES (@Name, @Email, @Password);";
            User userToCreate = new User();
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Name", name);
            userToCreate.Name = name;
            command.Parameters.AddWithValue("@Email", email);
            userToCreate.Email = email;
            command.Parameters.AddWithValue("@Password", password);
            userToCreate.Password = password;
            await using var reader = await command.ExecuteReaderAsync();
            await connection.CloseAsync();
            return userToCreate;
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<User> Put(int id, string name, string email, string password)
        {
            await using var connection = new MySqlConnection("Server=localhost;Port=3306;User ID=root;Password=admin;Database=movildb");
            await connection.OpenAsync();
            string sql = @"UPDATE user SET user_name = @Name, user_email = @Email, user_password = @Password WHERE id_user = @Id";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", password);
            User user = new User();
            user.Id = id;
            user.Name = name;
            user.Email = email;
            user.Password = password;
            await using var reader = await command.ExecuteReaderAsync();
            await connection.CloseAsync();
            return user;
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<User> Delete(int id)
        {
            User userDeleted = new User();
            await using var connection = new MySqlConnection("Server=localhost;Port=3306;User ID=root;Password=admin;Database=movildb");
            await connection.OpenAsync();
            string sqlSelectUser = @"SELECT * FROM user WHERE id_user = @Id";
            MySqlCommand selectCommand = new MySqlCommand(sqlSelectUser, connection);
            selectCommand.Parameters.AddWithValue("@Id", id);
            await using var reader = await selectCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                userDeleted.Id = reader.GetInt32(0);
                userDeleted.Name = reader.GetString(1);
                userDeleted.Email = reader.GetString(2);
                userDeleted.Password = reader.GetString(3);
            }
            await connection.CloseAsync();
            await connection.OpenAsync();
            string sql = @"DELETE FROM user WHERE id_user = @Id";
            MySqlCommand deleteCommand = new MySqlCommand(sql, connection);
            deleteCommand.Parameters.AddWithValue("@Id", id);
            await using var deleteReader = await deleteCommand.ExecuteReaderAsync();
            await connection.CloseAsync();
            return userDeleted;
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
