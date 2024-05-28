using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamespaceGPT.Data.Models
{
    public class ProductToPostable
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public string Attributes { get; set; } = string.Empty;
    }
}
