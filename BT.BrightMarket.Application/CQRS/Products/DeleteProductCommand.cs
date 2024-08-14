using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Products;
using BT.BrightMarket.Domain.Models.Users;
using FluentValidation;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Products
{
    public class DeleteProductCommand : IRequest
    {
        public int Id { get; set; }
        public int AuthenticatedUserId { get; set; }
    }

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator(IUnitofWork uow) {

            RuleFor(r => r)
                .MustAsync(async (request, cancellationToken, command) =>
                {
                    User user = await uow.UsersRepository.GetById(request.AuthenticatedUserId);
                    Product product = await uow.ProductsRepository.GetById(request.Id);

                    bool isAdmin = user.Role == Role.Admin;
                    bool isOwner = product.UserId == request.AuthenticatedUserId;
                    return isAdmin || isOwner;

                })
                .WithMessage("Not allowed to update this product.");

            RuleFor(p => p.Id)
                .MustAsync(async (id, cancellation) => await uow.ProductsRepository.ProductExistsAsync(id))
                .WithMessage("Product does not exist.");

        }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IUnitofWork uow;

        public DeleteProductCommandHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await uow.ProductsRepository.GetById(request.Id);

            if (product != null)
            {
                uow.ProductsRepository.Delete(product);
                await uow.Commit();
            }

        }
    }

}