using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NamespaceGPT.Api.Controllers;
namespace NamespaceGPT.WPF.Admin
{
    public partial class ProductsView : UserControl
    {
        private readonly ProductController productController;

        public ProductsView()
        {
            productController = Controller.GetInstance().ProductController;

            InitializeComponent();

            ProductsDataGrid.ItemsSource = productController.GetAllProducts();
        }
    }
}
