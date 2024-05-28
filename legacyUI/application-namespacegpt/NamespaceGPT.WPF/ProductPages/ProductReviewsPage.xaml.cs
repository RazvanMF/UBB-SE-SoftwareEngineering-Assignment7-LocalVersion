using System.Collections.ObjectModel;
using System.Windows.Controls;
using NamespaceGPT.Api.Controllers;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.WPF.ProductPages
{
    public partial class ProductReviewsPage : UserControl
    {
        private readonly ProductController productController;
        private readonly ReviewController reviewController;
        private readonly UserController userController;
        private readonly int productId;
        public Product? Product { get; set; }

        public ObservableCollection<dynamic> Reviews { get; set; } = new ObservableCollection<dynamic>();

        public ProductReviewsPage(int productId)
        {
            this.productId = productId;

            productController = Controller.GetInstance().ProductController;
            reviewController = Controller.GetInstance().ReviewController;
            userController = Controller.GetInstance().UserController;

            // reviewController.AddReview(new Review { ProductId = 1, UserId = 1, Rating = 3, Title = "Sanity checked", Description = "sword is nice, but i hate SE" });
            InitializeReviewsList();
            InitializeComponent();

            DataContext = this;
            Product = productController.GetProduct(productId);
        }

        private void InitializeReviewsList()
        {
            var reviews = reviewController.GetReviewsForProduct(productId);
            var users = userController.GetAllUsers();
            var updatedReviews = from review in reviews
                                 where review.ProductId == productId
                                 join user in users
                                 on review.UserId equals user.Id
                                 select new
                                 {
                                     Username = user.Username,
                                     Title = review.Title,
                                     Description = review.Description
                                 };
            Reviews.Clear();
            foreach (var review in updatedReviews)
            {
                Reviews.Add(review);
            }
        }
    }
}
