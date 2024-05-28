using System.Windows.Controls;
using NamespaceGPT.Api.Controllers;

namespace NamespaceGPT.WPF.Admin
{
    public partial class UsersView : UserControl
    {
        private readonly UserController userController;

        public UsersView()
        {
            userController = Controller.GetInstance().UserController;
            InitializeComponent();

            UsersDataGrid.ItemsSource = userController.GetAllUsers();
        }
    }
}
