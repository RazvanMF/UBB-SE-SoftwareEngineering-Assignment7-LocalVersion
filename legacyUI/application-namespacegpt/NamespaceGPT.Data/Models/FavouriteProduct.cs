namespace NamespaceGPT.Data.Models
{
    public class FavouriteProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }

        public User User
        {
            get => default;
            set
            {
            }
        }

        public Product Product
        {
            get => default;
            set
            {
            }
        }
    }
}
