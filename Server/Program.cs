using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using StowageApp.Server.Data;
using StowageApp.Server.Services;
using StowageApp.Shared.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<FileStowageContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<SeedingServices>();

//builder.Services.AddScoped<IStorageService, LocalStorageService>();

builder.Services.AddScoped<MyStorageBase, MyLocalDiskStorage>(provider =>
{
    //string rootDir = @"C:\Users\windows\Desktop\C#\Files";
    string rootDir = @"D:\Roberto_Macedo\Files";
    return new MyLocalDiskStorage(rootDir);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seedingService = services.GetRequiredService<SeedingServices>();
    seedingService.Seed();
}

app.UseCors(builder =>
{
    builder.AllowAnyOrigin();
    builder.AllowAnyMethod();
    builder.AllowAnyHeader();
});

app.Run();
