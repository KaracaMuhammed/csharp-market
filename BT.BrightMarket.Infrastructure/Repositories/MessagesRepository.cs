using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Conversations;
using BT.BrightMarket.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BT.BrightMarket.Infrastructure.Repositories
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly BrightMarketContext context;
        public MessagesRepository(BrightMarketContext context)
        {
            this.context = context;
        }

        public async Task<Message> Create(Message message)
        {
            await context.Messages.AddAsync(message);
            return message;
        }

        public async Task<List<Message>> GetPersonalMessagesByConversationId(int authenticatedUserId, int conversationId, int pageNumber, int pageSize = 15)
        {
            return await context.Messages
                .Where(m => m.ConversationId == conversationId &&
                    context.Conversations.Any(c => c.Id == conversationId &&
                        (c.BuyerId == authenticatedUserId || c.Product.UserId == authenticatedUserId)))
                .OrderByDescending(m => m.TimeStamp)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(m => m.Sender)
                .OrderBy(m => m.TimeStamp)
                .ToListAsync();
        }

        

    }
}
