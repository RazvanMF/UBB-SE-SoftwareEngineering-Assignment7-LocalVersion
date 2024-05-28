using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using NamespaceGPT.Api.Controllers;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.WPF.ProductPages
{
    public partial class ProductsByNameSearchPage : UserControl
    {
        public List<Product> Products { get; set; }
        private readonly ProductController productController;
        public ICommand ProductButtonCommand { get; set; } = new RelayCommand<int>(ProductButton_Click);
        public ProductsByNameSearchPage(string keyword)
        {
            productController = Controller.GetInstance().ProductController;

            Products = productController.GetAllProducts()
                .Where(p => p.Name.Contains(keyword, StringComparison.CurrentCultureIgnoreCase))
                .ToList();

            DataContext = this;
            InitializeComponent();

            ResultsLabel.Content = "Results for: " + keyword;
        }

        private static void ProductButton_Click(int itemId)
        {
            ProductPage productPage = new (itemId);
            Session.GetInstance().Frame.NavigationService.Navigate(productPage);
        }
    }
}
