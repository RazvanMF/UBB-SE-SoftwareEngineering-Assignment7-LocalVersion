using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Data.Repositories.Interfaces
{
    public interface IMarketplaceRepository
    {
        int AddMarketplace(Marketplace marketplace);
        bool DeleteMarketplace(int id);
        bool UpdateMarketplace(int id, Marketplace marketplace);
        IEnumerable<Marketplace> GetAllMarketplaces();
        Marketplace? GetMarketplace(int id);
    }
}
