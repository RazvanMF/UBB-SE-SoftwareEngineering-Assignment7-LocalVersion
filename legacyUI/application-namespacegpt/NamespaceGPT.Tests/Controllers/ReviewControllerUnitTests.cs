using System.Diagnostics;
using System.Data;
using NamespaceGPT.Api.Controllers;
using NamespaceGPT.Business.Services;
using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Repositories;
using NamespaceGPT.Data.Repositories.Interfaces;
using NamespaceGPT.Common.ConfigurationManager;
using NamespaceGPT.Data.Models;
using Microsoft.Data.SqlClient;

namespace NamespaceGPT.UnitTesting.Controllers
{
    [TestClass]
    public class ReviewControllerUnitTests
    {
        [TestInitialize]
        public void Initialize()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            using SqlConnection connection = new (configurationManager.GetConnectionString("appsettings.json"));
            connection.Open();

            SqlCommand truncateCommand = connection.CreateCommand();
            truncateCommand.CommandType = CommandType.Text;
            truncateCommand.CommandText = "DELETE FROM Review; DBCC CHECKIDENT('Review', RESEED, 0);";
            // TODO: needless reseeding of ID; if it's based on properly ordered IDs, then you know that you messed up
            truncateCommand.ExecuteNonQuery();
            truncateCommand.Dispose();

            SqlCommand addReviewCommand = connection.CreateCommand();
            addReviewCommand.CommandType = CommandType.Text;
            addReviewCommand.CommandText =
                "INSERT INTO Review(productId, userId, title, description, rating) " +
                "VALUES(@productId, @userId, @title, @description, @rating);";

            addReviewCommand.Parameters.AddWithValue("@productId", 1);
            addReviewCommand.Parameters.AddWithValue("@userId", 1);
            addReviewCommand.Parameters.AddWithValue("@title", "A mediocre review");
            addReviewCommand.Parameters.AddWithValue("@description", "A review mediocre by any standards.");
            addReviewCommand.Parameters.AddWithValue("@rating", 3);

            addReviewCommand.ExecuteNonQuery();
            addReviewCommand.Dispose();

            connection.Close();
            connection.Dispose();
        }

        private ReviewController InitializeController()
        {
            // TODO: awful boilerplate; use singleton/injection!
            IConfigurationManager configurationManager = new ConfigurationManager();
            IReviewRepository reviewRepository = new ReviewRepository(configurationManager);
            IReviewService reviewService = new ReviewService(reviewRepository);
            return new ReviewController(reviewService);
        }

        [TestMethod]
        public void TestAddReview_SuccessfulAdd_ReturnsReviewID()
        {
            ReviewController controller = InitializeController();

            Review review = new Review
            {
                ProductId = 1,
                UserId = 2,
                Title = "Very Nice",
                Description = "This product is very nice",
                Rating = 5
            };

            int reviewId = controller.AddReview(review);
            Debug.Assert(reviewId == 2);

            Review? getReview = controller.GetReview(2);
            Debug.Assert(getReview != null
                && getReview.ProductId == 1
                && getReview.UserId == 2
                && getReview.Title == "Very Nice"
                && getReview.Description == "This product is very nice"
                && getReview.Rating == 5);
        }

        [TestMethod]
        public void TestGetReview_SuccessfulGet_ReturnsReview()
        {
            ReviewController controller = InitializeController();

            Review? getReview = controller.GetReview(1);
            Debug.Assert(getReview != null
                && getReview.ProductId == 1
                && getReview.UserId == 1
                && getReview.Title == "A mediocre review"
                && getReview.Description == "A review mediocre by any standards."
                && getReview.Rating == 3);
        }

        [TestMethod]
        public void TestGetReview_FailureGet_ReturnsNull()
        {
            ReviewController controller = InitializeController();

            Review? getReview = controller.GetReview(100);
            Debug.Assert(getReview == null);
        }

