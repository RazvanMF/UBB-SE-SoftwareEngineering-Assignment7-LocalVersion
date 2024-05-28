using System.Windows.Controls;
using NamespaceGPT.Api.Controllers;

namespace NamespaceGPT.WPF.Admin
{
    public partial class ReviewsView : UserControl
    {
        private readonly ReviewController reviewController;

        public ReviewsView()
        {
            reviewController = Controller.GetInstance().ReviewController;
            InitializeComponent();
        }

        private void IdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(IdTextBox.Text, out int id))
            {
                ReviewsDataGrid.ItemsSource = reviewController.GetReviewsForProduct(id);
            }
            else
            {
                return;
            }
        }
    }
}
