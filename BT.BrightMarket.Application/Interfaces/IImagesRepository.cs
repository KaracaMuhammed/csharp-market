using BT.BrightMarket.Domain.Models.Products;

namespace BT.BrightMarket.Application.Interfaces
{
    public interface IImagesRepository
    {
        //Task<IEnumerable<Image>> GetAll();
        //Task<Image> GetById(int id);
        //Task<Image> Create(Image image);
        //Task<Image> Update(Image updatedImage);
        //Task Delete(Image image);
        //Task<bool> ImageExists(int imageId);
        Task<IEnumerable<Image>> CreateRange(IEnumerable<Image> images);
        Task<IEnumerable<Image>> GetAllImagesByProductId(int productId);
        Task<Image> GetFirstImageByProductId(int productId);
    }
}
