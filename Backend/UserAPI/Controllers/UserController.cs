using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using UserAPI.Context;
using UserAPI.Models;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDataContext _context;

        public UserController(UserDataContext context)
        {
            _context = context;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = _context.Users.ToListAsync<User>().Result;
            return users;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<User> Get(int id)
        {
            var user = _context.Users.Where(selectedUser => selectedUser.Id == id).FirstOrDefault();
            return user;
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<User> Post(string name, string email, string password)
        {
            User userToCreate = new User();
            userToCreate.Name = name;
            userToCreate.Email = email;
            userToCreate.Password = password;
            _context.Add(userToCreate);
            await _context.SaveChangesAsync();
            return userToCreate;
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<User> Put(int id, User user)
        {
            if (id != user.Id)
                return null;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
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
