using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Products;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Products
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>> 
    {
        public ItemType ItemType { get; set; }
    }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>> 
    {
        private readonly IUnitofWork uow;
        public GetAllProductsQueryHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await uow.ProductsRepository.GetAll(request.ItemType);
        }
    }

}
