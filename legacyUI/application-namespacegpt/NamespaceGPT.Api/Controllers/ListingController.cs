using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Api.Controllers
{
    public class ListingController
    {
        private readonly IListingService listingService;

        public ListingController(IListingService listingService)
        {
            this.listingService = listingService ?? throw new ArgumentNullException(nameof(listingService));
        }

        public int Addlisting(Listing listing)
        {
            return listingService.AddListing(listing);
        }

        public bool Deletelisting(int id)
        {
            return listingService.DeleteListing(id);
        }

        public IEnumerable<Listing> GetAllListings()
        {
            return listingService.GetAllListings();
        }

        public IEnumerable<Listing> GetAllListingsOfProduct(int productId)
        {
            return listingService.GetAllListingsOfProduct(productId);
        }

        public Listing? Getlisting(int id)
        {
            return listingService.GetListing(id);
        }

        public bool Updatelisting(int id, Listing listing)
        {
            return listingService.UpdateListing(id, listing);
        }
    }
}
