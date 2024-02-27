namespace Clothing_Store.ViewModels.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        
        public string? UserId { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
