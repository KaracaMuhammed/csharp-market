using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Conversations;
using FluentValidation;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Messages
{
    public class AddMessageCommand : IRequest<Message>
    {
        public string Text { get; set; }
        public int AuthenticatedUserId { get; set; }
        public int ConversationId { get; set; }
    }

    public class AddMessageCommandValidator : AbstractValidator<AddMessageCommand>
    {
        public AddMessageCommandValidator(IUnitofWork uow)
        {

            RuleFor(command => command)
                .MustAsync(async (request, cancellationToken) =>
                {
                    var conversation = await uow.ConversationsRepository.GetById(request.ConversationId);
                    return conversation != null &&
                           (conversation.BuyerId == request.AuthenticatedUserId ||
                            conversation.Product.UserId == request.AuthenticatedUserId);
                })
                .WithMessage((command, validationResult) =>
                {
                    return $"Access not allowed to this conversation";
                });
        }
    }

    public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, Message>
    {
        private readonly IUnitofWork uow;
        public AddMessageCommandHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<Message> Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {

            Message message = new Message
            {
                Text = request.Text,
                TimeStamp = DateTime.Now,
                SenderId = request.AuthenticatedUserId,
                Sender = await uow.UsersRepository.GetById(request.AuthenticatedUserId),
                ConversationId = request.ConversationId,
                Conversation = await uow.ConversationsRepository.GetById(request.ConversationId)
            };

            message = await uow.MessagesRepository.Create(message);

            var currentConversation = await uow.ConversationsRepository.GetById(request.ConversationId);
            currentConversation.LastUpdated = DateTime.Now;
            await uow.ConversationsRepository.Update(currentConversation);

            await uow.Commit();

            return message;
        }


    }
}
