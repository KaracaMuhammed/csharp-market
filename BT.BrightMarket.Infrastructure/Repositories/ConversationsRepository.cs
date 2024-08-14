using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Conversations;
using BT.BrightMarket.Domain.Models.Users;
using BT.BrightMarket.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BT.BrightMarket.Infrastructure.Repositories
{
    public class ConversationsRepository : IConversationsRepository
    {

        private readonly BrightMarketContext context;
        public ConversationsRepository(BrightMarketContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Conversation>> GetAll()
        { 
            return context.Conversations
                .Include(c => c.Product)
                .Include(c => c.Buyer)
                .ToList();
        }

        public async Task<Conversation> Create(Conversation conversation)
        {
            await context.Conversations.AddAsync(conversation);
            return conversation;
        }

        public async Task<IEnumerable<Conversation>> GetAllPersonalConversations(int authenticatedUserId)
        {
            var conversations = await context.Conversations
                .Where(c => (c.BuyerId == authenticatedUserId) || (c.Product.UserId == authenticatedUserId))
                .Include(c => c.Product.User)
                .Include(c => c.Buyer)
                .Select(c => new Conversation
                {
                    Id = c.Id,
                    ProductId = c.ProductId,
                    Product = c.Product,
                    BuyerId = c.BuyerId,
                    Buyer = c.Buyer,
                    LastUpdated = c.LastUpdated,
                    LatestMessage = c.Messages.OrderByDescending(m => m.TimeStamp).FirstOrDefault()
                })
                .OrderByDescending(c => c.LastUpdated)
                .ToListAsync();

            return conversations;
        }

        public async Task<bool> Exists(int productId, int buyerId)
        {
            return await context.Conversations.AnyAsync(c => (c.ProductId == productId) && (c.BuyerId == buyerId));
        }

        public async Task<Conversation> GetById(int id)
        {
            return await context.Conversations
                .Include(c => c.Product.User)
                .Include(c => c.Buyer)
                .FirstOrDefaultAsync(u => u.Id.Equals(id));
        }

        public async Task<Conversation> Update(Conversation updatedConversation)
        {
            context.Conversations.Update(updatedConversation);
            return updatedConversation;
        }

        public async Task<Conversation> GetByProductIdAndUserId(int productId, int authenticatedUserId)
        {
            return await context.Conversations
                .Include(c => c.Product.User)
                .Include(c => c.Buyer)
                .FirstOrDefaultAsync(c => c.ProductId == productId && (c.BuyerId == authenticatedUserId || c.Product.UserId == authenticatedUserId));
        }

    }
}
