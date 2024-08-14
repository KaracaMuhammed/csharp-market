using BT.BrightMarket.Application.Exceptions;
using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Users;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Regions
{
    public class GetRegionByIdQuery : IRequest<Region>
    {
        public int Id { get; set; }
    }

    public class GetRegionByIdQueryHandler : IRequestHandler<GetRegionByIdQuery, Region>
    {
        private readonly IUnitofWork uow;
        public GetRegionByIdQueryHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<Region> Handle(GetRegionByIdQuery request, CancellationToken cancellationToken)
        {
            var region = await uow.RegionsRepository.GetById(request.Id) ?? throw new RelationNotFoundException($"Region with id {request.Id} could not be found");
            return region;
        }
    }
}
