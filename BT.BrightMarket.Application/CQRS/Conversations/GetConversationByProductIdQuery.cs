using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Conversations;
using FluentValidation;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Conversations
{
    public class GetConversationByProductIdQuery : IRequest<Conversation>
    {
        public int ProductId { get; set; }
        public int AuthenticatedUserId { get; set; }
    }

    public class GetConversationByProductIdAndUserIdQueryHandler : IRequestHandler<GetConversationByProductIdQuery, Conversation>
    {
        private readonly IUnitofWork uow;
        public GetConversationByProductIdAndUserIdQueryHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<Conversation> Handle(GetConversationByProductIdQuery request, CancellationToken cancellationToken)
        {
            return await uow.ConversationsRepository.GetByProductIdAndUserId(request.ProductId, request.AuthenticatedUserId);
        }
    }
}
