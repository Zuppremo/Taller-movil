using MySqlConnector;
using System.Data.Common;
using UserAPI.Models;

namespace UserAPI.Data
{
    public class UserRepository(MySqlDataSource database)
    {
        
        public async Task<User?> FindOneAsync(int id)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT id_user, user_name, user_email, user_password FROM user WHERE id_user = @Id";
            command.Parameters.AddWithValue("@Id", id);
            var result = await ReadAllAsync(await command.ExecuteReaderAsync());
            return result.FirstOrDefault();
        }

        public async Task<IReadOnlyList<User>> LatestUsersAsync()
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT id_user, user_name, user_email, user_password FROM user ORDER BY `Id` DESC LIMIT 10;";
            return await ReadAllAsync(await command.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM `user`";
            await command.ExecuteNonQueryAsync();
        }

        public async Task InsertAsync(User user)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO user (user_name, user_email, user_password) VALUES (@Name, @Email, @Password);";
            BindParams(command, user);
            await command.ExecuteNonQueryAsync();
            user.Id = (int)command.LastInsertedId;
        }

        public async Task UpdateAsync(User user)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"UPDATE user SET user_name = @Name, user_email = @Email, user_password = @Password WHERE id_user = @Id";
            BindParams(command, user);
            BindId(command, user);
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(User user)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM `user` WHERE `Id` = @Id;";
            BindId(command, user);
            await command.ExecuteNonQueryAsync();
        }

        private async Task<IReadOnlyList<User>> ReadAllAsync(DbDataReader reader)
        {
            var users = new List<User>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var user = new User
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Email = reader.GetString(2),
                        Password = reader.GetString(2)
                    };
                    users.Add(user);
                }
            }
            return users;
        }

        private static void BindId(MySqlCommand cmd, User user)
        {
            cmd.Parameters.AddWithValue("@Id", user.Id);
        }

        private static void BindParams(MySqlCommand cmd, User user)
        {
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);
        }
    }

}
