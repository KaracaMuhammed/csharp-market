using BT.BrightMarket.Domain.Models.Products;
using BT.BrightMarket.Domain.Models.Users;

namespace BT.BrightMarket.Domain.DTOs
{
    public class MessageDTO
    {
        public string Text { get; set; }
        public int ConversationId { get; set; }

    }
}
