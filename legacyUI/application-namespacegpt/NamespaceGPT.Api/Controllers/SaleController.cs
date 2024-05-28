using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Api.Controllers
{
    public class SaleController
    {
        private readonly ISaleService saleService;

        public SaleController(ISaleService saleService)
        {
            this.saleService = saleService ?? throw new ArgumentNullException(nameof(saleService));
        }

        public int AddSale(Sale sale)
        {
            return saleService.AddSale(sale);
        }

        public bool DeleteSale(int id)
        {
            return saleService.DeleteSale(id);
        }

        public IEnumerable<Sale> GetAllPurchasesOfUser(int userId)
        {
            return saleService.GetAllPurchasesOfUser(userId);
        }

        public IEnumerable<Sale> GetAllSales()
        {
            return saleService.GetAllSales();
        }

        public IEnumerable<Sale> GetAllSalesOfListing(int listingId)
        {
            return saleService.GetAllSalesOfListing(listingId);
        }

        public Sale? GetSale(int id)
        {
            return saleService.GetSale(id);
        }

        public bool UpdateSale(int id, Sale sale)
        {
            return saleService.UpdateSale(id, sale);
        }
    }
}
