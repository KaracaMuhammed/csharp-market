using BT.BrightMarket.Domain.Models.Products;
using BT.BrightMarket.Domain.Models.Users;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BT.BrightMarket.Domain.Models.Conversations
{
    public class Conversation
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int BuyerId { get; set; }
        public User Buyer { get; set; }
        //public UserType UserType { get; set; }
        public DateTime LastUpdated { get; set; }
        [NotMapped] public Message? LatestMessage { get; set; }
        [JsonIgnore] public List<Message> Messages { get; set; }

    }
}
