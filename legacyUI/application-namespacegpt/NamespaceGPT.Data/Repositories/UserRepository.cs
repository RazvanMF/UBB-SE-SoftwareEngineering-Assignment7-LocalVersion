using System.Data;
using Microsoft.Data.SqlClient;
using NamespaceGPT.Common.ConfigurationManager;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories.Interfaces;

namespace NamespaceGPT.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString;

        public UserRepository(IConfigurationManager configurationManager)
        {
            connectionString = configurationManager.GetConnectionString("appsettings.json");
        }

        public int AddUser(User user)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO AppUser (username, password) VALUES (@username, @password); SELECT SCOPE_IDENTITY()";

            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@password", user.Password);

            int newUserId = Convert.ToInt32(command.ExecuteScalar());

            return newUserId;
        }

        public int UserExists(User user)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT AppUser.id FROM AppUser WHERE AppUser.username = @username AND AppUser.password = @password";

            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@password", user.Password);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return reader.GetInt32(0);
            }

            return -1;
        }

        public bool DeleteUser(int id)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "DELETE FROM AppUser WHERE AppUser.id = @id";

            command.Parameters.AddWithValue("@id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public IEnumerable<User> GetAllUsers()
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM AppUser";

            SqlDataReader reader = command.ExecuteReader();
            List<User> users = new List<User>();

            while (reader.Read())
            {
                User user = new ()
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2)
                };

                users.Add(user);
            }

            return users;
        }

        public User? GetUser(int id)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM AppUser WHERE AppUser.id = @id";

            command.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                User user = new ()
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2)
                };

                return user;
            }

            return null;
        }

        public bool UpdateUser(int id, User user)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "UPDATE AppUser " +
                                    "SET AppUser.username = @username, AppUser.password = @password " +
                                    "WHERE AppUser.id = @id";

            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@password", user.Password);
            command.Parameters.AddWithValue("@id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}
