using BT.BrightMarket.Shared.DTOs;
using BT.BrightMarket.Shared.Models.Conversations;

namespace BT.BrightMarket.BlazorUI.Interfaces
{
    public interface IConversationService
    {
        Task<List<Conversation>> GetPersonalConversationsAsync();
        Task<Conversation> CreateConversationAsync(ConversationDTO conversation);
        Task<Conversation> GetConversationByIdAsync(int conversationId);
        Task<Conversation> GetConversationByProductId(int productId);
    }
}
