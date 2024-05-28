using System.ComponentModel;
using System.Windows.Controls;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.WPF.ProductPages
{
    public partial class CompareProductsView : UserControl, INotifyPropertyChanged
    {
        public Product Product1 { get; set; }
        public Product Product2 { get; set; }
        public IDictionary<string, string> CommonAttributes1 { get; set; }
        public IDictionary<string, string> CommonAttributes2 { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public CompareProductsView(Product product1, Product product2)
        {
            InitializeComponent();
            Product1 = product1;
            Product2 = product2;

            CommonAttributes1 = new Dictionary<string, string>();
            CommonAttributes2 = new Dictionary<string, string>();

            var commonKeys = product1.Attributes.Keys.Intersect(product2.Attributes.Keys);
            if (commonKeys.Contains("Price"))
            {
                CommonAttributes1.Add("Price", product1.Attributes["Price"]);
                CommonAttributes2.Add("Price", product2.Attributes["Price"]);
            }
            foreach (var key in commonKeys)
            {
                if (!key.Equals("Price"))
                {
                    CommonAttributes1.Add(key, product1.Attributes[key]);
                    CommonAttributes2.Add(key, product2.Attributes[key]);
                }
            }

            OnPropertyChanged(nameof(Product1));
            OnPropertyChanged(nameof(Product2));
            OnPropertyChanged(nameof(CommonAttributes1));
            OnPropertyChanged(nameof(CommonAttributes2));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
