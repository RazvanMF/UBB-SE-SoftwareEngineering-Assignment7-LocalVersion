using System.Data;
using Microsoft.Data.SqlClient;
using NamespaceGPT.Common.ConfigurationManager;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories.Interfaces;

namespace NamespaceGPT.Data.Repositories
{
    public class UserActivityRepository : IUserActivityRepository
    {
        private readonly string connectionString;

        public UserActivityRepository(IConfigurationManager configurationManager)
        {
            connectionString = configurationManager.GetConnectionString("appsettings.json");
        }

        public int AddUserActivity(UserActivity userActivity)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO UserActivity (userId, actionType) VALUES (@userId, @actionType); SELECT SCOPE_IDENTITY()";

            command.Parameters.AddWithValue("@userId", userActivity.UserId);
            command.Parameters.AddWithValue("@actionType", userActivity.ActionType);

            int newUserActivityId = Convert.ToInt32(command.ExecuteScalar());

            return newUserActivityId;
        }

        public bool DeleteUserActivity(int id)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "DELETE FROM UserActivity WHERE UserActivity.id = @id";

            command.Parameters.AddWithValue("@id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public IEnumerable<UserActivity> GetAllUserActivities()
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM UserActivity";

            SqlDataReader reader = command.ExecuteReader();
            List<UserActivity> usersActivities = new List<UserActivity>();

            while (reader.Read())
            {
                UserActivity userActivity = new ()
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    ActionType = reader.GetString(2)
                };

                usersActivities.Add(userActivity);
            }

            return usersActivities;
        }

        public IEnumerable<UserActivity> GetUserActivitiesOfUser(int userId)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM UserActivity WHERE UserActivity.userId = @userId";

            command.Parameters.AddWithValue("@userId", userId);

            SqlDataReader reader = command.ExecuteReader();
            List<UserActivity> usersActivities = new List<UserActivity>();

            while (reader.Read())
            {
                UserActivity userActivity = new ()
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    ActionType = reader.GetString(2)
                };

                usersActivities.Add(userActivity);
            }

            return usersActivities;
        }

        public bool UpdateUserActivity(int id, UserActivity userActivity)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "UPDATE UserActivity " +
                                    "SET UserActivity.userId = @userId, UserActivity.actionType = @actionType " +
                                    "WHERE UserActivity.id = @id";

            command.Parameters.AddWithValue("@userId", userActivity.UserId);
            command.Parameters.AddWithValue("@actionType", userActivity.ActionType);
            command.Parameters.AddWithValue("@id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}
