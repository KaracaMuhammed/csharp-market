using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Products;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Images
{

    public class GetAllImagesByProductIdQuery : IRequest<IEnumerable<Image>>
    {
        public int ProductId { get; set; }
    }

    public class GetImagesByProductIdQueryHandler : IRequestHandler<GetAllImagesByProductIdQuery, IEnumerable<Image>>
    {
        private readonly IUnitofWork uow;
        public GetImagesByProductIdQueryHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<IEnumerable<Image>> Handle(GetAllImagesByProductIdQuery request, CancellationToken cancellationToken)
        {
            return await uow.ImagesRepository.GetAllImagesByProductId(request.ProductId);
        }

    }
}
