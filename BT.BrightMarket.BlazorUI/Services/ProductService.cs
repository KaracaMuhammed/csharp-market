using BT.BrightMarket.BlazorUI.Interfaces;
using BT.BrightMarket.Shared.DTOs;
using BT.BrightMarket.Shared.Models.Products;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BT.BrightMarket.BlazorUI.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;
        private readonly AuthenticationStateProvider authenticationStateProvider;
        public ProductService(IHttpClientFactory httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            this.httpClient = httpClient.CreateClient("BrightMarketAPI");
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<Product> CreateProductAsync(ProductDTO.PostProductObject product)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("products", product);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Product>();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create product.", ex);
            }
        }

        public async Task<List<Product>> GetAllProductsAsync(ItemType itemType)
        {
            try
            {
                var response = await httpClient.GetAsync($"products/all/{itemType}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Product>>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to retrieve products.", ex);
            }
        }

        public async Task<List<Product>> GetPersonalProductsAsync(ItemType itemType)
        {
            try
            {
                var response = await httpClient.GetAsync($"products/personal-products/{itemType}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Product>>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to retrieve products.", ex);
            }
        }

        public async Task<List<Product>> GetAllActiveProductsAsync(ItemType itemType)
        {
            try
            {
                var response = await httpClient.GetAsync($"products/active/{itemType}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Product>>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to retrieve products.", ex);
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"products/id/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Product>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to retrieve product with ID {id}.", ex);
            }
        }

        public async Task<Product> UpdateProductAsync(int id, ProductDTO.PostUpdateObject updatedProduct)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"products/id/{id}", updatedProduct);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Product>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to update product with ID {id}.", ex);
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"products/id/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to delete product with ID {id}.", ex);
            }
        }

        public async Task<Product> UpdateProductCreationDateAsync(int id)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"products/reactivate/id/{id}", "");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Product>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to update product with ID {id}.", ex);
            }
        }

        //public async Task<string> GetUserEmailAsync()
        //{
        //    var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        //    var user = authState.User;

        //    Console.WriteLine(user.Identity);

        //    Debug.WriteLine($"IsAuthenticated: {authState.User.Identity.IsAuthenticated}");

        //    // Check if the user is authenticated
        //    if (user.Identity.IsAuthenticated)
        //    {
        //        // Retrieve the user's email from claims
        //        var userEmail = user.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn")?.Value;
        //        Console.WriteLine(userEmail);
        //        return userEmail;
        //    }

        //    return null; // User is not authenticated or email claim not found
        //}

    }
}
