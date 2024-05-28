using System.Data;
using Microsoft.Data.SqlClient;
using NamespaceGPT.Common.ConfigurationManager;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories.Interfaces;

namespace NamespaceGPT.Data.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private readonly string connectionString;

        public ListingRepository(IConfigurationManager configurationManager)
        {
            connectionString = configurationManager.GetConnectionString("appsettings.json");
        }

        public int AddListing(Listing listing)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO Listing (product, marketplace,price) VALUES (@product, @marketplace, @price); SELECT SCOPE_IDENTITY()";

            command.Parameters.AddWithValue("@product", listing.ProductId);
            command.Parameters.AddWithValue("@marketplace", listing.MarketplaceId);
            command.Parameters.AddWithValue("@price", listing.Price);

            int newListingId = Convert.ToInt32(command.ExecuteScalar());

            return newListingId;
        }

        public bool DeleteListing(int id)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "DELETE FROM Listing WHERE id = @id";

            command.Parameters.AddWithValue("@id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public IEnumerable<Listing> GetAllListings()
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM Listing";

            SqlDataReader reader = command.ExecuteReader();
            List<Listing> listings = new List<Listing>();

            while (reader.Read())
            {
                Listing listing = new ()
                {
                    Id = reader.GetInt32(0),
                    ProductId = reader.GetInt32(1),
                    MarketplaceId = reader.GetInt32(2),
                    Price = reader.GetInt32(3),
                };

                listings.Add(listing);
            }

            return listings;
        }

        public IEnumerable<Listing> GetAllListingsOfProduct(int productId)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM Listing WHERE product = @productId";

            command.Parameters.AddWithValue("@productId", productId);

            SqlDataReader reader = command.ExecuteReader();
            List<Listing> listings = new List<Listing>();

            while (reader.Read())
            {
                Listing listing = new ()
                {
                    Id = reader.GetInt32(0),
                    ProductId = reader.GetInt32(1),
                    MarketplaceId = reader.GetInt32(2),
                    Price = reader.GetInt32(3),
                };

                listings.Add(listing);
            }

            return listings;
        }

        public Listing? GetListing(int id)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM Listing WHERE id = @id";

            command.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Listing listing = new ()
                {
                    Id = reader.GetInt32(0),
                    ProductId = reader.GetInt32(1),
                    MarketplaceId = reader.GetInt32(2),
                    Price = reader.GetInt32(3)
                };

                return listing;
            }

            return null;
        }

        public bool UpdateListing(int id, Listing listing)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "UPDATE Listing " +
                                    "SET product = @product, marketplace = @marketplace, price=@price " +
                                    "WHERE id = @id";

            command.Parameters.AddWithValue("@product", listing.ProductId);
            command.Parameters.AddWithValue("@marketplace", listing.MarketplaceId);
            command.Parameters.AddWithValue("@price", listing.Price);
            command.Parameters.AddWithValue("@id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}
