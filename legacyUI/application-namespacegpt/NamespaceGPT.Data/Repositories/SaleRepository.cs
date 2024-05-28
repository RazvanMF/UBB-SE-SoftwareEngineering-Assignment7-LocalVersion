using System.Data;
using Microsoft.Data.SqlClient;
using NamespaceGPT.Common.ConfigurationManager;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories.Interfaces;

namespace NamespaceGPT.Data.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly string connectionString;

        public SaleRepository(IConfigurationManager configurationManager)
        {
            connectionString = configurationManager.GetConnectionString("appsettings.json");
        }

        public int AddSale(Sale sale)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO Sale (buyerId, listingId) VALUES (@buyerId, @listingId); SELECT SCOPE_IDENTITY()";

            command.Parameters.AddWithValue("@buyerId", sale.BuyerId);
            command.Parameters.AddWithValue("@listingId", sale.ListingId);

            int newSaleId = Convert.ToInt32(command.ExecuteScalar());

            return newSaleId;
        }

        public bool DeleteSale(int id)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "DELETE FROM Sale WHERE Sale.id = @id";

            command.Parameters.AddWithValue("@id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public IEnumerable<Sale> GetAllPurchasesOfUser(int userId)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM Sale WHERE Sale.buyerId = @userId";

            command.Parameters.AddWithValue("@userId", userId);

            SqlDataReader reader = command.ExecuteReader();
            List<Sale> sales = new List<Sale>();

            while (reader.Read())
            {
                Sale sale = new ()
                {
                    Id = reader.GetInt32(0),
                    BuyerId = reader.GetInt32(1),
                    ListingId = reader.GetInt32(2)
                };

                sales.Add(sale);
            }

            return sales;
        }

        public IEnumerable<Sale> GetAllSales()
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM Sale";

            SqlDataReader reader = command.ExecuteReader();
            List<Sale> sales = new List<Sale>();

            while (reader.Read())
            {
                Sale sale = new ()
                {
                    Id = reader.GetInt32(0),
                    BuyerId = reader.GetInt32(1),
                    ListingId = reader.GetInt32(2)
                };

                sales.Add(sale);
            }

            return sales;
        }

        public IEnumerable<Sale> GetAllSalesOfListing(int listingId)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM Sale WHERE Sale.listingId = @listingId";

            command.Parameters.AddWithValue("@listingId", listingId);

            SqlDataReader reader = command.ExecuteReader();
            List<Sale> sales = new List<Sale>();

            while (reader.Read())
            {
                Sale sale = new ()
                {
                    Id = reader.GetInt32(0),
                    BuyerId = reader.GetInt32(1),
                    ListingId = reader.GetInt32(2)
                };

                sales.Add(sale);
            }

            return sales;
        }

        public Sale? GetSale(int id)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM Sale WHERE Sale.id = @id";

            command.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Sale sale = new ()
                {
                    Id = reader.GetInt32(0),
                    BuyerId = reader.GetInt32(1),
                    ListingId = reader.GetInt32(2)
                };

                return sale;
            }

            return null;
        }

        public bool UpdateSale(int id, Sale sale)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "UPDATE Sale " +
                                    "SET Sale.buyerId = @userId, Sale.listingId = @listingId " +
                                    "WHERE Sale.id = @id";

            command.Parameters.AddWithValue("@userId", sale.BuyerId);
            command.Parameters.AddWithValue("@listingId", sale.ListingId);
            command.Parameters.AddWithValue("@id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}
