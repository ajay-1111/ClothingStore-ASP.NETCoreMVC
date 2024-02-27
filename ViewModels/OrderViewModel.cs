namespace Clothing_Store.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
