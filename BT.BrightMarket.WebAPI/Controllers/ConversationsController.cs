using BT.BrightMarket.Application.CQRS.Categories;
using BT.BrightMarket.Application.CQRS.Conversations;
using BT.BrightMarket.Application.CQRS.Images;
using BT.BrightMarket.Application.CQRS.Products;
using BT.BrightMarket.Domain.DTOs;
using BT.BrightMarket.WebAPI.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BT.BrightMarket.WebAPI.Controllers
{

    public class ConversationsController : APIv1Controller
    {

        private readonly IMediator mediator;
        public ConversationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        //[HttpGet]
        //[Route("all")]
        //public async Task<IActionResult> GetAllConversations() // [FromQuery] int pageNr = 1, [FromQuery] int pageSize = 15
        //{
        //    return Ok(await mediator.Send(new GetAllConversationsQuery() { }));
        //}

        [HttpGet]
        [Authorize]
        [Route("personal-conversations")]
        public async Task<IActionResult> GetAllPersonalConversations()
        {
            var userIdClaim = HttpContext.User.FindFirst("UserId");
            if (int.TryParse(userIdClaim.Value, out int authenticatedUserId))
            {
                return Ok(await mediator.Send(new GetAllPersonalConversationsQuery() { AuthenticatedUserId = authenticatedUserId }));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddConversation([FromBody] ConversationDTO conversation)
        {
            var userIdClaim = HttpContext.User.FindFirst("UserId");
            if (int.TryParse(userIdClaim.Value, out int authenticatedUserId))
            {
                return Created("", await mediator.Send(new AddConversationCommand() { ProductId = conversation.ProductId, AuthenticatedUserId = authenticatedUserId }));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("conversationId/{conversationId}")]
        public async Task<IActionResult> GetConversationById(int conversationId)
        {
            var userIdClaim = HttpContext.User.FindFirst("UserId");
            if (int.TryParse(userIdClaim.Value, out int authenticatedUserId))
            {
                return Ok(await mediator.Send(new GetConversationByIdQuery() { ConversationId = conversationId, AuthenticatedUserId = authenticatedUserId })); //authenticatedUserId
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("productId/{productId}")]
        public async Task<IActionResult> GetConversationByProductId(int productId)
        {
            var userIdClaim = HttpContext.User.FindFirst("UserId");
            if (int.TryParse(userIdClaim.Value, out int authenticatedUserId))
            {
                return Ok(await mediator.Send(new GetConversationByProductIdQuery() { ProductId = productId, AuthenticatedUserId = authenticatedUserId })); //authenticatedUserId
            }
            else
            {
                return Unauthorized();
            }

        }

    }
}
