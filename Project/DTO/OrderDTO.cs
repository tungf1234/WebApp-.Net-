using Project.Models;

namespace Project.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ShipAddress { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
      
    }
}
