using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Products;
using MediatR;

namespace BT.BrightMarket.Application.CQRS.Images
{
    public class GetFirstImageByProductIdQuery : IRequest<Image>
    {
        public int ProductId { get; set; }
    }

    public class GetFirstImageByProductIdQueryHandler : IRequestHandler<GetFirstImageByProductIdQuery, Image>
    {
        private readonly IUnitofWork uow;
        public GetFirstImageByProductIdQueryHandler(IUnitofWork uow)
        {
            this.uow = uow;
        }

        public async Task<Image> Handle(GetFirstImageByProductIdQuery request, CancellationToken cancellationToken)
        {
            return await uow.ImagesRepository.GetFirstImageByProductId(request.ProductId);
        }

    }
}
