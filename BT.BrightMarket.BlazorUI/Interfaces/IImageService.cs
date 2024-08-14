using BT.BrightMarket.Shared.DTOs;
using BT.BrightMarket.Shared.Models.Products;

namespace BT.BrightMarket.BlazorUI.Interfaces
{
    public interface IImageService
    {
        Task<List<ImageDTO>> GetAllImagesByProductId(int productId);
        Task<Image> GetFirstImageByProductId(int productId);
    }
}
