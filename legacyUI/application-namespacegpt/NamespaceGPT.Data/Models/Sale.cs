namespace NamespaceGPT.Data.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public int ListingId { get; set; }

        public User User
        {
            get => default;
            set
            {
            }
        }
    }
}
