using BT.BrightMarket.BlazorUI.Interfaces;
using BT.BrightMarket.Shared.DTOs;
using BT.BrightMarket.Shared.Models.Conversations;
using System.Net.Http.Json;

namespace BT.BrightMarket.BlazorUI.Services
{
    public class ConversationService : IConversationService
    {
        private readonly HttpClient httpClient;
        public ConversationService(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient.CreateClient("BrightMarketAPI");
        }

        public async Task<List<Conversation>> GetPersonalConversationsAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("conversations/personal-conversations");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Conversation>>();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve conversations.", ex);
            }
        }

        public async Task<Conversation> CreateConversationAsync(ConversationDTO conversation)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("conversations", conversation);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Conversation>();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create conversation.", ex);
            }
        }

        public async Task<Conversation> GetConversationByIdAsync(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"conversations/conversationId/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Conversation>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve conversation with ID {id}.", ex);
            }
        }

        public async Task<Conversation> GetConversationByProductId(int productId)
        {
            try
            {
                var response = await httpClient.GetAsync($"conversations/productId/{productId}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Conversation>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve conversation for product with ID {productId}.", ex);
            }
        }


    }
}
