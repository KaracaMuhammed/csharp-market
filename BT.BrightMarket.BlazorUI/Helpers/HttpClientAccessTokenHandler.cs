using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BT.BrightMarket.BlazorUI.Helpers
{
    public class HttpClientAccessTokenHandler : DelegatingHandler
    {
        private readonly IAccessTokenProvider _accessTokenProvider;

        public HttpClientAccessTokenHandler(IAccessTokenProvider accessTokenProvider)
        {
            _accessTokenProvider = accessTokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tokenResult = await _accessTokenProvider.RequestAccessToken(
                new AccessTokenRequestOptions
                {
                    Scopes = new[] { "api://040cb84f-bafd-43c2-b40c-2e8ee3f609e9/access_as_user" }
                });

            if (!tokenResult.TryGetToken(out var token))
                throw new Exception("Error: Access token not available");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}