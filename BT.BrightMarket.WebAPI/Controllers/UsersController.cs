using BT.BrightMarket.Application.CQRS.Products;
using BT.BrightMarket.Application.CQRS.Users;
using BT.BrightMarket.Domain.DTOs;
using BT.BrightMarket.WebAPI.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BT.BrightMarket.WebAPI.Controllers
{
    public class UsersController : APIv1Controller
    {
        private readonly IMediator mediator;
        //private readonly SecurityContext securityContext;

        public UsersController(IMediator mediator) /*, SecurityContext security*/
        {
            this.mediator = mediator;
            //this.securityContext = security;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers() // [FromQuery] int pageNr = 1, [FromQuery] int pageSize = 15
        {
            return Ok(await mediator.Send(new GetAllUsersQuery() { }));
        }

        [HttpGet]
        [Route("id/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            return Ok(await mediator.Send(new GetUserByIdQuery() { Id = id }));
        }

        [HttpGet]
        [Authorize]
        [Route("personal")]
        public async Task<IActionResult> GetPersonalUser()
        {
            var userIdClaim = HttpContext.User.FindFirst("UserId");
            if (int.TryParse(userIdClaim.Value, out int authenticatedUserId))
            {
                return Ok(await mediator.Send(new GetPersonalUserQuery() { AuthenticatedUserId = authenticatedUserId }));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            return Ok(await mediator.Send(new GetUserByEmailQuery() { Email = email }));
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDTO.PostUserObject newUser)
        {
            return Created("", await mediator.Send(new AddUserCommand() { Name = newUser.Name, Email = newUser.Email, Role = newUser.Role, RegionId = newUser.RegionId }));
        }

        //[HttpPut]
        //[Route("{id}")]
        //public async Task<IActionResult> UpdateBroker(int id, [FromBody] PostBrokerDTO updatedBroker)
        //{
        //     return Ok(await mediator.Send(new UpdateBrokerCommand() { BrokerDTO = updatedBroker, AdminId = securityContext.Id }));
        //}

        //[HttpGet]
        //[Route("ID/{brokerId}/base64")]
        //public async Task<IActionResult> GetBrokerIDImageBase64(int brokerId)
        //{
        //    if(securityContext.Role == RoleType.BROKER) { return Ok(await mediator.Send(new GetBrokerIDImageQuery() { BrokerId = securityContext.Id, Role = securityContext.Role})); }
        //    return Ok(await mediator.Send(new GetBrokerIDImageQuery() { BrokerId = brokerId, Role=securityContext.Role, ConsignerOrAdminId = securityContext.Id }));
        //}

    }
}
