using BT.BrightMarket.Domain.Models.Products;

namespace BT.BrightMarket.Application.Interfaces
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetAll(ItemType itemType);
        //Task<IEnumerable<Product>> GetAllActive();
        Task<Product> GetById(int id);
        Task<Product> Create(Product product);
        Task<Product> Update(Product updatedProduct);
        Task Delete(Product product);
        Task<bool> ProductExistsAsync(int productId);
        Task<IEnumerable<Product>> GetAllPersonalProducts(int authenticatedUserId, ItemType itemType);
        Task<IEnumerable<Product>> GetAllActive(ItemType itemType);
    }
}
