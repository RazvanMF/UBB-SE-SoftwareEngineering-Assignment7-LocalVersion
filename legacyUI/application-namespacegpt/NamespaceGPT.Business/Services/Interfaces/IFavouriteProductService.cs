using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Business.Services.Interfaces
{
    public interface IFavouriteProductService
    {
        int AddFavouriteProduct(FavouriteProduct favouriteProduct);
        bool DeleteFavouriteProductFromUser(FavouriteProduct favouriteProduct);
        IEnumerable<FavouriteProduct> GetAllFavouriteProducts();
        IEnumerable<FavouriteProduct> GetAllFavouriteProductsOfUser(int userId);
        IEnumerable<int> GetAllUserIdsWhoMarkedProductAsFavourite(int productId);
        FavouriteProduct? GetFavouriteProduct(int id);
    }
}
