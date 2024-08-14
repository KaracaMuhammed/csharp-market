using BT.BrightMarket.Domain.Models.Products;

namespace BT.BrightMarket.Domain.DTOs
{
    public class ProductDTO
    {
        public class PostProductObject
        {
            public string Name { get; set; }
            public string? Description { get; set; }
            public double Price { get; set; }
            public Status Status { get; set; }
            public int CategoryId { get; set; }
            public Duration DisplayDuration { get; set; }
            public List<ImageDTO>? Images { get; set; }
            public ItemType ItemType { get; set; }
        }
    }
}
