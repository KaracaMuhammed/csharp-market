using BT.BrightMarket.Application.CQRS.Products;
using BT.BrightMarket.Domain.DTOs;
using BT.BrightMarket.Domain.Models.Products;
using BT.BrightMarket.WebAPI.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BT.BrightMarket.WebAPI.Controllers
{
    public class ProductsController : APIv1Controller
    {
        private readonly IMediator mediator;

        public ProductsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO.PostProductObject newProduct)
        {
            var userIdClaim = HttpContext.User.FindFirst("UserId");
            if (int.TryParse(userIdClaim.Value, out int authenticatedUserId))
            {
                return Created("", await mediator.Send(new AddProductCommand() { Name = newProduct.Name, Description = newProduct.Description, Price = newProduct.Price, Status = newProduct.Status, AuthenticatedUserId = authenticatedUserId, CategoryId = newProduct.CategoryId, DisplayDuration = newProduct.DisplayDuration, Images = newProduct.Images, ItemType = newProduct.ItemType }));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("all/{itemType}")]
        public async Task<IActionResult> GetAllProducts(ItemType itemType) // [FromQuery] int pageNr = 1, [FromQuery] int pageSize = 15
        {
            return Ok(await mediator.Send(new GetAllProductsQuery() { ItemType = itemType }));
        }

        [HttpGet]
        [Route("active/{itemType}")]
        public async Task<IActionResult> GetAllActiveProducts(ItemType itemType)
        {
            return Ok(await mediator.Send(new GetAllActiveProductsQuery() { ItemType = itemType }));
        }

        [HttpGet]
        [Authorize]
        [Route("personal-products/{itemType}")]
        public async Task<IActionResult> GetAllPersonalProducts(ItemType itemType)
        {
            var userIdClaim = HttpContext.User.FindFirst("UserId");
            if (int.TryParse(userIdClaim.Value, out int authenticatedUserId))
            {
                return Ok(await mediator.Send(new GetAllPersonalProductsQuery() { AuthenticatedUserId = authenticatedUserId, ItemType = itemType }));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("id/{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            return Ok(await mediator.Send(new GetProductByIdQuery() { Id = productId }));
        }

        [HttpPut]
        [Route("id/{productId}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] ProductDTO.PostProductObject updatedProduct)
        {
            var userIdClaim = HttpContext.User.FindFirst("UserId");
            if (int.TryParse(userIdClaim.Value, out int authenticatedUserId))
            {
                return Ok(await mediator.Send(new UpdateProductCommand() { ProductId = productId, Name = updatedProduct.Name, Description = updatedProduct.Description, Price = updatedProduct.Price, Status = updatedProduct.Status, AuthenticatedUserId = authenticatedUserId, CategoryId = updatedProduct.CategoryId, DisplayDuration = updatedProduct.DisplayDuration, Images = updatedProduct.Images, ItemType = updatedProduct.ItemType }));
            }
            else
            {
                return Unauthorized();
            }
        }

        
        [HttpDelete]
        [Route("id/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {

            var userIdClaim = HttpContext.User.FindFirst("UserId");
            if (int.TryParse(userIdClaim.Value, out int authenticatedUserId))
            {
                await mediator.Send(new DeleteProductCommand() { Id = productId, AuthenticatedUserId = authenticatedUserId });
                return NoContent();
            }
            else
            {
                return Unauthorized();
            }

        }

        
        [HttpPut]
        [Route("reactivate/id/{productId}")]
        public async Task<IActionResult> UpdateProductCreationDateProduct(int productId)
        {
            return Ok(await mediator.Send(new UpdateProductCreationDateCommand() { ProductId = productId }));
        }

    }
}
