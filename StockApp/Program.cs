using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StockApp;
using StockApp.Infrastructure;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.Authority = "https://accounts.google.com";
    options.ProviderOptions.ClientId = builder.Configuration["Google:ClientId"];
    options.ProviderOptions.ResponseType = "id_token token";
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Google:ClientId"]);
    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("email");
});
builder.Services.AddScoped<IdTokenProvider>();
builder.Services.AddTransient<AuthorizationHeaderHandler>();
builder.Services.AddHttpClient("Api", client =>
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]))
    .AddHttpMessageHandler<AuthorizationHeaderHandler>();

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("Api"));

await builder.Build().RunAsync();
