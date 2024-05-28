using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamespaceGPT.Data.Models
{
    public class PriceDropAlert : IAlert
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public float OldPrice { get; set; }
        public float NewPrice { get; set; }

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

        public bool Equals(PriceDropAlert other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Id == other.Id &&
                this.UserId == other.UserId &&
                this.ProductId == other.ProductId &&
                this.OldPrice == other.OldPrice &&
                this.NewPrice == other.NewPrice;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            PriceDropAlert other = obj as PriceDropAlert;
            return Equals(other);
        }
        public void Notify()
        {
            throw new NotImplementedException();
        }
    }
}
