
using BT.BrightMarket.Application.Interfaces;
using BT.BrightMarket.Domain.Models.Products;
using BT.BrightMarket.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BT.BrightMarket.Infrastructure.Repositories
{
    public class ImagesRepository : IImagesRepository
    {
        private readonly BrightMarketContext context;
        public ImagesRepository(BrightMarketContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Image>> GetAll()
        {
            return await context.Images.ToListAsync();
        }

        public async Task<Image> GetById(int id)
        {
            return await context.Images.FirstOrDefaultAsync(u => u.Id.Equals(id));
        }

        public async Task<Image> Create(Image image)
        {
            await context.Images.AddAsync(image);
            return image;
        }

        public async Task<Image> Update(Image updatedImage)
        {
            context.Images.Update(updatedImage);
            return updatedImage;
        }

        public async Task Delete(Image image)
        {
            context.Images.Remove(image);
        }

        public async Task<bool> ImageExists(int imageId)
        {
            return await context.Images.AnyAsync(p => p.Id == imageId);
        }

        public async Task<IEnumerable<Image>> CreateRange(IEnumerable<Image> images)
        {
            await context.Images.AddRangeAsync(images);
            return images;
        }

        public async Task<IEnumerable<Image>> GetAllImagesByProductId(int productId)
        {
            return await context.Images.Where(p => p.ProductId == productId).ToListAsync();
        }

        public async Task<Image> GetFirstImageByProductId(int productId)
        {
            return await context.Images
                .Where(i => i.ProductId == productId)
                .OrderBy(i => i.Id)
                .FirstOrDefaultAsync();
        }

    }
}
