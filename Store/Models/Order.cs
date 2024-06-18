namespace Store.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public List<Product>? Products { get; set; }
        public DateTime Date { get; set; }
        public string? Status { get; set; }
        public double? TotalPrice { get; set; }
        
    }
}
