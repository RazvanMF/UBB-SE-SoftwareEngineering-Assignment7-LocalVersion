using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamespaceGPT.Data.Models
{
    public class Listing
    {
        public int Id { get; set; } = 0;
        public int ProductId { get; set; } = 0;

        public int MarketplaceId { get; set; } = 0;

        public int Price { get; set; } = 0;

        public Product Product
        {
            get => default;
            set
            {
            }
        }

        public Marketplace Marketplace
        {
            get => default;
            set
            {
            }
        }
    }
}
