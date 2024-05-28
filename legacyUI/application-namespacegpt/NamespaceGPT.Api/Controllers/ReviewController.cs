using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Api.Controllers
{
    public class ReviewController
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
        }

        public int AddReview(Review review)
        {
            return reviewService.AddReview(review);
        }

        public bool DeleteReview(int id)
        {
            return reviewService.DeleteReview(id);
        }

        public IEnumerable<Review> GetAllReviews()
        {
            return reviewService.GetAllReviews();
        }

        public Review? GetReview(int id)
        {
            return reviewService.GetReview(id);
        }

        public IEnumerable<Review> GetReviewsForProduct(int productId)
        {
            return reviewService.GetReviewsForProduct(productId);
        }

        public IEnumerable<Review> GetReviewsFromUser(int userId)
        {
            return reviewService.GetReviewsFromUser(userId);
        }

        public bool UpdateReview(int id, Review review)
        {
            return reviewService.UpdateReview(id, review);
        }
    }
}
