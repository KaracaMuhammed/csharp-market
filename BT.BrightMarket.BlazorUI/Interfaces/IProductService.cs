using BT.BrightMarket.Shared.Models.Products;
using BT.BrightMarket.Shared.DTOs;

namespace BT.BrightMarket.BlazorUI.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(ProductDTO.PostProductObject newProduct);

        Task<List<Product>> GetAllProductsAsync(ItemType itemType);
        Task<List<Product>> GetPersonalProductsAsync(ItemType itemType);
        Task<Product> GetProductByIdAsync(int id);
        Task<List<Product>> GetAllActiveProductsAsync(ItemType itemType);

        Task<Product> UpdateProductAsync(int id, ProductDTO.PostUpdateObject updatedProduct);
        Task<Product> UpdateProductCreationDateAsync(int id);

        Task DeleteProductAsync(int id);

    }
}
