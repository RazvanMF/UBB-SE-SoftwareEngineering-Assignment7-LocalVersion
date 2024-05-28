using System.Windows;
using NamespaceGPT.Api.Controllers;

namespace NamespaceGPT.WPF.Admin
{
    public partial class AdminDashboard : Window
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }

        private void ShowUsers_Click(object sender, RoutedEventArgs e)
        {
            UsersView usersView = new ();
            MainFrame.NavigationService.Navigate(usersView);
        }

        private void ShowProducts_Click(object sender, RoutedEventArgs e)
        {
            ProductsView productsView = new ();
            MainFrame.NavigationService.Navigate(productsView);
        }

        private void ShowListings_Click(object sender, RoutedEventArgs e)
        {
            ListingsView listingsView = new ();
            MainFrame.NavigationService.Navigate(listingsView);
        }

        private void ShowMarketplaces_Click(object sender, RoutedEventArgs e)
        {
            MarketplacesView marketplacesView = new ();
            MainFrame.NavigationService.Navigate(marketplacesView);
        }

        private void ShowReviews_Click(object sender, RoutedEventArgs e)
        {
            ReviewsView reviewsView = new ();
            MainFrame.NavigationService.Navigate(reviewsView);
        }
    }
}
