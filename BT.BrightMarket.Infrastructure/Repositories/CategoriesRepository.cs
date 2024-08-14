using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Products;
using BT.BrightMarket.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BT.BrightMarket.Infrastructure.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly BrightMarketContext context;
        public CategoriesRepository(BrightMarketContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Category>> GetAll()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task<Category> Create(Category category)
        {
            await context.Categories.AddAsync(category);
            return category;
        }

    }
}
