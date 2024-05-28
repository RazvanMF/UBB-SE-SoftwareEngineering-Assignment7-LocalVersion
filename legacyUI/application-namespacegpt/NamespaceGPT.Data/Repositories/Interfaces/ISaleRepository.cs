using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Data.Repositories.Interfaces
{
    public interface ISaleRepository
    {
        int AddSale(Sale sale);
        bool DeleteSale(int id);
        bool UpdateSale(int id, Sale sale);
        Sale? GetSale(int id);
        IEnumerable<Sale> GetAllSales();
        IEnumerable<Sale> GetAllSalesOfListing(int listingId);
        IEnumerable<Sale> GetAllPurchasesOfUser(int userId);
    }
}
