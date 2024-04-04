using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
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
        public async Task<User> Get(int id)
        {
            await using var connection = new MySqlConnection("Server=localhost;Port=3306;User ID=root;Password=admin;Database=movildb");
            await connection.OpenAsync();

            using var command = new MySqlCommand(@"SELECT id_user, user_name, user_email, user_password FROM user WHERE id_user = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            await using var reader = await command.ExecuteReaderAsync();
            User user = new User();
            while (await reader.ReadAsync())
            {
                user.Id = reader.GetInt32(0);
                user.Name = reader.GetString(1);
                user.Email = reader.GetString(2);
                user.Password = reader.GetString(3);
            }
            await connection.CloseAsync();
            return user;
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

        [HttpPost("{email}, {password}")]
        public async Task<UserLogin> UserLogin(string email, string password)
        {
            UserLogin userLogin = new UserLogin();
            await using var connection = new MySqlConnection("Server=localhost;Port=3306;User ID=root;Password=admin;Database=movildb");
            await connection.OpenAsync();
            string sql = @"SELECT @user_email, @user_password FROM user;";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@user_email", email);
            command.Parameters.AddWithValue("@user_password", password);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                userLogin.Email = reader.GetString(0);
                userLogin.Password = reader.GetString(1);
            }
            await connection.CloseAsync();
            if (email == userLogin.Email && password == userLogin.Password)
            {
                string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                userLogin.LoginToken = token;
            }
            else
            {
                userLogin.LoginToken = "No access";
            }
            return userLogin;
        }
    }
}
