using BT.BrightMarket.Shared.DTOs;
using BT.BrightMarket.Shared.Models.Conversations;

namespace BT.BrightMarket.BlazorUI.Interfaces
{
    public interface IMessageService
    {
        Task<Message> CreateMessageAsync(MessageDTO message);
        Task<List<Message>> GetMessagesByConversationId(int conversationId, int pageNumber = 1, int pageSize = 15);

    }
}
