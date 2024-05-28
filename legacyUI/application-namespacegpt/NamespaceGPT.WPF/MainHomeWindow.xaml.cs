using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using NamespaceGPT.Api.Controllers;
using NamespaceGPT.Data.Models;
using NamespaceGPT.WPF.Authentication;
using NamespaceGPT.WPF.ProductPages;

namespace NamespaceGPT.WPF
{
    public partial class MainHomeWindow : Window
    {
        private readonly ProductController productController;
        public List<string> Categories { get; set; }
        public ICommand DisplayProductsByCategoryButtonCommand { get; set; } = new RelayCommand<string>(CategoryButton_Click);
        public MainHomeWindow()
        {
            InitializeComponent();

            productController = Controller.GetInstance().ProductController;
            Categories = GetCategories().Take(3).ToList();

            DataContext = this;

            HomePage homepage = new ();
            MainFrame.NavigationService.Navigate(homepage);
        }

        public List<string> GetCategories()
        {
            List<Product> products = productController.GetAllProducts().ToList();
            HashSet<string> categories = new HashSet<string>();
            foreach (Product product in products)
            {
                categories.Add(product.Category);
            }
            return categories.ToList();
        }

        private static void CategoryButton_Click(string category)
        {
            ProductsByCategoryPage productsfilteredPage = new (category);
            Session.GetInstance().Frame.NavigationService.Navigate(productsfilteredPage);
        }

        private void AllCategoriesButton_Click(object sender, RoutedEventArgs e)
        {
            CategoriesDisplayPage categoriesDisplayPage = new ();
            Session.GetInstance().Frame.NavigationService.Navigate(categoriesDisplayPage);
        }

        private void FavouriteProductsButton_Click(object sender, RoutedEventArgs e)
        {
            FavouriteProductsView favouriteProductsView = new (Session.GetInstance().UserId);
            Session.GetInstance().Frame.NavigationService.Navigate(favouriteProductsView);
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            Login loginPage = new ();
            loginPage.Show();
            Close();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text;

            ProductsByNameSearchPage productsByNameSearchPage = new (searchText);
            Session.GetInstance().Frame.NavigationService.Navigate(productsByNameSearchPage);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomePage homepage = new ();
            MainFrame.NavigationService.Navigate(homepage);
        }
    }
}
