using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Conversations;
using FluentValidation;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Conversations
{
    public class GetConversationByIdQuery : IRequest<Conversation>
    {
        public int ConversationId { get; set; }
        public int AuthenticatedUserId { get; set; }
    }

    public class GetConversationByIdQueryValidator : AbstractValidator<GetConversationByIdQuery>
    {
        public GetConversationByIdQueryValidator(IUnitofWork uow)
        {

            RuleFor(r => r.ConversationId)
                .MustAsync(async (request, cancellationToken, query) =>
                {
                    var conversation = await uow.ConversationsRepository.GetById(request.ConversationId);
                    return conversation != null &&
                           (conversation.BuyerId == request.AuthenticatedUserId ||
                            conversation.Product.UserId == request.AuthenticatedUserId);
                })
                .WithMessage("Access not allowed to this conversation")
                .When(r => r.ConversationId != 0);

        }
    }

    public class GetConversationByIdQueryHandler : IRequestHandler<GetConversationByIdQuery, Conversation>
    {
        private readonly IUnitofWork uow;
        public GetConversationByIdQueryHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<Conversation> Handle(GetConversationByIdQuery request, CancellationToken cancellationToken)
        {
            return await uow.ConversationsRepository.GetById(request.ConversationId);
        }
    }
}
