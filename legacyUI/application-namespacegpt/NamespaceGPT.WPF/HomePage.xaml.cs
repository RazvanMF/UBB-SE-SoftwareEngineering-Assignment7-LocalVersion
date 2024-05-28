using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using NamespaceGPT.Api.Controllers;
using NamespaceGPT.Data.Models;
using NamespaceGPT.WPF.ProductPages;

namespace NamespaceGPT.WPF
{
    public partial class HomePage : UserControl
    {
        public List<Product> Products { get; set; } = new List<Product>();
        private readonly ProductController productController;
        public ICommand ProductButtonCommand { get; set; } = new RelayCommand<int>(ProductButton_Click);

        public HomePage()
        {
            productController = Controller.GetInstance().ProductController;
            Products = productController.GetAllProducts().ToList();

            DataContext = this;
            InitializeComponent();
        }

        private static void ProductButton_Click(int itemId)
        {
            ProductPage productPage = new (itemId);
            Session.GetInstance().Frame.NavigationService.Navigate(productPage);
        }
    }
}
