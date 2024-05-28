using System.Data;
using Microsoft.Data.SqlClient;
using NamespaceGPT.Common.ConfigurationManager;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Data.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly string connectionString;

        public ReviewRepository(IConfigurationManager configurationManager)
        {
            connectionString = configurationManager.GetConnectionString("appsettings.json");
        }

        public int AddReview(Review review)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO Review (productId, userId, title, description, rating) VALUES (@productId, @userId, @title, @description, @rating); SELECT SCOPE_IDENTITY();";

            command.Parameters.AddWithValue("@productId", review.ProductId);
            command.Parameters.AddWithValue("@userId", review.UserId);
            command.Parameters.AddWithValue("@title", review.Title);
            command.Parameters.AddWithValue("@description", review.Description);
            command.Parameters.AddWithValue("@rating", review.Rating);

            int newReviewId = Convert.ToInt32(command.ExecuteScalar());

            return newReviewId;
        }

        public bool DeleteReview(int id)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "DELETE FROM Review WHERE Review.id = @id";

            command.Parameters.AddWithValue("@id", id);
            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public bool UpdateReview(int id, Review review)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "UPDATE Review " +
                                  "SET productId = @productId, userId = @userId, title = @title, description = @description, rating = @rating " +
                                  "WHERE id = @id";

            command.Parameters.AddWithValue("@productId", review.ProductId);
            command.Parameters.AddWithValue("@userId", review.UserId);
            command.Parameters.AddWithValue("@title", review.Title);
            command.Parameters.AddWithValue("@description", review.Description);
            command.Parameters.AddWithValue("@rating", review.Rating);
            command.Parameters.AddWithValue("@id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public IEnumerable<Review> GetAllReviews()
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM Review";

            SqlDataReader reader = command.ExecuteReader();
            List<Review> reviews = new List<Review>();

            while (reader.Read())
            {
                Review review = new ()
                {
                    Id = reader.GetInt32(0),
                    ProductId = reader.GetInt32(1),
                    UserId = reader.GetInt32(2),
                    Title = reader.GetString(3),
                    Description = reader.GetString(4),
                    Rating = reader.GetInt32(5)
                };

                reviews.Add(review);
            }

            return reviews;
        }

        public Review? GetReview(int id)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM Review WHERE Review.id = @id";

            command.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Review review = new ()
                {
                    Id = reader.GetInt32(0),
                    ProductId = reader.GetInt32(1),
                    UserId = reader.GetInt32(2),
                    Title = reader.GetString(3),
                    Description = reader.GetString(4),
                    Rating = reader.GetInt32(5)
                };

                return review;
            }

            return null;
        }

        public IEnumerable<Review> GetReviewsFromUser(int userId)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM Review WHERE userId = @userId";

            command.Parameters.AddWithValue("@userId", userId);

            SqlDataReader reader = command.ExecuteReader();
            List<Review> reviews = new List<Review>();

            while (reader.Read())
            {
                Review review = new ()
                {
                    Id = reader.GetInt32(0),
                    ProductId = reader.GetInt32(1),
                    UserId = reader.GetInt32(2),
                    Title = reader.GetString(3),
                    Description = reader.GetString(4),
                    Rating = reader.GetInt32(5)
                };

                reviews.Add(review);
            }

            return reviews;
        }

        public IEnumerable<Review> GetReviewsForProduct(int productId)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM Review WHERE productId = @productId";

            command.Parameters.AddWithValue("@productId", productId);

            SqlDataReader reader = command.ExecuteReader();
            List<Review> reviews = new List<Review>();

            while (reader.Read())
            {
                Review review = new ()
                {
                    Id = reader.GetInt32(0),
                    ProductId = reader.GetInt32(1),
                    UserId = reader.GetInt32(2),
                    Title = reader.GetString(3),
                    Description = reader.GetString(4),
                    Rating = reader.GetInt32(5)
                };

                reviews.Add(review);
            }

            return reviews;
        }
    }
}