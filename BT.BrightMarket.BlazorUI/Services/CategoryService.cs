using BT.BrightMarket.BlazorUI.Interfaces;
using BT.BrightMarket.Shared.DTOs;
using BT.BrightMarket.Shared.Models.Products;
using System.Net.Http.Json;

namespace BT.BrightMarket.BlazorUI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient httpClient;
        public CategoryService(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient.CreateClient("BrightMarketAPI");
        }

        public async Task<Category> CreateCategoryAsync(CategoryDTO.PostCategoryObject category)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("categories", category);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Category>();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create product.", ex);
            }
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("categories");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Category>>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to retrieve categories.", ex);
            }
        }
    }
}
