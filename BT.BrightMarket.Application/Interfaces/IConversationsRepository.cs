using BT.BrightMarket.Domain.Models.Conversations;

namespace BT.BrightMarket.Application.Interfaces
{
    public interface IConversationsRepository
    {
        Task<Conversation> Create(Conversation conversation);
        Task<IEnumerable<Conversation>> GetAll();
        Task<IEnumerable<Conversation>> GetAllPersonalConversations(int authenticatedUserId);
        Task<Conversation> GetById(int id);
        Task<bool> Exists(int productId, int buyerId);
        Task<Conversation> Update(Conversation updatedConversation);
        Task<Conversation> GetByProductIdAndUserId(int productId, int authenticatedUserId);
        //Task<Conversation> GetConversationByProductIdAndBuyerIdAsync(int productId, int buyerId);

    }
}
