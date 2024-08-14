using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Conversations;
using BT.BrightMarket.Domain.Models.Products;
using FluentValidation;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Conversations
{
    public class AddConversationCommand : IRequest<Conversation>
    {
        public int ProductId { get; set; }
        public int AuthenticatedUserId { get; set; } // product buyer
    }

    public class AddConversationCommandValidator : AbstractValidator<AddConversationCommand>
    {
        public AddConversationCommandValidator(IUnitofWork uow)
        {

            RuleFor(c => c.ProductId)
                .MustAsync(async (id, cancellation) => await uow.ProductsRepository.ProductExistsAsync(id))
                .WithMessage("Product does not exist.");

            RuleFor(r => r)
                .MustAsync(async (request, cancellationToken, command) =>
                {
                    var conversationExists = await uow.ConversationsRepository.Exists(request.ProductId, request.AuthenticatedUserId);
                    return !conversationExists;
                })
                .WithMessage("A conversation for this product and buyer already exists.");

            RuleFor(r => r)
                .MustAsync(async (request, cancellationToken, command) =>
                {
                    Product product = await uow.ProductsRepository.GetById(request.ProductId);
                    return product.UserId != request.AuthenticatedUserId;
                })
                .WithMessage("You cannot start a conversation for your own product.");

        }
    }

    public class AddConversationCommandHandler : IRequestHandler<AddConversationCommand, Conversation>
    {
        private readonly IUnitofWork uow;
        public AddConversationCommandHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<Conversation> Handle(AddConversationCommand request, CancellationToken cancellationToken)
        {

            Product product = await uow.ProductsRepository.GetById(request.ProductId);

            //Conversation conversation = await uow.ConversationsRepository.GetConversationByProductIdAndBuyerIdAsync(request.ProductId, request.AuthenticatedUserId); // done by the validator
            //if (conversation != null)
            //    return conversation;

            Conversation conversation = new Conversation() {
                ProductId = request.ProductId,
                Product = product,
                BuyerId = request.AuthenticatedUserId,
                Buyer = await uow.UsersRepository.GetById(request.AuthenticatedUserId),
                LastUpdated = DateTime.Now
            };

            await uow.ConversationsRepository.Create(conversation);
            await uow.Commit();

            return conversation;
        }

    }

}
