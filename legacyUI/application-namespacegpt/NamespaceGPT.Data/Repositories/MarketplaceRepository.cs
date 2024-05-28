using System.Data;
using Microsoft.Data.SqlClient;
using NamespaceGPT.Common.ConfigurationManager;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories.Interfaces;

namespace NamespaceGPT.Data.Repositories
{
    public class MarketplaceRepository : IMarketplaceRepository
    {
        private readonly string connectionString;

        public MarketplaceRepository(IConfigurationManager configurationManager)
        {
            connectionString = configurationManager.GetConnectionString("appsettings.json");
        }

        public int AddMarketplace(Marketplace marketplace)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO Marketplace (marketplacename,websiteurl,country) VALUES (@marketplacename, @websiteurl, @country); SELECT SCOPE_IDENTITY()";

            command.Parameters.AddWithValue("@marketplacename", marketplace.Name);
            command.Parameters.AddWithValue("@websiteurl", marketplace.WebsiteURL);
            command.Parameters.AddWithValue("@country", marketplace.CountryOfOrigin);

            int newMarketplaceId = Convert.ToInt32(command.ExecuteScalar());

            return newMarketplaceId;
        }

        public bool DeleteMarketplace(int id)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "DELETE FROM Marketplace WHERE id = @id";

            command.Parameters.AddWithValue("@id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public IEnumerable<Marketplace> GetAllMarketplaces()
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM Marketplace";

            SqlDataReader reader = command.ExecuteReader();
            List<Marketplace> marketplaces = new List<Marketplace>();

            while (reader.Read())
            {
                Marketplace marketplace = new ()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    WebsiteURL = reader.GetString(2),
                    CountryOfOrigin = reader.GetString(3)
                };

                marketplaces.Add(marketplace);
            }

            return marketplaces;
        }

        public Marketplace? GetMarketplace(int id)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM Marketplace WHERE id = @id";

            command.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Marketplace marketplace = new ()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    WebsiteURL = reader.GetString(2),
                    CountryOfOrigin = reader.GetString(3)
                };

                return marketplace;
            }

            return null;
        }

        public bool UpdateMarketplace(int id, Marketplace marketplace)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "UPDATE Marketplace " +
                                    "SET websiteurl = @websiteurl, marketplacename = @marketplacename, country=@country " +
                                    "WHERE id = @id";

            command.Parameters.AddWithValue("@marketplacename", marketplace.Name);
            command.Parameters.AddWithValue("@websiteurl", marketplace.WebsiteURL);
            command.Parameters.AddWithValue("@country", marketplace.CountryOfOrigin);
            command.Parameters.AddWithValue("@id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}
