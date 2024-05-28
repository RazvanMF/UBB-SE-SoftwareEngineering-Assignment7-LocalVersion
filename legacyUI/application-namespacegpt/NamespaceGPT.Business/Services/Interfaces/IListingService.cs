using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Business.Services.Interfaces
{
    public interface IListingService
    {
        int AddListing(Listing listing);
        bool DeleteListing(int id);
        bool UpdateListing(int id, Listing listing);
        IEnumerable<Listing> GetAllListings();
        IEnumerable<Listing> GetAllListingsOfProduct(int productId);
        Listing? GetListing(int id);
    }
}
