using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamespaceGPT.Data.Models
{
    public class BackInStockAlert : IAlert
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int MarketplaceId { get; set; }

        public IAlert IAlert
        {
            get => default;
            set
            {
            }
        }

        public IAlert IAlert1
        {
            get => default;
            set
            {
            }
        }

        public bool Equals(BackInStockAlert other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Id == other.Id &&
                this.UserId == other.UserId &&
                this.ProductId == other.ProductId &&
                this.MarketplaceId == other.MarketplaceId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            BackInStockAlert other = obj as BackInStockAlert;
            return Equals(other);
        }

        public void Notify()
        {
            throw new NotImplementedException();
        }
    }
}
