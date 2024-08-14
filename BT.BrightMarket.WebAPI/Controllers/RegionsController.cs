using BT.BrightMarket.Application.CQRS.Regions;
using BT.BrightMarket.WebAPI.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BT.BrightMarket.WebAPI.Controllers
{
    public class RegionsController : APIv1Controller
    {
        private readonly IMediator mediator;

        public RegionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            return Ok(await mediator.Send(new GetAllRegionsQuery() { }));
        }

        [HttpGet]
        [Route("id/{id}")]
        public async Task<IActionResult> GetRegionById(int id)
        {
            return Ok(await mediator.Send(new GetRegionByIdQuery() { Id = id }));
        }
    }
}
