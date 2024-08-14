using BT.BrightMarket.BlazorUI.Interfaces;
using BT.BrightMarket.Shared.DTOs;
using BT.BrightMarket.Shared.Models.Products;
using System.Net.Http.Json;

namespace BT.BrightMarket.BlazorUI.Services
{
    public class ImageService : IImageService
    {
        private readonly HttpClient httpClient;
        public ImageService(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient.CreateClient("BrightMarketAPI");
        }

        public async Task<List<ImageDTO>> GetAllImagesByProductId(int productId)
        {
            try
            {
                var response = await httpClient.GetAsync($"images/id/{productId}/all");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<ImageDTO>>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to retrieve images for product with ID {productId}.", ex);
            }
        }

        public async Task<Image> GetFirstImageByProductId(int productId)
        {
            try
            {
                var response = await httpClient.GetAsync($"images/id/{productId}/first");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Image>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to retrieve first image for product with ID {productId}.", ex);
            }
        }
    }
}
