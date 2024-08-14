using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Products;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BT.BrightMarket.Application.CQRS.Products
{

    public class GetAllPersonalProductsQuery : IRequest<IEnumerable<Product>>
    {
        public int AuthenticatedUserId { get; set; }
        public ItemType ItemType { get; set; }
    }

    public class GetAllPersonalProductsQueryHandler : IRequestHandler<GetAllPersonalProductsQuery, IEnumerable<Product>>
    {
        private readonly IUnitofWork uow;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetAllPersonalProductsQueryHandler(IUnitofWork uow, IHttpContextAccessor httpContextAccessor)
        {
            this.uow = uow;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllPersonalProductsQuery request, CancellationToken cancellationToken)
        {
            //var authenticatedUserId = GetAuthenticatedUserId(); //unnecesary anymore implemented on a outer level..

            //if (request.AuthenticatedUserId != authenticatedUserId)
            //    throw new ValidationException("Invalid user ID.");

            return await uow.ProductsRepository.GetAllPersonalProducts(request.AuthenticatedUserId, request.ItemType);
        }

        //private int GetAuthenticatedUserId() //unnecesary anymore implemented on a outer level..
        //{
        //    // Get the authenticated user's identity
        //    var identity = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;

        //    // Example: assuming your user ID is stored in a claim called "UserId"
        //    if (int.TryParse(identity?.FindFirst("UserId")?.Value, out int userId))
        //    {
        //        return userId;
        //    }

        //    // If the user ID is not found or cannot be parsed, return a default value (e.g., -1)
        //    return -1;
        //}
    }
}
