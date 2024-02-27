namespace Clothing_Store.ViewModels.Entities
{
    public class OrderItemEntity
    {
        public int Id { get; set; } // Primary key

        public int OrderId { get; set; } // Foreign key to the order

        public int ProductId { get; set; } // Foreign key to the product

        public int Quantity { get; set; }

        public double Price { get; set; }

        // Navigation properties
        public OrderEntity Order { get; set; }

        public ProductsEntity Product { get; set; }
    }
}
