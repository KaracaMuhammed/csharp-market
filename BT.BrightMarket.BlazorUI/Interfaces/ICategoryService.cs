using BT.BrightMarket.Shared.DTOs;
using BT.BrightMarket.Shared.Models.Products;

namespace BT.BrightMarket.BlazorUI.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> CreateCategoryAsync(CategoryDTO.PostCategoryObject newCategory);

    }
}
