using System.Collections.ObjectModel;
using System.Windows.Controls;
using NamespaceGPT.Api.Controllers;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.WPF.ProductPages
{
    public partial class FavouriteProductsView : UserControl
    {
        private readonly ProductController productController;
        private readonly FavouriteProductController favouriteProductController;
        private readonly int userId;

        public ObservableCollection<Product> FavouriteProducts { get; set; } = new ObservableCollection<Product>();

        public FavouriteProductsView(int userId)
        {
            this.userId = userId;
            productController = Controller.GetInstance().ProductController;
            favouriteProductController = Controller.GetInstance().FavouriteProductController;
            InitializeFavouriteProductsList();

            InitializeComponent();

            DataContext = this;
        }

        private void InitializeFavouriteProductsList()
        {
            FavouriteProducts = new ObservableCollection<Product>();

            var productsIds = new List<int>();

            foreach (var item in favouriteProductController.GetAllFavouriteProductsOfUser(userId))
            {
                productsIds.Add(item.ProductId);
            }

            foreach (var item in productsIds)
            {
                Product? product = productController.GetProduct(item);

                if (product != null)
                {
                    FavouriteProducts.Add(product);
                }
            }
        }
    }
}
