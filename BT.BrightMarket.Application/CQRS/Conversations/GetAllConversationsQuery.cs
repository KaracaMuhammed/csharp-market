using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Conversations;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Categories
{
    public class GetAllConversationsQuery : IRequest<IEnumerable<Conversation>> { }

    public class GetAllConversationsQueryHandler : IRequestHandler<GetAllConversationsQuery, IEnumerable<Conversation>> 
    {
        private readonly IUnitofWork uow;
        public GetAllConversationsQueryHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<IEnumerable<Conversation>> Handle(GetAllConversationsQuery request, CancellationToken cancellationToken)
        {
            return await uow.ConversationsRepository.GetAll();
        }
    }

}
