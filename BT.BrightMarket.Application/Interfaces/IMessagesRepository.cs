using BT.BrightMarket.Domain.Models.Conversations;

namespace BT.BrightMarket.Application.Interfaces
{
    public interface IMessagesRepository
    {
        Task<Message> Create(Message message);
        Task<List<Message>> GetPersonalMessagesByConversationId(int authenticatedUserId, int conversationId, int pageNumber, int pageSize = 15);
    }
}
