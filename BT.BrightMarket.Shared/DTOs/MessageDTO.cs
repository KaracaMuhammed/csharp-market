using BT.BrightMarket.Shared.Models.Conversations;

namespace BT.BrightMarket.Shared.DTOs
{
    public class MessageDTO
    {
        public string Text { get; set; }
        public int ConversationId { get; set; }
    }
}
