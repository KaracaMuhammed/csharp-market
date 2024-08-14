using BT.BrightMarket.Shared.Models.Users;

namespace BT.BrightMarket.Shared.Models.Conversations
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; }
    }
}
