using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Users;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Regions
{
    public class GetAllRegionsQuery : IRequest<IEnumerable<Region>> { }

    public class GetAllRegionsQueryHandler : IRequestHandler<GetAllRegionsQuery, IEnumerable<Region>>
    {
        private readonly IUnitofWork uow;
        public GetAllRegionsQueryHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<IEnumerable<Region>> Handle(GetAllRegionsQuery request, CancellationToken cancellationToken)
        {
            return await uow.RegionsRepository.GetAll();
        }
    }
}
