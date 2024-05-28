using NamespaceGPT.Business.Services;
using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Api.Controllers
{
    public class FavouriteProductController
    {
        private readonly IFavouriteProductService favouriteProductService;

        public FavouriteProductController(IFavouriteProductService favouriteProductService)
        {
            this.favouriteProductService = favouriteProductService;
        }

        public int AddFavouriteProduct(FavouriteProduct favouriteProduct)
        {
            return favouriteProductService.AddFavouriteProduct(favouriteProduct);
        }

        public bool DeleteFavouriteProductFromUser(FavouriteProduct favouriteProduct)
        {
            return favouriteProductService.DeleteFavouriteProductFromUser(favouriteProduct);
        }

        public IEnumerable<FavouriteProduct> GetAllFavouriteProducts()
        {
            return favouriteProductService.GetAllFavouriteProducts();
        }

        public IEnumerable<FavouriteProduct> GetAllFavouriteProductsOfUser(int userId)
        {
            return favouriteProductService.GetAllFavouriteProductsOfUser(userId);
        }

        public IEnumerable<int> GetAllUserIdsWhoMarkedProductAsFavourite(int productId)
        {
            return favouriteProductService.GetAllUserIdsWhoMarkedProductAsFavourite(productId);
        }

        public FavouriteProduct? GetFavouriteProduct(int id)
        {
            return favouriteProductService.GetFavouriteProduct(id);
        }
    }
}
