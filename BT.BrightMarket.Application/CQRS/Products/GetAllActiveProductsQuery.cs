using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Products;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Products
{
    public class GetAllActiveProductsQuery : IRequest<IEnumerable<Product>> 
    {
        public ItemType ItemType { get; set; }
    }

    public class GetAllActiveProductsQueryHandler : IRequestHandler<GetAllActiveProductsQuery, IEnumerable<Product>>
    {
        private readonly IUnitofWork uow;
        public GetAllActiveProductsQueryHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllActiveProductsQuery request, CancellationToken cancellationToken)
        {
            return await uow.ProductsRepository.GetAllActive(request.ItemType);
        }
    }
}
