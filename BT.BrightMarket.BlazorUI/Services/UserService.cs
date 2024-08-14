using BT.BrightMarket.BlazorUI.Interfaces;
using BT.BrightMarket.Shared.DTOs;
using BT.BrightMarket.Shared.Models.Users;
using System.Net.Http.Json;

namespace BT.BrightMarket.BlazorUI.Services
{
    public class UserService : IUserService
    {

        private readonly HttpClient httpClientSecured, httpClientUnsecured;
        public UserService(IHttpClientFactory httpClient) 
        {
            this.httpClientSecured = httpClient.CreateClient("BrightMarketAPI");
            this.httpClientUnsecured = httpClient.CreateClient("BrightMarketAPIUnsecured");
        }

        public async Task<User> GetPersonalUserAsync()
        {
            try
            {
                var response = await httpClientSecured.GetAsync("users/personal");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<User>();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve personal user.", ex);
            }
        }

        public async Task<User> CreateUserAsync(UserDTO.PostUserObject user)
        {
            try
            {
                var response = await httpClientUnsecured.PostAsJsonAsync("users", user);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<User>();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create user.", ex);
            }
        }



    }
}
