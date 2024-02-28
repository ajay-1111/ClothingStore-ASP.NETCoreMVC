namespace Clothing_Store.ViewModels.Entities
{
    public class ProductsEntity
    {
        public enum ProductCategory
        {
            Mens = 1,
            Women = 2,
            Kids = 3,
        }
        public int Id { get; set; }

        public string? Name { get; set; }

        public double Price { get; set; }

        public double Rating { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public ProductCategory ProductCategoryId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime ModifieDateTime { get; set; }
    }
}
