using BT.BrightMarket.Application.Exceptions;
using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Products;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Products
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IUnitofWork uow;
        public GetProductByIdQueryHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await uow.ProductsRepository.GetById(request.Id) ?? throw new RelationNotFoundException($"Product with id {request.Id} could not be found");
            return product;
        }
    }
}
