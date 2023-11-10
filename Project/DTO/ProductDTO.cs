using Project.Models;

namespace Project.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; } 
        public string name {  get; set; }
        public int? supplierId { get; set; }
        public int? categoryId { get; set; }
        public string? quantityPerUnit { get; set; }
        public decimal? price { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Supplier? Supplier { get; set; }
    }
}
