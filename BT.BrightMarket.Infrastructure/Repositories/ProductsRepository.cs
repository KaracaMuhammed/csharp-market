using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Products;
using BT.BrightMarket.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BT.BrightMarket.Infrastructure.Repositories
{
    public class ProductsRepository : IProductsRepository
    {

        private readonly BrightMarketContext context;
        public ProductsRepository(BrightMarketContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Product>> GetAll(ItemType itemType)
        {
            IQueryable<Product> query = context.Products
                .Include(p => p.User)
                .Include(p => p.User.Region)
                .Include(p => p.Category);

            if (itemType != ItemType.Undefined)
                query = query.Where(p => p.ItemType == itemType);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllActive(ItemType itemType)
        {
            var now = DateTime.Now;
            var query = context.Products.AsQueryable();

            if (itemType != ItemType.Undefined)
                query = query.Where(p => p.ItemType == itemType);

            var products = await query
                .Include(p => p.User)
                .Include(p => p.User.Region)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreationDate)
                .ToListAsync();

            return products.Where(p => p.ExpiryDate > now);
        }


        public async Task<Product> GetById(int id)
        {
            return await context.Products
                .Include(p => p.User)
                .Include(p => p.User.Region)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(u => u.Id.Equals(id));
        }

        public async Task<Product> Create(Product product)
        {
            await context.Products.AddAsync(product);
            return product;
        }

        public async Task<Product> Update(Product updatedProduct)
        {
            context.Products.Update(updatedProduct);
            return updatedProduct;
        }

        public async Task Delete(Product product)
        {
            context.Products.Remove(product);
        }

        public async Task<bool> ProductExistsAsync(int productId)
        {
            return await context.Products.AnyAsync(p => p.Id == productId);
        }

        public async Task<IEnumerable<Product>> GetAllPersonalProducts(int authenticatedUserId, ItemType itemType)
        {

            IQueryable<Product> query = context.Products
                .Where(p => p.UserId == authenticatedUserId);

            if (itemType != ItemType.Undefined)
                query = query.Where(p => p.ItemType == itemType);

            return await query
                .Include(p => p.User)
                .Include(p => p.User.Region)
                .Include(p => p.Category)
                .ToListAsync();

        }

    }
}
