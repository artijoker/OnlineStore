using BlazorClient;
using Blazored.LocalStorage;
using Blazored.Toast;
using HttpApiClient;
using HttpModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazoredToast();
builder.Services.AddSingleton<IClock, Clock>();
builder.Services.AddSingleton(new ShopClient("https://localhost:7254"));

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
