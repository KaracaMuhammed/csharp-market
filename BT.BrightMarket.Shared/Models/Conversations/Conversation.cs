using BT.BrightMarket.Shared.Models.Products;
using BT.BrightMarket.Shared.Models.Users;

namespace BT.BrightMarket.Shared.Models.Conversations
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
        public Message? LatestMessage { get; set; }

        //public List<Message> Messages { get; set; }

    }
}
