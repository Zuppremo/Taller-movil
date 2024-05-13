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
        public async Task<ResponseLogin> UserLogin([FromServices] MySqlDataSource db,[FromBody] RequestLogin requestLogin)
        {
            var repository = new UserRepository(db);
            ResponseLogin response = new ResponseLogin();
            var userId = await repository.FindExistingEmail(requestLogin);
            if (userId > 0)
            {
                var correctPassword = await repository.GetUserPasswordAsync(userId);
                Console.WriteLine("Email Exists");
                if(correctPassword == requestLogin.Password)
                {
                    string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    response.LoginToken = token;
                    response.Status = "Ok";
                    response.ResponseData = new ResponseData(200, "Sucess, correct password");
                    Console.WriteLine("It's correct");
                }
                else
                {
                    response.Status = "Error";
                    response.ResponseData = new ResponseData(500, "Incorrect Password");
                    Console.WriteLine("Wrong password");
                }
            }
            else 
            {
                ResponseData responseData = new ResponseData(404, "Email doesn't exist");
                response.LoginToken = string.Empty;
                response.Status = "Error";
                response.ResponseData = responseData;
            }
            return response;
        }
    }
}
