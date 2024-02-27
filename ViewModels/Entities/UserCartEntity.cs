namespace Clothing_Store.ViewModels.Entities
{
    public class UserCartEntity
    {
        public int Id { get; set; }

        public int productId { get; set; }

        public string userId { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}
