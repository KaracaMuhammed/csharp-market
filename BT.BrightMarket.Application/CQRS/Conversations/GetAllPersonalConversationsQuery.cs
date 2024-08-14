using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Conversations;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Conversations
{

    public class GetAllPersonalConversationsQuery : IRequest<IEnumerable<Conversation>>
    {
        public int AuthenticatedUserId { get; set; }
    }

    public class GetAllPersonalConversationsQueryHandler : IRequestHandler<GetAllPersonalConversationsQuery, IEnumerable<Conversation>>
    {
        private readonly IUnitofWork uow;

        public GetAllPersonalConversationsQueryHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<IEnumerable<Conversation>> Handle(GetAllPersonalConversationsQuery request, CancellationToken cancellationToken)
        {
            return await uow.ConversationsRepository.GetAllPersonalConversations(request.AuthenticatedUserId);
        }
    }
}
