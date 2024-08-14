using BT.BrightMarket.BlazorUI.Interfaces;
using BT.BrightMarket.Shared.DTOs;
using BT.BrightMarket.Shared.Models.Conversations;
using System.Diagnostics;
using System.Net.Http.Json;


namespace BT.BrightMarket.BlazorUI.Services
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient httpClient;
        public MessageService(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient.CreateClient("BrightMarketAPI");
        }


        public async Task<Message> CreateMessageAsync(MessageDTO message)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("messages", message);
                response.EnsureSuccessStatusCode();
                var msg = await response.Content.ReadFromJsonAsync<Message>();

                Debug.WriteLine(msg.Text);
                Debug.WriteLine(msg.SenderId);
                Debug.WriteLine(msg.TimeStamp);
                Debug.WriteLine(msg.ConversationId);

                return msg;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create message.", ex);
            }
        }

        public async Task<List<Message>> GetMessagesByConversationId(int conversationId, int pageNumber = 1, int pageSize = 15)
        {
            try
            {
                var url = $"messages/{conversationId}?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Message>>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to retrieve messages: {ex.Message}");
            }
        }

    }
}
