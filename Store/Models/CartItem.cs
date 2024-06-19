namespace Store.Models
{
    public class CartItem
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; } = 1;
    }
}
