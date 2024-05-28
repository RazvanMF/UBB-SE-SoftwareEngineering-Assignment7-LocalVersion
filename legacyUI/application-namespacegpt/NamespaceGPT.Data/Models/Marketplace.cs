using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamespaceGPT.Data.Models
{
    public class Marketplace
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;

        public string WebsiteURL { get; set; } = string.Empty;

        public string CountryOfOrigin { get; set; } = string.Empty;
    }
}
