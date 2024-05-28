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
    public partial class MarketplacesView : UserControl
    {
        private readonly MarketplaceController marketplaceController;

        public MarketplacesView()
        {
            marketplaceController = Controller.GetInstance().MarketplaceController;
            InitializeComponent();

            MarketplacesDataGrid.ItemsSource = marketplaceController.GetAllMarketplaces();
        }
    }
}
