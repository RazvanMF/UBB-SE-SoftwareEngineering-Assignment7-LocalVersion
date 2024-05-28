using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Data.Repositories.Interfaces
{
    public interface IListingRepository
    {
        int AddListing(Listing listing);
        bool DeleteListing(int id);
        bool UpdateListing(int id, Listing listing);
        IEnumerable<Listing> GetAllListings();
        IEnumerable<Listing> GetAllListingsOfProduct(int productId);
        Listing? GetListing(int id);
    }
}
