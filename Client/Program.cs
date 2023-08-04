using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StowageApp.Client;
using StowageApp.Shared.Entities;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<MyStorageBase, MyLocalDiskStorage>(provider =>
{
    //string rootDir = @"C:\Users\windows\Desktop\C#\Files";
    string rootDir = @"D:\Roberto_Macedo\Files";
    return new MyLocalDiskStorage(rootDir);
});

await builder.Build().RunAsync();
