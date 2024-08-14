using BT.BrightMarket.Domain.Models.Users;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BT.BrightMarket.Domain.Models.Products
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
        public Duration DisplayDuration { get; set; }
        public DateTime ExpiryDate => CreationDate.AddDays(GetDaysFromDuration(DisplayDuration));
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [JsonIgnore] public List<Image>? Images { get; set; }

        private int GetDaysFromDuration(Duration duration)
        {
            switch (duration)
            {
                case Duration.OneMonth:
                    return 30; // Assuming one month is 30 days
                case Duration.TwoWeeks:
                    return 14;
                case Duration.OneWeek:
                    return 7;
                default:
                    throw new ArgumentException("Invalid duration specified");
            }
        }
        public ItemType ItemType { get; set; }

    }
}
