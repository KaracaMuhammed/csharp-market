using System.Security.Claims;
using System.Threading.Tasks;
using BT.BrightMarket.Application.CQRS.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BT.BrightMarket.WebAPI.Middleware
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CustomAuthorizationMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var mediator = scopedServices.GetRequiredService<IMediator>();

                if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
                {
                    var claimsIdentity = (ClaimsIdentity)context.User.Identity;
                    var userEmail = claimsIdentity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn")?.Value;
                    var user = await mediator.Send(new GetUserByEmailQuery { Email = userEmail });

                    if (user is not null)
                    {
                        claimsIdentity.AddClaim(new Claim("UserId", user.Id.ToString()));
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));
                        claimsIdentity.AddClaim(new Claim("RegionId", user.RegionId.ToString()));
                    }

                }
            }

            await _next(context);
        }
    }
}
