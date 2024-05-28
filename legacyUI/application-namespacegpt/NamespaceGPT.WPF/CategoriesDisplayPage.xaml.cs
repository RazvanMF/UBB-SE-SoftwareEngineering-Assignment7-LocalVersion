using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using NamespaceGPT.Api.Controllers;
using NamespaceGPT.Data.Models;
using NamespaceGPT.WPF.ProductPages;

namespace NamespaceGPT.WPF
{
    public partial class CategoriesDisplayPage : UserControl
    {
        public ICommand DisplayProductsByCategoryButtonCommand { get; set; } = new RelayCommand<string>(DisplayProductsByCategoryButton_Click);
        private readonly ProductController productController;
        public List<string> Categories { get; set; }

        public CategoriesDisplayPage()
        {
            InitializeComponent();
            productController = Controller.GetInstance().ProductController;
            Categories = GetCategories().ToList();
            DataContext = this;
        }

        private List<string> GetCategories()
        {
            var products = productController.GetAllProducts().ToList();

            HashSet<string> categories = new HashSet<string>();
            foreach (Product product in products)
            {
                categories.Add(product.Category);
            }

            return categories.ToList();
        }

        private static void DisplayProductsByCategoryButton_Click(string category)
        {
            ProductsByCategoryPage productsfilteredPage = new (category);
            Session.GetInstance().Frame.NavigationService.Navigate(productsfilteredPage);
        }
    }
}
