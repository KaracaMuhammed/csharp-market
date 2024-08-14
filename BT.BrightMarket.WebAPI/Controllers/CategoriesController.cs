using BT.BrightMarket.Application.CQRS.Categories;
using BT.BrightMarket.Domain.DTOs;
using BT.BrightMarket.WebAPI.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BT.BrightMarket.WebAPI.Controllers
{
    public class CategoriesController : APIv1Controller
    {
        private readonly IMediator mediator;

        public CategoriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await mediator.Send(new GetAllCategoriesQuery() { }));
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO.PostCategoryObject category)
        {
            return Created("", await mediator.Send(new AddCategoryCommand() { Name = category.Name }));
        }

    }
}
