using BT.BrightMarket.Application.CQRS.Images;
using BT.BrightMarket.Application.CQRS.Products;
using BT.BrightMarket.WebAPI.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BT.BrightMarket.WebAPI.Controllers
{
    public class ImagesController : APIv1Controller
    {

        private readonly IMediator mediator;
        public ImagesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("id/{productId}/all")]
        public async Task<IActionResult> GetImagesByProductId(int productId)
        {
            return Ok(await mediator.Send(new GetAllImagesByProductIdQuery() { ProductId = productId }));
        }

        [HttpGet]
        [Route("id/{productId}/first")]
        public async Task<IActionResult> GetFirstImageByProductId(int productId)
        {
            return Ok(await mediator.Send(new GetFirstImageByProductIdQuery() { ProductId = productId }));
        }

    }
}
