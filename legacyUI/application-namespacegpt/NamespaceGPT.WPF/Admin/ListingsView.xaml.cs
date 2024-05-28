using System.Windows.Controls;
using NamespaceGPT.Api.Controllers;

namespace NamespaceGPT.WPF.Admin
{
    public partial class ListingsView : UserControl
    {
        private readonly ListingController listingController;

        public ListingsView()
        {
            listingController = Controller.GetInstance().ListingController;
            InitializeComponent();

            ListingsDataGrid.ItemsSource = listingController.GetAllListings();
        }
    }
}
