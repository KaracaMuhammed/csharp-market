using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Products;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Categories
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<Category>> { }

    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<Category>> 
    {
        private readonly IUnitofWork uow;
        public GetAllCategoriesQueryHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await uow.CategoriesRepository.GetAll();
        }
    }

}