        [TestMethod]
        public void TestDeleteReview_SuccessfulDelete_ReturnsBooleanStateTrue()
        {
            ReviewController controller = InitializeController();

            bool deleteState = controller.DeleteReview(1);
            Assert.IsTrue(deleteState);
            Debug.Assert(controller.GetReview(1) == null);
        }

        [TestMethod]
        public void TestDeleteReview_FailureDelete_ReturnsBooleanStateFalse()
        {
            ReviewController controller = InitializeController();

            bool deleteState = controller.DeleteReview(100);
            Assert.IsFalse(deleteState);
        }

        [TestMethod]
        public void TestUpdateReview_SuccessfulUpdate_ReturnsBooleanStateTrue()
        {
            ReviewController controller = InitializeController();

            Review? currentReview = controller.GetReview(1);
            currentReview!.Description = "Updated description";

            bool updateState = controller.UpdateReview(currentReview.Id, currentReview);
            Assert.IsTrue(updateState);

            Review? getReview = controller.GetReview(1);
            Debug.Assert(getReview != null
                && getReview.ProductId == 1
                && getReview.UserId == 1
                && getReview.Title == "A mediocre review"
                && getReview.Description == "Updated description"
                && getReview.Rating == 3);
        }

        [TestMethod]
        public void TestUpdateReview_FailureUpdate_ReturnsBooleanStateFalse()
        {
            ReviewController controller = InitializeController();

            Review? currentReview = controller.GetReview(1);
            currentReview!.Description = "It doesn't really matter";

            bool updateState = controller.UpdateReview(100, currentReview);
            Assert.IsFalse(updateState);
        }

        [TestMethod]
        public void TestGetAllReviews_SuccessfulGet_ReturnsAllReviews()
        {
            ReviewController controller = InitializeController();

            List<Review> reviews = controller.GetAllReviews().ToList();
            Debug.Assert(reviews.Count == 1);
            Debug.Assert(reviews[0].Id == 1 && reviews[0].Title == "A mediocre review");
        }

        [TestMethod]
        public void TestGetReviewsForProduct_SuccessfulGet_ReturnsReviews()
        {
            ReviewController controller = InitializeController();

            List<Review> reviews = controller.GetReviewsForProduct(1).ToList();
            Debug.Assert(reviews.Count == 1);
            Debug.Assert(reviews[0].Id == 1 && reviews[0].Title == "A mediocre review");
        }

        [TestMethod]
        public void TestGetReviewsFromUser_SuccessfulGet_ReturnsReviews()
        {
            ReviewController controller = InitializeController();

            List<Review> reviews = controller.GetReviewsFromUser(1).ToList();
            Debug.Assert(reviews.Count == 1);
            Debug.Assert(reviews[0].Id == 1 && reviews[0].Title == "A mediocre review");
        }

        [ClassCleanup]
        public static void RestoreReviewData()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            using SqlConnection connection = new (configurationManager.GetConnectionString("appsettings.json"));
            connection.Open();

            SqlCommand truncateCommand = connection.CreateCommand();
            truncateCommand.CommandType = CommandType.Text;
            truncateCommand.CommandText = "DELETE FROM Review; DBCC CHECKIDENT('Review', RESEED, 0);";
            truncateCommand.ExecuteNonQuery();
            truncateCommand.Dispose();

            string restoreQuery =
                """
                INSERT INTO Review VALUES (1, 7, 'Very good', 'I like this phone', 5);
                INSERT INTO Review VALUES (2, 7, 'Bad', 'I hate this phone', 1);
                INSERT INTO Review VALUES (2, 7, 'It is ok', 'Nothin spectacular about it, average', 3);
                INSERT INTO Review VALUES (1, 1, 'Nice', 'I am happy with this phone', 4);
                """;

            SqlCommand restoreCommand = connection.CreateCommand();
            restoreCommand.CommandType = CommandType.Text;
            restoreCommand.CommandText = restoreQuery;
            restoreCommand.ExecuteNonQuery();
            restoreCommand.Dispose();

            connection.Close();
            // and as if nothing ever happened...
        }
    }
}
