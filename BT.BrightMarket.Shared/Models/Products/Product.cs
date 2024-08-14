using BT.BrightMarket.Shared.DTOs;
using BT.BrightMarket.Shared.Models.Users;

namespace BT.BrightMarket.Shared.Models.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public Status Status { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public Duration DisplayDuration { get; set; }
        public Image FirstImage { get; set; }
        public List<ImageDTO> Images { get; set; }
        public ItemType ItemType { get; set; }

    }
}
