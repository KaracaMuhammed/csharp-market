using BT.BrightMarket.BlazorUI.Interfaces;
using BT.BrightMarket.Shared.Models.Users;
using System.Net.Http.Json;

namespace BT.BrightMarket.BlazorUI.Services
{
    public class RegionService : IRegionService
    {
        private readonly HttpClient httpClient;
        public RegionService(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient.CreateClient("BrightMarketAPIUnsecured");
        }

        public async Task<List<Region>> GetAllRegionsAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("Regions");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Region>>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to retrieve regions.", ex);
            }
        }
    }
}
