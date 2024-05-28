using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NamespaceGPT.Api.Controllers;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.WPF.ProductPages
{
    public partial class ProductPage : UserControl
    {
        private readonly int productId;
        public Product? Product { get; set; }
        private readonly ListingController listingController;
        private readonly ProductController productController;
        private readonly MarketplaceController marketplaceController;

        public ObservableCollection<Marketplace> Marketplaces { get; set; } = new ObservableCollection<Marketplace>();

        public ProductPage(int productId)
        {
            this.productId = productId;
            listingController = Controller.GetInstance().ListingController;
            productController = Controller.GetInstance().ProductController;
            marketplaceController = Controller.GetInstance().MarketplaceController;
            Product = productController.GetProduct(this.productId);

            DataContext = this;

            InitializeComponent();
            InitializeProductDetails();
        }

        private void InitializeProductDetails()
        {
            // Get price
            int lowestPrice = int.MaxValue;
            List<Listing> listings = listingController.GetAllListingsOfProduct(productId).ToList();

            foreach (Listing listing in listings)
            {
                if (listing.Price < lowestPrice)
                {
                    lowestPrice = listing.Price;
                    break;
                }
            }
            if (lowestPrice < int.MaxValue)
            {
                ProductMinPrice.Text = lowestPrice.ToString();
            }
            else
            {
                ProductMinPrice.Text = "Unknown";
            }

            var marketplaces = marketplaceController.GetAllMarketplaces().Where(marketplace =>
            {
                foreach (Listing listing in listings)
                {
                    if (listing.MarketplaceId == marketplace.Id)
                    {
                        return true;
                    }
                }

                return false;
            });

            foreach (Marketplace marketplace in marketplaces)
            {
                Marketplaces.Add(marketplace);
            }
        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (Product == null)
            {
                return;
            }

            ProductReviewsPage productReviewsPage = new (Product.Id);
            Session.GetInstance().Frame.NavigationService.Navigate(productReviewsPage);
        }

        private void CompareButton_Click(object sender, RoutedEventArgs e)
        {
            // For demonstration purpose only...
            Product? secondProduct = productController.GetProduct(2);

            if (Product == null || secondProduct == null)
            {
                return;
            }

            CompareProductsView compareProductsView = new (Product, secondProduct);
            Session.GetInstance().Frame.NavigationService.Navigate(compareProductsView);
        }

        private void FavouriteButton_Click(object sender, RoutedEventArgs e)
        {
            var favouriteProductsOfUser = Controller.GetInstance().FavouriteProductController
                .GetAllFavouriteProductsOfUser(Session.GetInstance().UserId);

            foreach (FavouriteProduct existingFavouriteProduct in favouriteProductsOfUser)
            {
                if (existingFavouriteProduct.ProductId == productId)
                {
                    MessageBox.Show("That product is already marked as favourite!");
                    return;
                }
            }

            FavouriteProduct newFavouriteProduct = new ()
            {
                UserId = Session.GetInstance().UserId,
                ProductId = productId
            };

            Controller.GetInstance().FavouriteProductController.AddFavouriteProduct(newFavouriteProduct);
            MessageBox.Show("Successfully added to favourites!");
        }
    }
}
