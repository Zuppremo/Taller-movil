using Microsoft.AspNetCore.Mvc;
using UserAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
            User user1 = new User();
            user1.Id = 0;
            user1.Name = "Juan Alberto";
            user1.Email = "juanalberto@gmail.com";
            user1.Password = "123456";
            users.Add(user1);
            User user2 = new User();
            user2.Id = 1;
            user2.Name = "Alberto Rey";
            user2.Email = "albertoking@gmail.com";
            user2.Password = "254535";
            users.Add(user2);
            User user3 = new User();
            user3.Id = 2;
            user3.Name = "Juana De Arco";
            user3.Email = "jaunitadeark@gmail.com";
            user3.Password = "24349495";
            users.Add(user3);
            return users;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            List<User> users = new List<User>();
            User user1 = new User();
            user1.Id = 0;
            user1.Name = "Juan Alberto";
            user1.Email = "juanalberto@gmail.com";
            user1.Password = "123456";
            users.Add(user1);
            User user2 = new User();
            user2.Id = 1;
            user2.Name = "Alberto Rey";
            user2.Email = "albertoking@gmail.com";
            user2.Password = "254535";
            users.Add(user2);
            return users[id].Name;
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<User>Post([FromBody] int id, string name, string email, string password)
        {
            User userToCreate = new User();
            userToCreate.Id = id;
            userToCreate.Name = name;
            userToCreate.Email = email;
            userToCreate.Password = password;
            return userToCreate;
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<string> Delete(int id)
        {
            List<User> users = new List<User>();
            User user1 = new User();
            user1.Id = 0;
            user1.Name = "Juan Alberto";
            user1.Email = "juanalberto@gmail.com";
            user1.Password = "123456";
            users.Add(user1);
            User user2 = new User();
            user2.Id = 1;
            user2.Name = "Alberto Rey";
            user2.Email = "albertoking@gmail.com";
            user2.Password = "254535";
            users.Add(user2);
            string removedUser = users[id].Name;
            users.Remove(users[id]);
            return $"Removed user was: {removedUser}";
        }
    }
}
