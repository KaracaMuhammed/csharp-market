using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BT.BrightMarket.BlazorUI;
using BT.BrightMarket.BlazorUI.Helpers;
using BT.BrightMarket.BlazorUI.Extensions;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<HttpClientAccessTokenHandler>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient("BrightMarketAPIUnsecured", client => client.BaseAddress = new Uri("http://localhost:5000/api/v1/")); // unsecured
builder.Services.AddHttpClient("BrightMarketAPI", client => client.BaseAddress = new Uri("http://localhost:5000/api/v1/")) // secured with token
       .AddHttpMessageHandler<HttpClientAccessTokenHandler>();
builder.Services.RegisterServices();

builder.Services.AddMsalAuthentication(options =>
{
    options.ProviderOptions.DefaultAccessTokenScopes.Add("api://040cb84f-bafd-43c2-b40c-2e8ee3f609e9/access_as_user");
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
});

await builder.Build().RunAsync();