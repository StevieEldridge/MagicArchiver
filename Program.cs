using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MagicArchiver;
using MagicArchiver.Pages.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string? connStr = builder.Configuration.GetValue<string>("Configuration:ConnectionString");

if (connStr == null) {
  Console.WriteLine("Warning: Connection string is null. Defaulting to development");
}

builder.Services.AddScoped(sp => new HttpClient { 
  BaseAddress = new Uri(connStr ?? "http://localhost:8090/api/")
});

builder.Services.AddScoped<IPocketBase, PocketBase>();
builder.Services.AddSingleton<IGlobalState, GlobalState>();




await builder.Build().RunAsync();
