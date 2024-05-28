using NamespaceGPT.Data.Models;

public interface IReviewRepository
{
    int AddReview(Review review);
    bool DeleteReview(int id);
    bool UpdateReview(int id, Review review);
    IEnumerable<Review> GetAllReviews();
    Review? GetReview(int id);
    IEnumerable<Review> GetReviewsFromUser(int userId);
    IEnumerable<Review> GetReviewsForProduct(int productId);
}
