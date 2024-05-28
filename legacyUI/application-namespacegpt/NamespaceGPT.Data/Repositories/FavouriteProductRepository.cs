using System.Data;
using Microsoft.Data.SqlClient;
using NamespaceGPT.Common.ConfigurationManager;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories.Interfaces;

namespace NamespaceGPT.Data.Repositories
{
    public class FavouriteProductRepository : IFavouriteProductRepository
    {
        private readonly string connectionString;
        private readonly IConfigurationManager configurationManager;

        public FavouriteProductRepository(IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
            connectionString = this.configurationManager.GetConnectionString("appsettings.json");
        }

        public int AddFavouriteProduct(FavouriteProduct favouriteProduct)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO FavouriteProduct (userId, productId) VALUES (@userId, @productId); SELECT SCOPE_IDENTITY()";

            command.Parameters.AddWithValue("@userId", favouriteProduct.UserId);
            command.Parameters.AddWithValue("@productId", favouriteProduct.ProductId);

            int newFavouriteProductId = Convert.ToInt32(command.ExecuteScalar());

            return newFavouriteProductId;
        }

        public bool DeleteFavouriteProductFromUser(FavouriteProduct favouriteProduct)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "DELETE FROM FavouriteProduct WHERE FavouriteProduct.userId = @userId AND FavouriteProduct.productId = @productId";

            command.Parameters.AddWithValue("@userId", favouriteProduct.UserId);
            command.Parameters.AddWithValue("@productId", favouriteProduct.ProductId);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public IEnumerable<FavouriteProduct> GetAllFavouriteProducts()
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM FavouriteProduct";

            SqlDataReader reader = command.ExecuteReader();
            List<FavouriteProduct> favouriteProducts = new List<FavouriteProduct>();

            while (reader.Read())
            {
                FavouriteProduct favouriteProduct = new ()
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    ProductId = reader.GetInt32(2)
                };

                favouriteProducts.Add(favouriteProduct);
            }

            return favouriteProducts;
        }

        public IEnumerable<FavouriteProduct> GetAllFavouriteProductsOfUser(int userId)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM FavouriteProduct WHERE FavouriteProduct.userId = @userId";

            command.Parameters.AddWithValue("@userId", userId);

            SqlDataReader reader = command.ExecuteReader();
            List<FavouriteProduct> favouriteProducts = new List<FavouriteProduct>();

            while (reader.Read())
            {
                FavouriteProduct favouriteProduct = new ()
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    ProductId = reader.GetInt32(2)
                };

                favouriteProducts.Add(favouriteProduct);
            }

            return favouriteProducts;
        }

        public IEnumerable<int> GetAllUserIdsWhoMarkedProductAsFavourite(int productId)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT FavouriteProduct.userId FROM FavouriteProduct WHERE FavouriteProduct.productId = @productId";

            command.Parameters.AddWithValue("@productId", productId);

            SqlDataReader reader = command.ExecuteReader();
            List<int> userIds = new List<int>();

            while (reader.Read())
            {
                userIds.Add(reader.GetInt32(0));
            }

            return userIds;
        }

        public FavouriteProduct? GetFavouriteProduct(int id)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM FavouriteProduct WHERE FavouriteProduct.id = @id";

            command.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                FavouriteProduct favouriteProduct = new ()
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    ProductId = reader.GetInt32(2)
                };

                return favouriteProduct;
            }

            return null;
        }
    }
}
