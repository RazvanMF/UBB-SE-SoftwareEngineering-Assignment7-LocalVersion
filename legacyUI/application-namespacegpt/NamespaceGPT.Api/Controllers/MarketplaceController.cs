using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Api.Controllers
{
    public class MarketplaceController
    {
        private readonly IMarketplaceService marketplaceService;

        public MarketplaceController(IMarketplaceService marketplaceService)
        {
            this.marketplaceService = marketplaceService ?? throw new ArgumentNullException(nameof(marketplaceService));
        }

        public int AddMarketplace(Marketplace marketplace)
        {
            return marketplaceService.AddMarketplace(marketplace);
        }

        public bool DeleteMarketplace(int id)
        {
            return marketplaceService.DeleteMarketplace(id);
        }

        public IEnumerable<Marketplace> GetAllMarketplaces()
        {
            return marketplaceService.GetAllMarketplaces();
        }

        public Marketplace? GetMarketplace(int id)
        {
            return marketplaceService.GetMarketplace(id);
        }

        public bool UpdateMarketplace(int id, Marketplace marketplace)
        {
            return marketplaceService.UpdateMarketplace(id, marketplace);
        }
    }
}
