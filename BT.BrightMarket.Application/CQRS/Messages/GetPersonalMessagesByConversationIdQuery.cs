using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Conversations;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Messages
{
    public class GetPersonalMessagesByConversationIdQuery : IRequest<IEnumerable<Message>>
    {
        public int ConversationId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 15;
        public int AuthenticatedUserId { get; set; }
    }

    public class GetPersonalMessagesByConversationIdQueryHandler : IRequestHandler<GetPersonalMessagesByConversationIdQuery, IEnumerable<Message>>
    {
        private readonly IUnitofWork uow;

        public GetPersonalMessagesByConversationIdQueryHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<IEnumerable<Message>> Handle(GetPersonalMessagesByConversationIdQuery request, CancellationToken cancellationToken)
        {
            return await uow.MessagesRepository.GetPersonalMessagesByConversationId(request.AuthenticatedUserId, request.ConversationId, request.PageNumber, request.PageSize);
        }

    }
}
