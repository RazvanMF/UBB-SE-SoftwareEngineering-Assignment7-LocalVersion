using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamespaceGPT.Data.Models
{
    public class NewProductAlert : IAlert
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }

        public IAlert IAlert
        {
            get => default;
            set
            {
            }
        }

        public bool Equals(NewProductAlert other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Id == other.Id &&
                this.UserId == other.UserId &&
                this.ProductId == other.ProductId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            NewProductAlert other = obj as NewProductAlert;
            return Equals(other);
        }

        public void Notify()
        {
            throw new NotImplementedException();
        }
    }
}
