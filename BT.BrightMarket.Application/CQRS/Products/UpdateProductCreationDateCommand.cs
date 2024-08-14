using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Products;
using FluentValidation;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Products
{
    public class UpdateProductCreationDateCommand : IRequest<Product>
    {
        public int ProductId { get; set; }
    }

    public class UpdateProductCreationDateCommandValidator : AbstractValidator<UpdateProductCreationDateCommand>
    {
        public UpdateProductCreationDateCommandValidator(IUnitofWork uow)
        {

            RuleFor(p => p.ProductId)
                .MustAsync(async (id, cancellation) => await uow.ProductsRepository.ProductExistsAsync(id))
                .WithMessage("Product does not exist.");

        }
    }

    public class UpdateProductCreationDateCommandHandler : IRequestHandler<UpdateProductCreationDateCommand, Product>
    {
        private readonly IUnitofWork uow;
        public UpdateProductCreationDateCommandHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<Product> Handle(UpdateProductCreationDateCommand request, CancellationToken cancellationToken)
        {
            var currentProduct = await uow.ProductsRepository.GetById(request.ProductId);

            if (currentProduct != null)
            {
                currentProduct.CreationDate = DateTime.Now;
            }

            await uow.ProductsRepository.Update(currentProduct);
            await uow.Commit();

            return currentProduct;
        }

    }
}
