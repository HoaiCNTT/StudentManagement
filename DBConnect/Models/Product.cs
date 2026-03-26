namespace DBConnect.Models
{
    public class Product
    {
        public int Id { get; set; }  // khóa chính
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
