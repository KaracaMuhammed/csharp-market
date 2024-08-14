using BT.BrightMarket.Domain.Models.Products;

namespace BT.BrightMarket.Application.Interfaces
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category> Create(Category category);
    }
}
